using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class ObjectFormulaDTO
   {
      public IObjectBase ObjectBase { set; get; }
      public IFormula Formula { set; get; }

      public string Name
      {
         get { return ObjectBase.Name; }
      }
   }
}