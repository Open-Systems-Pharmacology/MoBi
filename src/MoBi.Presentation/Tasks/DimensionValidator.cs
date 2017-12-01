using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.FuncParser;
using OSPSuite.Utility.Extensions;
using DimensionInfo = OSPSuite.FuncParser.DimensionInfo;
using static MoBi.Assets.AppConstants.Parameters;
using static MoBi.Assets.AppConstants.DimensionNames;

namespace MoBi.Presentation.Tasks
{
   public class DimensionValidator : IDimensionValidator
   {
      private readonly IDimensionParser _dimensionParser;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IUserSettings _userSettings;
      private bool _checkDimensions;
      private IBuildConfiguration _buildConfiguration;
      private ValidationResult _result;
      private readonly IDictionary<string, string> _hiddenNotifications;
      private bool _checkRules;

      public DimensionValidator(IDimensionParser dimensionParser, IObjectPathFactory objectPathFactory, IUserSettings userSettings)
      {
         _dimensionParser = dimensionParser;
         _objectPathFactory = objectPathFactory;
         _userSettings = userSettings;
         _hiddenNotifications = new Dictionary<string, string>
         {
            {SURFACE_AREA_INTERSTITIAL_INTRACELLULAR, dimensionAreaMessage},
            {BSA, dimensionAreaMessage},
            {PERMEABILITY, dimensionVelocityMessage},
            {SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR, dimensionVelocityMessage},
            {RADIUS_SOLUTE, dimensionValidationMessage(LENGTH)},
            {SECRETION_OF_LIQUID, dimensionFlowMessage},
            {Constants.Parameters.VOLUME, $"Organism|Saliva|Volume' {dimensionValidationMessage(Constants.Dimension.VOLUME)}"},
            {RELEASE_RATE_OF_TABLET, dimensionValidationMessage(Constants.Dimension.AMOUNT_PER_TIME)},
            {V_MAX, dimensionValidationMessage(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME)},
            {LYMPH_FLOW_RATE, dimensionFlowMessage},
            {LYMPH_FLOW_RATE_INCL_MUCOSA, dimensionFlowMessage},
            {FLUID_RECIRCULATION_FLOW_RATE, dimensionFlowMessage},
            {FLUID_RECIRCULATION_FLOW_RATE_INCL_MUCOSA, dimensionFlowMessage},
            {CALCULATED_SPECIFIC_INTESTINAL_PERMEABILITY_TRANSCELLULAR, dimensionVelocityMessage},
            {EFFECTIVE_MOLECULAR_WEIGHT, "Arguments of MINUS-function must have the same dimension (Formula: MW - F * 0.000000017 - Cl * 0.000000022 - Br * 0.000000062 - I * 0.000000098)"}
         };
      }

      private static string dimensionValidationMessage(string dimensionName) => AppConstants.Validation.DoesNotEvaluateTo(dimensionName);
      private static string dimensionFlowMessage { get; } =  AppConstants.Validation.DoesNotEvaluateTo(FLOW);
      private static string dimensionVelocityMessage { get; } =  AppConstants.Validation.DoesNotEvaluateTo(VELOCITY);
      private static string dimensionAreaMessage { get; } =  AppConstants.Validation.DoesNotEvaluateTo(AREA);

      public Task<ValidationResult> Validate(IContainer container, IBuildConfiguration buildConfiguration) => Validate(new[] { container }, buildConfiguration);

      public Task<ValidationResult> Validate(IModel model, IBuildConfiguration buildConfiguration) => Validate(new[] { model.Root, model.Neighborhoods }, buildConfiguration);

      public Task<ValidationResult> Validate(IEnumerable<IContainer> containers, IBuildConfiguration buildConfiguration)
      {
         return Task.Run(() =>
         {
            try
            {
               _result = new ValidationResult();
               _checkDimensions = _userSettings.CheckDimensions;
               _checkRules = _userSettings.CheckRules;
               _buildConfiguration = buildConfiguration;
               containers.Each(c => c.AcceptVisitor(this));
               return _result;
            }
            finally
            {
               _buildConfiguration = null;
               _result = null;
            }
         });
      }

