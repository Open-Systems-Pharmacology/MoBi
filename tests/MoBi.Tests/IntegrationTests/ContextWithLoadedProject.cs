using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Services;

namespace MoBi.IntegrationTests
{
   public abstract class ContextWithLoadedProject : ContextForIntegration<IMoBiProject>
   {
      public TObject LoadPKML<TObject>(string pkmlFileName, ReactionDimensionMode reactionDimensionMode = ReactionDimensionMode.AmountBased)
      {
         var projectFile = HelperForSpecs.TestFileFullPath($"{pkmlFileName}.pkml");
         var serializationTask = IoC.Resolve<ISerializationTask>();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
         context.CurrentProject.ReactionDimensionMode = reactionDimensionMode;
         return serializationTask.Load<TObject>(projectFile);
      }

      public IMoBiProject LoadProject(string projectFileName)
      {
         var projectFile = HelperForSpecs.TestFileFullPath($"{projectFileName}.mbp3");
         var serializationTask = IoC.Resolve<ISerializationTask>();
         var context = IoC.Resolve<IMoBiContext>();
         serializationTask.LoadProject(projectFile);
         return context.CurrentProject;
      }
   }
}