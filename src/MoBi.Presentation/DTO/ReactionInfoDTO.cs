using System.ComponentModel;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class ReactionInfoDTO : ObjectBaseDTO
   {
      private string _stoichiometricFormula;
      public string Kinetic { get; set; }

      public string StoichiometricFormula
      {
         get { return _stoichiometricFormula; }
         set
         {
            _stoichiometricFormula = value;
            OnPropertyChanged(() => StoichiometricFormula);
         }
      }

      public override void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var reactionBuilder = sender.DowncastTo<IReactionBuilder>();
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