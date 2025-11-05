using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public class OutputSelectionsRetriever : QuantitySelectionsRetriever, IOutputSelectionsRetriever
   {
      private readonly IMoBiApplicationController _applicationController;

      public OutputSelectionsRetriever(IMoBiApplicationController applicationController, IEntityPathResolver entityPathResolver) : base(entityPathResolver)
      {
         _applicationController = applicationController;
      }

      public OutputSelections OutputSelectionsFor(IMoBiSimulation simulation)
      {
         using (var presenter = _applicationController.Start<IOutputSelectionsPresenter>())
         {
            return presenter.StartSelection(simulation);
         }
      }
   }
}