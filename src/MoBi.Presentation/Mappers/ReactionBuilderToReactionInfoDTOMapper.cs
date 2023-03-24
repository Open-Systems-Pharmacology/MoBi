using OSPSuite.Utility;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToReactionInfoDTOMapper : IMapper<IReactionBuilder, ReactionInfoDTO>
   {
   }

   public class ReactionBuilderToReactionInfoDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionBuilderToReactionInfoDTOMapper
   {
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;

      public ReactionBuilderToReactionInfoDTOMapper(IStoichiometricStringCreator stoichiometricStringCreator)
      {
         _stoichiometricStringCreator = stoichiometricStringCreator;
      }

      public ReactionInfoDTO MapFrom(IReactionBuilder reactionBuilder)
      {
         var dto = Map(new ReactionInfoDTO(reactionBuilder));
         dto.StoichiometricFormula = _stoichiometricStringCreator.CreateFrom(reactionBuilder.Educts, reactionBuilder.Products);
         if (reactionBuilder.Formula != null)
         {
            dto.Kinetic = reactionBuilder.Formula.ToString();
            reactionBuilder.Formula.PropertyChanged += dto.FormulaChangedHandler;
         }
         return dto;
      }
   }
}