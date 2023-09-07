using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands;

namespace MoBi.Core.Commands
{
   public interface IMoBiCommand : IOSPSuiteCommand<IMoBiContext>
   {
   }

   public class MoBiEmptyCommand : OSPSuiteEmptyCommand<IMoBiContext>, IMoBiCommand
   {
   }

   public abstract class MoBiCommand : OSPSuiteCommand<IMoBiContext>, IMoBiCommand
   {
   }

   public abstract class MoBiReversibleCommand : OSPSuiteReversibleCommand<IMoBiContext>, IMoBiCommand
   {
   }
}