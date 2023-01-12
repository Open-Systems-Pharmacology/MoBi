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
   public interface IParameterStartValuesPresenter : IStartValuesPresenter<ParameterStartValueDTO>, IEditPresenter<IParameterStartValuesBuildingBlock>
   {
      void UpdateDimension(ParameterStartValueDTO startValueObject, IDimension newDimension);
   }

   public class ParameterStartValuesPresenter
      : StartValuePresenter<IParameterStartValuesView,
            IParameterStartValuesPresenter,
            IParameterStartValuesBuildingBlock,
            ParameterStartValueDTO, ParameterStartValue>,
         IParameterStartValuesPresenter
   {
      private readonly IParameterStartValuesTask _parameterStartValuesTask;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ParameterStartValuesPresenter(
         IParameterStartValuesView view,
         IParameterStartValueToParameterStartValueDTOMapper startValueMapper,
         IRefreshStartValueFromOriginalBuildingBlockPresenter refreshStartValuesPresenter,
         IParameterStartValuesTask parameterStartValuesTask,
         IParameterStartValuesCreator csvCreator,
         IMoBiContext context,
         IDisplayUnitRetriever displayUnitRetriever,
         ILegendPresenter legendPresenter,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, startValueMapper, refreshStartValuesPresenter, parameterStartValuesTask, csvCreator, context, legendPresenter, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _parameterStartValuesTask = parameterStartValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         view.HideIsPresentView();
         view.HideNegativeValuesAllowedView();
      }

      public override void AddNewFormula(ParameterStartValueDTO parameterStartValueDTO)
      {
         var parameterStartValue = StartValueFrom(parameterStartValueDTO);
         AddNewFormula(parameterStartValueDTO, parameterStartValue);
      }

      public void UpdateDimension(ParameterStartValueDTO startValueObject, IDimension newDimension)
      {
         var macroCommand = new MoBiMacroCommand();
         var startValue = StartValueFrom(startValueObject);

         macroCommand.CommandType = AppConstants.Commands.EditCommand;
         macroCommand.Description = AppConstants.Commands.UpdateDimensionsAndUnits;
         macroCommand.ObjectType = new ObjectTypeResolver().TypeFor<ParameterStartValue>();

         var value = startValue.ConvertToDisplayUnit(startValue.Value);

         macroCommand.AddCommand(_parameterStartValuesTask.UpdateStartValueDimension(_buildingBlock, startValue, newDimension));
         macroCommand.AddCommand(_parameterStartValuesTask.SetDisplayValueWithUnit(startValue, value, _displayUnitRetriever.PreferredUnitFor(startValue), _buildingBlock));

         AddCommand(macroCommand);
      }
   }
}