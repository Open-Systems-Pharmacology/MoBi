namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionParameter : PathAndValueEntity, IStartValue
   {
      /// <summary>
      /// Do not use! When refactoring on promotion to core, this should be removed
      /// </summary>
      public double? StartValue { get; set; }
   }
}
