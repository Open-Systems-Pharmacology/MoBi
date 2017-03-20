using System.Drawing;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.UI.Diagram.Nodes
{
   public class ParameterNode : ElementBaseNode
   {
      public ParameterNode()
      {
         UserFlags = 3;
         LabelSpot = MiddleRight;

         NodeBaseSize = new SizeF(10F, 10F);
         NodeSize = OSPSuite.Core.Diagram.NodeSize.Middle;

         Port.IsValidFrom = false;
         Port.IsValidTo = false;

         Text = "";
      }
   }
}
