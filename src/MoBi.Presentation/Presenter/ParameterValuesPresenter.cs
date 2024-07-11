using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
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
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterValuesPresenter : IExtendablePathAndValueBuildingBlockPresenter<ParameterValueDTO>, IEditPresenter<ParameterValuesBuildingBlock>, IPresenterWithContextMenu<IViewItem>
   {
      void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension);
      void AddNewParameterValue();
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
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IModalPresenter _modalPresenter;
      private readonly ISelectReferenceAtParameterValuePresenter _referenceAtParamValuePresenter;

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
         IDimensionFactory dimensionFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IModalPresenter modalPresenter,
         ISelectReferenceAtParameterValuePresenter selectReferenceAtParameterValuePresenter) : base(view, valueMapper, parameterValuesTask, parameterValuesCreator, context, deletePathAndValueEntityPresenter, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _parameterValuesTask = parameterValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper = parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _modalPresenter = modalPresenter;
         _referenceAtParamValuePresenter = selectReferenceAtParameterValuePresenter;
         _modalPresenter.Encapsulate(this);
         _modalPresenter.Text = AppConstants.Captions.SelectLocalReferencePoint;
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

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation) =>
         _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this).Show(_view, popupLocation);

      public void AddNewParameterValue()
      {
         _referenceAtParamValuePresenter.Init(null, new List<IObjectBase>(), null);

         _modalPresenter.Encapsulate(_referenceAtParamValuePresenter);
         if (!_modalPresenter.Show())
            return;

         AddNewEmptyPathAndValueEntity(_referenceAtParamValuePresenter.GetSelection());
         _view.InitializePathColumns();
      }

      public void AddNewEmptyPathAndValueEntity(ObjectPath entityPath)
      {
         var newParameterValue = _emptyStartValueCreator.CreateEmptyStartValue(_interactionTasksForExtendablePathAndValueEntity.GetDefaultDimension());
         newParameterValue.Path = entityPath;


         // newRecord.ContainerPath = entityPath.ContainerPath();
         // newRecord.Name = entityPath.Last();
         // newRecord.ParameterValue.Name = entityPath.Last();


         var newRecord = _valueMapper.MapFrom(newParameterValue, _buildingBlock);

         _startValueDTOs.Insert(0, newRecord);
         BindToView();
      }
    }
}