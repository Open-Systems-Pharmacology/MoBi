using MoBi.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public class AddNewIndividualTransporterBuildingBlock : IUICommand
   {
      private readonly IPKSimStarter _pkSimStarter;

      public AddNewIndividualTransporterBuildingBlock(IPKSimStarter pkSimStarter)
      {
         _pkSimStarter = pkSimStarter;
      }

      public void Execute()
      {
         _pkSimStarter.CreateTransporterExpression();
      }
   }

   public class AddNewBindingPartnerBuildingBlock : IUICommand
   {
      private readonly IPKSimStarter _pkSimStarter;

      public AddNewBindingPartnerBuildingBlock(IPKSimStarter pkSimStarter)
      {
         _pkSimStarter = pkSimStarter;
      }

      public void Execute()
      {
         _pkSimStarter.CreateBindingPartnerExpression();
      }
   }

   public class AddNewMetabolizingEnzymeBuildingBlock : IUICommand
   {
      private readonly IPKSimStarter _pkSimStarter;

      public AddNewMetabolizingEnzymeBuildingBlock(IPKSimStarter pkSimStarter)
      {
         _pkSimStarter = pkSimStarter;
      }

      public void Execute()
      {
         _pkSimStarter.CreateMetabolizingEnzymeExpression();
      }
   }
}