using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Reporting.Items
{
   public class FormulaTextBox
   {
      private readonly string _caption;
      private readonly IFormula _formula;

      public FormulaTextBox(string caption, IFormula formula)
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