      public void Visit(IEntity entity)
      {
         if (!_checkRules) return;

         entity.Rules.BrokenBy(entity).Messages.Each(x=> addNotification(NotificationType.Warning, entity, x));
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
            addWarning( parameter, AppConstants.Validation.RHSDimensionMismatch(displayPath, rhsFormula.Name));

         checkExplicitFormula(parameter, displayPath, rhsDimBaseRep, rhsFormula as ExplicitFormula);
      }

      private void addWarning(IObjectBase entityToValidate, string warning) => addNotification(NotificationType.Warning, entityToValidate, warning);

      private void addNotification(NotificationType notificationType, IObjectBase entityToValidate, string notification)
      {
         var builder = _buildConfiguration.BuilderFor(entityToValidate);
         if (!shouldShowNotifiction(entityToValidate, notification))
            return;

         _result.AddMessage(notificationType, builder, notification);
      }

      private bool shouldShowNotifiction(IObjectBase entityToValidate, string notification)
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

         var dimensionInfos = new List<IQuantityDimensionInfo>();
         if (isDoubleString(explicitFormula.FormulaString))
            return;

         foreach (var objectReference in explicitFormula.ObjectReferences.Where(x => x.Object.Dimension != null))
         {
            var pathDim = explicitFormula.ObjectPaths.Where(path => path.Alias.Equals(objectReference.Alias)).Select(path => path.Dimension).FirstOrDefault();
            if (!Equals(objectReference.Object.Dimension, pathDim))
               addWarning(entityUsingFormula, AppConstants.Validation.ReferenceDimensionMissmatch(displayPath, objectReference, pathDim));

            dimensionInfos.Add(new QuantityDimensionInfo(objectReference.Alias, createDimensionInfoFromBaseRepresentation(objectReference.Object.Dimension.BaseRepresentation)));
         }

         IFuncParserErrorData ed = new FuncParserErrorData();
         var verifyDimension = _dimensionParser.GetDimensionInfoFor(explicitFormula.FormulaString, dimensionInfos, ed);

         if (!ed.ErrorNumber.Equals(errNumber.err_OK))
         {
            //ignore some dimension check errors.
            //Reason: some formulas are written so that dimension exponents cannot be calculated, e.g.
            //x^y where y is not dimensionless. 
            //
            //In this case, err_CANNOTCALC_DIMENSION is returned. 
            //In all other cases, generate error or warning, depending on the error number returned

            if (ed.ErrorNumber != errNumber.err_CANNOTCALC_DIMENSION || (_userSettings.ShowCannotCalcErrors && ed.ErrorNumber == errNumber.err_CANNOTCALC_DIMENSION))
            {
               var notificationType = ed.ErrorNumber.IsOneOf(errNumber.err_DIMENSION, errNumber.err_CANNOTCALC_DIMENSION)
                  ? NotificationType.Warning
                  : NotificationType.Error;

               addNotification( notificationType, entityUsingFormula, ed.Description);
            }
         }

         else if (!verifyDimension.AreEquals(createDimensionInfoFromBaseRepresentation(baseDimensionRepresentation)))
         {
            addWarning(entityUsingFormula, AppConstants.Validation.FormulaDimensionMismatch(displayPath, explicitFormula.Dimension.Name));
         }
      }

      private bool isDoubleString(string stringTocheck)
      {
         return double.TryParse(stringTocheck, out double value);
      }

      private DimensionInfo createDimensionInfoFromBaseRepresentation(BaseDimensionRepresentation baseRepresentation)
      {
         return new DimensionInfo(baseRepresentation.LengthExponent,
            baseRepresentation.MassExponent,
            baseRepresentation.TimeExponent,
            baseRepresentation.ElectricCurrentExponent,
            baseRepresentation.TemperatureExponent,
            baseRepresentation.AmountExponent,
            baseRepresentation.LuminousIntensityExponent);
      }
   }
}
