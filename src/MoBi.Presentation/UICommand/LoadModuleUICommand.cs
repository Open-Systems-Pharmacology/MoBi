using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public class LoadModuleUICommand : IUICommand
   {
      private readonly IModuleLoader _moduleLoader;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public LoadModuleUICommand(IModuleLoader moduleLoader, IMoBiProjectRetriever projectRetriever)
      {
         _moduleLoader = moduleLoader;
         _projectRetriever = projectRetriever;
      }

      public void Execute()
      {
         _moduleLoader.LoadModule(_projectRetriever.Current);
      }
   }
}