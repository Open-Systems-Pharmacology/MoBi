using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public class OutputSelectionsRetriever : IOutputSelectionsRetriever
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IEntityPathResolver _entityPathResolver;

      public OutputSelectionsRetriever(IMoBiApplicationController applicationController, IEntityPathResolver entityPathResolver)
      {
         _applicationController = applicationController;
         _entityPathResolver = entityPathResolver;
      }

      public OutputSelections OutputSelectionsFor(IMoBiSimulation simulation)
      {
         using (var presenter = _applicationController.Start<IOutputSelectionsPresenter>())
         {
            return presenter.StartSelection(simulation);
         }
      }

      public QuantitySelection SelectionFrom(IQuantity quantity)
      {
         return new QuantitySelection(_entityPathResolver.PathFor(quantity), quantity.QuantityType);
      }
   }
}