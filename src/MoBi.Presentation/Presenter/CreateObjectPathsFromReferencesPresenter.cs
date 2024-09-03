using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateObjectPathsFromReferencesPresenter : IPresenter<ICreateObjectPathsFromReferencesView>
   {
      void Init(IEntity localReferencePoint, IReadOnlyList<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject);
      IReadOnlyList<ObjectPath> GetAllSelections();
      void AddSelection();
   }


   public class CreateObjectPathsFromReferencesPresenter : AbstractCommandCollectorPresenter<ICreateObjectPathsFromReferencesView, ICreateObjectPathsFromReferencesPresenter>, ICreateObjectPathsFromReferencesPresenter
   {
      private readonly ISelectReferenceAtParameterValuePresenter _selectReferencePresenter;

      public CreateObjectPathsFromReferencesPresenter(ICreateObjectPathsFromReferencesView view, ISelectReferenceAtParameterValuePresenter selectReferencePresenter) : base(view)
      {
         _selectReferencePresenter = selectReferencePresenter;
         _subPresenterManager.Add(_selectReferencePresenter);
         view.AddReferenceSelectionView(_selectReferencePresenter.View);
         _selectReferencePresenter.SelectionChangedEvent += enableDisableButtons;
      }

      private void enableDisableButtons() => _view.CanAdd(_selectReferencePresenter.CanClose);

      public void Init(IEntity localReferencePoint, IReadOnlyList<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject) => _selectReferencePresenter.Init(localReferencePoint, contextSpecificEntitiesToAddToReferenceTree, editedObject);

      public IReadOnlyList<ObjectPath> GetAllSelections() => convertTextToObjectPaths(_view.AllPaths.Where(x => !string.IsNullOrWhiteSpace(x)).ToList());

      public void AddSelection() => _view.AddSelectedPaths(_selectReferencePresenter.GetAllSelections().Select(x => x.PathAsString).ToList());

      private IReadOnlyList<ObjectPath> convertTextToObjectPaths(IReadOnlyList<string> pathsAsString) => pathsAsString.Select(x => new ObjectPath(x.ToPathArray())).ToList();

      public override bool CanClose => true;
   }
}
