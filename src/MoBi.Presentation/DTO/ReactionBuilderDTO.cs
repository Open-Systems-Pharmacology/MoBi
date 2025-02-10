using System.ComponentModel;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.DTO
{
   public class ReactionBuilderDTO : ReactionInfoDTO
   {
      public ReactionBuilder ReactionBuilder { get; }

      public ReactionBuilderDTO(ReactionBuilder reactionBuilder) : base(reactionBuilder)
      {
         ReactionBuilder = reactionBuilder;
      }

      public BindingList<ReactionPartnerBuilderDTO> Educts { get; set; }
      public BindingList<ReactionPartnerBuilderDTO> Products { get; set; }

      public bool CreateProcessRateParameter => ReactionBuilder.CreateProcessRateParameter;

      public bool ProcessRateParameterPersistable
      {
         get => ReactionBuilder.ProcessRateParameterPersistable;
         set => ReactionBuilder.ProcessRateParameterPersistable = value;
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
      public ReactionPartnerBuilder PartnerBuilder { get; }

      public ReactionPartnerBuilderDTO(ReactionPartnerBuilder reactionPartnerBuilder)
      {
         PartnerBuilder = reactionPartnerBuilder;
      }

      public string MoleculeName
      {
         get => PartnerBuilder.MoleculeName;
         set
         {
            /*nothing to do here. Set in command*/
         }
      }

      public bool IsEduct { get; set; }

      public double StoichiometricCoefficient
      {
         get => PartnerBuilder.StoichiometricCoefficient;
         set
         {
            /*nothing to do here. Set in command*/
         }
      }
   }
}