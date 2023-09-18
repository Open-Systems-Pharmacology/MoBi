using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Extensions
{
   public static class ModuleExtensions
   {
      public static bool IsTemplateMatchFor(this Module module1, Module module2)
      {
         return module1 != null && module2.IsNamed(module1.Name);
      }
   }
}