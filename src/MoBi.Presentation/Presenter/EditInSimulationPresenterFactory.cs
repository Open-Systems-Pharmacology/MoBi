using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Presenter
{
   public class TrackableSimulation
   {
      public IMoBiSimulation Simulation { get; }
      public SimulationEntitySourceReferenceCache ReferenceCache { get; }

      public TrackableSimulation(IMoBiSimulation simulation, SimulationEntitySourceReferenceCache referenceCache)
      {
         Simulation = simulation;
         ReferenceCache = referenceCache;
      }

      public SimulationEntitySourceReference SourceFor(IParameter parameter) => ReferenceCache[parameter];
   }

   public interface IEditInSimulationPresenter : IEditPresenter
   {
      TrackableSimulation TrackableSimulation { get; set; }
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

         if (entity.IsAnImplementationOf<Reaction>())
            return _container.Resolve<IEditReactionInSimulationPresenter>();

         if (entity.IsAnImplementationOf<IContainer>())
            return _container.Resolve<IEditContainerInSimulationPresenter>();

         return null;
      }
   }
}