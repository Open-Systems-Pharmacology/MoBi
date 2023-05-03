using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class MoleculeStartValuesFolderNode : AbstractNode<Classification>
   {
      public MoleculeStartValuesFolderNode() : base(new Classification())
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.MoleculeStartValues;
         Icon = ApplicationIcons.MoleculeStartValuesFolder;
      }
      public override string Id { get; }
   }
}