using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
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
      Size? ModalSize { get; }
      void SetRelativePathSelectorVisible(bool visible);
   }


   public interface ICreateInitialConditionPathsFromReferencesPresenter : ICreateObjectPathsFromReferencesPresenter
   {
   }

   public class CreateObjectPathsFromReferencesPresenter : AbstractCommandCollectorPresenter<ICreateObjectPathsFromReferencesView, ICreateObjectPathsFromReferencesPresenter>, ICreateObjectPathsFromReferencesPresenter
   {
      private readonly ISelectReferencePresenter _selectReferencePresenter;

      public CreateObjectPathsFromReferencesPresenter(ICreateObjectPathsFromReferencesView view, ISelectReferenceAtParameterValuePresenter selectReferencePresenter) : this(view, selectReferencePresenter, AppConstants.Captions.ParameterPathsToAdd)
      {
      }

      protected CreateObjectPathsFromReferencesPresenter(ICreateObjectPathsFromReferencesView view, ISelectReferencePresenter selectReferencePresenter, string pathsToAddCaption) : base(view)
      {
         _selectReferencePresenter = selectReferencePresenter;
         _subPresenterManager.Add(_selectReferencePresenter);
         view.AddReferenceSelectionView(_selectReferencePresenter.View);
         view.PathsToAddCaption = pathsToAddCaption;
         _selectReferencePresenter.SelectionChangedEvent += enableDisableButtons;
      }
      
      private void enableDisableButtons() => _view.CanAdd(_selectReferencePresenter.CanClose);

      public void Init(IEntity localReferencePoint, IReadOnlyList<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject) => 
         _selectReferencePresenter.Init(localReferencePoint, contextSpecificEntitiesToAddToReferenceTree, editedObject);

      public IReadOnlyList<ObjectPath> GetAllSelections() => convertTextToObjectPaths(_view.AllPaths.Where(x => !string.IsNullOrWhiteSpace(x)).ToList());

      public void AddSelection() => _view.AddSelectedPaths(_selectReferencePresenter.GetAllSelections().Select(x => x.PathAsString).ToList());
      public Size? ModalSize => _view.ModalSize;

      private IReadOnlyList<ObjectPath> convertTextToObjectPaths(IReadOnlyList<string> pathsAsString) => pathsAsString.Select(x => new ObjectPath(x.ToPathArray())).ToList();

      public override bool CanClose => true;

      public void SetRelativePathSelectorVisible(bool visible)
      {
         _selectReferencePresenter.View.SetLayoutItemLocalisationVisible(visible);
         _selectReferencePresenter.View.SetRelativePathRadioButtonEnabled(visible);
      }
   }

   public class CreateInitialConditionPathsFromReferencesPresenter : CreateObjectPathsFromReferencesPresenter, ICreateInitialConditionPathsFromReferencesPresenter
   {
      public CreateInitialConditionPathsFromReferencesPresenter(ICreateObjectPathsFromReferencesView view, ISelectReferenceAtInitialConditionPresenter selectReferencePresenter) : base(view, selectReferencePresenter, AppConstants.Captions.InitialConditionPathsToAdd)
      {
      }
   }
}
