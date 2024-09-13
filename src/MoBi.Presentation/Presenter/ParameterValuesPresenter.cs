using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterValuesPresenter : IExtendablePathAndValueBuildingBlockPresenter<ParameterValueDTO>, IEditPresenter<ParameterValuesBuildingBlock>, IPresenterWithContextMenu<IViewItem>
   {
      void UpdateDimension(ParameterValueDTO valueObject, IDimension newDimension);
      void AddNewParameterValues();
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
      private readonly IDialogCreator _dialogCreator;

      public ParameterValuesPresenter(
         IParameterValuesView view,
         IParameterValueToParameterValueDTOMapper valueMapper,
         IParameterValuesTask parameterValuesTask,
         IParameterValuesCreator parameterValuesCreator,
         IMoBiContext context,
         IDisplayUnitRetriever displayUnitRetriever,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IParameterValueDistributedPathAndValueEntityPresenter distributedParameterPresenter,
         IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper,
         IDimensionFactory dimensionFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IDialogCreator dialogCreator) : base(view, valueMapper, parameterValuesTask, parameterValuesCreator, context, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _parameterValuesTask = parameterValuesTask;
         _displayUnitRetriever = displayUnitRetriever;
         _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper = parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _dialogCreator = dialogCreator;
         view.HideElement(HideableElement.RefreshButton);
         view.HideElement(HideableElement.PresenceRibbon);
         view.HideElement(HideableElement.NegativeValuesRibbon);
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

      public void AddNewParameterValues()
      {
         var allSelected = _parameterValuesTask.GetNewPaths();
         var buildingBlockWasEmpty = !_buildingBlock.Any();
         _context.AddToHistory(addParameterValuesForObjectPaths(allSelected, _buildingBlock));

         if (buildingBlockWasEmpty)
            _view.InitializePathColumns();
      }

      private IMoBiCommand addParameterValuesForObjectPaths(IReadOnlyList<ObjectPath> objectPaths, ParameterValuesBuildingBlock buildingBlockToAddTo)
      {
         var allSkipped = objectPaths.Where(x => alreadyIn(x, buildingBlockToAddTo)).ToList();

         var objectPathsToAdd = objectPaths.Except(allSkipped).ToList();

         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = new ObjectTypeResolver().TypeFor<ParameterValue>(),
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddNewParameterValues(buildingBlockToAddTo.DisplayName)
         };

         macroCommand.AddRange(objectPathsToAdd.Select(entityPath => addAndUpdatePath(buildingBlockToAddTo, entityPath)));

         if (allSkipped.Any())
            _dialogCreator.MessageBoxInfo(AppConstants.Captions.BuildingBlockAlreadyContains(allSkipped.Select(x => x.PathAsString).ToList()));

         return macroCommand;
      }

      private IMoBiCommand addAndUpdatePath(ParameterValuesBuildingBlock buildingBlockToAddTo, ObjectPath entityPath)
      {
         var addedDTO = AddNewEmptyPathAndValueEntity();
         return _parameterValuesTask.SetFullPath(addedDTO.ParameterValue, entityPath, buildingBlockToAddTo);
      }

      private bool alreadyIn(ObjectPath objectPath, ParameterValuesBuildingBlock buildingBlock)
      {
         return buildingBlock.FindByPath(objectPath) != null;
      }
   }
}