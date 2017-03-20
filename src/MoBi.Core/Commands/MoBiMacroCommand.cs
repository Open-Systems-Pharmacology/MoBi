using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands;

namespace MoBi.Core.Commands
{
   public interface IMoBiMacroCommand : IMacroCommand<IMoBiContext>, IMoBiCommand
   {
   }

   public class MoBiMacroCommand : OSPSuiteMacroCommand<IMoBiContext>, IMoBiMacroCommand
   {
   }
}