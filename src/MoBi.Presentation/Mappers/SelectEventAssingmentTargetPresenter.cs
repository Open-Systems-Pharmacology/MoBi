using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToDummyReactionDTOMapper
   {
      DummyReactionDTO MapFrom(ReactionBuilder reactionBuilder, IContainer container);
   }

   internal class ReactionBuilderToDummyReactionDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionBuilderToDummyReactionDTOMapper
   {
      public DummyReactionDTO MapFrom(ReactionBuilder reactionBuilder, IContainer container)
      {
         var dto = Map(new DummyReactionDTO(reactionBuilder));
         dto.StructureParent = container;
         return dto;
      }
   }
}