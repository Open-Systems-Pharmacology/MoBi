using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class TextNode : AbstractNode
   {
      public TextNode(string text) : this(text, ShortGuid.NewGuid())
      {
      }

      public TextNode(string text, string id)
      {
         Text = text;
         Id = id;
      }

      public override string Id { get; }

      public override object TagAsObject => Text;
   }
}