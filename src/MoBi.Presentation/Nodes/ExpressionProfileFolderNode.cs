using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ParameterStartValuesFolderNode : AbstractNode<Classification>
   {
      public ParameterStartValuesFolderNode() : base(new Classification())
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ParameterStartValues;
         Icon = ApplicationIcons.ParameterStartValuesFolder;
      }
      public override string Id { get; }
   }

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

   public class ExpressionProfileFolderNode : AbstractNode
   {
      public ExpressionProfileFolderNode()
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ExpressionProfiles;
         Icon = ApplicationIcons.ExpressionProfileFolder;
      }
      public override string Id { get; }
      public override object TagAsObject { get; }
   }
}