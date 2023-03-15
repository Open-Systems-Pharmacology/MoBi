using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class DummyReactionDTO : ObjectBaseDTO, IDummyContainer
   {
      public IContainer StructureParent { get; set; }
      public IReactionBuilder ReactionBuilder { get; set; }
   }
}