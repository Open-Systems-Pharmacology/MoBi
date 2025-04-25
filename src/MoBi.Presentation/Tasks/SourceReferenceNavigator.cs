using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Tasks
{
   public interface ISourceReferenceNavigator
   {
      void GoTo(SimulationEntitySourceReference sourceReference);
   }

   public class SourceReferenceNavigator : ISourceReferenceNavigator
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiContext _context;

      public SourceReferenceNavigator(IMoBiApplicationController applicationController, IMoBiContext context)
      {
         _applicationController = applicationController;
         _context = context;
      }

      public void GoTo(SimulationEntitySourceReference sourceReference)
      {
         if (sourceReference != null)
            _applicationController.Select(sourceReference.Source, sourceReference.BuildingBlock, _context.HistoryManager);
      }
   }
}