using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IFormulaCachePresenter : IEditPresenter<IBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IListener<FormulaChangedEvent>,
      IListener<EntitySelectedEvent>,
      IListener<BulkUpdateStartedEvent>,
      IListener<BulkUpdateFinishedEvent>
   {
      void Select(FormulaBuilderDTO formulaDTO);
      void Remove(FormulaBuilderDTO formulaDTO);
      void Rename(FormulaBuilderDTO formulaDTO);
      void Clone(FormulaBuilderDTO formulaDTO);
      void Copy(FormulaBuilderDTO formulaDTO);
      void Paste(FormulaBuilderDTO formulaDTO);
      void Select(IFormula formula);
      bool FormulasExistOnClipBoard();
   }

   public class FormulaCachePresenter : AbstractEditPresenter<IFormulaCacheView, IFormulaCachePresenter, IBuildingBlock>, IFormulaCachePresenter
   {
      private IFormulaCache _cache;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaBuilderToDTOFormulaBuilderMapper;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private IEditTypedFormulaPresenter _editPresenter;
      private IBuildingBlock _buildingBlock;
      private readonly IMoBiContext _context;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IFormulaUsageChecker _formulaUsageChecker;
      private readonly IObjectBaseNamingTask _namingTask;
      private IEnumerable<FormulaBuilderDTO> _dtoFormulaBuilders;
      private bool _disableEventsForHeavyWork;
      private readonly IClipboardManager _clipboardManager;

      public FormulaCachePresenter(IFormulaCacheView view, IFormulaToFormulaBuilderDTOMapper formulaBuilderToDTOFormulaBuilderMapper,
         IFormulaPresenterCache formulaPresenterCache, IMoBiContext context, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IDialogCreator dialogCreator, ICloneManagerForBuildingBlock cloneManager, IFormulaUsageChecker formulaUsageChecker, IObjectBaseNamingTask namingTask, IClipboardManager clipboardManager)
         : base(view)
      {
         _formulaBuilderToDTOFormulaBuilderMapper = formulaBuilderToDTOFormulaBuilderMapper;
         _cloneManager = cloneManager;
         _formulaUsageChecker = formulaUsageChecker;
         _namingTask = namingTask;
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _dialogCreator = dialogCreator;
         _formulaPresenterCache = formulaPresenterCache;
         _clipboardManager = clipboardManager;
      }

      public override void Edit(IBuildingBlock objectToEdit)
      {
         _cache = objectToEdit.FormulaCache;
         _buildingBlock = objectToEdit;
         updateFormulasInView(_cache.FirstOrDefault());
      }

      private void updateFormulasInView(IFormula selectedFormula)
      {
         Select(selectedFormula);
         _dtoFormulaBuilders = _cache.MapAllUsing(_formulaBuilderToDTOFormulaBuilderMapper);
         _view.Show(_dtoFormulaBuilders);
      }

      public override object Subject => _cache;

      private void addToParent(IFormula formula)
      {
         AddCommand(new AddFormulaToFormulaCacheCommand(_buildingBlock, formula).Run(_context));
         Edit(_buildingBlock);
         Select(formula);
      }

      public void Select(FormulaBuilderDTO formulaDTO)
      {
         if (formulaDTO == null) return;
         Select(getFormulaForDTO(formulaDTO));
      }

      private IFormula getFormulaForDTO(FormulaBuilderDTO dtoFormulaBuilder)
      {
         return _cache.FindById(dtoFormulaBuilder.Id);
      }

      public void Remove(FormulaBuilderDTO formulaDTO)
      {
         var formula = getFormulaForDTO(formulaDTO);
         if (formula == null) return;
         if (_formulaUsageChecker.FormulaUsedIn(_buildingBlock, formula))
         {
            _dialogCreator.MessageBoxInfo(AppConstants.Exceptions.FormulaInUse(formula));
            return;
         }

         var result = _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(formula.Name));
         if (result == ViewResult.No) 
            return;

         AddCommand(new RemoveFormulaFromFormulaCacheCommand(_buildingBlock, formula).Run(_context));
         //Ensure that if an invalid formula is removed, the invalid message is removed as well
         _context.PublishEvent(new FormulaValidEvent(formula, _buildingBlock));
         Edit(_buildingBlock);
      }

      public void Rename(FormulaBuilderDTO formulaDTO)
      {
         var formula = getFormulaForDTO(formulaDTO);

         if (formula == null)
            return;

         var newName = getNewNameForFormula(formula);

         if (string.IsNullOrEmpty(newName))
            return;

         AddCommand(new RenameObjectBaseCommand(formula, newName, _buildingBlock).Run(_context));
         Edit(_buildingBlock);
      }

      private string getNewNameForFormula(IFormula formula)
      {
         return _namingTask.RenameFor(formula, _buildingBlock.FormulaCache.Select(x => x.Name).ToList());
      }

      public void Clone(FormulaBuilderDTO formulaDTO)
      {
         var formula = getFormulaForDTO(formulaDTO);
         if (formula == null) 
            return;

         var newName = _namingTask.NewName(AppConstants.Captions.NewName, AppConstants.Captions.CloneFormulaTitle, formula.Name, _buildingBlock.FormulaCache.Select(x => x.Name));
         if (string.IsNullOrEmpty(newName))
            return;

         var cloneFormula = _cloneManager.Clone(formula, new FormulaCache());
         cloneFormula.Name = newName;
         addToParent(cloneFormula);
      }

      public void Copy(FormulaBuilderDTO formulaDTO)
      {
         _clipboardManager.CopyToClipBoard(getFormulaForDTO(formulaDTO));
      }

      public void Paste(FormulaBuilderDTO formulaDTO)
      {
         if (_clipboardManager.ObjectsExistOnClipBoard<IFormula>())
            _clipboardManager.PasteFromClipBoard<IFormula>(addFormulaAndSelect);
      }

      private void addFormulaAndSelect(IFormula formula)
      {
         if (formulaNameConflicts(formula) && !renameConflictingFormula(formula))
            return;

         addToParent(formula);
         _view.Select(dtoFor(formula));
      }

      private bool formulaNameConflicts(IFormula formula)
      {
         return _cache.FindByName(formula.Name) != null;
      }

      private bool renameConflictingFormula(IFormula formula)
      {
         var newName = getNewNameForFormula(formula);
         if (string.IsNullOrEmpty(newName))
            return false;

         // We can rename this formula without a command because it is not in the cache yet
         formula.Name = newName;
         return true;
      }

      public bool FormulasExistOnClipBoard()
      {
         return _clipboardManager.ObjectsExistOnClipBoard<IFormula>();
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
      }

      public void Select(IFormula formula)
      {
         if (formula == null) return;
         _editPresenter = _formulaPresenterCache.PresenterFor(formula);
         _editPresenter.BuildingBlock = _buildingBlock;
         _view.SetEditView(_editPresenter.BaseView);
         _editPresenter.InitializeWith(CommandCollector);
         _editPresenter.Edit(formula);
      }

      private bool formulaIsBeingEdited(IFormula formula)
      {
         if (_editPresenter == null)
            return false;
         return Equals(_editPresenter.Subject, formula);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         var formula = eventToHandle.ObjectBase as IFormula;
         if (formula == null) return;
         if (_cache.Contains(formula))
         {
            Select(formula);
            _view.Select(dtoFor(formula));
         }
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_disableEventsForHeavyWork)
            return;

         if (_cache == null) return;
         var formula = eventToHandle.AddedObject as IFormula;
         if (formula == null) return;

         if (!Equals(eventToHandle.Parent, _buildingBlock))
            return;

         Edit(_buildingBlock);
         Select(formula);
      }

      public void Handle(FormulaChangedEvent eventToHandle)
      {
         var formula = eventToHandle.Formula;
         if (!_cache.Contains(formula)) return;

         if (!formulaIsBeingEdited(formula))
            return;

         Select(formula);
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_disableEventsForHeavyWork)
            return;

         if (_cache == null) return;
         if (!Equals(eventToHandle.Parent, _buildingBlock))
            return;

         Edit(_buildingBlock);
      }

      private FormulaBuilderDTO dtoFor(IFormula formula)
      {
         return _dtoFormulaBuilders.FindById(formula.Id);
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = true;
      }

      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = false;
         Edit(_buildingBlock);
      }
   }
}