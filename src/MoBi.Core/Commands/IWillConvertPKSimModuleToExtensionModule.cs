using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public interface IWillConvertPKSimModuleToExtensionModule : IMoBiCommand
   {
      bool WillConvertPKSimModuleToExtensionModule { get; }
      Module Module { get; }
   }
}