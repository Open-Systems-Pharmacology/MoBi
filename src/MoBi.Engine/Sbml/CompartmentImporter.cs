using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public class CompartmentImporter : SBMLImporter
   {
      internal IMoBiSpatialStructure SpatialStructure;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      internal IContainer _topContainer;
      internal IContainer _eventsTopContainer;
      private readonly IDimensionFactory _dimensionFactory;
      private IFormulaFactory _formulaFactory;

      public CompartmentImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory,
         IMoBiSpatialStructureFactory spatialStructureFactory, IMoBiDimensionFactory moBiDimensionFactory,
         ASTHandler astHandler, IMoBiContext context, IFormulaFactory formulaFactory)
         : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _objectBaseFactory = objectBaseFactory;
         _spatialStructureFactory = spatialStructureFactory;
         _dimensionFactory = moBiDimensionFactory;
         _formulaFactory = formulaFactory;
      }

      protected override void Import(Model sbmlModel)
      {
         CreateTopContainer(sbmlModel);
         CreateEventsTopContainer();

         for (long i = 0; i < sbmlModel.getNumCompartments(); i++)
         {
            _topContainer.Add(CreateContainerFromCompartment(sbmlModel.getCompartment(i)));
         }
         CreateSpatialStructureFromModel(_topContainer, sbmlModel.getModel());
         AddToProject();
      }

      /// <summary>
      ///     Creates the Events Topcontainer that is necessary for connecting
      ///     the Events to the Project.
      /// </summary>
      internal void CreateEventsTopContainer()
      {
         _eventsTopContainer = _objectBaseFactory.Create<IContainer>()
            .WithName(SBMLConstants.SBML_EVENTS_TOP_CONTAINER)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Compartment);
      }

      /// <summary>
      ///     Creates the TopContainer of the Spatial Structure.
      /// </summary>
      internal void CreateTopContainer(Model model)
      {
         _topContainer = _objectBaseFactory.Create<IContainer>()
            .WithId(SBMLConstants.SBML_TOP_CONTAINER)
            .WithName("TOPCONTAINER" + SBMLConstants.SBML + model.getName())
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Compartment);
      }

      /// <summary>
      ///     Creates the Spatial Structure of the SBML Model with the TopContainer.
      /// </summary>
      internal void CreateSpatialStructureFromModel(IContainer topContainer, Model model)
      {
         SpatialStructure = _spatialStructureFactory.Create().DowncastTo<IMoBiSpatialStructure>()
            .WithName(SBMLConstants.SBML_MODEL + model.getName())
            .WithTopContainer(topContainer)
            .WithDescription(SBMLConstants.SBML_NOTES + model.getNotesString() + SBMLConstants.SPACE +
                             SBMLConstants.SBML_METAID + model.getMetaId());

         SpatialStructure.AddTopContainer(_eventsTopContainer);
      }

      /// <summary>
      ///     Creates a MoBi Container for a SBML compartment.
      /// </summary>
      public IContainer CreateContainerFromCompartment(Compartment compartment)
      {
         var container = _objectBaseFactory.Create<IContainer>()
            .WithName(compartment.getId())
            .WithContainerType(ContainerType.Compartment)
            .WithMode(ContainerMode.Physical)
            .WithDescription(SBMLConstants.SBML_NOTES + compartment.getNotesString());

         CreateMoleculeProperties(container);
         container.Add(CreateVolumeParameter(compartment));

         if (!compartment.isSetSize()) return container;
         var sizeParameter = CreateSizeParameter(compartment);
         container.Add(sizeParameter);

         return container;
      }

      private void CreateMoleculeProperties(IContainer container)
      {
         var moleculePropertiesContainer = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithContainerType(ContainerType.Compartment)
            .WithMode(ContainerMode.Logical);
         container.Add(moleculePropertiesContainer);
      }

      /// <summary>
      ///     Creates a Volume Parameter for a MoBi Container. 
      /// </summary>
      private IEntity CreateVolumeParameter(Compartment compartment)
      {
         var volume = 1.0;
         if (compartment.isSetVolume())
            volume = compartment.getVolume();
         else if (compartment.isSetSize())
            volume = compartment.getSize();
         var dimension = _dimensionFactory.Dimension(Constants.Dimension.VOLUME);
         IFormula formula = _formulaFactory.ConstantFormula(volume, dimension);

         var volumeParameter = _objectBaseFactory.Create<IParameter>()
            .WithName(SBMLConstants.VOLUME)
            .WithDimension(dimension)
            .WithFormula(formula);

         return volumeParameter;
      }

      /// <summary>
      ///     Creates a container's size parameter to import the size of the particular SBML compartment.
      /// </summary>
      private IParameter CreateSizeParameter(Compartment compartment)
      {
         IFormula formula = ObjectBaseFactory.Create<ConstantFormula>().WithValue(compartment.getSize());

         var sizeParameter = _objectBaseFactory.Create<IParameter>()
            .WithName(SBMLConstants.SIZE)
            .WithDescription(SBMLConstants.SBML_SIZE_DESCRIPTION)
            .WithFormula(formula);

         return SetDimension(compartment, sizeParameter);
      }

      /// <summary>
      ///     Sets the dimension of a MoBi container to the Unit of the SBML compartment.
      /// </summary>
      private IParameter SetDimension(Compartment compartment, IParameter sizeParameter)
      {
         var spatialDimensions = compartment.getSpatialDimensions();

         IDimension sizeDimension;
         switch (spatialDimensions)
         {
            case (SBMLConstants.SBML_CONTAINER_3D):
               sizeDimension = !compartment.isSetUnits()
                  ? _dimensionFactory.Dimension(Constants.Dimension.VOLUME)
                  : GetDimensionFromSBMLUnit(compartment.getUnits());
               break;
            case (SBMLConstants.SBML_CONTAINER_2D):
               sizeDimension = !compartment.isSetUnits()
                  ? _dimensionFactory.Dimension(SBMLConstants.AREA)
                  : GetDimensionFromSBMLUnit(compartment.getUnits());
               break;
            case (SBMLConstants.SBML_CONTAINER_1D):
               sizeDimension = !compartment.isSetUnits()
                  ? _dimensionFactory.Dimension(SBMLConstants.LENGTH)
                  : GetDimensionFromSBMLUnit(compartment.getUnits());
               break;
            case (SBMLConstants.SBML_CONTAINER_NO): 
               //no size & units allowed
               return null;
            default:
               sizeDimension = _dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS);
               break;
         }
         if (sizeDimension != null)
            sizeParameter.Dimension = sizeDimension;

         return sizeParameter;
      }

      /// <summary>
      ///     Gets the MoBi dimension from a SBML unit. 
      /// </summary>
      private IDimension GetDimensionFromSBMLUnit(string unit)
      {
         if (
            _sbmlInformation.MobiDimension.ContainsKey(unit))
            return _sbmlInformation.MobiDimension[unit];
         return null;
      }

      /// <summary>
      ///     Adds the main Spatial Structure for the SBML Import to the MoBi Project.
      /// </summary>
      public override void AddToProject()
      {
         _command.AddCommand(new AddBuildingBlockCommand<IMoBiSpatialStructure>(SpatialStructure).Run(_context));
      }
   }
}