using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Reporting.Items
{
   public class DistributionFormulaTextBox
   {
      private readonly string _caption;
      private readonly IDistributionFormula _formula;

      public DistributionFormulaTextBox(string caption, IDistributionFormula formula)
      {
         _caption = caption;
         _formula = formula;
      }

      public string Caption
      {
         get { return _caption; }
      }

      public IFormula Formula
      {
         get { return _formula; }
      }
   }
}
