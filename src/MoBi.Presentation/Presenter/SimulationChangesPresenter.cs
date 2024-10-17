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
         _view.BindTo(_simulation.OriginalQuantityValues.OrderBy(x => x.Path).Select(createDTO).ToList());
      }

      private OriginalQuantityValueDTO createDTO(OriginalQuantityValue originalQuantityValue)
      {
         var path = new ObjectPath(originalQuantityValue.Path.ToPathArray());
         return new OriginalQuantityValueDTO(originalQuantityValue).WithCurrentQuantity(path.TryResolve<IQuantity>(_simulation.Model.Root));
      }

      public override object Subject => _simulation;
   }

   public interface ISimulationChangesPresenter : IPresenter<ISimulationChangesView>, IEditPresenter<IMoBiSimulation>
   {
   }
}