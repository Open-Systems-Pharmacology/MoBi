using System.Linq;
using System.Xml.Linq;
using MoBi.Assets;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Converter.v5_5;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Assets;

namespace MoBi.Core.Serialization.Converter.v3_5
{
   public class Converter341To351 : IMoBiObjectConverter,
      IVisitor<DataRepository>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<IMoleculeBuildingBlock>,
      IVisitor<ISpatialStructure>,
      IVisitor<IBuildConfiguration>,
      IVisitor<IPassiveTransportBuildingBlock>

   {
      private readonly Converter541To551 _coreConverter;
      private readonly ICloneManagerForSimulation _cloneManager;
      private readonly IParameterFactory _parameterFactory;
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IDistributionFormulaFactory _distributionFormulaFactory;
      private readonly IMoBiFormulaTask _formulaTask;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private IMoBiProject _project;

      //this fields indicate if a MoBi simulation is being loaded (typically from project) or if a pkml simulation is being loaded. 
      //in the latter, we do not add the template simulation settings to the project
      private bool _loadingMoBiSimulation;

      private const string EffectiveSurfaceAreaVariabilityFactor = "Effective surface area variability factor";
      private const string EffectiveSurfaceAreaEnhancementFactor = "Effective surface area enhancement factor";
      private const string EffectiveSurfaceArea = "Effective surface area";
      private const string GeometricSurfaceArea = "Geometric surface area";

      public Converter341To351(Converter541To551 coreConverter, ICloneManagerForSimulation cloneManager,
         IParameterFactory parameterFactory, ISimulationSettingsFactory simulationSettingsFactory,
         IObjectBaseFactory objectBaseFactory, IDistributionFormulaFactory distributionFormulaFactory,
         IMoBiFormulaTask formulaTask, IDimensionFactory dimensionFactory, IObjectPathFactory objectPathFactory)
      {
         _coreConverter = coreConverter;
         _cloneManager = cloneManager;
         _parameterFactory = parameterFactory;
         _simulationSettingsFactory = simulationSettingsFactory;
         _objectBaseFactory = objectBaseFactory;
         _distributionFormulaFactory = distributionFormulaFactory;
         _formulaTask = formulaTask;
         _dimensionFactory = dimensionFactory;
         _objectPathFactory = objectPathFactory;
      }

      public  bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V3_4_1;
      }

      public  int Convert(object objectToUpdate, IMoBiProject project)
      {
         try
         {
            _project = project;
            _coreConverter.Convert(objectToUpdate);
            this.Visit(objectToUpdate);
            return ProjectVersions.V3_5_1;
         }
         finally
         {
            _project = null;
         }
      }

      public  int ConvertXml(XElement element, IMoBiProject project)
      {
         _coreConverter.ConvertXml(element);

         element.DescendantsAndSelfNamed(AppConstants.XmlNames.BuildConfiguration).Each(convertBuildConfigurationElement);
         element.DescendantsAndSelfNamed("Simulation", AppConstants.XmlNames.MoBiSimulation).Each(convertMoBiSimulationElement);

         //only happens when loading an actual MoBiSimulation from a project
         _loadingMoBiSimulation = (element.Name == AppConstants.XmlNames.MoBiSimulation);

         return ProjectVersions.V3_5_1;
      }

      private void convertBuildConfigurationElement(XElement buildConfigurationElement)
      {
         if (buildConfigurationElement == null) return;
         //create an empty element. This is enough to instantiate the object 
         buildConfigurationElement.Add(new XElement(AppConstants.XmlNames.SimulationSettingsInfo));
      }

      private void convertMoBiSimulationElement(XElement element)
      {
         const string oldParameterIdetificationWorkingDirectory = "parameterIdetificationWorkingDirectory";
         const string newParameterIdentificationWorkingDirectory = "parameterIdentificationWorkingDirectory";

         _coreConverter.ConvertSimuationElement(element);

         var buildConfigurationElement = element.Element(AppConstants.XmlNames.BuildConfiguration);
         if (buildConfigurationElement == null) return;

         const string originalBuildingBlockId = "originalBuildingBlockId";
         var query = buildConfigurationElement.Descendants()
            .Where(x => x.Attribute(originalBuildingBlockId) != null)
            .Select(x => new {Element = x, Attr = x.Attribute(originalBuildingBlockId)});

         foreach (var buildingBlockInfoElement in query.ToList())
         {
            buildingBlockInfoElement.Element.AddAttribute("templateBuildingBlockId", buildingBlockInfoElement.Attr.Value);
            buildingBlockInfoElement.Attr.Remove();
         }


         var oldAttribute = element.Attribute(oldParameterIdetificationWorkingDirectory);
         if (oldAttribute == null)
            return;

         element.AddAttribute(newParameterIdentificationWorkingDirectory, oldAttribute.Value);
         oldAttribute.Remove();
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         var mobiSimulation = simulation as IMoBiSimulation;
         if (mobiSimulation == null) return;

         //historical results will be deserialized in a separate call
         Visit(mobiSimulation.Results);
         Visit(mobiSimulation.BuildConfiguration);
         addSimulationSettingsTemplateToProject(mobiSimulation);
         updateGlobalReactionContainerIcon(simulation);
      }

