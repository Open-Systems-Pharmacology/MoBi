using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Assets;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Serialization.Converter.v3_2
{
   public class Converter313To32 : IMoBiObjectConverter,
                                   IVisitor<DataRepository>,
                                   IVisitor<ISpatialStructure>,
                                   IVisitor<IMoleculeBuildingBlock>,
                                   IVisitor<IParameterStartValuesBuildingBlock>,
                                   IVisitor<IMoleculeStartValuesBuildingBlock>,
                                   IVisitor<IPassiveTransportBuildingBlock>,
                                   IVisitor<IObserverBuildingBlock>,
                                   IVisitor<IReactionBuildingBlock>,
                                   IVisitor<IEventGroupBuildingBlock>,
                                   IVisitor<IMoBiSimulation>,
                                   IVisitor<SimulationTransfer>,
                                   IVisitor<IMoBiProject>
   {
      private readonly IDimensionConverter _dimensionConverter;
      private readonly IMoBiDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaMapper _formulaMapper;
      private readonly IDimensionMapper _dimensionMapper;
      private readonly IEventPublisher _eventPublisher;
      private readonly ObjectTypeResolver _objectTypeResolver;
      private bool _suspendWarning;

      public Converter313To32(IDimensionConverter dimensionConverter, IMoBiDimensionFactory dimensionFactory,
                              IObjectBaseFactory objectBaseFactory, IFormulaMapper formulaMapper, IDimensionMapper dimensionMapper, IEventPublisher eventPublisher)
      {
         _dimensionConverter = dimensionConverter;
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _dimensionMapper = dimensionMapper;
         _eventPublisher = eventPublisher;
         _objectTypeResolver = new ObjectTypeResolver();
         _suspendWarning = false;
      }

      public bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V3_1_3;
      }

      public int Convert(object objectToConvert, IMoBiProject project)
      {
         this.Visit(objectToConvert);
         return ProjectVersions.V3_2_1;
      }

      public int ConvertXml(XElement element, IMoBiProject project)
      {
         _dimensionConverter.ConvertDimensionIn(element);
         convertOutputSchemaForSimulations(element);
         convertParameterContainerToContainer(element);
         convertActiveTransportContainersToChildren(element);
         convertPassiveTransportsToChildren(element);
         convertApplicationTransportsToChildren(element);
         convertDimensionFactory(element);
         convetSimulation(element);
         convertObservers(element);
         
         return ProjectVersions.V3_2_1;
      }

      private void convertPassiveTransportsToChildren(XElement element)
      {
         var passiveTransports = element.Descendants("AllPassiveTransports");
         foreach (var passiveTransportsElement in passiveTransports)
         {
            var childList = getOrRetrieveChidrensElementFrom(passiveTransportsElement.Parent);
            childList.Add(passiveTransportsElement.Elements("TransportBuilder"));
         }
      }

      private void convertObservers(XElement element)
      {
         foreach (var observerBuildingBlock in descendantsAndSelfNamed(element, AppConstants.XmlNames.ObserverBuildingBlock, AppConstants.XmlNames.Observers))
         {
            var builders = observerBuildingBlock.Element("Builders");
            if (builders == null)
               continue;

            foreach (var observerNode in builders.Elements())
            {
               var moleculeList = new XElement(AppConstants.XmlNames.MoleculeList, observerNode.Attribute("forAll"));
               var moleculeNamesNodes = observerNode.Descendants("MoleculeNames");
               foreach (var moleculeName in moleculeNamesNodes)
               {
                  moleculeList.Add(moleculeName);
               }
               observerNode.Add(moleculeList);
            }
         }
      }

      private static IEnumerable<XElement> descendantsAndSelfNamed(XElement element, params string[] names)
      {
         return element.DescendantsAndSelf().Where(x => names.Contains(x.Name.ToString()));
      }
      private void convetSimulation(XElement element)
      {
         foreach (var settings in element.Descendants(AppConstants.XmlNames.CVODESettings))
         {
            var newNode = new XElement(AppConstants.XmlNames.Solver, settings.Attributes(), settings.Nodes());
            settings.Parent.Add(newNode);
         }
      }

      private void convertDimensionFactory(XElement element)
      {
         foreach (var dimensionElement in element.DescendantsAndSelf("DimensionFactory").SelectMany(x => x.Descendants("Dimension")))
         {
            var oldBaseUnit = dimensionElement.Attribute("BaseUnit");
            if (oldBaseUnit == null) continue;

            dimensionElement.AddAttribute("baseUnit", oldBaseUnit.Value);

            var oldDefaultUnit = dimensionElement.Attribute("DefaultUnit");
            if (oldDefaultUnit == null) continue;

            dimensionElement.AddAttribute("defaultUnit", oldDefaultUnit.Value);
         }
      }

      private void convertApplicationTransportsToChildren(XElement element)
      {
         var transports = element.Descendants("Transports");
         foreach (var transport in transports)
         {
            var childrenList = getOrRetrieveChidrensElementFrom(transport.Parent);
            childrenList.Add(transport.Elements("TransportBuilder"));
         }
         var applicationMolecuBuilderList = element.Descendants("MoleculeNames");
         foreach (var moleculeBuilder in applicationMolecuBuilderList)
         {
            var childrenList = getOrRetrieveChidrensElementFrom(moleculeBuilder.Parent);
            childrenList.Add(moleculeBuilder.Elements("ApplicationMoleculeBuilder"));
         }
      }

      private void convertActiveTransportContainersToChildren(XElement element)
      {
         var activeTransoporterList = element.Descendants("ActiveTransportRealizations");
         foreach (var transportList in activeTransoporterList)
         {
            var childList = getOrRetrieveChidrensElementFrom(transportList.Parent);
            childList.Add(transportList.Elements("TransportBuilder"));
         }
      }

      private void convertParameterContainerToContainer(XElement element)
      {
         var parametersList = element.Descendants("Parameters");
         foreach (var parametersElement in parametersList)
         {
            var childrenElement = getOrRetrieveChidrensElementFrom(parametersElement.Parent);
            childrenElement.Add(parametersElement.Elements());
         }
      }

      private XElement getOrRetrieveChidrensElementFrom(XElement parent)
      {
         var childrenElment = parent.Element("Children");
         if (childrenElment == null)
         {
            childrenElment = parent.AddElement(new XElement("Children"));
         }
         return childrenElment;
      }

      private void convertOutputSchemaForSimulations(XElement element)
      {
         foreach (var outputSchemaElement in element.Descendants("OutputSchema"))
         {
            var intervals = outputSchemaElement.Descendants("OutputInterval");
            outputSchemaElement.Add(newId());
            outputSchemaElement.AddAttribute("name", Constants.OUTPUT_SCHEMA);
            var outputSchemaChildren = new XElement("Children");

            int i = 0;
            foreach (var interval in intervals)
            {
               var outputIntervalElementChildren = new XElement("Children");
               outputIntervalElementChildren.Add(parameterElement(Constants.Parameters.START_TIME, Constants.Dimension.TIME, interval.GetAttribute("startTime")));
               outputIntervalElementChildren.Add(parameterElement(Constants.Parameters.END_TIME, Constants.Dimension.TIME, interval.GetAttribute("endTime")));
               outputIntervalElementChildren.Add(parameterElement(Constants.Parameters.RESOLUTION, Constants.Dimension.RESOLUTION, interval.GetAttribute("resolution")));
               outputSchemaChildren.Add(new XElement("OutputInterval", new XAttribute("name", string.Format("{0} {1}", Constants.OUTPUT_INTERVAL, i)), outputIntervalElementChildren));
               i++;
            }
            outputSchemaElement.Add(outputSchemaChildren);
         }
      }

      private XElement parameterElement(string name, string dimension, string value)
      {
         return new XElement("Parameter",
                             newId(),
                             new XAttribute("name", name),
                             new XAttribute(Constants.Serialization.Attribute.Dimension, dimension),
                             new XAttribute(Constants.Serialization.Attribute.VALUE, value)
            );
      }

      private XAttribute newId()
      {
         return new XAttribute("id", Guid.NewGuid().ToString());
      }

      public void Visit(DataRepository dataRepository)
      {
         _dimensionConverter.ConvertDimensionIn(dataRepository);
      }

      private void convert<T>(IBuildingBlock<T> buildingBlock) where T : class, IObjectBase
      {
         _dimensionConverter.ConvertDimensionIn(buildingBlock);
         createDimensionWarningsForBuildingBlock(buildingBlock);
      }

      public void Visit(ISpatialStructure spatialStructure)
      {
         convert(spatialStructure);
         spatialStructure.TopContainers.Each(convertSpecialParametersIn);
         updateForPKSimChanges(spatialStructure);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         convert(moleculeBuildingBlock);
         updateForPKSimChanges(moleculeBuildingBlock);
      }

      private void updateForPKSimChanges(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var calculatedSpecificIntestinalPermeabilityTranscellularFormula =
            moleculeBuildingBlock.FormulaCache.FindByName("PARAM_P_int_InVitro");
         if (calculatedSpecificIntestinalPermeabilityTranscellularFormula == null) return;
         var velocity = _dimensionFactory.GetDimension(AppConstants.DimensionNames.VELOCITY);
         moleculeBuildingBlock.Each(mb=>addCalculatedSpecificIntestinalPermeabilityTranscellularParameterTo(mb,calculatedSpecificIntestinalPermeabilityTranscellularFormula,velocity));
      }

      private void addCalculatedSpecificIntestinalPermeabilityTranscellularParameterTo(IMoleculeBuilder moleculeBuilder, IFormula formula, IDimension dimension)
      {
         if (!isPKSimMolecule(moleculeBuilder)) return;
         moleculeBuilder.Add(_objectBaseFactory
            .Create<IParameter>()
            .WithName("Calculated specific intestinal permeability (transcellular)")
            .WithFormula(formula)
            .WithDimension(dimension)
            .WithMode(ParameterBuildMode.Global));
      }

      private bool isPKSimMolecule(IMoleculeBuilder moleculeBuilder)
      {
         return moleculeBuilder.ContainsName("Effective molecular weight");
      }

      private void updateForPKSimChanges(ISpatialStructure spatialStructure)
      {
         if (canAddNewParametersTo(spatialStructure))
         {
            var blood2PlasmaFormula = createBlood2PlasmaFormula();
            var mucosaPermeabilityScaleFactorTranscellularFormula =createMucosaPermeabilityScaleFactor_transcellularFormula();
            var defaultIntestinalPermeabilityTranscellularFormula =creteDefaultIntestinalPermeabilityTranscellularFormula();
            spatialStructure.AddFormula(blood2PlasmaFormula);
            spatialStructure.AddFormula(mucosaPermeabilityScaleFactorTranscellularFormula);
            spatialStructure.AddFormula(defaultIntestinalPermeabilityTranscellularFormula);
            spatialStructure.GlobalMoleculeDependentProperties.Add(createBlood2PlasmaRatioParameter(blood2PlasmaFormula));
            spatialStructure.GlobalMoleculeDependentProperties.Add(createMucosaPermeabilityScaleFactorTranscellularParameter(mucosaPermeabilityScaleFactorTranscellularFormula));
            spatialStructure.GlobalMoleculeDependentProperties.Add(createDefaultIntestinalPermeabilityTranscellularParameter(defaultIntestinalPermeabilityTranscellularFormula));
         }
         if (is2PoreModel(spatialStructure))
         {
            var moleculeProperties = spatialStructure.NeighborhoodsContainer.GetSingleChildByName<IContainer>("EndogenousIgG_int_EndogenousIgG_pls").GetSingleChildByName<IContainer>(Constants.MOLECULE_PROPERTIES);
            if(moleculeProperties==null) return;
            var partitionCoefficient =
               moleculeProperties.GetSingleChildByName<IParameter>("Partition coefficient (interstitial/plasma)");
            // Change Formula to Constant
            if(partitionCoefficient==null) return;
               partitionCoefficient.Formula = _objectBaseFactory.Create<ConstantFormula>().WithValue(1);
         }
      }

      private bool is2PoreModel(ISpatialStructure spatialStructure)
      {
         return spatialStructure.NeighborhoodsContainer.ContainsName("EndogenousIgG_int_EndogenousIgG_pls");
      }

      private IParameter createDefaultIntestinalPermeabilityTranscellularParameter(IFormula formula)
      {
         return _objectBaseFactory.Create<IParameter>().WithName("Default Intestinal permeability (transcellular)")
            .WithMode(ParameterBuildMode.Global)
            .WithFormula(formula)
            .WithDimension(_dimensionFactory.GetDimension(AppConstants.DimensionNames.VELOCITY));
      }

      private IParameter createMucosaPermeabilityScaleFactorTranscellularParameter(IFormula mucosaPermeabilityScaleFactorTranscellularFormula)
      {
         return _objectBaseFactory.Create<IParameter>().WithName("Mucosa permeability scale factor (transcellular)")
            .WithMode(ParameterBuildMode.Global)
            .WithFormula(mucosaPermeabilityScaleFactorTranscellularFormula)
            .WithDimension(Constants.Dimension.NO_DIMENSION);
      }

      private IFormula creteDefaultIntestinalPermeabilityTranscellularFormula()
      {
         var velocity = _dimensionFactory.GetDimension(AppConstants.DimensionNames.VELOCITY);
         IFormula defaultIntestinalPermeabilityTranscellularFormula = _objectBaseFactory
            .Create<ExplicitFormula>()
            .WithFormulaString("P_int_InVitro")
            .WithDimension(velocity)
            .WithName("PARAM_P_int_trans_default");
         defaultIntestinalPermeabilityTranscellularFormula.AddObjectPath(
            createFormulaUsablePath(new string[]{ ObjectPath.PARENT_CONTAINER,"Calculated specific intestinal permeability (transcellular)"},"P_int_InVitro",velocity)
            );
         return defaultIntestinalPermeabilityTranscellularFormula;
      }

      private IFormula createMucosaPermeabilityScaleFactor_transcellularFormula()
      {
         IFormula mucosaPermeabilityScaleFactor_transcellularFormula = _objectBaseFactory
            .Create<ExplicitFormula>()
            .WithFormulaString("P_int_trans_default >0 ? P_int_trans / P_int_trans_default : 0")
            .WithDimension(Constants.Dimension.NO_DIMENSION)
            .WithName("PARAM_P_int_scalefactor");
         mucosaPermeabilityScaleFactor_transcellularFormula.AddObjectPath(
            createFormulaUsablePath(new[] { ObjectPath.PARENT_CONTAINER, "Default Intestinal permeability (transcellular)" },"P_int_trans_default",_dimensionFactory.GetDimension(AppConstants.DimensionNames.VELOCITY)));
         mucosaPermeabilityScaleFactor_transcellularFormula.AddObjectPath(
            createFormulaUsablePath(new[] { ObjectPath.PARENT_CONTAINER, "Intestinal permeability (transcellular)" }, "P_int_trans", _dimensionFactory.GetDimension(AppConstants.DimensionNames.VELOCITY)));
         return mucosaPermeabilityScaleFactor_transcellularFormula;
      }

      /// <summary>
      ///    Creates the blood2 plasma formula.
      /// </summary>
      private IFormula createBlood2PlasmaFormula()
      {
         IFormula blood2PlasmaFormula = _objectBaseFactory
            .Create<ExplicitFormula>()
            .WithFormulaString(
               "(f_water_rbc + f_lipids_rbc * 10 ^ LogP + f_proteins_rbc * KProt) * fu * HCT - HCT + 1")
            .WithDimension(Constants.Dimension.NO_DIMENSION)
            .WithName("PARAM_Blood2Plasma");
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {ObjectPathKeywords.MOLECULE, "Fraction unbound (plasma)"}, "fu", _dimensionFactory.GetDimension(AppConstants.DimensionNames.FRACTION)));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {ObjectPathKeywords.MOLECULE, "Lipophilicity"}, "LogP", _dimensionFactory.GetDimension("Log Units")));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {ObjectPathKeywords.MOLECULE, "Partition coefficient (water/protein)"}, "KProt", Constants.Dimension.NO_DIMENSION));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {"Organism", "Hematocrit"}, "HCT", Constants.Dimension.NO_DIMENSION));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {"Organism", "Vf (lipid, blood cells)"}, "f_lipids_rbc", Constants.Dimension.NO_DIMENSION));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {"Organism", "Vf (protein,blood cells)"}, "f_proteins_rbc", Constants.Dimension.NO_DIMENSION));
         blood2PlasmaFormula.AddObjectPath(
            createFormulaUsablePath(new[] {"Organism", "Vf (water,blood cells)"}, "f_water_rbc", Constants.Dimension.NO_DIMENSION));
         return blood2PlasmaFormula;
      }

      private FormulaUsablePath createFormulaUsablePath(string[] pathEntries, string paraAlias, IDimension dimension)
      {
         return new FormulaUsablePath(pathEntries)
            .WithAlias(paraAlias)
            .WithDimension(dimension);
      }

      private IParameter createBlood2PlasmaRatioParameter(IFormula blood2PlasmaFormula)
      {
         return _objectBaseFactory.Create<IParameter>().WithName("Blood/Plasma concentration ratio")
            .WithMode(ParameterBuildMode.Property)
            .WithFormula(blood2PlasmaFormula)
            .WithDimension(Constants.Dimension.NO_DIMENSION);
      }

      private bool canAddNewParametersTo(ISpatialStructure spatialStructure)
      {
         //spatialStructure has needed parameters, so it is assumed it is original from PK-Sim
         var organism = spatialStructure.TopContainers.FirstOrDefault(tc => tc.IsNamed("Organism"));
         if (organism == null) return false;
         return
            organism.ContainsNames(new[] {"Hematocrit", "Vf (lipid, blood cells)", "Vf (protein,blood cells)", "Vf (water,blood cells)"});
      }

      public void Visit(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         convert(parameterStartValuesBuildingBlock);
         convertSpecialParametersIn(parameterStartValuesBuildingBlock);
      }

      public void Visit(IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         
         moleculeStartValuesBuildingBlock.Each(msv=>msv.Dimension = _dimensionFactory.GetDimension(Constants.Dimension.AMOUNT));
         convert(moleculeStartValuesBuildingBlock);
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         convert(passiveTransportBuildingBlock);
      }

      public void Visit(IObserverBuildingBlock observerBuildingBlock)
      {
         convert(observerBuildingBlock);
      }

      public void Visit(IReactionBuildingBlock reactionBuildingBlock)
      {
         convert(reactionBuildingBlock);
      }

      public void Visit(IEventGroupBuildingBlock eventGroupBuildingBlock)
      {
         convert(eventGroupBuildingBlock);
         var inversedVolume = _dimensionFactory.GetDimension(AppConstants.DimensionNames.INVERSED_VOLUME);
         eventGroupBuildingBlock.Each(eg => convertSpecialParameter(eg, "Number_Of_Particles_Factor",1000,inversedVolume));
      }

      public void Visit(IMoBiSimulation simulation)
      {
         try
         {
            _suspendWarning = true;
            simulation.BuildConfiguration.AcceptVisitor(this);
            _dimensionConverter.ConvertDimensionIn(simulation);
            if (simulation.Results != null) 
               _dimensionConverter.ConvertDimensionIn(simulation.Results);

            convertSpecialParametersIn(simulation.Model.Root);
            simulation.HasChanged = true;
         }
         finally
         {
            _suspendWarning = false;
         }
      }

      private void convertSpecialParametersIn(IContainer container)
      {
         var inversedLength = _dimensionFactory.GetDimension(AppConstants.DimensionNames.INVERSED_LENGTH);
         convertSpecialParameter(container, "Surface/Volume ratio (blood cells)", 10, inversedLength);
         convertSpecialParameter(container, "SA proportionality factor", 1.0 / 100, inversedLength);
      }

      private void convertSpecialParameter(IContainer container, string parameterName, double conversionFactor, IDimension newDimension)
      {
         var surfaceVolumeRatioParameter =
            container.GetAllChildren<IParameter>(p => p.IsNamed(parameterName));
         surfaceVolumeRatioParameter.Each(p => convertSpecial(p, conversionFactor, newDimension));
      }

      private void convertSpecial(IParameter parameter, double conversionFactor, IDimension newDimension)
      {
         if (parameter.Dimension.Equals(Constants.Dimension.NO_DIMENSION))
         {
            parameter.Dimension = newDimension;
         }
         if (parameter.Formula.IsConstant())
         {
            ((ConstantFormula) parameter.Formula).Value = parameter.Value * conversionFactor;
            if (parameter.IsFixedValue)
            {
               parameter.Value = parameter.Value * conversionFactor;
            }
         }
      }

      public void Visit(SimulationTransfer objToVisit)
      {
         Visit((IMoBiSimulation) objToVisit.Simulation);
         objToVisit.AllObservedData.Each(_dimensionConverter.ConvertDimensionIn);
      }

      private void createDimensionWarningsForBuildingBlock(IBuildingBlock buildingBlock)
      {
         if (_suspendWarning) return;
         var messages = new List<NotificationMessage>();
         foreach (var formula in buildingBlock.FormulaCache.Where(x => x.IsExplicit()))
         {
            showWarningsFor(formula.DowncastTo<ExplicitFormula>(), buildingBlock, messages);
         }
         _eventPublisher.PublishEvent(new ShowNotificationsEvent(messages));
      }

      private void showWarningsFor(ExplicitFormula formula, IBuildingBlock buildingBlock, List<NotificationMessage> messages)
      {
         if (_formulaMapper.FormulaWasConverted(formula.FormulaString))
            return;

         if (formulaUsesOnlyOneDimension(formula))
            return;

         bool warningRequired =
            _dimensionMapper.NeededConversion(formula.Dimension) ||
            formula.ObjectPaths.Any(objectPath => _dimensionMapper.NeededConversion(objectPath.Dimension));

         if (!warningRequired)
            return;

         messages.Add(new NotificationMessage(formula, MessageOrigin.Formula, buildingBlock, NotificationType.Warning)
            {
               Message = "Dimension check warning",
               BuildingBlockType = _objectTypeResolver.TypeFor(buildingBlock),
               ObjectType =ObjectTypes.Formula
            });
      }

      private bool formulaUsesOnlyOneDimension(ExplicitFormula formula)
      {
         return formula.ObjectPaths.All(x => x.Dimension == formula.Dimension || x.Dimension == Constants.Dimension.NO_DIMENSION);
      }

      private void convertSpecialParametersIn(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         var inversedLength = _dimensionFactory.GetDimension(AppConstants.DimensionNames.INVERSED_LENGTH);
         convertSpecialParameter(parameterStartValuesBuildingBlock, "Organism|Surface/Volume ratio (blood cells)", 10, inversedLength);
         convertSpecialParameter(parameterStartValuesBuildingBlock, "Organism|SA proportionality factor", 1.0 / 100, inversedLength);
      }

      private void convertSpecialParameter(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, string parameterPath, double conversionFactor, IDimension newDimension)
      {
         var surfaceVolumeRatioParameter =
            parameterStartValuesBuildingBlock.Where(p => p.Path.PathAsString.Equals(parameterPath));
         surfaceVolumeRatioParameter.Each(p => convertSpecial(p, conversionFactor, newDimension));
      }

      private void convertSpecial(IParameterStartValue parameterStartValue, double conversionFactor, IDimension newDimension)
      {
         if (parameterStartValue.Dimension.Equals(Constants.Dimension.NO_DIMENSION))
         {
            parameterStartValue.Dimension = newDimension;
         }
         parameterStartValue.StartValue = parameterStartValue.StartValue * conversionFactor;
      }

      public void Visit(IMoBiProject objToVisit)
      {
         objToVisit.AllObservedData.Each(Visit);
      }
   }
}