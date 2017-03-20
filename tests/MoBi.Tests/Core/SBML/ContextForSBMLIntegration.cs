using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using NUnit.Framework;

namespace MoBi.Core.SBML
{
   [Category("SBML")]
   [Ignore("")]
   public abstract class ContextForSBMLIntegration<T> : ContextForIntegration<T>
   {  
      protected ISBMLTask _sbmlTask;
      protected IMoBiProject _moBiProject;
      protected string _fileName;

      protected override void Context()
      {
         base.Context();
         _sbmlTask = IoC.Resolve<ISBMLTask>();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
         _moBiProject = context.CurrentProject;
         context.LoadFrom(_moBiProject);
      }

      protected override void Because()
      {
         _sbmlTask.ImportModelFromSBML(_fileName, _moBiProject);
      }
   }
}