      private void updateGlobalReactionContainerIcon(IModelCoreSimulation simulation)
      {
         simulation.Model.Root.GetChildren<IContainer>(c => c.ContainerType == ContainerType.Reaction)
            .Each(c => c.Icon = IconNames.REACTION);
      }

      private void addSimulationSettingsTemplateToProject(IMoBiSimulation mobiSimulation)
      {
         if (!_loadingMoBiSimulation) return;
         var simulationSettingsInfo = mobiSimulation.MoBiBuildConfiguration.SimulationSettingsInfo;
         var templateSimulationSettings = _cloneManager.CloneBuidingBlock(simulationSettingsInfo.BuildingBlock);
         simulationSettingsInfo.TemplateBuildingBlock = templateSimulationSettings;
         _project.AddBuildingBlock(templateSimulationSettings);
      }

      public void Visit(DataRepository dataRepository)
      {
         _coreConverter.UpdateQuantityTypeForDataRepository(dataRepository);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         foreach (var moleculeBuilder in moleculeBuildingBlock.Where(x => !x.ContainsName(Constants.Parameters.CONCENTRATION)).ToList())
         {
            var concentrationParameter = _parameterFactory.CreateConcentrationParameter(moleculeBuildingBlock.FormulaCache);
            moleculeBuilder.AddParameter(concentrationParameter);
         }

         convertFormulaReferencingAeffIn(moleculeBuildingBlock);
      }

      public void Visit(ISpatialStructure spatialStructure)
      {
         foreach (var container in spatialStructure.PhysicalContainers.Where(x => !x.ContainsName(Constants.Parameters.VOLUME)).ToList())
         {
            container.Add(_parameterFactory.CreateVolumeParameter());
         }
         addVariabilityParameterIn(spatialStructure);
         addEffectiveSurfaceAreaParameter(spatialStructure);
         convertFormulaReferencingAeffIn(spatialStructure);
      }


      private static void replaceAgeomXAeffFactorWithAeffIn(ExplicitFormula formula)
      {
         if (!formula.FormulaString.Contains("Ageom * AeffFactor") && !formula.FormulaString.Contains("Ageom*AeffFactor")) return;

         if (formula.IsNamed("PARAM_EffectiveSurfaceArea")) return;

         formula.FormulaString = formula.FormulaString.Replace("Ageom * AeffFactor", "Aeff").Replace("Ageom*AeffFactor", "Aeff");
         var objectPath = formula.ObjectPaths.Find(x => x.Alias == "Ageom");
         objectPath.Remove(GeometricSurfaceArea);
         objectPath.Add(EffectiveSurfaceArea);
         objectPath.Alias = "Aeff";

         var aeffFactorObjectPath = formula.ObjectPaths.Find(x => x.Alias == "AeffFactor");
         formula.RemoveObjectPath(aeffFactorObjectPath);
      }

      public void Visit(IBuildConfiguration buildConfiguration)
      {
         Visit(buildConfiguration.Molecules);
         Visit(buildConfiguration.SpatialStructure);
         Visit(buildConfiguration.PassiveTransports);

         if (buildConfiguration.SimulationSettings == null)
            buildConfiguration.SimulationSettings = _simulationSettingsFactory.CreateDefault();
      }

