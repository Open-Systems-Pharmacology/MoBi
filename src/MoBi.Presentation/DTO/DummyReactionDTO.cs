using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.DTO
{
   public class DummyReactionDTO : ObjectBaseDTO, IDummyContainer
   {
      public IContainer StructureParent { get; set; }
      public ReactionBuilder ReactionBuilder { get; }

      public DummyReactionDTO(ReactionBuilder reactionBuilder) : base(reactionBuilder)
      {
         ReactionBuilder = reactionBuilder;
         Id = ShortGuid.NewGuid();
      }
   }
}