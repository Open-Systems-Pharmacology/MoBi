using System.Collections.Generic;
using System.Linq;
using libsbmlcs;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using Model = libsbmlcs.Model;
using Unit = OSPSuite.Core.Domain.UnitSystem.Unit;

namespace MoBi.Engine.Sbml
{
   public class SpeciesImporter : SBMLImporter
   {
      private readonly IMoleculeStartValuesCreator _moleculeStartValuesCreator;
      internal IMoleculeBuildingBlock MoleculeBuildingBlock;
      private IMoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      private readonly IMoleculeBuilderFactory _moleculeBuilderFactory;
      private readonly IMoBiDimensionFactory _moBiDimensionFactory;
      private readonly Dictionary<string, Dimension> _dimensionDictionary;
      private IUnitDefinitionImporter _unitDefinitionImporter;
      private int _counter;

      public SpeciesImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, IMoleculeBuilderFactory moleculeBuilderFactory, IMoleculeStartValuesCreator moleculeStartValuesCreator, IMoBiDimensionFactory moBiDimensionFactory, ASTHandler astHandler, IMoBiContext context, IUnitDefinitionImporter unitDefinitionImporter)
          : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _moleculeBuilderFactory = moleculeBuilderFactory;
         _moleculeStartValuesCreator = moleculeStartValuesCreator;
         _moBiDimensionFactory = moBiDimensionFactory;
         _counter = 1;
         _dimensionDictionary = new Dictionary<string, Dimension>();
         _unitDefinitionImporter = unitDefinitionImporter;
      }

      protected override void Import(Model model)
      {
         CreateMoleculeBuildingBlock();
         for (long i = 0; i < model.getNumSpecies(); i++)
         {
            CreateMoleculeFromSpecies(model.getSpecies(i));
         }
         CheckMoleculeNameContainer();
         CreateDummySpecies();
         CreateMoleculeStartValueBuildingBlock(model);
         SetMoleculeStartValues(model);
         SetDummyMSVs();
         AddToProject();
      }

      private void CreateMoleculeStartValueBuildingBlock(Model model)
      {
         _moleculeStartValuesBuildingBlock = _moleculeStartValuesCreator.CreateFrom(GetMainSpatialStructure(model),
            MoleculeBuildingBlock)
            .WithId(SBMLConstants.SBML_MOLECULESTARTVALUES_BB)
            .WithName(SBMLConstants.SBML_MOLECULESTARTVALUES_BB)
            .WithDescription(SBMLConstants.SBML_MOLECULESTARTVALUES_DESCRIPTION);
      }

      /// <summary>
      ///     Creates one dummy Species for each Compartment.
      /// </summary>
      private void CreateDummySpecies()
      {
         foreach (var child in GetMainTopContainer().GetAllChildren<IContainer>())
         {
            if (child.Name == Constants.MOLECULE_PROPERTIES) continue;
            var mbuilder = _moleculeBuilderFactory.Create(MoleculeBuildingBlock.FormulaCache)
                .WithName(SBMLConstants.SBML_DUMMYSPECIES + child.Name)
                .WithDescription(SBMLConstants.SBML_DUMMYSPECIES + child.Name);

            MoleculeBuildingBlock.Add(mbuilder);
            _sbmlInformation.DummyNameContainerDictionary[mbuilder.Name] = child.Name;
         }
      }

      /// <summary>
      ///     Creates the Molecule - and the Molecule Start Values Building Block.
      /// </summary>
      internal void CreateMoleculeBuildingBlock()
      {
         MoleculeBuildingBlock = ObjectBaseFactory.Create<IMoleculeBuildingBlock>()
             .WithName(SBMLConstants.SBML_SPECIES_BB);
      }

      /// <summary>
      ///     Creates a MoBi Molecule from a given SBML species. 
      /// </summary>
      internal void CreateMoleculeFromSpecies(Species species)
      {
         var speciesName = species.getName();
         if (!species.isSetName())
         {
            CreateMolecule(species, species.getId(), false);
         }
         else
         {
            if (_sbmlInformation.MoleculeInformation.All(info => info.GetSpeciesNameIfOne() != speciesName))
            {
               CreateMolecule(species, speciesName, false);
            }
            else
            {
               var molecule = GetMoleculeFromSpecies(speciesName);
               var speciesCompartment = GetContainerFromCompartment_(species.getCompartment());
               var molinfo = _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.GetMoleculeBuilderName() == molecule.Name);
               if (molinfo == null) return;
               var moleculeContainer = molinfo.GetContainer();

               if (moleculeContainer.Any(x => x.Name == speciesCompartment.Name))
               {
                  CreateMolecule(species, species.getName(), true);
               }
               else
               {
                  var container = GetContainerFromCompartment_(species.getCompartment());
                  molinfo.AddContainer(container);
                  molinfo.SetMoleculeBuilder(molecule);
                  molinfo.SetSpecies(species);
               }
            }
         }
      }

      /// <summary>
      ///     Creates a Molecule.
      /// </summary>
      private void CreateMolecule(Species species, string name, bool existant)
      {
         if (existant)
         {
            name = name + _counter;
            _counter++;
         }

         var mbuilder = _moleculeBuilderFactory.Create(MoleculeBuildingBlock.FormulaCache)
             .WithName(name)
             .WithDescription(SBMLConstants.SBML_NOTES + species.getNotesString() + species.getId());

         var molInfo = new MoleculeInformation(species, mbuilder);
         _sbmlInformation.MoleculeInformation.Add(molInfo);

         var container = GetContainerFromCompartment_(species.getCompartment());
         if (container != null) molInfo.AddContainer(container);
         MoleculeBuildingBlock.Add(mbuilder);
      }

      /// <summary>
      ///     Check if for one key (moleculeName) more occurences of the same compartment are there.
      ///     It's senseless if e.g. Beta_Catenin occurs more than once in Compartment c1.
      /// </summary>
      private void CheckMoleculeNameContainer()
      {
         foreach (var molinfo in _sbmlInformation.MoleculeInformation)
         {
            if (!molinfo.IsMultipleTimesInOneCompartment()) continue;
            var msg = new NotificationMessage(MoleculeBuildingBlock, MessageOrigin.All, null, NotificationType.Warning)
            {
               Message = "Warning - more than one Species.Name are in the same compartment! Does that make sense?"
            };
            _sbmlInformation.NotificationMessages.Add(msg);
         }
      }

      /// <summary>
      ///     Checks of a molecule with this name is already existant.
      /// </summary>
      private IMoleculeBuilder GetMoleculeFromSpecies(string name)
      {
         return MoleculeBuildingBlock.FirstOrDefault(mol => mol.Name == name);
      }

      /// <summary>
      ///     Sets all the autogenerated Molecule Start Values to the right values and updates their 
      ///     "IsPresent" property. 
      /// </summary>
      private void SetMoleculeStartValues(Model model)
      {
         foreach (var molInfo in _sbmlInformation.MoleculeInformation)
         {
            foreach (var msv in _moleculeStartValuesBuildingBlock)
            {
               if (msv.Name != molInfo.GetMoleculeBuilderName()) continue;
               if (molInfo.GetContainer().Any(x => x.Name == msv.ContainerPath.LastOrDefault()))
               {
                  msv.IsPresent = true;
                  var sbmlSpecies = molInfo.GetSpeciesIfOne();
                  if (sbmlSpecies == null) return;
                  var sbmlUnit = GetUnit(sbmlSpecies, model);
                  var amountDimension = _unitDefinitionImporter.DimensionFor(sbmlUnit);

                  //unit is set by the Unit of SubstanceUnit
                  if (sbmlSpecies.isSetInitialAmount())
                  {
                     msv.StartValue = sbmlSpecies.getInitialAmount();
                     if (amountDimension != null)
                     {
                        msv.Dimension = amountDimension;
                        molInfo.SetDimension(amountDimension);
                     }
                  }
                  if (!sbmlSpecies.isSetInitialConcentration()) continue;

                  //unit is {unit of amount}/{unit of size}
                  var startValue = _unitDefinitionImporter.ToMobiBaseUnit(sbmlUnit, new[] { sbmlSpecies.getInitialConcentration() })[0];
                  msv.StartValue = startValue;
                  msv.Formula = new ConstantFormula(startValue);

                  var sizeDimension = GetSizeDimensionFromCompartment(sbmlSpecies, model);
                  if (amountDimension == null) continue;
                  if (sizeDimension == null) continue;

                  var newDim = _moBiDimensionFactory.DimensionForUnit($"{amountDimension.BaseUnit.Name}/{sizeDimension.BaseUnit.Name}") ?? CreateNewDimension(amountDimension, sizeDimension);
                  msv.Dimension = newDim;
                  molInfo.SetDimension(newDim);
               }
               else
               {
                  msv.IsPresent = false;
                  msv.StartValue = 0;
               }
            }
         }
      }

      /// <summary>
      ///     Creates a new dimension by two known dimensions.
      /// </summary>
      private IDimension CreateNewDimension(IDimension amountDimension, IDimension sizeDimension)
      {
         var dimName = SBMLConstants.DIMENSION + amountDimension.Name + SBMLConstants.DIVIDE + sizeDimension.Name;
         if (_dimensionDictionary.ContainsKey(dimName))
            return _dimensionDictionary[dimName];
         if (_moBiDimensionFactory.Dimensions.Any(dim => dim.Name == dimName))
            return _moBiDimensionFactory.Dimension(dimName);

         var newBaseDimRepresentation = CreateNewBaseDimRepresentation(amountDimension.BaseRepresentation, sizeDimension.BaseRepresentation);
         var newFactor = GetNewFactor(amountDimension, sizeDimension); //Faktoren dividieren
         var baseUnitName = SBMLConstants.SBML_BASE_UNIT + amountDimension.DisplayName + SBMLConstants.DIVIDE + sizeDimension.Name;
         var unitName = SBMLConstants.UNIT + amountDimension.Name + SBMLConstants.DIVIDE + sizeDimension.Name;
         var newBaseUnit = new Unit(baseUnitName, 1, 0) { Visible = false };
         var newDim = new Dimension(newBaseDimRepresentation, dimName, newBaseUnit.Name);
         var newUnit = new Unit(unitName, newFactor, 0);

         newDim.AddUnit(newUnit);
         _dimensionDictionary.Add(dimName, newDim);
         var tmp = _moBiDimensionFactory.Dimensions.Count();
         _moBiDimensionFactory.AddDimension(newDim);
         var tmp2 = _moBiDimensionFactory.Dimensions.Any(dim => dim.Name == newDim.Name);
         return newDim;
      }

      /// <summary>
      ///     Divides the Factors of two given Dimensions to get a new factor for a 
      ///     new dimension to achieve: {unit of amount}/{unit of size}
      /// </summary>
      internal double GetNewFactor(IDimension amountDimension, IDimension sizeDimension)
      {
         var amountUnit = amountDimension.DefaultUnit;
         var sizeUnit = sizeDimension.DefaultUnit;
         var newFactor = amountUnit.Factor / sizeUnit.Factor;
         return newFactor;
      }

      /// <summary>
      ///     Creates the BaseDimensionRepresentation for two BaseDimensionRepresentation (baserep1/baserep2 => substraction of the exponents)
      /// </summary>
      private BaseDimensionRepresentation CreateNewBaseDimRepresentation(BaseDimensionRepresentation amountBaseRep, BaseDimensionRepresentation sizeBaseRep)
      {
         var baseDimensionRepresentation = new BaseDimensionRepresentation
         {
            AmountExponent = amountBaseRep.AmountExponent - sizeBaseRep.AmountExponent,
            ElectricCurrentExponent = amountBaseRep.ElectricCurrentExponent - sizeBaseRep.ElectricCurrentExponent,
            LengthExponent = amountBaseRep.LengthExponent - sizeBaseRep.LengthExponent,
            LuminousIntensityExponent = amountBaseRep.LuminousIntensityExponent - sizeBaseRep.LuminousIntensityExponent,
            MassExponent = amountBaseRep.MassExponent - sizeBaseRep.MassExponent,
            TemperatureExponent = amountBaseRep.TemperatureExponent - sizeBaseRep.TemperatureExponent,
            TimeExponent = amountBaseRep.TimeExponent - sizeBaseRep.TimeExponent
         };
         return baseDimensionRepresentation;
      }

      /// <summary>
      ///     Gets the dimension of the size Parameter of the container/compartment in which the given Species is located.
      /// </summary>
      private IDimension GetSizeDimensionFromCompartment(Species species, Model model)
      {
         var compartmentSizeUnit = model.getCompartment(species.getCompartment()).getUnits();

         var sizeDimension = _moBiDimensionFactory.TryGetDimensionCaseInsensitiveFromUnit(compartmentSizeUnit);
         if (sizeDimension == Constants.Dimension.NO_DIMENSION) return sizeDimension;

         if (_sbmlInformation.MobiDimension.ContainsKey(compartmentSizeUnit))
            sizeDimension = _sbmlInformation.MobiDimension[compartmentSizeUnit];

         if (_moBiDimensionFactory.Dimensions.All(dim => dim.Name != sizeDimension.Name))
            _moBiDimensionFactory.AddDimension(sizeDimension);

         return sizeDimension;
      }

      private string GetUnit(Species s, Model model)
      {
         return s.isSetSubstanceUnits() ? s.getSubstanceUnits() : model.getSubstanceUnits();
      }

      /// <summary>
      ///     Gets the path to a Container by a given compartment id.
      /// </summary>
      protected internal IObjectPath GetPathToContainerOfCompartmentId(string compartmentId)
      {
         return (from container in GetMainTopContainer().Children where container.Name == compartmentId select ObjectPathFactory.CreateAbsoluteObjectPath(container)).FirstOrDefault();
      }

      /// <summary>
      ///     Creates the Molecule Start Values Building Block, sets the Molecule Start Values and 
      ///     adds the MBB and MSVBB to the SBMLProject.
      /// </summary>
      public override void AddToProject()
      {
         _command.AddCommand(new AddBuildingBlockCommand<IMoleculeBuildingBlock>(MoleculeBuildingBlock).Run(_context));
         _command.AddCommand(new AddBuildingBlockCommand<IMoleculeStartValuesBuildingBlock>(_moleculeStartValuesBuildingBlock).Run(_context));
      }

      /// <summary>
      ///     Sets foreach Dummy Species their compartment. 
      /// </summary>
      private void SetDummyMSVs()
      {
         foreach (var dummySpecies in _sbmlInformation.DummyNameContainerDictionary)
         {
            foreach (var msv in _moleculeStartValuesBuildingBlock)
            {
               if (msv.Name != dummySpecies.Key) continue;
               if (dummySpecies.Value == msv.ContainerPath.LastOrDefault())
               {
                  msv.IsPresent = true;
                  msv.StartValue = 0;
               }
               else
               {
                  msv.IsPresent = false;
                  msv.StartValue = 0;
               }
            }
         }
      }
   }
}
