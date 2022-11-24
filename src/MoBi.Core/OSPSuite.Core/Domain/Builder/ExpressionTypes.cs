namespace OSPSuite.Core.Domain.Builder
{
   public static class ExpressionTypes
   {
      public static ExpressionType TransportProtein = new ExpressionType(Assets.IconNames.Transporter, Assets.Captions.Transporter);
      public static ExpressionType MetabolizingEnzyme = new ExpressionType(Assets.IconNames.Enzyme, Assets.Captions.Enzyme);
      public static ExpressionType ProteinBindingPartner = new ExpressionType(Assets.IconNames.Protein, Assets.Captions.Protein);
   }

   public class ExpressionType
   {
      public string IconName { get; }
      public string DisplayName { get; }
      public ExpressionType(string iconName, string displayName)
      {
         IconName = iconName;
         DisplayName = displayName;
      }
   }
}