using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Presenter
{
   public interface IEditInSimulationPresenter : IEditPresenter
   {
      IMoBiSimulation Simulation { get; set; }
   }

   public interface IEditInSimulationPresenterFactory
   {
      IEditInSimulationPresenter PresenterFor(IEntity entity);
   }

   public class EditInSimulationPresenterFactory : IEditInSimulationPresenterFactory
   {
      private readonly IoC _container;

      public EditInSimulationPresenterFactory(IoC container)
      {
         _container = container;
      }

      public IEditInSimulationPresenter PresenterFor(IEntity entity)
      {
         if (entity.IsAnImplementationOf<IQuantity>())
            return _container.Resolve<IEditQuantityInSimulationPresenter>();

         if (entity.IsAnImplementationOf<IReaction>())
            return _container.Resolve<IEditReactionInSimulationPresenter>();

         if (entity.IsAnImplementationOf<IContainer>())
            return _container.Resolve<IEditContainerInSimulationPresenter>();

         return null;
      }
   }
}