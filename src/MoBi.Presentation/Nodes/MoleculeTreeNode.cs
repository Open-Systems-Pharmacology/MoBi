using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Nodes;
using System.ComponentModel;

namespace MoBi.Presentation.Nodes
{
   public interface IMoleculeTreeNode : ITreeNode
   {
   }

   public class MoleculeTreeNode : AbstractNode, IMoleculeTreeNode
   {
      private IObjectBaseDTO _tag;

      public MoleculeTreeNode(IObjectBaseDTO tag)
      {
         _tag = tag;
         _tag.PropertyChanged += OnPropertyChanged;
      }

      public override string Id
      {
         get { return _tag.Id; }
      }

      public override object TagAsObject
      {
         get { return _tag; }
      }

      public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if(e.PropertyName.Equals("Name"))
         {
            Text = _tag.Name;
         }
      }
   }
}