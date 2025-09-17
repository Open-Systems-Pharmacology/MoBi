using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerInSimulationPresenter : IEditPresenterWithParameters, IEditInSimulationPresenter
   {
   }

   public class EditContainerInSimulationPresenter : AbstractCommandCollectorPresenter<IEditContainerInSimulationView, IEditContainerInSimulationPresenter>, IEditContainerInSimulationPresenter
   {
      private readonly IEditContainerPresenter _editContainerPresenter;
      private IContainer _container;
      private TrackableSimulation _trackableSimulation;

      public TrackableSimulation TrackableSimulation
      {
         get => _trackableSimulation;
         set
         {
            _trackableSimulation = value;
            _editContainerPresenter.EnableSimulationTracking(value);
         }
      }

      public EditContainerInSimulationPresenter(IEditContainerInSimulationView view, IEditContainerPresenter editContainerPresenter) : base(view)
      {
         _editContainerPresenter = editContainerPresenter;
         _editContainerPresenter.EditMode = EditParameterMode.ValuesOnly;
         editContainerPresenter.ReadOnly = true;
         _view.SetContainerView(_editContainerPresenter.BaseView);
         AddSubPresenters(editContainerPresenter);
      }

      public void Edit(object objectToEdit)
      {
         var container = objectToEdit.DowncastTo<IContainer>();
         if (Equals(_container, container))
            return;

         if (_container == null)
            _editContainerPresenter.ShowParameters();

         _container = container;
         _editContainerPresenter.Edit(container);
      }

      public object Subject => _container;

      public void SelectParameter(IParameter childParameter)
      {
         _editContainerPresenter.SelectParameter(childParameter);
      }
   }
}