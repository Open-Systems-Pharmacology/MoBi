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
   public interface IInitialConditionsPresenter : IBuildingBlockWithInitialConditionsPresenter<InitialConditionsBuildingBlock>, IPresenterWithContextMenu<IViewItem>
   {
      void AddNewInitialConditions();
   }

   public class InitialConditionsPresenter : BuildingBlockWithInitialConditionsPresenter<
         IInitialConditionsView,
         IBuildingBlockWithInitialConditionsPresenter,
         InitialConditionsBuildingBlock>,
      IInitialConditionsPresenter
   {
      private readonly IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionsTask;
      private readonly IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper _buildingBlockMapper;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IDialogCreator _dialogCreator;

      public InitialConditionsPresenter(
         IInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper dtoMapper,
         IInitialConditionsTask<InitialConditionsBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IInitialConditionsDistributedPathAndValueEntityPresenter distributedParameterPresenter,
         IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper buildingBlockMapper,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IDialogCreator dialogCreator)
         : base(view, dtoMapper, initialConditionsTask, msvCreator, context, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _initialConditionsTask = initialConditionsTask;
         _buildingBlockMapper = buildingBlockMapper;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _dialogCreator = dialogCreator;
      }

      public void AddNewInitialConditions()
      {
         var allSelected = _initialConditionsTask.GetNewPaths();
         _context.AddToHistory(addInitialConditionsForObjectPaths(allSelected, _buildingBlock));
      }

      private int longestPathLength(InitialConditionsBuildingBlock buildingBlock) => buildingBlock.Any() ? buildingBlock.Max(x => x.Path.Count) : 0;

      private IMoBiCommand addInitialConditionsForObjectPaths(IReadOnlyList<ObjectPath> objectPaths, InitialConditionsBuildingBlock buildingBlockToAddTo)
      {
         var allSkipped = objectPaths.Where(x => buildingBlockToAddTo.FindByPath(x) != null).ToList();

         var objectPathsToAdd = objectPaths.Except(allSkipped).ToList();

         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = new ObjectTypeResolver().TypeFor<InitialCondition>(),
            CommandType = AppConstants.Commands.AddCommand,
            Description = AppConstants.Commands.AddNewInitialConditions(buildingBlockToAddTo.DisplayName)
         };

         var maxPathLengthBeforeAdd = longestPathLength(_buildingBlock);
         macroCommand.AddRange(objectPathsToAdd.Select(entityPath => addAndUpdatePath(buildingBlockToAddTo, entityPath)));

         var maxPathLengthAfterAdd = longestPathLength(_buildingBlock);
         if (maxPathLengthAfterAdd > maxPathLengthBeforeAdd)
            _view.InitializePathColumns();

         if (allSkipped.Any())
            _dialogCreator.MessageBoxInfo(AppConstants.Captions.InitialConditionsBuildingBlockAlreadyContains(allSkipped.Select(x => x.PathAsString).ToList()));

         return macroCommand;
      }

      private IMoBiCommand addAndUpdatePath(InitialConditionsBuildingBlock buildingBlockToAddTo, ObjectPath entityPath)
      {
         var addedDTO = AddNewEmptyPathAndValueEntity();
         return _initialConditionsTask.AddWithFullPath(addedDTO.InitialCondition, entityPath, buildingBlockToAddTo);
      }

      protected override IReadOnlyList<InitialConditionDTO> ValueDTOsFor(InitialConditionsBuildingBlock buildingBlock) => _buildingBlockMapper.MapFrom(buildingBlock).ParameterDTOs;
      protected override void SelectEntity(InitialConditionDTO dto) => _view.Select(dto);
      protected override InitialConditionDTO DTOForBuilder(InitialCondition builder) => _startValueDTOs.FirstOrDefault(x => Equals(x.PathWithValueObject, builder));

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation) =>
         _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this).Show(_view, popupLocation);
   }
}