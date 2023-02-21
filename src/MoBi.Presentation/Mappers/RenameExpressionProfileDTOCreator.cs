using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IRenameExpressionProfileDTOCreator
   {
      RenameExpressionProfileDTO Create(string molecule, string species, string category, ExpressionType expressionType);
   }
   
   public class RenameExpressionProfileDTOCreator : IRenameExpressionProfileDTOCreator
   {
      public RenameExpressionProfileDTO Create(string molecule, string species, string category, ExpressionType expressionType)
      {
         return new RenameExpressionProfileDTO
         {
            Species = species,
            Category = category,
            MoleculeName = molecule,
            Type = expressionType.DisplayName
         };
      }
   }
}