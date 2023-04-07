using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Reporting.Items
{
   public class DistributionFormulaTextBox
   {
      private readonly string _caption;
      private readonly DistributionFormula _formula;

      public DistributionFormulaTextBox(string caption, DistributionFormula formula)
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
