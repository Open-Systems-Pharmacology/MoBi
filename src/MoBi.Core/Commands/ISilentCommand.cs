namespace MoBi.Core.Commands
{
   public interface ISilentCommand: IMoBiCommand
   {
      bool Silent { get; set; }
   }
}