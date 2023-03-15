using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ReactionDTO : ObjectBaseDTO
   {
      public ReactionDTO(IReaction reaction) : base(reaction)
      {
      }

      public string Kinetic { get; set; }
      public string Stoichiometric { get; set; }
   }
}