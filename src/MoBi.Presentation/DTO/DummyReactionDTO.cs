using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.DTO
{
   public class DummyReactionDTO : ObjectBaseDTO, IDummyContainer
   {
      public IContainer StructureParent { get; set; }
      public IReactionBuilder ReactionBuilder { get; }

      public DummyReactionDTO(IReactionBuilder reactionBuilder) : base(reactionBuilder)
      {
         ReactionBuilder = reactionBuilder;
         Id = ShortGuid.NewGuid();
      }
   }
}