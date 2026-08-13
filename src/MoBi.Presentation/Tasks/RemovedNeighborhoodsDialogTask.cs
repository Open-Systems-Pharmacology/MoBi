using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface IRemovedNeighborhoodsDialogTask
   {
      /// <summary>
      ///    Shows a dialog describing all neighborhoods that were removed from the simulation because a module redefines them
      ///    without neighbors. Nothing is shown if no neighborhood was removed
      /// </summary>
      void ShowRemovedNeighborhoodsFrom(ValidationResult validationResult);
   }

   public class RemovedNeighborhoodsDialogTask : IRemovedNeighborhoodsDialogTask
   {
      private readonly IDialogCreator _dialogCreator;

      public RemovedNeighborhoodsDialogTask(IDialogCreator dialogCreator)
      {
         _dialogCreator = dialogCreator;
      }

      public void ShowRemovedNeighborhoodsFrom(ValidationResult validationResult)
      {
         if (validationResult == null)
            return;

         //removal warnings are identified by the neighborhood builder without neighbors they were created for
         var removalWarnings = validationResult.Messages
            .Where(x => x.NotificationType == NotificationType.Warning)
            .Where(x => x.Object is NeighborhoodBuilder neighborhoodBuilder && neighborhoodBuilder.HasNoNeighbors)
            .Select(x => x.Text)
            .ToList();

         if (!removalWarnings.Any())
            return;

         _dialogCreator.MessageBoxInfo(removalWarnings.ToString("\n\n"));
      }
   }
}
