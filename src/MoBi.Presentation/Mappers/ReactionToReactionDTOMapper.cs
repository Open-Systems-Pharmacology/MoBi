using OSPSuite.Utility;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;

using OSPSuite.Core.Domain;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionToReactionDTOMapper : IMapper<IReaction, ReactionDTO>
   {
   }

   internal class ReactionToReactionDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionToReactionDTOMapper
   {
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;

      public ReactionToReactionDTOMapper(IStoichiometricStringCreator stoichiometricStringCreator)
      {
         _stoichiometricStringCreator = stoichiometricStringCreator;
      }

      public ReactionDTO MapFrom(IReaction reaction)
      {
         var dto = Map(new ReactionDTO(reaction));
         dto.Icon = ApplicationIcons.Reaction;
         dto.Kinetic = reaction.Formula.ToString();
         dto.Stoichiometric = _stoichiometricStringCreator.CreateFrom(reaction.Educts, reaction.Products);
         return dto;
      }
   }
}