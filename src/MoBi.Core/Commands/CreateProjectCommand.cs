using MoBi.Core.Domain.Model;
using OSPSuite.Core;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class CreateProjectCommand : MoBiCommand
   {
      public CreateProjectCommand()
      {
         ObjectType = ObjectTypes.Project;
         CommandType = Command.CommandTypeAdd;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var configuration = context.Resolve<IApplicationConfiguration>();
         Description = Command.CreateProjectDescription(configuration.Version);
      }

      protected override void ClearReferences()
      {
         /*nothing to do here*/
      }
   }
}