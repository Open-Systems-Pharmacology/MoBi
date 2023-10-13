using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter.BasePresenter
{
   public abstract class AbstractContainerEditPresenterWithParameters<TView, TPresenter, TContainer> : AbstractEntityEditPresenter<TView, TPresenter, TContainer>, IEditPresenterWithParameters<TContainer>,
      ICanEditPropertiesPresenter, IPresenterWithFormulaCache
      where TView : IView<TPresenter>, IEditViewWithParameters
      where TPresenter : IPresenter
      where TContainer : IContainer
   {
      protected readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      protected readonly IMoBiContext _context;
      protected readonly IEditTaskForContainer _editTask;
      protected abstract IContainer SubjectContainer { get; }

      protected AbstractContainerEditPresenterWithParameters(TView view, IEditParametersInContainerPresenter editParametersInContainerPresenter, IMoBiContext context, IEditTaskForContainer editTask) : base(view)
      {
         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _context = context;
         _editTask = editTask;
         AddSubPresenters(_editParametersInContainerPresenter);
         view.AddParameterView(editParametersInContainerPresenter.BaseView);
         InitParameterListPresenter();
      }

      protected virtual void InitParameterListPresenter()
      {
         _editParametersInContainerPresenter.BlackBoxAllowed = true;
         _editParametersInContainerPresenter.ChangeLocalisationAllowed = false;
      }

      public virtual void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editParametersInContainerPresenter.Select(parameter);
      }

      public override void Edit(TContainer container, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _editParametersInContainerPresenter.Edit(container);
      }

      public virtual IBuildingBlock BuildingBlock
      {
         get => _editParametersInContainerPresenter.BuildingBlock;
         set => _editParametersInContainerPresenter.BuildingBlock = value;
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public IEnumerable<FormulaBuilderDTO> GetFormulas() => _editParametersInContainerPresenter.GetFormulas();

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, SubjectContainer, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTask.Rename(SubjectContainer, SubjectContainer.ParentContainer, BuildingBlock);
      }
   }
}