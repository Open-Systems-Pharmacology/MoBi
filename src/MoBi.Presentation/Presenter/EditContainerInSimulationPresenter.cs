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
         Edit(objectToEdit.DowncastTo<IContainer>());
      }

      public void Edit(IContainer container)
      {
         _container = container;
         _editContainerPresenter.Edit(container);
      }

      public object Subject
      {
         get { return _container; }
      }

      public void SelectParameter(IParameter childParameter)
      {
         _editContainerPresenter.SelectParameter(childParameter);
      }
   }
}