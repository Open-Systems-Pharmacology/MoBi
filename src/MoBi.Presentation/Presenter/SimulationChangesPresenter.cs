using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public class SimulationChangesPresenter : AbstractEditPresenter<ISimulationChangesView, ISimulationChangesPresenter, IMoBiSimulation>, ISimulationChangesPresenter
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
         return simulation.OriginalQuantityValues.OrderBy(x => x.Path).
            Select(x => (originalQuantityValue:x, quantity:quantityFrom(x, simulation.Model.Root))).
            Where(x => x.quantity != null).
            Select(x => new OriginalQuantityValueDTO(x.originalQuantityValue).WithCurrentQuantity(x.quantity)).ToList();
      }

      private IQuantity quantityFrom(OriginalQuantityValue originalQuantityValue, IContainer modelRoot)
      {
         return new ObjectPath(originalQuantityValue.Path.ToPathArray()).TryResolve<IQuantity>(modelRoot);
      }


      public override object Subject => _simulation;
   }

   public interface ISimulationChangesPresenter : IPresenter<ISimulationChangesView>, IEditPresenter<IMoBiSimulation>
   {
   }
}