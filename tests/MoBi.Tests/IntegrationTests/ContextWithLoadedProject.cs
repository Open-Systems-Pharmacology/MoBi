using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public abstract class ContextWithLoadedProject : ContextForIntegration<MoBiProject>
   {
      public TObject LoadPKML<TObject>(string pkmlFileName, ReactionDimensionMode reactionDimensionMode = ReactionDimensionMode.AmountBased)
      {
         var projectFile = DomainHelperForSpecs.TestFileFullPath($"{pkmlFileName}.pkml");
         var serializationTask = IoC.Resolve<ISerializationTask>();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
         context.CurrentProject.ReactionDimensionMode = reactionDimensionMode;
         return serializationTask.Load<TObject>(projectFile);
      }

      public MoBiProject LoadProject(string projectFileName)
      {
         var projectFile = DomainHelperForSpecs.TestFileFullPath($"{projectFileName}.mbp3");
         var serializationTask = IoC.Resolve<ISerializationTask>();
         var context = IoC.Resolve<IMoBiContext>();
         serializationTask.LoadProject(projectFile);
         return context.CurrentProject;
      }
   }
}