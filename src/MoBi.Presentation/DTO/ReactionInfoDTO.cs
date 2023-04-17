using System.ComponentModel;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.DTO
{
   public class ReactionInfoDTO : ObjectBaseDTO
   {
      private string _stoichiometricFormula;
      public string Kinetic { get; set; }

      public ReactionInfoDTO(ReactionBuilder reactionBuilder) : base(reactionBuilder)
      {
      }

      public string StoichiometricFormula
      {
         get => _stoichiometricFormula;
         set
         {
            _stoichiometricFormula = value;
            OnPropertyChanged(() => StoichiometricFormula);
         }
      }

      protected override void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var reactionBuilder = sender.DowncastTo<ReactionBuilder>();
         if (e.PropertyName == "Formula")
         {
            Kinetic = reactionBuilder.Formula == null ? string.Empty : reactionBuilder.Formula.ToString();
         }

         base.HandlePropertyChanged(sender, e);
      }

      public void FormulaChangedHandler(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals("FormulaString"))
         {
            var formula = sender as ExplicitFormula;
            if (formula == null) return;

            Kinetic = formula.FormulaString;
            OnPropertyChanged(() => Kinetic);
         }
      }
   }
}