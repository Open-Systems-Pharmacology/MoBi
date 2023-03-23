using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.FuncParser;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class DimensionValidator : IDimensionValidator
   {
      private readonly DimensionParser _dimensionParser;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IUserSettings _userSettings;
      private bool _checkDimensions;
      private SimulationConfiguration _simulationConfiguration;
      private ValidationResult _result;
      private readonly IDictionary<string, string> _hiddenNotifications;
      private bool _checkRules;

      public DimensionValidator(DimensionParser dimensionParser, IObjectPathFactory objectPathFactory, IUserSettings userSettings)
      {
         _dimensionParser = dimensionParser;
         _objectPathFactory = objectPathFactory;
         _userSettings = userSettings;
         _hiddenNotifications = new Dictionary<string, string>
         {
            {AppConstants.Parameters.SURFACE_AREA_INTERSTITIAL_INTRACELLULAR, dimensionAreaMessage},
            {AppConstants.Parameters.BSA, dimensionAreaMessage},
            {AppConstants.Parameters.PERMEABILITY, dimensionVelocityMessage},
            {AppConstants.Parameters.SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR, dimensionVelocityMessage},
            {AppConstants.Parameters.RADIUS_SOLUTE, dimensionValidationMessage(AppConstants.DimensionNames.LENGTH)},
            {AppConstants.Parameters.SECRETION_OF_LIQUID, dimensionFlowMessage},
            {Constants.Parameters.VOLUME, $"Organism|Saliva|Volume' {dimensionValidationMessage(Constants.Dimension.VOLUME)}"},
            {AppConstants.Parameters.RELEASE_RATE_OF_TABLET, dimensionValidationMessage(Constants.Dimension.AMOUNT_PER_TIME)},
            {AppConstants.Parameters.V_MAX, dimensionValidationMessage(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME)},
            {AppConstants.Parameters.LYMPH_FLOW_RATE, dimensionFlowMessage},
            {AppConstants.Parameters.LYMPH_FLOW_RATE_INCL_MUCOSA, dimensionFlowMessage},
            {AppConstants.Parameters.FLUID_RECIRCULATION_FLOW_RATE, dimensionFlowMessage},
            {AppConstants.Parameters.FLUID_RECIRCULATION_FLOW_RATE_INCL_MUCOSA, dimensionFlowMessage},
            {AppConstants.Parameters.CALCULATED_SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR, dimensionVelocityMessage},
            {AppConstants.Parameters.EFFECTIVE_MOLECULAR_WEIGHT, "Arguments of MINUS-function must have the same dimension (Formula: MW - F * 0.000000017 - Cl * 0.000000022 - Br * 0.000000062 - I * 0.000000098)"}
         };
      }

      private static string dimensionValidationMessage(string dimensionName) => AppConstants.Validation.DoesNotEvaluateTo(dimensionName);
      private static string dimensionFlowMessage { get; } = AppConstants.Validation.DoesNotEvaluateTo(AppConstants.DimensionNames.FLOW);
      private static string dimensionVelocityMessage { get; } = AppConstants.Validation.DoesNotEvaluateTo(AppConstants.DimensionNames.VELOCITY);
      private static string dimensionAreaMessage { get; } = AppConstants.Validation.DoesNotEvaluateTo(AppConstants.DimensionNames.AREA);

      public Task<ValidationResult> Validate(IContainer container, SimulationConfiguration simulationConfiguration) => Validate(new[] {container}, simulationConfiguration);

      public Task<ValidationResult> Validate(IModel model, SimulationConfiguration simulationConfiguration) => Validate(new[] {model.Root, model.Neighborhoods}, simulationConfiguration);

      public Task<ValidationResult> Validate(IEnumerable<IContainer> containers, SimulationConfiguration simulationConfiguration)
      {
         return Task.Run(() =>
         {
            try
            {
               _result = new ValidationResult();
               _checkDimensions = _userSettings.CheckDimensions;
               _checkRules = _userSettings.CheckRules;
               _simulationConfiguration = simulationConfiguration;
               containers.Each(c => c.AcceptVisitor(this));
               return _result;
            }
            finally
            {
               _simulationConfiguration = null;
               _result = null;
            }
         });
      }

      public void Visit(IEntity entity)
      {
         if (!_checkRules) return;

         entity.Rules.BrokenBy(entity).Messages.Each(x => addNotification(NotificationType.Warning, entity, x));
      }

      public void Visit(IUsingFormula entityUsingFormula)
      {
         try
         {
            Visit(entityUsingFormula as IEntity);

            if (!_checkDimensions) return;

            var formula = entityUsingFormula.Formula;
            if (formula.IsConstant()) return; //do not need to check constants

            var displayPath = _objectPathFactory.CreateAbsoluteObjectPath(entityUsingFormula).PathAsString;
            if (entityUsingFormula.Dimension == null)
            {
               addWarning(entityUsingFormula, AppConstants.Validation.NoDimensionSet(displayPath));
               return;
            }

            checkFormula(entityUsingFormula, displayPath);
            checkRHSFormula(entityUsingFormula as IParameter, displayPath);
         }
         catch (Exception exception)
         {
            addNotification(NotificationType.Error, entityUsingFormula, exception.Message);
         }
      }

      private void checkRHSFormula(IParameter parameter, string displayPath)
      {
         var rhsFormula = parameter?.RHSFormula;
         if (rhsFormula == null) return;

         var rhsDimBaseRep = new BaseDimensionRepresentation(parameter.Dimension.BaseRepresentation);
         rhsDimBaseRep.TimeExponent = rhsDimBaseRep.TimeExponent - 1;

         if (!Equals(rhsDimBaseRep, rhsFormula.Dimension.BaseRepresentation))
            addWarning(parameter, AppConstants.Validation.RHSDimensionMismatch(displayPath, rhsFormula.Name));

         checkExplicitFormula(parameter, displayPath, rhsDimBaseRep, rhsFormula as ExplicitFormula);
      }

      private void addWarning(IObjectBase entityToValidate, string warning) => addNotification(NotificationType.Warning, entityToValidate, warning);

      private void addNotification(NotificationType notificationType, IObjectBase entityToValidate, string notification)
      {
         var builder = _simulationConfiguration.BuilderFor(entityToValidate);
         if (!shouldShowNotification(entityToValidate, notification))
            return;

         _result.AddMessage(notificationType, builder, notification);
      }

      private bool shouldShowNotification(IObjectBase entityToValidate, string notification)
      {
         if (_userSettings.ShowPKSimDimensionProblemWarnings)
            return true;

         if (!_hiddenNotifications.Keys.Contains(entityToValidate.Name))
            return true;

         var messageEnd = _hiddenNotifications[entityToValidate.Name];
         if (notification.EndsWith(messageEnd))
            return false;

         return true;
      }

      private void checkFormula(IUsingFormula entityUsingFormula, string displayPath)
      {
         if (!Equals(entityUsingFormula.Dimension, entityUsingFormula.Formula.Dimension))
            addWarning(entityUsingFormula, AppConstants.Validation.DimensionMismatch(displayPath, entityUsingFormula.Formula.Name));

         checkExplicitFormula(entityUsingFormula, displayPath, entityUsingFormula.Dimension.BaseRepresentation, entityUsingFormula.Formula as ExplicitFormula);
      }

      private void checkExplicitFormula(IUsingFormula entityUsingFormula, string displayPath, BaseDimensionRepresentation baseDimensionRepresentation, ExplicitFormula explicitFormula)
      {
         if (explicitFormula == null) return;

         var dimensionInfos = new List<QuantityDimensionInformation>();
         if (isDoubleString(explicitFormula.FormulaString))
            return;

         foreach (var objectReference in explicitFormula.ObjectReferences.Where(x => x.Object.Dimension != null))
         {
            var pathDim = explicitFormula.ObjectPaths.Where(path => path.Alias.Equals(objectReference.Alias)).Select(path => path.Dimension).FirstOrDefault();
            if (!Equals(objectReference.Object.Dimension, pathDim))
               addWarning(entityUsingFormula, AppConstants.Validation.ReferenceDimensionMissmatch(displayPath, objectReference, pathDim));

            dimensionInfos.Add(new QuantityDimensionInformation(objectReference.Alias, createDimensionInfoFromBaseRepresentation(objectReference.Object.Dimension.BaseRepresentation)));
         }

         var (dimInfo, parseSuccess, calculateDimensionSuccess, errorMessage ) = _dimensionParser.GetDimensionInformationFor(explicitFormula.FormulaString, dimensionInfos);

         if (!parseSuccess)
         {
            addNotification(NotificationType.Error, entityUsingFormula, errorMessage);
            return;
         }


         if (calculateDimensionSuccess)
         {
            if (!dimInfo.AreEquals(createDimensionInfoFromBaseRepresentation(baseDimensionRepresentation)))
            {
               addWarning(entityUsingFormula, AppConstants.Validation.FormulaDimensionMismatch(displayPath, explicitFormula.Dimension.Name));
            }

            return;
         }

         //ignore some dimension check errors.
         //Reason: some formulas are written so that dimension exponents cannot be calculated, e.g.
         //x^y where y is not dimensionless. 
         if (_userSettings.ShowCannotCalcErrors)
         {
            addWarning(entityUsingFormula, errorMessage);
         }
      }

      private bool isDoubleString(string stringToCheck)
      {
         return double.TryParse(stringToCheck, out _);
      }

      private DimensionInformation createDimensionInfoFromBaseRepresentation(BaseDimensionRepresentation baseRepresentation)
      {
         return new DimensionInformation(baseRepresentation.LengthExponent,
            baseRepresentation.MassExponent,
            baseRepresentation.TimeExponent,
            baseRepresentation.ElectricCurrentExponent,
            baseRepresentation.TemperatureExponent,
            baseRepresentation.AmountExponent,
            baseRepresentation.LuminousIntensityExponent);
      }
   }
}