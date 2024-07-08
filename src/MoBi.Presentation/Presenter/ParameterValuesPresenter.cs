using System.Collections.Generic;
using System.Drawing;
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
   public interface IParameterValuesPresenter : IExtendablePathAndValueBuildingBlockPresenter<ParameterValueDTO>, IEditPresenter<ParameterValuesBuildingBlock>
   {
      void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension);
   }

   public class ParameterValuesPresenter
      : ExtendablePathAndValueBuildingBlockPresenter<IParameterValuesView,
            IParameterValuesPresenter,
            ParameterValuesBuildingBlock,
            ParameterValueDTO, ParameterValue>,
         IParameterValuesPresenter
   {
      private readonly IParameterValuesTask _parameterValuesTask;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;

      public ParameterValuesPresenter(
         IParameterValuesView view,
         IParameterValueToParameterValueDTOMapper valueMapper,
         IParameterValuesTask parameterValuesTask,
         IParameterValuesCreator parameterValuesCreator,
         IMoBiContext context,
         IDisplayUnitRetriever displayUnitRetriever,
         IDeletePathAndValueEntityPresenter deletePathAndValueEntityPresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IParameterValueDistributedPathAndValueEntityPresenter distributedParameterPresenter,
         IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, valueMapper, parameterValuesTask, parameterValuesCreator, context, deletePathAndValueEntityPresenter, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _parameterValuesTask = parameterValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper = parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;
         view.HideIsPresentView();
         view.HideRefreshView();
         view.HideNegativeValuesAllowedView();
      }

      protected override string RemoveCommandDescription()
      {
         return AppConstants.Commands.RemoveMultipleParameterValues;
      }

      protected override IReadOnlyList<ParameterValueDTO> ValueDTOsFor(ParameterValuesBuildingBlock buildingBlock)
      {
         return _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper.MapFrom(buildingBlock).ParameterDTOs;
      }

      public void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension)
      {
         var macroCommand = new MoBiMacroCommand();
         var pathAndValueEntity = PathAndValueEntityFrom(valueObject);

         macroCommand.CommandType = AppConstants.Commands.EditCommand;
         macroCommand.Description = AppConstants.Commands.UpdateDimensionsAndUnits;
         macroCommand.ObjectType = new ObjectTypeResolver().TypeFor<ParameterValue>();

         var value = pathAndValueEntity.ConvertToDisplayUnit(pathAndValueEntity.Value);

         macroCommand.AddCommand(_parameterValuesTask.UpdatePathAndValueEntityDimension(_buildingBlock, pathAndValueEntity, newDimension));
         macroCommand.AddCommand(_parameterValuesTask.SetDisplayValueWithUnit(pathAndValueEntity, value, _displayUnitRetriever.PreferredUnitFor(pathAndValueEntity), _buildingBlock));

         AddCommand(macroCommand);
      }

      public void ShowContextMenu(object o, Point eLocation) => o=null; // todo remove and implement
        // _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this).Show(_view, popupLocation);

   }
}