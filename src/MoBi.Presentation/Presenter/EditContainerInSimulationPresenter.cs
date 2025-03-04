using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerInSimulationPresenter : IEditPresenterWithParameters, IEditInSimulationPresenter
   {
   }

   public class EditContainerInSimulationPresenter : AbstractCommandCollectorPresenter<IEditContainerInSimulationView, IEditContainerInSimulationPresenter>, IEditContainerInSimulationPresenter
   {
      private readonly IEditContainerPresenter _editContainerPresenter;
      private IContainer _container;
      public IMoBiSimulation Simulation { get; set; }

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

         _container = container;
         _editContainerPresenter.Edit(container);
         _editContainerPresenter.ShowParameters();
      }

      public object Subject => _container;

      public void SelectParameter(IParameter childParameter)
      {
         _editContainerPresenter.SelectParameter(childParameter);
      }
   }
}