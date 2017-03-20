using System.ComponentModel;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.DTO
{
   public class ReactionBuilderDTO : ReactionInfoDTO
   {
      public IReactionBuilder ReactionBuilder { get; private set; }

      public ReactionBuilderDTO(IReactionBuilder reactionBuilder)
      {
         ReactionBuilder = reactionBuilder;
      }

      public BindingList<ReactionPartnerBuilderDTO> Educts { get; set; }
      public BindingList<ReactionPartnerBuilderDTO> Products { get; set; }

      public bool CreateProcessRateParameter
      {
         get { return ReactionBuilder.CreateProcessRateParameter; }
         set { ReactionBuilder.CreateProcessRateParameter = value; }
      }

      public bool ProcessRateParameterPersistable
      {
         get { return ReactionBuilder.ProcessRateParameterPersistable; }
         set { ReactionBuilder.ProcessRateParameterPersistable = value; }
      }
   }

   public class ReactionModifierBuilderDTO : IViewItem
   {
      public ReactionModifierBuilderDTO(string modifierName)
      {
         ModiferName = modifierName;
      }

      public string ModiferName { get; set; }
   }

   public class ReactionPartnerBuilderDTO : IViewItem
   {
      public IReactionPartnerBuilder PartnerBuilder { get; }

      public ReactionPartnerBuilderDTO(IReactionPartnerBuilder reactionPartnerBuilder)
      {
         PartnerBuilder = reactionPartnerBuilder;
      }

      public string MoleculeName
      {
         get { return PartnerBuilder.MoleculeName; }
         set
         {
            /*nothign to do here. Set in command*/
         }
      }

      public bool IsEduct { get; set; }

      public double StoichiometricCoefficient
      {
         get { return PartnerBuilder.StoichiometricCoefficient; }
         set
         {
            /*nothign to do here. Set in command*/
         }
      }
   }
}