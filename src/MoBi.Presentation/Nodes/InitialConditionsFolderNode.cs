using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class InitialConditionsFolderNode : AbstractNode<Classification>
   {
      public InitialConditionsFolderNode() : base(new Classification())
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.InitialConditions;
         Icon = ApplicationIcons.InitialConditionsFolder;
      }
      public override string Id { get; }
   }
}