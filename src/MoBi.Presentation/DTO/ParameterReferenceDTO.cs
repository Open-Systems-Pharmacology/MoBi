using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ParameterReferenceDTO
   {
      /// <summary>
      ///    The object whose formula references the parameter
      /// </summary>
      public IUsingFormula UsedByObject { get; set; }

      /// <summary>
      ///    Path of the object whose formula references the parameter
      /// </summary>
      public string UsedBy { get; set; }

      /// <summary>
      ///    Type of the object whose formula references the parameter (Parameter, Observer etc.)
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      ///    Indicates whether the parameter is referenced in the formula or in the right hand side formula
      /// </summary>
      public string UsedAs { get; set; }

      /// <summary>
      ///    Alias under which the parameter is used in the formula
      /// </summary>
      public string Alias { get; set; }

      public string FormulaName { get; set; }

      public string Formula { get; set; }
   }
}
