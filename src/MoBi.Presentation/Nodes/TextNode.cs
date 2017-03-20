using System;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class TextNode : AbstractNode
   {
      private readonly string _id;

      public TextNode(string text):this(text,ShortGuid.NewGuid())
      {
      }
      public TextNode(string text, string id)
      {
         Text = text;
         _id = id;
      }

      public override string Id
      {
         get { return _id; }
      }

      public override object TagAsObject
      {
         get { return Text; }
      }
   }
}