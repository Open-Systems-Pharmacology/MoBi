using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core.Services
{
   public class QualificationStepRunnerFactory :  OSPSuite.Core.Services.QualificationStepRunnerFactory
   {
      public QualificationStepRunnerFactory(IContainer container) : base(container)
      {
      }

      public override IQualificationStepRunner CreateFor(IQualificationStep qualificationStep)
      {
         throw new System.NotImplementedException();
      }
   }
}
