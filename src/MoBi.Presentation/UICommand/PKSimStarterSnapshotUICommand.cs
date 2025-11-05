using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class PKSimStarterSnapshotUICommand<TSubject> : ObjectUICommand<TSubject> where TSubject : class
   {
      protected readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IMoBiContext _context;

      protected PKSimStarterSnapshotUICommand(IMoBiProjectRetriever projectRetriever, IHeavyWorkManager heavyWorkManager, IMoBiContext context)
      {
         _projectRetriever = projectRetriever;
         _heavyWorkManager = heavyWorkManager;
         _context = context;
      }

      protected override void PerformExecute()
      {
         TSubject objectToLoad = null;

         _heavyWorkManager.Start(() =>
            {
               objectToLoad = LoadFromSnapshot();
            }, AppConstants.Captions.Loading.WithEllipsis()
         );

         if (objectToLoad != null)
            _context.AddToHistory(AddToProject(objectToLoad));
      }

      protected abstract IMoBiCommand AddToProject(TSubject objectToLoad);

      protected abstract TSubject LoadFromSnapshot();
   }
}