using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ReloadAllObservedDataCommand : ObjectUICommand<DataRepository>
   {
      private readonly IMoBiContext _executionContext;
      private readonly IObservedDataTask _observedDataTask;

      public ReloadAllObservedDataCommand(
         IMoBiContext executionContext,
         IObservedDataTask observedDataTask
      )
      {
         _executionContext = executionContext;
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         if (string.IsNullOrEmpty(Subject.ConfigurationId))
            return;

         var project = _executionContext.Project;
         var configurationId = Subject.ConfigurationId;

         //we should check this
         var observedDataFromSameFile =
            project.AllObservedData.Where(r => !string.IsNullOrEmpty(r.ConfigurationId) && r.ConfigurationId == configurationId); //actually the question here is: configID means they come from the same file right?

         var configuration = project.ImporterConfigurationBy(configurationId);
         _observedDataTask.AddAndReplaceObservedDataFromConfigurationToProject(configuration, observedDataFromSameFile);
      }
   }
}