      private void addEffectiveSurfaceAreaParameter(ISpatialStructure spatialStructure)
      {
         var organism = getOrganism(spatialStructure);
         if (organism == null) return;
         var lumen = organism.EntityAt<IContainer>("Lumen");
         if (lumen == null) return;

         var variabilityFactor = lumen.EntityAt<IParameter>(EffectiveSurfaceAreaVariabilityFactor);
         var area = _dimensionFactory.Dimension("Area");
         foreach (var containerWithSurfaceArea in organism.GetAllChildren<IContainer>(c => c.ContainsName(EffectiveSurfaceAreaEnhancementFactor)))
         {
            var effectiveSurfaceArea = _objectBaseFactory.Create<IParameter>()
               .WithName(EffectiveSurfaceArea)
               .WithDimension(area)
               .WithGroup("GI_ANATOMY_AREA")
               .WithDescription("Effective surface area of GI segment");

            var geometricSurfaceArea = containerWithSurfaceArea.EntityAt<IParameter>(GeometricSurfaceArea);
            if (geometricSurfaceArea == null)
               continue;

            var effectiveSurfaceAreaEnahncementFactor = containerWithSurfaceArea.EntityAt<IParameter>(EffectiveSurfaceAreaEnhancementFactor);

            containerWithSurfaceArea.Add(effectiveSurfaceArea);

            var formula = spatialStructure.FormulaCache.FindByName("PARAM_EffectiveSurfaceArea") as ExplicitFormula;
            if (formula == null)
            {
               formula = _formulaTask.CreateNewFormula<ExplicitFormula>(area).WithName("PARAM_EffectiveSurfaceArea");
               formula.AddObjectPath(_objectPathFactory.CreateRelativeFormulaUsablePath(effectiveSurfaceArea, variabilityFactor).WithAlias("AeffVariabilityFactor"));
               formula.AddObjectPath(_objectPathFactory.CreateRelativeFormulaUsablePath(effectiveSurfaceArea, geometricSurfaceArea).WithAlias("Ageom"));
               formula.AddObjectPath(_objectPathFactory.CreateRelativeFormulaUsablePath(effectiveSurfaceArea, effectiveSurfaceAreaEnahncementFactor).WithAlias("AeffFactor"));
               formula.FormulaString = "Ageom * AeffFactor * AeffVariabilityFactor";
               spatialStructure.AddFormula(formula);
            }

            effectiveSurfaceArea.Formula = formula;
         }
      }

      private void addVariabilityParameterIn(ISpatialStructure spatialStructure)
      {
         var organism = getOrganism(spatialStructure);
         if (organism == null) return;

         var lumen = organism.EntityAt<IContainer>("Lumen");
         if (lumen == null) return;

         var parameter = _objectBaseFactory.Create<IDistributedParameter>()
            .WithName(EffectiveSurfaceAreaVariabilityFactor)
            .WithDescription("Effective surface area variability factor");

         parameter.WithGroup("GI_ANATOMY_AEFF_FACTOR");

         var meanParameter = effectiveSurfaceAreaMeanParameter();
         var deviationParameter = effectiveSurfaceAreaDeviationParameter();
         var percentileParameter = effectiveSurfaceAreaPercentileParameter();
         parameter.Add(meanParameter);
         parameter.Add(deviationParameter);
         parameter.Add(percentileParameter);
         parameter.Formula = _distributionFormulaFactory.CreateLogNormalDistributionFormulaFor(parameter, meanParameter, deviationParameter);

         lumen.Add(parameter);
      }

      private static IContainer getOrganism(ISpatialStructure spatialStructure)
      {
         return spatialStructure.TopContainers.FindByName(Constants.ORGANISM);
      }

      private IParameter effectiveSurfaceAreaDeviationParameter()
      {
         return distributionParameter(Constants.Distribution.GEOMETRIC_DEVIATION, 1.6);
      }

      private IParameter effectiveSurfaceAreaMeanParameter()
      {
         return distributionParameter(Constants.Distribution.MEAN, 1);
      }

      private IParameter effectiveSurfaceAreaPercentileParameter()
      {
         return distributionParameter(Constants.Distribution.PERCENTILE, 0.5);
      }

      private IParameter distributionParameter(string name, double value)
      {
         var dimension = _dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS);
         var meanFormula = _formulaTask.CreateNewFormula<ConstantFormula>(dimension).WithValue(value);
         return _objectBaseFactory.Create<IParameter>()
            .WithName(name)
            .WithFormula(meanFormula);
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuilding)
      {
         convertFormulaReferencingAeffIn(passiveTransportBuilding);
      }

      private void convertFormulaReferencingAeffIn(IBuildingBlock buildingBlock)
      {
         foreach (var explicitFormula in buildingBlock.FormulaCache.OfType<ExplicitFormula>())
         {
            replaceAgeomXAeffFactorWithAeffIn(explicitFormula);
         }
      }
   }
}