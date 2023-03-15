using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToDummyReactionDTOMapper
   {
      DummyReactionDTO MapFrom(IReactionBuilder reactionBuilder, IContainer container);
   }

   internal class ReactionBuilderToDummyReactionDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionBuilderToDummyReactionDTOMapper
   {
      public DummyReactionDTO MapFrom(IReactionBuilder reactionBuilder, IContainer container)
      {
         var dto = Map(new DummyReactionDTO());
         dto.Id = ShortGuid.NewGuid();
         dto.ReactionBuilder = reactionBuilder;
         dto.StructureParent = container;
         return dto;
      }
   }
}