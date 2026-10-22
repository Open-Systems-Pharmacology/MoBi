using System.Linq;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Extensions;

public static class ValidationMessageExtensions
{
   /// <summary>
   ///    Returns the text of the <paramref name="validationMessage" /> followed by its details, if any
   /// </summary>
   public static string TextWithDetails(this ValidationMessage validationMessage) =>
      validationMessage.Details != null && validationMessage.Details.Any()
         ? $"{validationMessage.Text} - {string.Join(", ", validationMessage.Details)}"
         : validationMessage.Text;
}
