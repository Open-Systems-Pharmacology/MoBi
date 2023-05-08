using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterValuesPresenter : IStartValuesPresenter<ParameterValueDTO>, IEditPresenter<ParameterValuesBuildingBlock>
   {
      void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension);
   }

   public class ParameterValuesPresenter
      : StartValuePresenter<IParameterValuesView,
            IParameterValuesPresenter,
            ParameterValuesBuildingBlock,
            ParameterValueDTO, ParameterValue>,
         IParameterValuesPresenter
   {
      private readonly IParameterValuesTask _parameterValuesTask;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ParameterValuesPresenter(
         IParameterValuesView view,
         IParameterValueToParameterValueDTOMapper valueMapper,
         IRefreshStartValueFromOriginalBuildingBlockPresenter refreshStartValuesPresenter,
         IParameterValuesTask parameterValuesTask,
         IParameterValuesCreator csvCreator,
         IMoBiContext context,
         IDisplayUnitRetriever displayUnitRetriever,
         ILegendPresenter legendPresenter,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, valueMapper, refreshStartValuesPresenter, parameterValuesTask, csvCreator, context, legendPresenter, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _parameterValuesTask = parameterValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         view.HideIsPresentView();
         view.HideNegativeValuesAllowedView();
      }

      public override void AddNewFormula(ParameterValueDTO parameterValueDTO)
      {
         var parameterValue = StartValueFrom(parameterValueDTO);
         AddNewFormula(parameterValueDTO, parameterValue);
      }

      public void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension)
      {
         var macroCommand = new MoBiMacroCommand();
         var startValue = StartValueFrom(valueObject);

         macroCommand.CommandType = AppConstants.Commands.EditCommand;
         macroCommand.Description = AppConstants.Commands.UpdateDimensionsAndUnits;
         macroCommand.ObjectType = new ObjectTypeResolver().TypeFor<ParameterValue>();

         var value = startValue.ConvertToDisplayUnit(startValue.Value);

         macroCommand.AddCommand(_parameterValuesTask.UpdateStartValueDimension(_buildingBlock, startValue, newDimension));
         macroCommand.AddCommand(_parameterValuesTask.SetDisplayValueWithUnit(startValue, value, _displayUnitRetriever.PreferredUnitFor(startValue), _buildingBlock));

         AddCommand(macroCommand);
      }
   }
}