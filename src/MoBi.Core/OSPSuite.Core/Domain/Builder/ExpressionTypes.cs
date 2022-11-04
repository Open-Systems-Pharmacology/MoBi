namespace OSPSuite.Core.Domain.Builder
{
   public static class ExpressionTypes
   {
      public static ExpressionType TransportProtein = new ExpressionType(_transporter);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(_enzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(_protein);

      private const string _transporter = "Transporter";
      private const string _protein = "Protein";
      private const string _enzyme = "Enzyme";
   }

   public class ExpressionType
   {
      public string IconName { get; }

      public ExpressionType(string iconName)
      {
         IconName = iconName;
      }
   }
}