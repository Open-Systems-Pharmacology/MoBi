using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace MoBi.Assets
{
   public static class AppConstants
   {
      public static readonly int LayoutVersion = 24;
      public static readonly string NotMatch = "not tagged with";
      public static readonly string Match = "tagged with";
      public static readonly string MatchAll = "in all containers";
      public static readonly string NullFormulaDescription = "No Formula";
      public static readonly string DefaultSkin = "Office 2013 Light Gray";
      public static readonly string NewFormulaDescription = "Create New Formula";
      public static readonly string NewFormulaName = "New Formula";
      public static readonly string SimulationRun = "Run Simulation";
      public static readonly string ParameterType = "Type";
      public static readonly string NaN = "<NaN>";
      public static readonly string Project = "Project";
      public static readonly string Time = "TIME";
      public static readonly double DEFAULT_PARAMETER_START_VALUE = 0.0;
      public static readonly double DEFAULT_PERCENTILE = 0.5;
      public const int DEFAULT_NUMBER_OF_BINS = 20;
      public const int DEFAULT_NUMBER_OF_INDIVIDUALS_PER_BIN = 100;
      public static readonly int NotificationToolTipDelay = 30000;
      public static readonly int NotFoundIndex = -1;
      public static readonly string NullString = "null";
      public static readonly string PRODUCT_NAME = "MoBi";
      public static readonly string Website = "www.open-systems-pharmacology.org";
      public static readonly string ProductSiteDownload = "http://setup.open-systems-pharmacology.org";
      public static readonly string IssueTrackerUrl = "http://www.open-systems-pharmacology.org/mobi/issues";
      public static readonly string Persitable = "Persitable";
      public static readonly string CVODE_1002_2_SOLVER = "CVODE1002_2";
      public static readonly int MAX_PATH_DEPTH = 10;
      public static readonly string CONCENTRATION_FORMULA = "ConcFormula";

      public static class Parameters
      {
         public static readonly string CONCENTRATION = "Concentration";
         public static readonly string MOLECULAR_WEIGHT = "Molecular weight";
         public static readonly string EFFECTIVE_MOLECULAR_WEIGHT = "Effective molecular weight";
         public static readonly string SURFACE_AREA_INTERSTITIAL_INTRACELLULAR = "Surface area (interstitial/intracellular)";
         public static readonly string BSA = "BSA";
         public static readonly string PERMEABILITY = "Permeability";
         public static readonly string SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR = "Specific intestinal permeability (transcellular)";
         public static readonly string RADIUS_SOLUTE = "Radius (solute)";
         public static readonly string SECRETION_OF_LIQUID = "Secretion of liquid";
         public static readonly string RELEASE_RATE_OF_TABLET = "Release rate of the tablet";
         public static readonly string V_MAX = "Vmax";
         public static readonly string LYMPH_FLOW_RATE = "Lymph flow rate";
         public static readonly string LYMPH_FLOW_RATE_INCL_MUCOSA = "Lymph flow rate (incl. mucosa)";
         public static readonly string FLUID_RECIRCULATION_FLOW_RATE = "Fluid recirculation flow rate";
         public static readonly string FLUID_RECIRCULATION_FLOW_RATE_INCL_MUCOSA = "Fluid recirculation flow rate (incl. mucosa)";
         public static readonly string CALCULATED_SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR= "Calculated specific intestinal permeability (transcellular)";
      }

      public static class Groups
      {
         public static readonly string EmptyColumn = " ";

         public static readonly string ReportCreationStarted = "Report creation started...";
         public static readonly string ReportCreationFinished = "Report created!";
         public static readonly string Concentration = "Concentration";
      }

      public static class Organs
      {
         public static readonly string ENDOGENOUS_IGG = "EndogenousIgG";

         public static readonly IReadOnlyList<string> DefaultIsPresentShouldBeFalse = new[]
         {
            ENDOGENOUS_IGG
         };
      }

      public static class DefaultNames
      {
         public static readonly string MoleculeBuildingBlock = "Molecules";
         public static readonly string ReactionBuildingBlock = "Reaction";
         public static readonly string SpatialStructure = "Organism";
         public static readonly string PassiveTransportBuildingBlock = "Passive Transports";
         public static readonly string EventBuildingBlock = "Events";
         public static readonly string ObserverBuildingBlock = "Observer";
         public static readonly string SimulationSettings = "Simulation Settings";
         public static readonly string EmptyCalculationMethod = "No Calculation Method";
         public static string EmptyCalculationMethodDescription = "";

         public static readonly string GlobalEventTag = "Events";

         public static readonly IReadOnlyList<string> PKSimStaticObservers = new []
         {
            "Plasma (Peripheral Venous Blood)",
            "Plasma Unbound (Peripheral Venous Blood)",
            "Tissue",
            "Fraction excreted"
         };

         public static readonly IReadOnlyList<string> PKSimDynamicObservers = new[]
         {
            "Fraction of dose"
         };
      }

      public static class SpecialFileNames
      {
         public static readonly string COMPANY_FOLDER_NAME = "Open Systems Pharmacology";
         public static readonly string APPLICATION_FOLDER_PATH = Path.Combine(COMPANY_FOLDER_NAME, PRODUCT_NAME);
         public static readonly string TEMPLATES_FOLDER = "Templates";
         public static readonly string LOCAL_TEX_TEMPLATE_FOLDER_NAME = "Templates";
         public static readonly string GROUP_REPOSITORY_FILE_NAME = "GroupRepository.xml";
         public static readonly string CALCULATION_METHOD_REPOSITORY_FILE_NAME = "AllCalculationMethods.pkml";
         public static readonly string SPATIAL_STRUCTURE_TEMPLATE = "SpaceOrganismTemplate.mbdt";
         public static readonly string STANDARD_MOLECULE = "Standard Molecule.pkml";
      }

      public static class XmlNames
      {
         public static readonly string Molecules = "Molecules";
         public static readonly string MoleculeBuildingBlock = "MoleculeBuildingBlock";
         public static readonly string ObserverBuildingBlock = "ObserverBuildingBlock";
         public static readonly string EventGroupBuildingBlock = "EventGroupBuildingBlock";
         public static readonly string MoleculeStartValuesBuildingBlock = "MoleculeStartValuesBuildingBlock";
         public static readonly string ParameterStartValuesBuildingBlock = "ParameterStartValuesBuildingBlock";
         public static readonly string PassiveTransportBuildingBlock = "PassiveTransportBuildingBlock";
         public static readonly string MoBiReactionBuildingBlock = "MoBiReactionBuildingBlock";
         public static readonly string ReactionBuildingBlock = "ReactionBuildingBlock";
         public static readonly string MoBiSpatialStructure = "MoBiSpatialStructure";
         public static readonly string ModelCoreSimulation = "ModelCoreSimulation";
         public static readonly string Observers = "Observers";
         public static readonly string EventGroups = "EventGroups";
         public static readonly string PassiveTransports = "PassiveTransports";
         public static readonly string Reactions = "Reactions";
         public static readonly string SpatialStructure = "SpatialStructure";
         public static readonly string ParameterStartValues = "ParameterStartValues";
         public static readonly string MoleculeStartValues = "MoleculeStartValues";
         public static readonly string Model = "Model";
         public static readonly string MoBiSimulation = "MoBiSimulation";
         public static readonly string NameAttribute = "name";
         public static readonly string BuildConfiguration = "BuildConfiguration";
         public static readonly string CVODESettings = "CVODESettings";
         public static readonly string Solver = "Solver";
         public static readonly string MoleculeList = "MoleculeList";
         public static readonly string SimulationSettingsInfo = "SimulationSettingsInfo";
         public static readonly string CurveChart = "CurveChart";
         public static readonly string Chart = "Chart";
      }

      public static class RibbonPages
      {
         public static readonly string File = "File";
         public static readonly string Modeling = "Modeling";
         public static readonly string WorkingJournal = "Working Journal";
         public static readonly string ImportExport = "Import/Export";
         public static readonly string Utilities = "Utilities";
         public static readonly string DynamicMolecules = "Edit Molecule";
         public static readonly string DynamicReactions = "Edit Reaction";
         public static readonly string DynamicOrganisms = "Edit Organism";
         public static readonly string DynamicPassiveTransports = "Edit Passive Transport";
         public static readonly string DynamicObservers = "Edit Observer";
         public static readonly string DynamicEvents = "Edit Event";
         public static readonly string DynamicMoleculeStartValues = "Edit Molecule Start Values";
         public static readonly string DynamicParameterStartValues = "Edit Parameter Start Values";
         public static readonly string DynamicRunSimulation = "Run & Analyze";
         public static readonly string Views = "Views";
         public static readonly string ParameterIdentification = "Parameter Identification";
      }

      public static class RibbonCategories
      {
         public static readonly string Molecules = "Molecules";
         public static readonly string Reactions = "Reactions";
         public static readonly string Organisms = "Organisms";
         public static readonly string PassiveTransports = "Passive Transports";
         public static readonly string Observers = "Observers";
         public static readonly string Events = "Events";
         public static readonly string MoleculesStartValues = "Molecule Start Values";
         public static readonly string ParameterStartValues = "Parameter Start Values";
         public static readonly string Simulation = "Simulation";
         public static readonly string Skins = "Skins";

         public static IEnumerable<string> AllDynamicCategories()
         {
            return new List<string>
            {
               Molecules,
               Reactions,
               Organisms,
               PassiveTransports,
               Observers,
               Events,
               MoleculesStartValues,
               ParameterStartValues,
               Simulation,
               OSPSuite.Assets.RibbonCategories.ParameterIdentification,
               OSPSuite.Assets.RibbonCategories.SensitivityAnalysis
            };
         }
      }

      public static class Commands
      {
         public static readonly string AddCommand = Command.CommandTypeAdd;
         public static readonly string DeleteCommand = Command.CommandTypeDelete;
         public static readonly string CreateCommand = Command.CommandTypeCreate;
         public static readonly string EditCommand = Command.CommandTypeEdit;
         public static readonly string RenameCommand = Command.CommandTypeRename;
         public static readonly string SetToDefaultDefaultDescription = "Sets Values to Defaults defined in Molecules and Spatial Structure";
         public static readonly string SetToDefaultDefaultType = "Set To Default";
         public static readonly string ExtendDescription = "Adding new start values from Molecules and Spatial Structure";
         public static readonly string AddManyMoleculesDescription = "Add some Molecules to Reaction Diagram";
         public static readonly string MergeCommand = "Merge";
         public static readonly string AddParameterIdentificationResults = "Add parameter identification results to start values collection";
         public static readonly string ImportParameterStartValues = "Parameter Start Values were imported";
         public static readonly string ImportMoleculeStartValues = "Molecule Start Values were imported";
         public static readonly string RemoveManyParameterStartValues = "Remove some Parameter Start Values";
         public static readonly string RemoveManyMoleculeStartValues = "Remove some Molecule Start Values";
         public static readonly string CommitCommand = "Commit";
         public static readonly string UpdateManyParameterStartValues = "Update some Parameter Start Values";
         public static readonly string UpdateManyMoleculeStartValues = "Update some Molecule Start Values";

         public static readonly string UpdateCommand = "Update";
         public static readonly string ImportCommand = "Import";
         public static readonly string MergeBuildingBlocks = "Merging two building blocks";
         public static readonly string MatchAllCondition = "Match All Condition";
         public static readonly string MatchTagCondition = "Match Tag Condition";
         public static readonly string NotMatchTagCondition = "Not Match Tag Condition";
         public static readonly string Name = "Name";
         public static readonly string UpdateDimensionsAndUnits = "Changing dimensions and units";
         public static readonly string RefreshStartValuesFromBuildingBlocks = "Refreshing start values from original building blocks";
         public static readonly string SetStartValueAndFormula = "Set start value and formula";
         public static readonly string AddFormulaToBuildingBlock = "Add formula to building block";
         public static readonly string ExtendCommand = "Extend";
         public static readonly string ImportMultipleParameters = "Importing multiple parameter values";
         public static readonly string RenamingEntities = "Renaming entities";
         public static readonly string DeleteAllResultsFromAllSimulations = "Delete all results from all simulations";
         public static readonly string RemoveSimulationsFromProject = "Remove simulations from project";
         public static readonly string RemoveMultipleResultsFromSimulations = "Remove multiple results from simulations";
         public static readonly string RemoveMultipleStartValues = "Remove multiple start values";

         public static string DeleteResultsFromSimulation(string simulationName)
         {
            return $"Delete all results from simulation '{simulationName}'";
         }

         public static string DisplayValue(double displayValue, string displayUnit)
         {
            return DisplayValue(displayValue.ConvertedTo<string>(), displayUnit);
         }

         public static string DisplayValue(string displayValue, string displayUnit)
         {
            if (string.IsNullOrEmpty(displayUnit))
               return displayValue;

            return $"{displayValue} {displayUnit}";
         }

         public static string AddDependentDescription(IWithName addTo, string addEntitiesType, string addToType)
         {
            return $"Adding dependent {addEntitiesType}s to {addToType} '{addTo.Name}'";
         }

         public static string SimulationLabelComment(int warningsCount)
         {
            return $"Solver reports {warningsCount} Warnings";
         }

         public static string AddToProjectDescription(string objectType, string objectName)
         {
            return AddToDescription(objectType, objectName, Project);
         }

         public static string RemoveFromProjectDescription(string objectType, string objectName)
         {
            return RemoveFromDescription(objectType, objectName, Project);
         }

         public static string SwapBuildingCommandDescription(string buildingBlockType, string buildingBlockName)
         {
            return $"Swap {buildingBlockType} '{buildingBlockName}'";
         }

         public static string UpdateTemplateBuildingCommandDescription(string buildingBlockType, string buildingBlockName)
         {
            return $"Update template {buildingBlockType} '{buildingBlockName}'";
         }

         public static string AddMoleculeToListDescription(string moleculeName, string parentType, string parentPath, string listType)
         {
            return $"Add molecule '{moleculeName}' to the {listType.ToLowerInvariant()} of {parentType} '{parentPath}'";
         }

         public static string RemoveMoleculeFromListDescription(string moleculeName, string parentType, string parentPath, string listType)
         {
            return $"Remove molecule '{moleculeName}' from the {listType.ToLowerInvariant()} of {parentType} '{parentPath}'";
         }

         public static string AddToDescription(string objectType, string objectName, string parentName)
         {
            return $"Add {objectType} '{objectName}' to '{parentName}'";
         }

         public static string AddToDescription(string objectType, string name, string parentName, string buildingBlockType, string buildingBlockName)
         {
            return $"Add {objectType} '{name}' to '{parentName}' in {buildingBlockType} building block '{buildingBlockName}'";
         }

         public static string RemoveFromDescription(string objectType, string name, string parentName)
         {
            return $"Remove {objectType} '{name}' from '{parentName}'.";
         }

         public static string RemoveFromDescription(string objectType, string name, string parentName, string buildingBlockType, string buildingBlockName)
         {
            return $"Remove {objectType} '{name}' from '{parentName}' in {buildingBlockType} '{buildingBlockName}'";
         }

         public static string CreateDescription(string objectType)
         {
            return $"Create new {objectType} at Project";
         }

         public static string EditStochiometricCoefficient(double newValue, double oldValue, string reactionName, string moleculeName, string partnerType)
         {
            return $"Update stochiometric coefficient of {partnerType.ToLowerInvariant()} '{moleculeName}' from '{oldValue}' to '{newValue}' for {ObjectTypes.Reaction.ToLowerInvariant()} '{reactionName}'";
         }

         public static string EditDescription(string objectType, string propertyName, string oldValueAsString, string newValueAsString, string name)
         {
            if (string.IsNullOrEmpty(oldValueAsString))
               return SetDescription(objectType, propertyName, newValueAsString, name);

            return $"Update {propertyName.ToLowerInvariant()} from '{oldValueAsString}' to '{newValueAsString}' for {objectType} '{name}'";
         }

         public static string EditTagDescription(string objectType, string oldTag, string newTag, string name)
         {
            return EditDescription(objectType, "tag", oldTag, newTag, name);
         }

         public static string SetDescription(string objectType, string propertyName, string newValue, string name)
         {
            return $"{propertyName.ToLowerInvariant()} set to '{newValue}' for {objectType} '{name}'";
         }

         public static string SetQuantityValueInSimulation(string quantityType, double newDisplayValue, string newDisplayUnit, double oldDisplayValue, string oldDisplayUnit, string name, string simulationName)
         {
            return setQuantityValueIn(quantityType, newDisplayValue, newDisplayUnit, oldDisplayValue, oldDisplayUnit, name, simulationName, "simulation");
         }

         public static string SetQuantityValueInBuildingBlock(string quantityType, double newDisplayValue, string newDisplayUnit, double oldDisplayValue, string oldDisplayUnit, string name, string buildingBlockName)
         {
            return setQuantityValueIn(quantityType, newDisplayValue, newDisplayUnit, oldDisplayValue, oldDisplayUnit, name, buildingBlockName, "building block");
         }

         private static string setQuantityValueIn(string quantityType, double newDisplayValue, string newDisplayUnit, double oldDisplayValue, string oldDisplayUnit, string name, string blockName, string blockType)
         {
            return string.Format("Value of {0} '{1}' set from '{2}' to '{3}' in {5} '{4}'.", quantityType, name, DisplayValue(oldDisplayValue, oldDisplayUnit), DisplayValue(newDisplayValue, newDisplayUnit), blockName, blockType);
         }

         public static string RenameDescription(IWithName objectBase, string newName)
         {
            return $"Rename '{objectBase.Name}' to '{newName}'";
         }

         public static string AddFromMoBi20(string fileName)
         {
            return $"Adding BuildingBlocks from MoBi 2 Project '{fileName}'";
         }

         public static string CreateFromSelectionDescription(string name, IEnumerable<string> selectionNames)
         {
            var displayNames = selectionNames.Select(s => $"{s}, ");

            var display = displayNames.Aggregate(string.Empty, string.Concat);
            if (display.Any())
            {
               display = display.Remove(display.Count() - 2);
            }
            return $"Create new {ObjectTypes.MoleculeBuildingBlock}: '{name}' from {display}";
         }

         public static string ChangePathElementDescription(string formulaAlias, string newElement, string oldElement, string formulaName)
         {
            return $"Replace '{oldElement}' with '{newElement}' in path with alias '{formulaAlias}' of formula '{formulaName}'";
         }

         public static string ChangeContainerPathElementDescription(string name, string newElement, string oldElement)
         {
            return $"Replace '{oldElement}' with '{newElement}' in container path of application molecule builder:'{name}'";
         }

         public static string AddMany(string objectName)
         {
            return $"Add many {objectName}s";
         }

         public static string AddToConditionDescription(string objectType, string newTag, string parentName)
         {
            return $"Add {objectType}: '{newTag}' to '{parentName}' Criteria";
         }

         public static string RemoveTagFromConditionDescription(string objectType, string tag, string parentName)
         {
            return $"Remove {objectType}: '{tag}' from '{parentName}' Criteria";
         }

         public static string SettingIsPresentCommandDescription(bool isPresent)
         {
            return $"Setting is present flag to '{isPresent}'";
         }

         public static string SettingNegativeValuesAllowedCommandDescription(bool negativeValuesAllowed)
         {
            return $"Setting negative values allowed to '{negativeValuesAllowed}'";
         }

         public static string EditIsAdvancedParameterCommandDescription(IParameter parameter, bool newValue)
         {
            return $"Making parameter '{parameter.Name}'{(newValue ? string.Empty : " non")} advanced parameter";
         }

         public static string SetUseDerivedValues(bool value, TableFormula tableFormula)
         {
            return $"Sets property 'Use Derived Values' to {value} at {Captions.TableFormula} '{tableFormula.Name}'";
         }

         public static string ResetValuesCommandDescription(string simulationName)
         {
            return $"Reseting all values in Simulation: '{simulationName}' to values from last build.";
         }

         public static string UnDoResetValuesCommandDescription(string simulationName)
         {
            return $"Undo reset of all values in Simulation: '{simulationName}' .";
         }

         public static string EditManyDescription(string objectType, string name, string childType, string propertyName)
         {
            return $"Change {propertyName} of some {childType}s in {objectType}: '{name}'";
         }

         public static string SetValuePointDescription(string propertyName, double newValueInDisplayUnit, string formulaName, Unit unit)
         {
            return string.Format("Sets property '{0}' = '{1} {5}' for {2} in {3}:'{4}' ", propertyName, newValueInDisplayUnit, ObjectTypes.ValuePoint, ObjectTypes.TableFormula, formulaName, unit.Name);
         }

         public static string EditValuePointDescription(string propertyName, string newValueAsString, string formulaName)
         {
            return $"Sets property '{propertyName}' = '{newValueAsString}' for {ObjectTypes.ValuePoint} in {ObjectTypes.TableFormula}:'{formulaName}' ";
         }

         public static string AddValuePointDescription(double xDisplayValue, double yDisplayValue, string formulaName, Unit xDisplayUnit, Unit yDisplayUnit)
         {
            return string.Format("Add {3} X: '{0} {5}' Y: '{1} {6}' to {4} '{2}'", xDisplayValue, yDisplayValue, formulaName, ObjectTypes.ValuePoint, ObjectTypes.TableFormula, xDisplayUnit.Name, yDisplayUnit.Name);
         }

         public static string RemoveValuePointDescription(double xDisplayValue, double yDisplayValue, string formulaName, Unit xDisplayUnit, Unit yDisplayUnit)
         {
            return string.Format("Remove {3} X: '{0} {5}' Y: '{1} {6}' from {4} '{2}'", xDisplayValue, yDisplayValue, formulaName, ObjectTypes.ValuePoint, ObjectTypes.TableFormula, xDisplayUnit.Name, yDisplayUnit.Name);
         }

         public static string CommitCommandDescription(IBuildingBlock buildingBlock, IModelCoreSimulation simulation, string buildingBlockType)
         {
            return string.Format("Commit changes made in Simulation: '{0}' to {2}: '{1}'", simulation.Name, buildingBlock.Name, buildingBlockType);
         }

         public static string UpdateCommandDescription(string buildingBlockName, IModelCoreSimulation simulation, string buildingBlockType)
         {
            return string.Format("Updates simulation: '{0}' with actual information from {2}: '{1}'", simulation.Name, buildingBlockName, buildingBlockType);
         }

         public static string EditDescriptionMoleculeList(string objectType, MoleculeList newMoleculeList, string name)
         {
            return $"Sets property '{ObjectTypes.MoleculeList}' for {objectType} '{name}'";
         }

         public static string EditSolverPropertyDescription(string propertyName, string newValue, string simulationSettings)
         {
            return $"Sets solver property '{propertyName}' = '{newValue}' for {ObjectTypes.SimulationSettings} '{simulationSettings}'";
         }

         public static string EditSolverPropertyInSimulationDescription(string propertyName, string newValue, string simulationName)
         {
            return $"Sets solver property '{propertyName}' = '{newValue}' for {ObjectTypes.Simulation} '{simulationName}'";
         }

         public static string UpdateOutputSelectionInBuildingBlockDescription(string buildingBlockName)
         {
            return string.Format("Output selection updated in {1} '{0}'", buildingBlockName, ObjectTypes.SimulationSettings.ToLowerInvariant());
         }

         public static string UpdateOutputSelectionInSimulationDescription(string simulationName)
         {
            return $"Output selection updated in simulation '{simulationName}'";
         }

         public static string AddChartTemplateToBuildingBlock(string chartTemplate, string buildingBlockName)
         {
            return $"Add chart template '{chartTemplate}' to building block '{buildingBlockName}'";
         }

         public static string AddChartTemplateToSimulation(string chartTemplate, string simulationName)
         {
            return $"Add chart template '{chartTemplate}' to simulation '{simulationName}'";
         }

         public static string UpdateChartTemplateInSimulation(string chartTemplate, string simulationName)
         {
            return $"Chart template '{chartTemplate}' updated in simulation '{simulationName}'";
         }

         public static string RemoveChartTemplateFromBuildingBlock(string chartTemplate, string buildingBlockName)
         {
            return $"Remove chart template '{chartTemplate}' from building block '{buildingBlockName}'";
         }

         public static string RemoveChartTemplateFromSimulation(string chartTemplate, string simulationName)
         {
            return $"Remove chart template '{chartTemplate}' from simulation '{simulationName}'";
         }

         public static string EditChartTemplateInBuildingBlock(string buildingBlockName)
         {
            return $"Chart Templates edited in building block '{buildingBlockName}'";
         }

         public static string EditChartTemplateInSimulation(string simulationName)
         {
            return $"Chart Templates edited in simulation '{simulationName}'";
         }

         public static string UpdateParameterStartValue(IObjectPath path, double? value, Unit displayUnit)
         {
            return $"Updated parameter start value at path {path} with value {value} {displayUnit}";
         }

         public static string UpdateMoleculeStartValue(IObjectPath path, double? value, bool present, Unit displayUnit, double scaleFactor, bool negativeValuesAllowed)
         {
            return $"Updated molecule start value at path: {path} with value: {value} {displayUnit},  present: {present},  scale divisor: {scaleFactor}, neg. values allowed: {negativeValuesAllowed}";
         }

         public static string AddedStartValue(IStartValue startValue, string buildingBlockName)
         {
            return $"Added a Start Value to building block '{buildingBlockName}' at path: {startValue.Path}, with value: {startValue.StartValue} {startValue.DisplayUnit}";
         }

         public static string RemoveOutputIntervalFrom(string objectName)
         {
            return $"Output interval was removed from '{objectName}'";
         }

         public static string AddOutputIntervalTo(string objectName)
         {
            return $"Output interval was added to '{objectName}'";
         }

         public static string RemoveStartValue(IStartValue startValue, string buildingBlockName)
         {
            return string.Format("Removed a Start Value from building block '{3}' at path {0}, with value: {1} {2}", startValue.Path, startValue.ConvertToDisplayUnit(startValue.StartValue), startValue.DisplayUnit, buildingBlockName);
         }

         public static string UpdateScaleDivisorValue(string name, double oldScaleDivisor, double newScaleDivisor)
         {
            return $"Scale divisor for molecule '{name}' set from '{oldScaleDivisor}' to '{newScaleDivisor}'";
         }

         public static string UpdateScaleDivisorValuesInSimulation(string name)
         {
            return $"Calculate scale divisor for simulation '{name}'";
         }

         public static string EditPath(string objectType, IObjectPath originalPath, IObjectPath newPath)
         {
            return $"Changing {objectType} original path {originalPath} to new path {newPath}";
         }

         public static string UpdateDimensions(string objectName, string objectType, IDimension oldDimension, IDimension newDimension, string buildingBlockName)
         {
            return string.Format("Changing dimension of {1} '{0}' from '{2}' to '{3}'", objectName, objectType.ToLowerInvariant(), oldDimension, newDimension, buildingBlockName);
         }

         public static string CreateFormula(string formulaName)
         {
            return $"Creating new formula named '{formulaName}'";
         }

         public static string SetConstantValueFormula(string objectType, ConstantFormula constantFormula, string newValueInDisplayUnits, string oldValueInDisplayUnits, string ownerIdentifier)
         {
            return string.Format("Value of {3} '{2}' set from '{1}' to '{0}'", newValueInDisplayUnits, oldValueInDisplayUnits, ownerIdentifier, objectType.ToLowerInvariant());
         }

         public static string UpdateFromParameterIdentification(IModelCoreSimulation simulation)
         {
            return $"Set values in simulation: '{simulation.Name}' from parameter identification results";
         }

         public static string SetParameterDimension(string parameterPath, string oldDimensionName, string newDimensionName)
         {
            return $"Update dimension for parameter '{parameterPath}' from '{oldDimensionName}' to '{newDimensionName}'";
         }

         public static string UpdateAssigmentObjectPath(string assignmentPath, string path)
         {
            return $"Update object path for assignment '{assignmentPath}' to '{path}'";
         }

         public static string UpdateMoleculeStartValueScaleDivisor(string path, double oldScaleDivisor, double newScaleDivisor)
         {
            return $"Updating Scale Divisor for start value with path {path} from {oldScaleDivisor} to {newScaleDivisor}";
         }

         public static string UpdateDistributedFormulaCommandDescription(string parameterPath, string formulaType)
         {
            return $"Update distributed formula to '{formulaType}' for parameter '{parameterPath}'";
         }

         public static string ObjectConvertedCommand(string objectName, string objectType, string fromVersion, string toVersion)
         {
            return string.Format("{1} '{0}' converted from version '{2}' to '{3}'", objectName, objectType, fromVersion, toVersion);
         }

         public static string ChangeFormulaAlias(string formulaName, string oldAlias, string newAlias, string buildingBlockName)
         {
            return $"Changed formula alias from '{oldAlias}' to '{newAlias}' for formula named '{formulaName}' in building block '{buildingBlockName}'";
         }

         public static string ChangeFormulaPathDimension(string formulaName, string oldDimension, string newDimension, string buildingBlockName, string alias)
         {
            return string.Format("Changed formula usable path dimension for alias '{4}' from '{0}' to '{1}' for formula named '{2}' in building block '{3}'", oldDimension, newDimension, formulaName, buildingBlockName, alias);
         }

         public static string EditFormulaUsablePath(string oldPath, string newPath, string alias, string formulaName, string buildingBlockName)
         {
            return string.Format("Changed formula usable path for alias '{4}' from '{0}' to '{1}' for formula named '{2}' in building block '{3}'", oldPath, newPath, formulaName, buildingBlockName, alias);
         }

         public static string ChangeFormulaString(string formulaName, string newFormulaString, string oldFormulaString, string buildingBlockName)
         {
            return $"Changed formula string from '{oldFormulaString}' to '{newFormulaString}' for formula named '{formulaName}' in building block '{buildingBlockName}'";
         }

         public static string AddTagToEntity(string tag, string entityName, string containerPath, string buildingBlockName)
         {
            return $"Adding tag '{tag}' to entity named '{entityName}' in container path '{containerPath}' in building block '{buildingBlockName}'";
         }

         public static string AddParameterToContainerDescription(string containerPath, string parameterName, string buildingBlockName)
         {
            return $"Adding parameter '{parameterName}' to container '{containerPath}' in building block '{buildingBlockName}'";
         }

         public static string UpdateMoleculeStartValueIsPresent(string startValuePath, bool oldIsPresent, bool newIsPresent)
         {
            return $"Changed Is Present for molecule start value path '{startValuePath}' from {oldIsPresent} to {newIsPresent}";
         }

         public static string UpdateMoleculeStartValueNegativeValuesAllowed(string startValuePath, bool oldNegativeValuesAllowed, bool newNegativeValuesAllowed)
         {
            return $"Changed negative values allowed for molecule start value path '{startValuePath}' from {oldNegativeValuesAllowed} to {newNegativeValuesAllowed}";
         }

         public static string SetTableFormulaYDisplayUnits(string tableFormulaName, string oldUnit, string newUnit, string buildingBlockName)
         {
            return $"Changed Y axis display units for table formula named '{tableFormulaName}' from '{oldUnit}' to '{newUnit}' in building block '{buildingBlockName}'";
         }

         public static string SetTableFormulaXDisplayUnits(string tableFormulaName, string oldUnit, string newUnit, string buildingBlockName)
         {
            return $"Changed X axis units for table formula named '{tableFormulaName}' from '{oldUnit}' to '{newUnit}' in building block '{buildingBlockName}'";
         }

         public static string SetRestartSolverInValuePoint(string tableFormulaName, string xCoordinate, string yCoordinate, bool newRestartSolverValue, bool oldRestartSolverValue, string buildingBlockName)
         {
            return $"Changed restart solver value in table formula '{tableFormulaName}' from '{oldRestartSolverValue}' to '{newRestartSolverValue}' for point '{xCoordinate},{yCoordinate}' in building block {buildingBlockName}";
         }

         public static string SetValuePointValueCommand(string tableFormulaName, string oldXCoordinate, string oldYCoordinate, string newXCoordinate, string newYCoordinate, string buildingBlockName)
         {
            return $"Changed value point in table formula '{tableFormulaName}' from '{oldXCoordinate},{oldYCoordinate}' to '{newXCoordinate},{newYCoordinate}'";
         }

         public static string UpdateParameterBuildMode(string parameterName, string newBuildMode, string oldBuildMode, string buildingBlockName)
         {
            return updateParameterProperty(parameterName, newBuildMode, oldBuildMode, buildingBlockName, "build mode");
         }

         private static string updateParameterProperty(string parameterName, string newValue, string oldValue, string buildingBlockName, string propertyName)
         {
            return string.Format("Changed parameter {0} {4} from '{1}' to '{2}' in building block {3}", parameterName, oldValue, newValue, buildingBlockName, propertyName);
         }

         public static string ChangeParameterDescription(string parameterName, string oldDescription, string newDescription, string buildingBlockName)
         {
            return updateParameterProperty(parameterName, newDescription, oldDescription, buildingBlockName, "description");
         }

         public static string ChangeParameterName(string parameterName, string oldName, string newName, string buildingBlockName)
         {
            return updateParameterProperty(parameterName, newName, oldName, buildingBlockName, "name");
         }

         public static string ChangeParameterRHSFormula(string parameterName, string newFormulaName, string oldFormulaName, string buildingBlockName)
         {
            return updateParameterProperty(parameterName, newFormulaName, oldFormulaName, buildingBlockName, "RHS formula");
         }

         public static string ChangeParameterFormula(string parameterName, string newFormulaName, string oldFormulaName, string buildingBlockName)
         {
            return updateParameterProperty(parameterName, newFormulaName, oldFormulaName, buildingBlockName, "formula");
         }
      }

      public static class BarNames
      {
         public static readonly string Menu = "MainMenu";
         public static readonly string Status = "StatusBar";
         public static readonly string Tools = "Extras";
         public static readonly string DisplayUnits = "Display Units";
         public static readonly string Import = "Import";
         public static readonly string ExportProject = "Export Project";
         public static readonly string History = MenuNames.HistoryView;
         public static readonly string Simulation = MenuNames.Simulation;
         public static readonly string NotificationList = "Notifications";
         public static readonly string Comparison = "Comparison";
         public static readonly string Workflows = "Workflows";
         public static readonly string BuildingBlocks = "Create";
         public static readonly string Views = "Views";
         public static readonly string Add = "Add";
         public static readonly string Edit = "Edit";
         public static readonly string Journal = "Journal";
         public static readonly string File = "File";
         public static readonly string Favorites = "Favorites";
      }

      public static class RibbonButtonNames
      {
         public static readonly string ObservedData = "Observed Data";
         public static readonly string Simulation = MenuNames.Simulation;
         public static readonly string Molecules = "Molecules";
         public static readonly string Reactions = "Reactions";
         public static readonly string SpatialStructure = "Spatial Structure";
         public static readonly string PassiveTransport = "Passive Transport";
         public static readonly string Events = "Events";
         public static readonly string Observer = "Observer";
         public static readonly string SimulationSettings = "Simulation Settings";
         public static readonly string New = "New";
         public static readonly string Load = "Load";
         public static readonly string LoadFromTemplate = "Load From Template";
         public static readonly string NewMolecule = "Insert Molecule";
         public static readonly string AddPKSimMolecule = "PK-Sim Molecule";
      }

      public static class Dialog
      {
         public static readonly string DoYouWantToSaveTheCurrentProject = "Do you want to save the current project?";
         public static readonly string GetReactionMoleculeName = "Please enter new Molecule Name";

         public static string LoadProject = "Load Project";

         public static string LoadSimulation = "Load Simulation";
         public static readonly string ApplyDefaultNaming = "Apply default renaming";
         public static readonly string RemoveSelectedResultsFromSimulations = "Do you really want to remove the selected results from simulations";
         public static readonly string RemoveSelectedResultsFromProject = "Do you really want to remove the selected result(s) from the project";

         public static string RemoveSimulationsFromProject(string projectName)
         {
            return $"Do you really want to remove the simulations from '{projectName}'? The simulations will also be removed from parameter identifications and sensitivity analyses";
         }

         public static string RemoveAllObservedDataFrom(string projectName)
         {
            return $"Do you really want to remove the observed data from '{projectName}'?";
         }

         public static string RemoveAllResultsFrom(string simulationName)
         {
            return $"Do you really want to remove all results from '{simulationName}'?";
         }

         public static string RemoveAllResultsFromProject()
         {
            return "Do you really want to remove all simulation results of all simulations?";
         }

         public static string RemoveSimulationResultsFromSimulation(string dataRepositoryName, string simulationName)
         {
            return Remove("simulation results", dataRepositoryName, simulationName);
         }

         public static string Remove(string objectType, string objectName, string parentName)
         {
            var baseString = $"Do you really want to remove {objectType} '{objectName}' from {parentName}.";
            if (string.Equals(objectType, ObjectTypes.Simulation))
               return baseString + $"  {objectName} will also be removed from parameter identifications and sensitivity analyses";

            return baseString;
         }

         public static readonly string NewMatchTag = "Tag to match";
         public static readonly string NewNotMatchTag = "Tag not to match";
         public static readonly string AskForPopulationWorkingDirectory = "Select Working Directory for Population Simulation";
         public static readonly string AskForParameterIdentificationWorkingDirectory = "Select Working Directory for Parameter Identification";
         public static readonly string AskForSave = "Save as ";
         public static readonly string AskForSaveProject = "Save Project As";
         public static readonly string KeepChart = "Keep Chart in Project";

         public static string AskForChangedName(string oldName, string typeName)
         {
            return $"A {typeName.ToLower()} named '{oldName}' already exists.\nPlease enter a new name";
         }

         public static string AskForNewName(string name)
         {
            return string.Format("Please enter new name");
         }

         public static string AskFileOverride(string fileName)
         {
            return $"File: {fileName} already exits. Overwrite ?";
         }

         public static string AskForNewNeighborhoodBuilderName(string container1Name, string container2Name)
         {
            return $"Please enter new name for Neighborhood between '{container1Name}' and '{container2Name}'";
         }

         public static readonly string ExportSimulationModelToFileTitle = "Export model structure to text file";
         public static readonly string ExportSimulationResultsToExcel = "Export simulation results to Excel®";
         public static readonly string ExportSimulationMatlabODE = "Export simulation to Matlab® ODE";
         public static readonly string LoadSBMLProject = "Load SBML Project";

         public static string Load(string objectType)
         {
            return $"Load {objectType.ToLowerInvariant()} from file";
         }

         public static string LoadFromTemplate(string objectType)
         {
            return $"Load {objectType.ToLowerInvariant()} from template file";
         }

         public static string AddedFileForIdentification(string fileName)
         {
            return $"Already Models present for Identification. Model: '{fileName}' is added.\n Please make sure that Model is selected for identification";
         }

         public static string PendingBuildingBlockChangesInfo(string typeName, string name)
         {
            return string.Format("Commited new values To {0}: '{1}'. Changes made at the {0} are still present", typeName, name);
         }

      }

      public static class Filter
      {
         public static readonly string MOBI_PROJECT_EXTENSION = ".mbp3";
         public static readonly string MOBI_DIAGRAM_TEMPLATE_EXTENSION = ".mbdt";
         public static readonly string MOBI_PROJECT_FILTER = $"*{MOBI_PROJECT_EXTENSION}";
         public static readonly string MOBI_PROJECT_FILE_FILTER = Constants.Filter.FileFilter("MoBi Project", MOBI_PROJECT_EXTENSION);
         public static readonly string MOBI_DIAGRAM_TEMPLATE_FILTER = Constants.Filter.FileFilter("MoBi Diagram Template", MOBI_DIAGRAM_TEMPLATE_EXTENSION);
         public static readonly string SIM_MODEL_FILE_FILTER = Constants.Filter.XmlFilter("xml");
         public static readonly string LICENSE_FILE_FILTER = Constants.Filter.FileFilter("MoBi License", Constants.Filter.TEXT_EXTENSION);
         public static readonly string MOBI2_PROJECT_FILTER = Constants.Filter.FileFilter("MoBi 2 Project", ".mbp");
         public static readonly string SBML_MODEL_FILE_FILTER = Constants.Filter.XmlFilter("SBML");
         public static readonly string PKSIM_FILE_FILTER = Constants.Filter.FileFilter("PKSim", ".exe");
      }

      public static class DirectoryKey
      {
         public static readonly string LAYOUT = "Layout";
      }

      public static class MenuNames
      {
         public static readonly string NewProject = "&New";
         public static readonly string NewAmountProject = "Amount Based Reactions";
         public static readonly string NewConcentrationProject = "Concentration Based Reactions";
         public static readonly string OpenProject = "&Open...";
         public static readonly string SaveProject = "&Save";
         public static readonly string SaveAs = "Save As...";
         public static readonly string NewSimulation = "Create";
         public static readonly string Merge = "Merge";
         public static readonly string CloseProject = "&Close";
         public static readonly string GarbageCollection = "Start Garbage Collection";
         public static readonly string About = "&About...";
         public static readonly string Options = "Options";
         public static readonly string Run = "Run";
         public static readonly string RunWithSettings = "Define Settings and Run";
         public static readonly string CalculateScaleDivisor = "Calculate Scale Divisor";
         public static readonly string AddLabel = "Add Label";
         public static readonly string Undo = "Undo";
         public static readonly string Stop = "Stop";
         public static readonly string ShowResults = "Show Simulation Results";
         public static readonly string HistoryView = "History";
         public static readonly string File = "&File";
         public static readonly string View = "View";
         public static readonly string Extras = "Extras";
         public static readonly string Simulation = "Simulation";
         public static readonly string Exit = "E&xit";
         public static readonly string RecentProjects = "&Recent Projects";
         public static readonly string Edit = "Edit...";
         public static readonly string Rename = "Rename...";
         public static readonly string Remove = OSPSuite.Assets.MenuNames.Remove;
         public static readonly string Delete = OSPSuite.Assets.MenuNames.Delete;
         public static readonly string DeleteAllResults = "Delete all Results...";
         public static readonly string SaveToPkmlFormat = "&Save Simulation to MoBi pkml Format...";
         public static readonly string OpenSimulation = "Open Simulation...";
         public static readonly string LoadIntoProject = "Load Simulation into Project...";
         public static readonly string Data = "Data";
         public static readonly string AddObservedData = "Add &Observed Data...";
         public static readonly string LoadObservedData = AddExisting(ObjectTypes.ObservedData);
         public static readonly string CompareSimulationResults = "Compare Results...";
         public static readonly string SaveGroup = "Save";
         public static readonly string NewMoleculeBuildingBlock = AddNew("Molecule Building Block");
         public static readonly string NewReactionBuildingBlock = AddNew("Reaction Building Block");
         public static readonly string NewPassiveTransportBuildingBlock = AddNew("Passive Transport Building Block");
         public static readonly string NewEventsBuildingBlock = AddNew("Event Building Block");
         public static readonly string NewObserverBuildingBlock = AddNew("Observer Building Block");
         public static readonly string NewSimulationSettingsBuildingBlock = AddNew("Simulation Settings Building Block");
         public static readonly string RelativePath = "Relative Path";
         public static readonly string AbsolutePath = "Absolute Path";
         public static readonly string ExportHistory = Captions.ExportHistory;
         public static readonly string StartPopulationSimualtion = "Send Simulation to PK-Sim for Population Simulation...";
         public static readonly string ExportSimModelXml = "Export Simulation for Matlab®/R...";
         public static readonly string BuildingBlockExplorer = "Building Blocks";
         public static readonly string SimulationExplorer = "Simulations";
         public static readonly string New = "New";
         public static readonly string NewMolecule = AddNew(ObjectTypes.Molecule);
         public static readonly string LoadMolecule = AddExisting(ObjectTypes.Molecule);
         public static readonly string LoadMoleculeFromTemplate = AddExistingFromTemplate(ObjectTypes.Molecule);
         public static readonly string NewReaction = AddNew(ObjectTypes.Reaction);
         public static readonly string LoadReaction = AddExisting(ObjectTypes.Reaction);
         public static readonly string LoadReactionFromTemplate = AddExistingFromTemplate(ObjectTypes.Reaction);
         public static readonly string NewObserver = AddNew(ObjectTypes.ObserverBuilder);
         public static readonly string LoadObserver = AddExisting(ObjectTypes.ObserverBuilder);
         public static readonly string LoadObserverFromTemplate = AddExistingFromTemplate(ObjectTypes.ObserverBuilder);
         public static readonly string NewAmountObserver = AddNew(ObjectTypes.AmountObserverBuilder);
         public static readonly string LoadAmountObserver = AddExisting(ObjectTypes.AmountObserverBuilder);
         public static readonly string LoadAmountObserverFromTemplate = AddExistingFromTemplate(ObjectTypes.ObserverBuilder);
         public static readonly string NewContainerObserver = AddNew(ObjectTypes.ContainerObserverBuilder);
         public static readonly string LoadContainerObserver = AddExisting(ObjectTypes.ContainerObserverBuilder);
         public static readonly string LoadContainerObserverFromTemplate = AddExistingFromTemplate(ObjectTypes.ContainerObserverBuilder);
         public static readonly string NewSpatialStructure = AddNew(ObjectTypes.SpatialStructure);
         public static readonly string LoadSpatialStructure = AddExisting(ObjectTypes.SpatialStructure);
         public static readonly string LoadSpatialStructureFromTemplate = AddExistingFromTemplate(ObjectTypes.SpatialStructure);
         public static readonly string NewEvent = AddNew(ObjectTypes.EventBuilder);
         public static readonly string LoadEvent = AddExisting(ObjectTypes.EventBuilder);
         public static readonly string LoadEventFromTemplate = AddExistingFromTemplate(ObjectTypes.EventBuilder);
         public static readonly string NewPassiveTransport = AddNew(ObjectTypes.ActiveTransport);
         public static readonly string LoadPassiveTransport = AddExisting(ObjectTypes.ActiveTransport);
         public static readonly string LoadPassiveTransportFromTemplate = AddExistingFromTemplate(ObjectTypes.ActiveTransport);
         public static readonly string AddPKSimMolecule = "Add PK-Sim Molecule...";
         public static readonly string ZoomIn = "Zoom In";
         public static readonly string ZoomOut = "Zoom Out";
         public static readonly string FitToPage = "Fit to Page";
         public static readonly string ExportHistoryToExcel = "Export to Excel®...";
         public static readonly string ExportHistoryToPDF = "Export to PDF...";
         public static readonly string ExportToPDF = "Export to PDF...";
         public static readonly string ProjectReport = "Project Report";
         public static readonly string SimulationReport = "Create Simulation Report...";
         public static readonly string SetToDefault = "Set to Default";
         public static readonly string Extend = "Extend";
         public static readonly string ExportSimulationResultsToExcel = "Export Results to Excel®...";
         public static readonly string All = "All";
         public static readonly string Selection = "Selection...";
         public static readonly string NewFromSelection = "Create New Building Block with Existing Molecules";
         public static readonly string Clone = "Clone";
         public static readonly string ParameterIdentification = "Start Parameter Identification...";
         public static readonly string NewTopContainer = AddNew("Top Container");
         public static readonly string LoadTopContainer = AddExisting("Top Container");
         public static readonly string LoadTopContainerFromTemplate = AddExistingFromTemplate("Top Container");
         public static readonly string Help = "Help";
         public static readonly string SearchView = "Search";
         public static readonly string NotificationView = "Notifications";
         public static readonly string ComparisonView = "Comparison";
         public static readonly string MatlabDifferentialSystemExport = "Export Simulation to Matlab® Differential Equations...";
         public static readonly string GoTo = "Go To...";
         public static readonly string DiscardResults = "Discard";
         public static readonly string KeepResults = "Keep";
         public static readonly string Commit = "Commit to building block ...";
         public static readonly string Update = "Update from building block ...";
         public static readonly string RemoveAll = "Remove All";
         public static readonly string Import = "Import ...";
         public static readonly string LoadChartTemplate = "Apply Template";
         public static readonly string CreateNewTemplate = "Create New...";
         public static readonly string SaveChartTemplate = "Save Template";
         public static readonly string NoTemplateAvailable = "No Template Available";
         public static readonly string UpdateExistingTemplate = "Update Existing";
         public static readonly string ChartTemplate = "Chart Templates";
         public static readonly string ManageTemplates = "Manage Templates...";
         public static readonly string NewParameterStartValue = "New Parameter Start Value";
         public static readonly string NewMoleculeStartValue = "New Molecule Start Value";
         public static readonly string ModelParts = "Model Parts";
         public static readonly string ImportSBML = "Open SBML Model...";
         public static readonly string SaveAsPKML = "Save As PKML...";

         public static string AddNew(string objectTypeName) => $"Create {objectTypeName}...";

         public static string AddExisting(string objectTypeName) => $"Load {objectTypeName}...";

         public static string AddExistingFromTemplate(string objectTypeName) => $"Load {objectTypeName} from Template...";
      }

      public static class DimensionNames
      {
         public static readonly string FRACTION = "Fraction";
         public static readonly string INVERSED_LENGTH = "Inversed length";
         public static readonly string LENGTH = "Length";
         public static readonly string FLOW = "Flow";
         public static readonly string MOL_WEIGHT = "Molecular weight";
         public static readonly string VELOCITY = "Velocity";
         public static readonly string INVERSED_VOLUME = "Inversed volume";
         public static readonly string MASS = "Mass";
         public static readonly string AREA = "Area";
      }

      public class Messages
      {
         public static readonly string OptimizationFinished = "Optimization finished";
      }

      public class Warnings
      {
         public static string PassiveTransporBuildingBlockCreatedAutomatically(string name)
         {
            return $"A passive transport building block named '{name}' was generated to account for changes in passive processes usage.";
         }

         public static string TheImportedDimensionDoesNotMatchTheExistingQuantity(string path, IDimension existingDimension, IDimension importedDimension)
         {
            return $"The imported dimension '{importedDimension}' does not match the existing dimension '{existingDimension}' with the path {path}.";
         }

         public static string FormatAsStartValueImportWarning(int rowIndex, string dataRowString, string suggestion)
         {
            return $"WARNING: row #{rowIndex} => {dataRowString} {suggestion}";
         }

         public static string CannotAddNewParameterFromImportToSimulation(string name, string path)
         {
            return string.Format("Cannot find a parameter for path {1} in simulation {0} during import. Only updates are allowed", name, path);
         }

         public static readonly string ThisItNotATemplateBuildingBlock = "This is not the template building block!";
      }

      public class Exceptions
      {
         public static readonly string CanNotRemoveLastItem = "You can not remove the last item";
         public static readonly string UnableToCreateStartValues = "Unable to create start values. At least one molecule and one spatial structure building block are required.";
         public static readonly string NoSelectionWithNew = "You need to select an item or create new one";
         public static readonly string NoBuildingBlockAvailable = "No building block available";
         public static readonly string SelectUniqueMolecules = "Select Unique Molecules";
         public static readonly string MergeBuildingBlocksCountError = "Building blocks to merge and target building blocks do not have the same length";
         public static readonly string MissingName = "Name missing";
         public static readonly string DeserializationFailed = "Deserialization failed";
         public static readonly string SourceBuildingBlockNotInProject = "Building Block used to create start values is not present in project";
         public static readonly string ShouldNeverHappen = "Should never happen";
         public static readonly string ErrorInFormula = "Error in Formula";
         public static readonly string ApplicatedMoleculeNotInProject = "Applicated Molecule is not in Project";
         public static readonly string CoundNotCreateSimulation = "Unable to create the simulation. Please check warnings or errors in the notification view.";
         public static readonly string TemplateShouldContainAtLeastOneCurve = "Template should contain at least one curve.";
         public static readonly string StartValueDimensionModeDoesNotMatchBuildingBlockDimensionMode = "The imported start value dimension mode does not match the building block dimension mode. Modify the start value or the building block so that they are both concentration based, or amount based";
         public static readonly string FrameworkExceptionOccurred = "An exception occurred";
         public static readonly string MergingSpatialStructuresIsNotSupported = "Merging Spatial Structures is not supported.";
         public static readonly string ImportedStartValueMustHaveName = "The imported start value must have a name";
         public static readonly string ImportedStartValueMustHaveContainerPath = "The imported start value must have a container path";
         public static readonly string ReactionNodeMissingInLink = "Reaction Node missing in Link";
         public static readonly string MoleculeNodeMissingInLink = "Molecule Node missing in Link";
         public static readonly string FileInNotAnExcelFile = "File is not an Excel file";

         public static string DuplicatedImportedStartValue(string path) => $"Duplicated entry for imported start value with path '{path}'";

         public static string CannotConvertAConcentrationModelBasedIntoAnAmountBasedModel(string modelType)
         {
            return string.Format("Cannot convert concentration {0} into amount {0}.", modelType.ToLowerInvariant());
         }

         public static string CannotImportFromExcelFile(string filePath) => $"Failed to import from {filePath}. It may not be valid Excel file format";

         public static string TableShouldBeNColumns(int n) => $"Table should be {n} columns or more";

         public static string ColumnNMustBeNumeric(string columnValue, int N) => $"Column {N} must be numeric type: {columnValue}";

         public static string ProjectWillBeOpenedAsReadOnly(string errorMessage) => $"{errorMessage}\nAny change made to the project will not be saved.";

         public static string InvalidStartValuesConfiguration(string startValuesBuildingBlock, string buildingBlockType)
         {
            return $"Selected {buildingBlockType.ToLower()} '{startValuesBuildingBlock}' does not match the selected {ObjectTypes.MoleculeBuildingBlock.ToLower()} and {ObjectTypes.SpatialStructure.ToLower()}.";
         }

         public static string CouldNotFindAReporterFor(Type type) => $"Unable to find a reporter for {type.Name}";

         public static string UnknownProjectItem(Type type) => $"Unable to find presenter for '{type.Name}'";

         public static string UnknownDistributedFormula(Type type) => $"Unknown Distributed Formula Type '{type.Name}'";

         public static string EmptyCollection(string collectionName) => $"{collectionName} contains no Elements";

         public static string NoInformationFoundException(string fileName, string lookForElementName, string rootElementName)
         {
            var displayRootElementName = rootElementName.Replace("BuildingBlock", string.Empty);
            return string.Format("File '{0}' only contains loadable content for '{2}' and cannot be loaded as '{1}'", fileName, lookForElementName, displayRootElementName);
         }

         public static string NoInformationFoundException(string typeName)
         {
            return $"File contains no loadable content for  '{typeName}'";
         }

         public static string CircularReferenceException(IFormulaUsablePath path, IFormula formula)
         {
            return $"Not adding path {path.PathAsString} to formula {formula.Name}.\nAdding path would create circular reference";
         }

         public static string CircularReferenceFormulaException(IFormula formula)
         {
            return $"Not setting formula to '{formula.Name}', would create circular reference";
         }

         public static string UnableToSetFormulaFor(string name)
         {
            return $"Unable to set formula for {name}";
         }

         public static string NameAlreadyUsed(string newName)
         {
            return $"Name '{newName}' already used";
         }

         public static string NoEditPresenterFoundFor(IObjectBase objectToEdit)
         {
            return $"No edit presenter found for {objectToEdit.Name}. {ShouldNeverHappen}";
         }

         public static string FormulaInUse(IFormula formula)
         {
            return $"Unable to remove Formula '{formula.Name}' still in use.";
         }

         public static string NotSupportedFormulaType(Type type)
         {
            return $"Formula type {type} is not supported";
         }

         public static string RemovedToPreventErrorDoubleImport<T>(IEnumerable<T> containerDoubleImported) where T : IObjectBase
         {
            var displayNames = containerDoubleImported.Select(s => $"{s.Name}, ");
            var display = displayNames.Aggregate(string.Empty, (current, displayName) => string.Concat(current, displayName));
            if (display.Any())
            {
               display = display.Remove(display.Count() - 2);
            }
            return $"'{display}' are not imported to prevent errors, because it is already imported as child of another Container. \n You may add them in a second step if necessary";
         }

         public static string BuildingBlockNotFoundFor(IObjectBase objectBase)
         {
            return $"BuildingBlock not found for {objectBase.Name}";
         }

         public static string UnknownDimension(string name)
         {
            return $"Dimension '{name}' not available in DimensionFactory.";
         }

         public static string CouldNotFindDimensionFromUnits(string columnValue)
         {
            return $"Could not find the dimension for this unit: {columnValue}";
         }

         public static string CannotRemoveParameter(string parameterName, string containerName, string containerType)
         {
            return string.Format("Parameter '{0}' is a mandatory parameter of {2} '{1}' and cannot be removed.", parameterName, containerName, containerType.ToLowerInvariant());
         }

         public static string AliasNotUnique(string alias, string formulaName)
         {
            return $"Alias '{alias}' is not unique in formula '{formulaName}'.";
         }

         public static string ImportedDimensionNotRecognized(string dimensionName, IEnumerable<string> unitNames)
         {
            return $"The imported dimension is not correct. The dimension must be {dimensionName}. The possible units are: {unitNames.ToString(",")}";
         }
      }

      public static class Captions
      {
         public static readonly string Modifiers = "Modifiers";
         public static readonly string Dimension = "Dimension";
         public static readonly string Alias = "Alias";
         public static readonly string Unit = "Unit";
         public static readonly string FormulaCreationCaption = "Formula Creation Wizard";
         public static readonly string SimulationCreationCaption = "Simulation Creation Wizard";
         public static readonly string ProjectExplorer = "Project Explorer";
         public static readonly string BuildingBlockExplorer = "Building Block Explorer";
         public static readonly string WarningsCaption = "Solver Warnings";
         public static readonly string HistoryBrowser = "History";
         public static readonly string NameInUse = "New Name";
         public static readonly string ReanameWizardCaption = "Rename also";
         public static readonly string AddReactionMolecule = "Molecule Name";
         public static readonly string NewName = "New Name";
         public static readonly string Tag = "Tag";
         public static readonly string ValidationMessages = "Validation Results";
         public static readonly string SelectReferencesView = "References to add";
         public static readonly string References = "References";
         public static readonly string NewMolculeStartValues = "Create New Molecule Start Values";
         public static readonly string NewParameterStartValues = "Create New Parameter Start values";
         public static readonly string ExportHistory = "Export History";
         public static readonly string MoleculeNames = "Molecule Name";
         public static readonly string StoichiometricCoefficient = "Stoichiometric Coefficient";
         public static readonly string FormulaType = "Formula Type";
         public static readonly string FormulaName = "Formula Name";
         public static readonly string Formula = "Formula";
         public static readonly string ExplicitFormula = "Formula (an explicit formula)";
         public static readonly string ConstantFormula = "Constant (a single numeric value)";
         public static readonly string TableFormula = "Table (multiple time discrete and piecewise constant numeric values)";
         public static readonly string BlackBoxFormula = "Calculation Method (definition dependent on a calculation method)";
         public static readonly string Value = "Value";
         public static readonly string SpatialStructure = "Spatial Structure";
         public static readonly string SpatialStructures = "Spatial Structures";
         public static readonly string Molecules = "Molecules";
         public static readonly string Molecule = "Molecule";
         public static readonly string Events = "Events";
         public static readonly string Event = "Event";
         public static readonly string Observers = "Observers";
         public static readonly string Simulations = "Simulations";
         public static readonly string Observer = "Observer";
         public static readonly string ObservedData = "Observed Data";
         public static readonly string PassiveTransports = "Passive Transports";
         public static readonly string Reactions = "Reactions";
         public static readonly string MoleculeStartValues = "Molecule Start Values";
         public static readonly string ParameterStartValues = "Parameter Start Values";
         public static readonly string NextButton = "&Next";
         public static readonly string PreviousButton = "&Previous";
         public static readonly string FinishButton = "&Finish";
         public static readonly string CancelButton = "&Cancel";
         public static readonly string OKButton = "&OK";
         public static readonly string Name = "Name";
         public static readonly string LoadMoBi2 = "Load MoBi 2.0 Project";
         public static readonly string SaveChangesAsNewBuildingBlock = "Save Changes as new building block";
         public static readonly string AutomaticallyGeneratedValues = "Automatically generated values";
         public static readonly string NewlyAddedValues = "Newly added values";
         public static readonly string Save = "Save";
         public static readonly string DrugDrugInteractionFile = "Simulation file containing the inhibitor for Drug Drug Interaction";
         public static readonly string ParentMetaboliteFile = "Simulation file containing the metabolite for Parent-Metabolite coupling";
         public static readonly string IsMatching = "Condition";
         public static readonly string Options = "Options";
         public static readonly string EndTime = "End Time";
         public static readonly string StartTime = "Start Time";
         public static readonly string Resolution = "Resolution";
         public static readonly string OutputIntervals = "Output Intervals";
         public static readonly string SelectFormulasForObjectBaseSelection = "Select formula for objects";
         public static readonly string SimulationSettings = "Simulation Settings";
         public static readonly string SimulationSettingsDescription = "Select the curves that will be generated by the simulation.";
         public static readonly string ShouldRenameDependentObjects = "Rename Dependent Objects";
         public static readonly string SimulationPathForMerge = "Simulation path";
         public static readonly string SourceSimulationFileForMerge = "Select simulation file containg the building blocks to merge";
         public static readonly string BuildingBlockToMerge = "Source building block";
         public static readonly string TargetBuildingBlock = "Embed in target building block";
         public static readonly string MergeSimulationIntoProject = "Merge Simulation into Project";
         public static readonly string ImportAsNew = "<Import as new>";
         public static readonly string CreatingSimulation = "Creating...";
         public static readonly string AddParameter = "Add Parameter ";
         public static readonly string AddMolecule = "Add Molecule";
         public static readonly string LoadParameter = "Load Parameter ";
         public static readonly string InContainerWith = "In Container With";
         public static readonly string ContainerCriteria = "Container Criteria";
         public static readonly string Products = "Products";
         public static readonly string Educts = "Educts";
         public static readonly string ApplicationMoleculeBuilder = "Application Molecule Builder";
         public static readonly string AdministeredMolecule = "Administered Molecule";
         public static readonly string ContainingBuildingBlock = "Building Block";
         public static readonly string DependentRename = "Dependent Renaming";
         public static readonly string Error = "Error";
         public static readonly string Errors = "Errors";
         public static readonly string Warning = "Warning";
         public static readonly string Warnings = "Warnings";
         public static readonly string Debug = "Debug";
         public static readonly string Messages = "Messages";
         public static readonly string SaveLog = "Save Log...";
         public static readonly string SaveLogToFile = "Export simulation log to file...";
         public static readonly string SaveLayoutToFile = "Save Layout Template...";
         public static readonly string Stoichiometry = "Stoichiometry";
         public static readonly string AboutProduct = "About...";
         public static readonly string SelectChangedEntiy = "Select changed entity";
         public static readonly string TransporterName = "Transporter alias";
         public static readonly string CalculatedForFollowingMolecules = "Calculated for following molecules";
         public const string Description = "Description";
         public static readonly string Assignment = "Assignment";
         public static readonly string Condition = "Condition";
         public static readonly string TranporterMoleculeName = "Transporter molecule name";
         public static readonly string Reset = "Reset";
         public static readonly string EnterNewFormulaName = "Enter new name for formula";
         public static readonly string CloneFormulaTitle = "Clone formula";
         public static readonly string AddFormula = "Add Formula";
         public static readonly string AddAssignment = "Add Assignment";
         public static readonly string CalculationMethod = "Calculation method";
         public static readonly string Category = "Category";
         public static readonly string UseAsValue = "Use assignment as value";
         public static readonly string SelectLocalReferencePoint = "Select local reference point";
         public static readonly string RelativeContainerPath = "Relative container path";
         public static readonly string Parameters = "Parameters";
         public static readonly string ChangedEntityPath = "Changed entity path";
         public static readonly string ChangedEntity = "Changed entity";
         public static readonly string NewFormula = "New formula";
         public static readonly string StartValue = "Start Value";
         public static readonly string ValueDescription = "Value Description";
         public static readonly string ScaleDivisor = "Scale Divisor";
         public static readonly string IsPresent = "Is Present";
         public static readonly string NegativeValuesAllowed = "Neg. Values Allowed";
         public static readonly string NegativeValues = "Neg. Values";
         public static readonly string MoleculeName = "Molecule Name";
         public static readonly string ParameterName = "Parameter Name";
         public static readonly string AllPresent = "Mark all as present";
         public static readonly string AllNotPresent = "Mark all as not present";
         public static readonly string SelectedNotPresent = "Mark selected as not present";
         public static readonly string SelectedPresent = "Mark selected as present";
         public static readonly string AllNegativeValuesAllowed = "Mark all as allowing neg. values";
         public static readonly string AllNegativeValuesNotAllowed = "Mark all as not allowing neg. values";
         public static readonly string SelectedNegativeValuesAllowed = "Mark selected as allowing neg. values";
         public static readonly string SelectedNegativeValuesNotAllowed = "Mark selected as not allowing neg. values";
         public static readonly string PerformCheckSelection = "Apply";
         public static readonly string BuildConfiguration = "Configuration";
         public static readonly string Search = "Search";
         public static readonly string SearchScope = "Scope";
         public static readonly string SearchResults = "Search results";
         public static readonly string SearchWholeName = "Match whole word";
         public static readonly string SearchRegEx = "Use regular expression";
         public static readonly string ProjectItem = "Project Item";
         public static readonly string TypeName = "Type";
         public static readonly string Path = "Path";
         public static readonly string Kinetic = "Kinetic";
         public static readonly string ShowAdvancedParameters = "Show advanced parameters";
         public static readonly string GroupParameters = "Group parameters";
         public static readonly string IsAdvancedParamerter = "Advanced parameter";
         public static readonly string CaseSensitive = "Match case";
         public static readonly string AddValuePoint = "Add Point";
         public static readonly string UseDerivedValues = "Use derivative values";
         public static readonly string OffsetObjectPath = "Path to offset object";
         public static readonly string TableObjectPath = "Path to table object";
         public static readonly string TableFormulaWithOffset = "Table Formula with Offset";
         public static readonly string VariableName = "Variable name";
         public static readonly string SumFormula = "Sum Formula";
         public static readonly string Criteria = "Parameter criteria";
         public static readonly string RemoveCondition = "Remove condition";
         public static readonly string NewMatchTagCondition = "New match tag condition";
         public static readonly string NewNotMatchTagCondition = "New not match tag condition";
         public static readonly string AddMatchAllCondition = "Add match all tag condition";

         public static readonly string Persistable = "Plot parameter";
         public static readonly string Properties = "Properties";
         public static readonly string Tags = "Tags";
         public static readonly string Group = "Group";
         public static readonly string AddTag = "Add Tag";
         public static readonly string ContainerTags = "Container Tags";
         public static readonly string ModelDiagram = "Diagram";
         public static readonly string SimulationParameters = "Parameters";
         public static readonly string Results = "Results";
         public static readonly string IsStationary = "Stationary";
         public static readonly string DefaultStartAmount = "Amount";
         public static readonly string MoleculeType = "Molecule Type";
         public static readonly string IncludeList = "Include List";
         public static readonly string ExcludeList = "Exclude List";

         public static readonly string OneTimeEvent = "One Time";

         public static readonly string Settings = "Settings";
         public static readonly string SolverSettings = "Solver Settings";
         public static readonly string FinalOptions = "Final Options";
         public static readonly string InitialValue = "Initial Value";
         public static readonly string Amount = "Amount";
         public static readonly string Concentration = "Concentration";
         public static readonly string UsedCalculationMethods = "Used Calculation Methods";

         public static readonly string DisplayNameYValue = "Y-Value";

         public static readonly string NewValuePoint = "New Value Point";
         public static readonly string RealativeContainerPath = "Change Realtive Container Path";
         public static readonly string Details = "Details";
         public static readonly string LoadingDiagram = "Loading Diagram...";
         public static readonly string LoadingProject = "Loading Project...";
         public static readonly string Type = "Type";
         public static readonly string ObjectType = "Object Type";
         public static readonly string ObjectName = "Object Name";
         public static readonly string BuildingBlockType = "Building Block Type";
         public static readonly string BuildingBlockName = "Building Block Name";
         public static readonly string Message = "Message";
         public static readonly string SaveToFile = "Save...";
         public static readonly string DecimalPlace = "Decimal place";
         public static readonly string SelectDataToExport = "Select data to Export";
         public static readonly string ReportFile = "File";
         public static readonly string ReportVerbose = "Verbose output (Descriptions, images etc..)";
         public static readonly string ReportDeleteWorkingFolder = "Delete working folder";
         public static readonly string ReportFirstPageSettings = "First page settings";
         public static readonly string ReportOptions = "Options";
         public static readonly string ReportOutput = "Output settings";
         public static readonly string ReportAuthor = "Author";
         public static readonly string ReportTitle = "Title";
         public static readonly string ReportType = "Type";
         public static readonly string ReportSubtitle = "Subtitle";
         public static readonly string ReportOutputFile = "File";
         public static readonly string ReportColor = "Color";
         public static readonly string ReportGrayScale = "Grayscale";
         public static readonly string ReportBlackAndWhite = "Black & White";
         public static readonly string ReportToPDFTitle = "Create PDF Report...";
         public static readonly string SelectReportFile = "Select report file...";
         public static readonly string ReportTemplateSelection = "Template selection";
         public static readonly string ReportTemplate = "Template";
         public static readonly string ValidateDimensions = "Validate dimensions";
         public static readonly string Tree = "Tree";
         public static readonly string MessageOrigin = "Origin";
         public static readonly string CanBeVariedInPopulation = "Can be varied in population";
         public static readonly string ImportObservedData = "Import Observed Data";
         public static readonly string AddUnitMap = "Add Unit";
         public static readonly string DisplayUnit = "Display Unit";
         public static readonly string DefaultDisplayUnits = "Default Display Units";
         public static readonly string ApplicationSettings = "Application";
         public static readonly string OutputSelections = "Output Selections";
         public static readonly string StartImport = "Start Import";
         public static readonly string ImportParameterStartValues = "Import Parameter Start Values";
         public static readonly string ImportParameterQuantitiesFileFormatHint = "The file format must have at least 4 columns. Columns should be Container Path, Parameter Name, Value, and Units." + Environment.NewLine + "The first row is ignored for import.";
         public static readonly string Transfer = "Transfer";
         public static readonly string ManageChartTemplates = "Manage Chart Templates";
         public static readonly string AddTemplate = "Add Template";
         public static readonly string CurveAndAxisSettings = "Curve and Axis Settings";
         public static readonly string ChartSettings = "Chart Options";
         public static readonly string ImportMoleculeStartValues = "Import Molecule Start Values";
         public static readonly string ImportMoleculeStartValuesFileFormatHint = "The file format must have at least 7 columns. Columns should be Container Path, Molecule Name, Is Present, Value, Units, Scale Divisor and Neg. Values Allowed." + Environment.NewLine + "The first row is ignored for import.";
         public static readonly string RunSimulation = "Run Simulation";
         public static readonly string CreateProcessRateParameter = "Create process rate parameter";
         public static readonly string SaveSimulationSettings = "Save Settings into";
         public static readonly string SaveSimulationSettingsToProject = "Project";
         public static readonly string SaveSimulationSettingsToUserSettings = "User Profile";
         public static readonly string CloneTemplate = "Clone Template";
         public static readonly string CreateNewTemplate = "Create New Template";
         public static readonly string LoadTemplate = "Load Template";
         public static readonly string RenameTemplate = "Rename Template";
         public static readonly string Calculate = "Calculate";
         public static readonly string CalculateScaleDivisor = "Calculate Scale Divisor";
         public static readonly string CreatePKSimMoleculeFromTemplate = "Create PK-Sim Molecule From Template";
         public static readonly string RefreshValues = "Start Values";
         public static readonly string RefreshAll = "Refresh all from source";
         public static readonly string RefreshSelected = "Refresh selected from source";
         public static readonly string SaveUnitsToFile = "Save Units";
         public static readonly string LoadUnitsFromFile = "Load Units";
         public static readonly string SaveUnits = "Save Units";
         public static readonly string LoadUnits = "Load Units";
         public static readonly string ExportModelAsTables = "Export Model as Tables...";
         public static readonly string SelectProjectRateDimension = "Select Reaction Rate Base for Project";
         public static readonly string EmptyColumn = " ";
         public static readonly string ReportCreationStarted = "Report creation started...";
         public static readonly string ReportCreationFinished = "Report created!";
         public static readonly string NoValuesFound = "No values imported";
         public static readonly string SelectProjectRateDimensionDescription = "Reactions and their formulas can either compute amount (µmol) or concentration (µmol/l) changes. Please select:";
         public static readonly string AmountBasedModel = "Amount based reactions";
         public static readonly string ConcentrationBasedModel = "Concentration based reactions";
         public static readonly string ContainerMode = "Container Mode";
         public static readonly string ShowOnlyChangedStartValues = "Show only changed values";
         public static readonly string ShowOnlyNewStartValues = "Show only new values";
         public static readonly string Cancel = "Cancel";
         public static readonly string KeepExisting = "Keep existing";
         public static readonly string ReplaceWithMerged = "Use the newly merged";
         public static readonly string ResolveMergeConflict = "Resolve Merge Conflict";
         public static readonly string MergeEntity = "Entity to be merged";
         public static readonly string TargetEntity = "Existing entity";
         public static readonly string AddWithNewName = "Add merged with new name";
         public static readonly string Merge = "Merge";
         public static string AmountRightHandSide = ParameterRightHandSide("N");
         public static string ConcentrationRightHandSide = ParameterRightHandSide("C");
         public static readonly string NumberOfParameters = "Number Of Parameters";
         public static readonly string TransporterMolecules = "Transporter Molecules";
         public static readonly string NumberOfChildren = "Number of Children";
         public static readonly string SourceDescriptor = "Source Descriptor";
         public static readonly string TargetDescriptor = "Target Descriptor";
         public static readonly string ForAll = "For All";
         public static readonly string MoleculesToExclude = "Molecules to Exclude";
         public static readonly string MoleculesToInclude = "Molecules to Include";
         public static readonly string Data = "Data";
         public static readonly string StartValueIsModified = "Start value is modified";
         public static readonly string ExportSelectedObservedDataDescription = "Select the curve that will be exported.";
         public static readonly string ValidationOptions = "Validation Options";
         public static readonly string MRUListItemCount = "Number of recent file items shown";
         public static readonly string ShowPKSimParameterWarnings = "Show known dimension warnings for PK-Sim parameters";
         public static readonly string ShowUnableCalculateWarnings = "Show warnings when formulas dimension could not be calculated";
         public static readonly string ValidatePKSimStandardObserver = "Show warnings from PK-Sim standard observers";
         public static readonly string PlotProcessRateParameter = "Plot process rate parameter";
         public static readonly string Source = "Source";
         public static readonly string Target = "Target";
         public static readonly string CouldNotResolveSource = "Source of start value not defined";
         public static readonly string CurveName = "Curve Name";
         public static readonly string XDataPath = "X-Path";
         public static readonly string YDataPath = "Y-Path";
         public static readonly string XQuantityType = "X-Type";
         public static readonly string YQuantityType = "Y-Type";
         public static readonly string User = "User";
         public static readonly string ImportSimulationParameters = "Import Simulation Parameters";
         public static readonly string DefaultLayout = "Default Layout";
         public static readonly string DefaultChartYScaling = "Default Chart Y Scale";
         public static readonly string DefaultSizeOfNewReaction = "Default Size of New Reaction";
         public static readonly string DefaultSizeOfNewMolecule = "Default Size of New Molecule";
         public static readonly string DefaultSizeOfNewObserver = "Default Size of New Observer";
         public static readonly string ChartItemDefaultColors = "Chart Item Default Colors";
         public static readonly string ContainerLogical = "Container Logical";
         public static readonly string ContainerPhysical = "Container Physical";
         public static readonly string ContainerOpacity = "Container Opacity";
         public static readonly string NeighborhoodLink = "Neighborhood Link";
         public static readonly string NeighborhoodNode = "Neighborhood Node";
         public static readonly string NeighborhoodPort = "Neighborhood Port";
         public static readonly string TransportLink = "Transport Link";
         public static readonly string ObserverNode = "Observer Node";
         public static readonly string ObserverLink = "Observer Link";
         public static readonly string MoleculeNode = "Molecule Node";
         public static readonly string ReactionNode = "Reaction Node";
         public static readonly string ReactionPortEduct = "Reaction Port Educt";
         public static readonly string ReactionLinkEduct = "Reaction Link Educt";
         public static readonly string ReactionPortProduct = "Reaction Port Product";
         public static readonly string ReactionLinkProduct = "Reaction Link Product";
         public static readonly string ReactionPortModifier = "Reaction Port Modifier";
         public static readonly string ReactionLinkModifier = "Reaction Link Modifier";
         public static readonly string DefaultBackground = "Default Background";
         public static readonly string OpenLayoutFromFile = OSPSuite.Assets.Captions.OpenLayoutFromFile;
         public static readonly string SaveChanges = "Save Changes";
         public static readonly string EnableEditing = "Enable Editing";
         public static readonly string DeleteSelected = "Delete Selected";
         public static readonly string DeleteSourceNotDefined = "Delete Source Not Defined";
         public static readonly string DeleteValues = "Delete Values";
         public static readonly string DeleteLinksFromMoleculesFirst = "Delete links from molecules first";
         public static readonly string RHSFormula = "Right hand side";
         public static readonly string ExportObservedDataToExcel = "Export observed data to Excel";
         public static readonly string ContainerPath = "Container Path";
         public static readonly string ValidateRules = "Validate value constraints";
         public static readonly string OpenAnyway = "&Open Anyway";
         public static readonly string MoleculeObserver = "Molecule Observer";
         public static readonly string ContainerObserver = "Container Observer";
         public static readonly string SelectMolecules = "Select Molecules";
         public static readonly string LoadingApplication = "Loading Application...";
         public static readonly string PKSimPath = "PK-Sim executable path";
         public static readonly string UseWatermark = "Use watermark";
         public static readonly string WatermarkText = "Watermark";
         public static readonly string SelectPKSimExecutablePath = "Select PK-Sim executable path";
         public static readonly string CloseView = "Close";
         public static readonly string CloseAll = "Close All Documents";
         public static readonly string CloseAllButThis = "Close All But This";

         public static string ManageDisplayUnits(string type)
         {
            return $"Manage {type} Display Units";
         }

         public static string ParameterRightHandSide(string name)
         {
            return $"d{name}/dt = ";
         }

         public static string ReportCreationStartedMessage(string reportFullPath)
         {
            return "This might take a while...";
         }

         public static string ReportCreationFinishedMessage(string reportFullPath)
         {
            return $"Report can be found at {reportFullPath}";
         }

         public static string ConvertingExcelSheetToQuantities(string tableName)
         {
            return $"Converting Excel sheet {tableName}";
         }

         public static string AddingMoleculeStartValue(IObjectPath moleculePath, double startValueInDisplayUnit, Unit displayUnit, bool isPresent, string moleculeName, bool negativeValueAllowed)
         {
            return $"Adding Molecule Start Value {moleculePath}={startValueInDisplayUnit} {displayUnit}, Is Present={isPresent}, Molecule Name={moleculeName}, Neg. Values Allowed={negativeValueAllowed}";
         }

         public static string AddingParameterStartValue(IObjectPath parameterPath, double startValueInDisplayUnit, Unit displayUnit)
         {
            return $"Adding Parameter Start Value {parameterPath}={startValueInDisplayUnit} {displayUnit}";
         }

         public static string ListOf(string item)
         {
            return $"List of {item}";
         }

         public static string SimulationSettingsSaved(string settingsType)
         {
            return $"Simulation settings where saved into {settingsType.ToLowerInvariant()}.";
         }

         public static string PassiveTransportCaption(string name)
         {
            return buildingBlockCaption(PassiveTransports, name);
         }

         private static string buildingBlockCaption(string builderTypeName, string name)
         {
            return $"{builderTypeName}: {name}";
         }

         public static string EventsBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(Events, name);
         }

         public static string ObserverBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(Observers, name);
         }

         public static string MoleculesBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(Molecules, name);
         }

         public static string ReactionsBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(Reactions, name);
         }

         public static string MoleculeStartValuesBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(MoleculeStartValues, name);
         }

         public static string ParameterStartValuesBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(ParameterStartValues, name);
         }

         public static string SpatialStructureBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(SpatialStructure, name);
         }

         public static string SimulationSettingsBuildingBlockCaption(string name)
         {
            return buildingBlockCaption(SimulationSettings, name);
         }

         public static string SimulationCaption(string name)
         {
            return $"Simulation: {name}";
         }

         public static string NewWindow(string objectName)
         {
            return $"New {objectName}";
         }

         public static string TemporaireStartValuesBasedOn(string startValueType, string name)
         {
            return $"{startValueType} used in simulation based on '{name}'";
         }

         public static string SelectEntitiesToLoad(string searchedEntityType)
         {
            return $"Select {searchedEntityType.ToLower()}s to load";
         }

         public static string ReallyDeleteFormula(string name)
         {
            return $"Really delete formula '{name}'?";
         }

         public static string NumberOfExportedCurveIs(int numberOfSelectedMolecules)
         {
            return $"Number of curve that will be exported: {numberOfSelectedMolecules}";
         }

         public static string NumberOfGeneratedCurves(int numberOfSelectedMolecules)
         {
            return $"Number of curves to generate for this simulation : {numberOfSelectedMolecules}";
         }

         public static string UpdatingSimulation(IObjectBase moBiSimulation)
         {
            return $"Updating simulation: '{moBiSimulation.Name}'";
         }

         public static string CommitToBuildingBlock(IBuildingBlock buildingBlock)
         {
            return $"Commiting changes to building block: '{buildingBlock.Name}'";
         }

         public static string ConfigureSimulation(string simulatioName)
         {
            return $"Configure Simulation: {simulatioName}";
         }

         public const string StartValueNotAvailable = "<Not Available>";
         public const string FormulaNotAvailable = "<Not Available>";

         public static string ApplyToRemaining(int remainingConflicts)
         {
            return $"Apply my choice to {remainingConflicts} remaining conflicts";
         }

         public static string MergeBuildingBlockIntoTarget(string buildingBlockType, string targetBuildingBlockName)
         {
            return $"Merge {buildingBlockType} into {targetBuildingBlockName}";
         }

         public static string SourceBuildingBlockFileForMerge(string buildingBlockType)
         {
            return $"Select file containg the {buildingBlockType.ToLowerInvariant()} to merge";
         }

         public static string UpdatingMoleculeStartValue(IObjectPath moleculePath, double startValueInDisplayUnit, Unit displayUnit, bool isPresent, string moleculeName, bool negativeStartValueAllowed)
         {
            return !double.IsNaN(startValueInDisplayUnit)
               ? $"Updating Molecule Start Value {moleculePath}={startValueInDisplayUnit} {displayUnit}, Is Present={isPresent}, Molecule Name={moleculeName}, Neg. Values Allowed={negativeStartValueAllowed}"
               : $"Updating Molecule Start Value {moleculePath}, Is Present={isPresent}, Molecule Name={moleculeName}, Neg. Values Allowed={negativeStartValueAllowed}";
         }

         public static string UpdatingParameterStartValue(IObjectPath parameterPath, double startValueInDisplayUnit, Unit displayUnit)
         {
            return $"Updating Parameter Start Value {parameterPath}={startValueInDisplayUnit} {displayUnit}";
         }

         public static string UpdatingParameterValueInSimulation(IObjectPath path, double valueInDisplayUnit, Unit displayUnit)
         {
            return $"Updating parameter value in simulation {path}={valueInDisplayUnit} {displayUnit}";
         }

         public static string AddingParameterValueToSimulation(IObjectPath path, double valueInDisplayUnit, Unit displayUnit)
         {
            return $"Adding parameter value in simulation {path}={valueInDisplayUnit} {displayUnit}";
         }

         public static string SelectMoleculePartnersFor(string reactionName, string partnerType)
         {
            return $"Select {partnerType.ToLowerInvariant()} for reaction '{reactionName}'";
         }

         public static string PartnerForReaction(string partnerType, string reactionName)
         {
            return $"{partnerType} for reaction '{reactionName}'";
         }
      }

      public static class Validation
      {
         public static readonly string EmptyName = "Name has to be specified";
         public static readonly string EmptyTransportName = "Transport Name has to be specified";
         public static readonly string MissingStartValues = "Invalid Start Values, create new ones or expand these";
         public static readonly string Percentile = "Percentile should be between 0 and 100";
         public static readonly string EmptyFormula = "Formula has to be specified";
         public static readonly string UndefinedParameter = "Parameter value is not defined, may cause error during computation";
         public static readonly string ChangedEntityNotSet = "Changed entity has to be set";
         public static readonly string NameAllreadyUsed = "Name already in use";
         public static readonly string TransportNameAllreadyUsed = "Transport name already in use";
         public static readonly string ChangeBuildModeWarning = "Changing build mode may corrupt references to this parameter.";
         public static readonly string FixedValueSimulationWarning = "Default is overridden";
         public static readonly string RelativeContainerPathNotSet = "Realtive Container Path has to be specified";
         public static readonly string CannotSetPathElementWhenPreviousElementsAreEmpty = "The path element cannot be set when previous elements in the path are empty";
         public static readonly string EmptyAlias = "Alias has to be specified";
         public static readonly string AliasAllreadyUsed = "Alias is already in use";
         public static readonly string EmptyPath = "Path has to be specified";

         public static string XDimensionColumnMustNotHaveRepeatedValues(string dimensionName)
         {
            return $"{dimensionName} column must not have repeated values";
         }

         public static string MinMax(double min, double max)
         {
            return $"Minimum ({min}) has to be less or equal than Maximum ({max})";
         }

         public static string NoDimensionSet(string name)
         {
            return $"No dimension set for '{name}'";
         }

         public static string ReferenceDimensionMissmatch(string entityUsingFormula, IObjectReference objectReference, IDimension pathDim)
         {
            return $"Referenced object '{objectReference.Object.Name}' at '{entityUsingFormula}' has not the correct dimension '{pathDim}'";
         }

         public static string PropertyShouldBeSet(string propertyName)
         {
            return $"Property '{propertyName}' has to be set";
         }

         public static string GreaterEqualZero(string objectName)
         {
            return $"{objectName} should be greater or equal than 0";
         }

         public static string RenameReaction(string oldName, string newName, IReactionBuildingBlock buildingBlock)
         {
            return string.Format("'{2}' already contains a different reaction named '{0}'. Renamed to '{1}'", oldName, newName, buildingBlock.Name);
         }

         public static string NoDimensionFoundFor(string objectName, string unit)
         {
            return $"Unable to set dimension for '{objectName}'. Unit '{unit}' is not known in any dimension";
         }

         public static string NoDimensionFoundFor(string objectName)
         {
            return $"Unable to set dimension for '{objectName}', no unit specified";
         }

         public static string RHSDimensionMismatch(string objectName, string formulaName)
         {
            return $"The dimension of formula '{formulaName}' does not match the rhs dimension of '{objectName}'";
         }

         public static string DimensionMismatch(string objectName, string formulaName)
         {
            return $"The dimension of formula '{formulaName}' does not match the dimension of '{objectName}'";
         }

         public static string FormulaDimensionMismatch(string displayPath, string dimensionName)
         {
            return $"The formula of '{displayPath}' {DoesNotEvaluateTo(dimensionName)}";
         }

         public static string DoesNotEvaluateTo(string dimensionName) => $"does not evaluate to the dimension '{dimensionName}'";

         public static string PathIsIdenticalToExistingPath(IObjectPath path)
         {
            return $"The path {path} is identical to an existing path in the building block";
         }

         public static string DimensionConversionWarning(string formulaName, string formulaString)
         {
            return $"Dimension check warning: Formula '{formulaName}': {formulaString} possibly needs manual conversion.";
         }

         public static string ValueNotValidForInsert(string path)
         {
            return $"The value with path {path} does not contain a start value and an existing start value with the same path cannot be found";
         }

         public static string ValueNotValidForUpdate(string path)
         {
            return $"The value with path {path} does not contain either a start value or scale divisor. It must contain one for updating the existing start value";
         }

         public static string NameIsAlreadyUsedInThisContainer(string containerPath, string name)
         {
            return $"A start value with the name '{name}' already exists for the container {containerPath}";
         }
      }

      public static readonly string TimeColumName = "Simulationtime";

      public static readonly string ResultName = "Results ";
      public static readonly string BrowseForFile = "Select File";
      public static readonly string Undefined = "Undefined";
      public static readonly string PleaseSelectCurveInChartEditor = "Please select a curve from the chart editor to be displayed in the chart";
      public static readonly IReadOnlyList<string> DefaultObservedDataCategories = new[] {ObservedData.MOLECULE, ObservedData.COMPARTMENT, ObservedData.ORGAN};

      public static string PathType(string pathTypeAsString)
      {
         return $"{pathTypeAsString} path";
      }

      public static class Diagram
      {
         public static class Base
         {
            public static readonly SizeF GridCellSize = new SizeF(10F, 15F);
            public static readonly float PortGravity = 40F;
            public static readonly float MinLimitDocScale = 1 / 5F;
            public static readonly float MaxLimitDocScale = 2F;
            public static readonly PointF InsertLocationOffset = new PointF(0F, 30F);
            public static readonly float ZoomFitToPageFactor = 0F;
            public static readonly int LayoutDepthChildren = 0;
            public static readonly int LayoutDepthGrandChildren = 1;
            public static readonly int LayoutDepthAll = 5;
         }

         public static class Space
         {
            public static readonly float ZoomInFactor = 3 / 2F;
         }

         public static class Model
         {
            public static readonly float ZoomInFactor = 3 / 2F;
         }

         public static readonly double SplitterDiagramRatio = 0.2;

         public static string MoleculeNodeAlreadyExistsForMolecule(string moleculeName)
         {
            return "MoleculeNode for '" + moleculeName + "' already exists. Do you want to add a twin node?";
         }
      }

      public static string RHSDefaultUnitName(IDimension dimension)
      {
         return $"{dimension.BaseUnit.Name}/min";
      }

      public static string RHSDimensionSuffix = " per time";

      public static string RHSDimensionName(IDimension dimension)
      {
         return $"{dimension.Name}{RHSDimensionSuffix}";
      }

      public static readonly IEnumerable<string> UnallowedNames = new List<string>
      {
         string.Empty,
         "E",
         "PI",
         "ACOS",
         "AND",
         "ASIN",
         "ATAN",
         "COS",
         "COSH",
         "EQ",
         "EXP",
         "IF",
         "GT",
         "GEQ",
         "LT",
         "LEQ",
         "LN",
         "LOG",
         "LOG10",
         "MAX",
         "MIN",
         "NOT",
         "OR",
         "POW",
         "RND",
         "SIN",
         "SINH",
         "SQRT",
         "SRND",
         "TAN",
         "TANH",
         "NEQ"
      };


      public static readonly string None = "<None>";
      public static readonly string PKSimTopContainer = "Organism";
      public static readonly string AmountAlias = "M";
      public static readonly string NeighborhoodTag = "Neighborhood";
      public static readonly string OutputIntervalId = "OutputIntervalId";
      public static readonly string SolverSettingsId = "SolverSettingsId";
      public static readonly string SimulationSettingsId = "SimulationSettingsId";
      public static readonly string OffsetAlias = "OFFSET";
      public static readonly string TableAlias = "TABLE";
      public static readonly string Param = "Param";
      public static readonly string RHSFormula = "RHS Formula";

      public static string CloneName(IObjectBase objectToClone)
      {
         return $"Clone of {objectToClone.Name}";
      }

      public static string CompositeNameFor(string name1, string name2)
      {
         return $"{name1}-{name2}";
      }

      public static string ProjectVersionCannotBeLoaded(int projectVersion, int currentVersion, string dowloadUrl)
      {
         if (projectVersion > currentVersion)
            return $"The application is too old (compatible version {currentVersion}) and cannot load a project created with a newer version (project version {projectVersion}).\nVisit our download page at {dowloadUrl}";

         return $"Work in progress.\nThis project file is too old (version {projectVersion}) and cannot be loaded.\nSorry :-(";
      }

      public static class ProjectUpdateMessages
      {
         public static string UpdatePSV(string name)
         {
            return $"Update Parameter Start Values Building Block: '{name}'. Adding Dimension Information";
         }

         public static string UnableToGetDimensionFor(IParameterStartValue psv, string parentName)
         {
            return
               $"Unable to get dimension information for Parameter Start Value: '{psv.Path.PathAsString}' in Parameter Start Values Building Block: '{parentName}'. Dimension is set to No Dimension";
         }

         public static string UpdateErrors(string projectName, IEnumerable<string> messages)
         {
            return messages.Aggregate($"Error(s) during upate of project: '{projectName}'\n", (current, m) => string.Concat(current, $"\n{m}"));
         }

         public static string UpdateErrors(IEnumerable<string> messages)
         {
            return messages.Aggregate(string.Empty, (current, m) => string.Concat(current, $"\n{m}"));
         }
      }

      public static string ContactSupport(string productDisplayName, string support)
      {
         return $"For more information, please contact your {productDisplayName} support ({support})";
      }

      public static class PKSim
      {
         public static readonly string PopulationSimulationArgument = "/pop";
         public static readonly string JournalFileArgument = "/j";
         public static readonly string NotInstalled = "PK-Sim was not found on current system. Please make sure that PK-Sim was installed using the provided setup. Alternatively, you can specify where PK-Sim is installed on your system under Utilities -> Options";
      }

      public static string DefaultFileNameForModelPartsExport(string projectName, string simulationName)
      {
         return $"{projectName}_{simulationName}_Model_Parts";
      }

      public static string CannotRemoveBuildingBlockFromProject(string buildingBlockName, IEnumerable<string> referringBuildingBlockNames)
      {
         return $"Cannot remove building block {buildingBlockName} from the project. It is being used as a template by {referringBuildingBlockNames.ToString(", ")}";
      }

      public class UIConstants
      {
         public static readonly int LargeButtonWidth = 140;
         public static readonly int ButtonHeight = 24;
      }

      public class Diff
      {
         public const string OlderVersion = "Is Older";
         public const string NewerVersion = "Is Newer";
         public const string Version = "Version";

         public static string VersionDescription(string s)
         {
            return $"Left buildingblock {s.ToLower()} then right one";
         }
      }

      public class MoBiObjectTypes
      {
         public static readonly string Data = "Data";
      }
   }
}