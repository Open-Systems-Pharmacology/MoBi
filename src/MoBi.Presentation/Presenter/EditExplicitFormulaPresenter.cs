using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditExplicitFormulaPresenter :
      IEditTypedFormulaPresenter,
      IListener<FormulaChangedEvent>
   {
      void Validate(string formulaString);

      /// <summary>
      ///    Sets the formula string to a new value from the old value
      /// </summary>
      /// <param name="newFormulaString">The new value</param>
      void SetFormulaString(string newFormulaString);
   }

   public class EditExplicitFormulaPresenter : EditTypedFormulaPresenter<IEditExplicitFormulaView, IEditExplicitFormulaPresenter, ExplicitFormula>,
      IEditExplicitFormulaPresenter
   {
      private readonly IExplicitFormulaToExplicitFormulaDTOMapper _explicitFormulaMapper;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;
      private readonly IEditFormulaPathListPresenter _editFormulaPathListPresenter;

      public EditExplicitFormulaPresenter(
         IEditExplicitFormulaView view,
         IExplicitFormulaToExplicitFormulaDTOMapper explicitFormulaMapper,
         IMoBiFormulaTask moBiFormulaTask,
         IReactionDimensionRetriever reactionDimensionRetriever,
         IDisplayUnitRetriever displayUnitRetriever,
         IEditFormulaPathListPresenter editFormulaPathListPresenter) : base(view, displayUnitRetriever)
      {
         _explicitFormulaMapper = explicitFormulaMapper;
         _moBiFormulaTask = moBiFormulaTask;
         _reactionDimensionRetriever = reactionDimensionRetriever;
         _editFormulaPathListPresenter = editFormulaPathListPresenter;
         AddSubPresenters(_editFormulaPathListPresenter);
         _view.AddFormulaPathListView(_editFormulaPathListPresenter.BaseView);
         _editFormulaPathListPresenter.DragDropAllowedFor = DragDropAllowedFor;
      }

      private void updateFormulaCaption()
      {
         var reactionMode = _reactionDimensionRetriever.SelectedDimensionMode;
         var caption = _moBiFormulaTask.GetFormulaCaption(UsingObject, reactionMode, IsRHS);

         if (string.IsNullOrEmpty(caption))
            _view.HideFormulaCaption();
         else
            _view.SetFormulaCaption(caption);
      }

      public override IBuildingBlock BuildingBlock
      {
         set
         {
            base.BuildingBlock = value;
            _editFormulaPathListPresenter.BuildingBlock = value;
         }
      }

      public override bool ReadOnly
      {
         set
         {
            base.ReadOnly = value;
            _editFormulaPathListPresenter.ReadOnly = value;
         }
      }

      public override bool IsRHS
      {
         set
         {
            base.IsRHS = value;
            _editFormulaPathListPresenter.CheckCircularReference = !IsRHS;
         }
      }

      public override void Edit(ExplicitFormula formula)
      {
         if (formula.IsExplicit())
         {
            _view.Enabled = true;
            _formula = formula;
            _view.Show(_explicitFormulaMapper.MapFrom(_formula, UsingObject));
            _editFormulaPathListPresenter.Edit(_formula, UsingObject);
            Validate(_formula.FormulaString);
         }

         if (formula.IsBlackBox())
            _view.Enabled = false;

         updateFormulaCaption();
      }

      public void SetFormulaString(string newFormulaString)
      {
         AddCommand(_moBiFormulaTask.SetFormulaString(_formula, newFormulaString, BuildingBlock));
      }

      public bool DragDropAllowedFor(ReferenceDTO droppedReferenceDTO)
      {
         if (ReadOnly)
            return false;

         if (droppedReferenceDTO?.Path == null)
            return false;

         if (droppedReferenceDTO.Path.IsAnImplementationOf<TimePath>())
            return true;

         return true;
      }

      public void Validate(string formulaString)
      {
         var (_, message) = _moBiFormulaTask.Validate(formulaString, _formula, BuildingBlock);
         _view.SetValidationMessage(message);
      }

      public override void Edit(IFormula formula, IEntity formulaOwner = null)
      {
         base.Edit(formula, formulaOwner);
         _editFormulaPathListPresenter.Edit(formula, UsingObject);
      }

      public void Handle(FormulaChangedEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      private void handle(FormulaEvent eventToHandle)
      {
         if (!canHandle(eventToHandle)) return;
         Edit(_formula, _formulaOwner);
      }

      private bool canHandle(FormulaEvent formulaEvent) => Equals(formulaEvent.Formula, _formula);
   }
}