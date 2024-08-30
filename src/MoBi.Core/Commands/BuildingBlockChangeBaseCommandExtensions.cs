using MoBi.Core.Domain;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   static class BuildingBlockChangeBaseCommandExtensions
   {
      public static BuildingBlockChangeCommandBase<T> AsInverseFor<T>(this BuildingBlockChangeCommandBase<T> inverseCommand, BuildingBlockChangeCommandBase<T> originalCommand) where T : class, IBuildingBlock 
      {
         CommandExtensions.AsInverseFor(inverseCommand, originalCommand);
         inverseCommand.ShouldIncrementVersion = !originalCommand.ShouldIncrementVersion;
         inverseCommand.ConversionOption = getReverseConversionOption(originalCommand.ConversionOption, originalCommand.HasChangedModuleType);
         return inverseCommand;
      }

      private static PKSimModuleConversion getReverseConversionOption(PKSimModuleConversion conversionOption, bool hastChangedModuleType)
      {
         if (!hastChangedModuleType)
            return PKSimModuleConversion.NoChange;

         switch (conversionOption)
         {
            case PKSimModuleConversion.SetAsExtensionModule:
               return PKSimModuleConversion.SetAsPKSimModule;
            case PKSimModuleConversion.SetAsPKSimModule:
               return PKSimModuleConversion.SetAsExtensionModule;
            default:
               return PKSimModuleConversion.NoChange;
         }
      }
   }
}