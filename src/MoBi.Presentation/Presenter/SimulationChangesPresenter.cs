using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public class SimulationChangesPresenter : AbstractEditPresenter<ISimulationChangesView, ISimulationChangesPresenter, IMoBiSimulation>, ISimulationChangesPresenter, IListener<SimulationStatusChangedEvent>
   {
      private IMoBiSimulation _simulation;

      public SimulationChangesPresenter(ISimulationChangesView view) : base(view)
      {
      }

      public override void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _view.BindTo(allDTOsWithModelQuantitiesFrom(_simulation));
      }

      private List<OriginalQuantityValueDTO> allDTOsWithModelQuantitiesFrom(IMoBiSimulation simulation)
      {
         return quantityChanges(simulation).Concat(scaleChanges(simulation)).ToList();
      }

      private List<OriginalQuantityValueDTO> scaleChanges(IMoBiSimulation simulation)
      {
         var originalQuantityValues = simulation.OriginalQuantityValues.Where(x => x.IsScaleChange);
         return simulationQuantitiesFor<MoleculeAmount>(simulation, originalQuantityValues).
            Select(x => new OriginalQuantityValueDTO(x.originalQuantityValue, x.quantity.ScaleDivisor)).ToList();
      }

      private List<OriginalQuantityValueDTO> quantityChanges(IMoBiSimulation simulation)
      {
         return simulationQuantitiesFor<IQuantity>(simulation, simulation.OriginalQuantityValues.Where(x => x.IsQuantityChange)).
            Select(x => new OriginalQuantityValueDTO(x.originalQuantityValue, x.quantity.Value)).ToList();
      }

      private IEnumerable<(OriginalQuantityValue originalQuantityValue, TQuantity quantity)> simulationQuantitiesFor<TQuantity>(IMoBiSimulation simulation, IEnumerable<OriginalQuantityValue> originalQuantityValues) where TQuantity : class
      {
         return originalQuantityValues.OrderBy(x => x.Path).
            Select(x => (originalQuantityValue:x, quantity:quantityFrom<TQuantity>(x, simulation.Model.Root))).
            Where(x => x.quantity != null);
      }

      private TQuantity quantityFrom<TQuantity>(OriginalQuantityValue originalQuantityValue, IContainer modelRoot) where TQuantity : class
      {
         return new ObjectPath(originalQuantityValue.Path.ToPathArray()).TryResolve<TQuantity>(modelRoot);
      }


      public override object Subject => _simulation;
      public void Handle(SimulationStatusChangedEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            Edit(_simulation);
      }

      private bool canHandle(SimulationStatusChangedEvent eventToHandle)
      {
         return eventToHandle.Simulation == _simulation;
      }
   }

   public interface ISimulationChangesPresenter : IPresenter<ISimulationChangesView>, IEditPresenter<IMoBiSimulation>
   {
   }
}