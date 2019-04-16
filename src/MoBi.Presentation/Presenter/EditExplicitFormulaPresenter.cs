using MoBi.Core.Domain.Model;
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
using OSPSuite.Utility.Exceptions;
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
      /// <param name="oldFormulaString">The old value</param>
      void SetFormulaString(string newFormulaString, string oldFormulaString);
   }

   public class EditExplicitFormulaPresenter : EditTypedFormulaPresenter<IEditExplicitFormulaView, IEditExplicitFormulaPresenter, ExplicitFormula>,
      IEditExplicitFormulaPresenter
   {
      private readonly IMoBiContext _context;
      private readonly IExplicitFormulaToExplicitFormulaDTOMapper _explicitFormulaMapper;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;
      private readonly IEditFormulaPathListPresenter _editFormulaPathListPresenter;

      public EditExplicitFormulaPresenter(
         IEditExplicitFormulaView view,
         IExplicitFormulaToExplicitFormulaDTOMapper explicitFormulaMapper,
         IMoBiContext context,
         IMoBiFormulaTask moBiFormulaTask,
         IReactionDimensionRetriever reactionDimensionRetriever,
         IDisplayUnitRetriever displayUnitRetriever,
         IEditFormulaPathListPresenter editFormulaPathListPresenter) : base(view, displayUnitRetriever)
      {
         _explicitFormulaMapper = explicitFormulaMapper;
         _moBiFormulaTask = moBiFormulaTask;
         _reactionDimensionRetriever = reactionDimensionRetriever;
         _editFormulaPathListPresenter = editFormulaPathListPresenter;
         _context = context;
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

      public override void Edit(ExplicitFormula objectToEdit)
      {
         if (objectToEdit.IsExplicit())
         {
            _view.Enabled = true;
            _formula = objectToEdit;
            _view.Show(_explicitFormulaMapper.MapFrom(_formula, UsingObject));
            _editFormulaPathListPresenter.Edit(_formula, UsingObject);
            Validate(_formula.FormulaString);
         }

         if (objectToEdit.IsBlackBox())
            _view.Enabled = false;

         updateFormulaCaption();
      }

      public void SetFormulaString(string newFormulaString, string oldFormulaString)
      {
         if (string.Equals(newFormulaString, oldFormulaString)) return;
         AddCommand(_moBiFormulaTask.SetFormulaString(_formula, newFormulaString, oldFormulaString, BuildingBlock));
      }

      public bool DragDropAllowedFor(ReferenceDTO droppedReferenceDTO)
      {
         if (ReadOnly) return false;
         if (droppedReferenceDTO?.Path == null) return false;
         if (droppedReferenceDTO.Path.IsAnImplementationOf<TimePath>()) return true;
         if (droppedReferenceDTO.BuildMode == ParameterBuildMode.Local && referencesToLocalForbidden) return false;
         return true;
      }

      private bool referencesToLocalForbidden
      {
         get
         {
            var parameter = UsingObject as IParameter;
            if (parameter == null) return false;
            return !parameter.BuildMode.Equals(ParameterBuildMode.Local);
         }
      }

      public void Validate(string formulaString)
      {
         try
         {
            _formula.Validate(formulaString);
            _view.SetParserError(null);
            _context.PublishEvent(new FormulaValidEvent(_formula, BuildingBlock));
         }
         catch (OSPSuiteException parserException)
         {
            _view.SetParserError(parserException.Message);
            _context.PublishEvent(new FormulaInvalidEvent(_formula, BuildingBlock, parserException.Message));
         }
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

      private bool canHandle(FormulaEvent formulaEvent)
      {
         return Equals(formulaEvent.Formula, _formula);
      }
   }
}