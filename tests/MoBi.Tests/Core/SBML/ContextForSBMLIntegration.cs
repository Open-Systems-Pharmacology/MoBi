using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Engine.Sbml;
using MoBi.IntegrationTests;
using NUnit.Framework;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.SBML
{
   [Category("SBML")]
   [Ignore("")]
   public abstract class ContextForSBMLIntegration<T> : ContextForIntegration<T>
   {  
      protected SbmlTask _sbmlTask;
      protected MoBiProject _moBiProject;
      protected string _fileName;

      protected override void Context()
      {
         base.Context();
         _sbmlTask = IoC.Resolve<ISbmlTask>().DowncastTo<SbmlTask>();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
         _moBiProject = context.CurrentProject;
         context.LoadFrom(_moBiProject);
      }

      protected override void Because()
      {
         _sbmlTask.ImportModelFromSbml(_fileName, _moBiProject);
      }
   }
}