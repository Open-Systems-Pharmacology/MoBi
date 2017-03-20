using OSPSuite.Presentation.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.Nodes
{
   internal class SpatialStructureReferenceRootNode : AbstractNode
   {
      public SpatialStructureReferenceRootNode()
      {
         Text = "Localized Parameter";
      }

      public override string Id
      {
         get { return "SpatialStructureReference"; }
      }

      public override object TagAsObject
      {
         get { return null; }
      }
   }

   internal class MoleculeParamterReferenceRootNode : AbstractNode
   {
      public MoleculeParamterReferenceRootNode()
      {
         Text = "Molecule Properties";
         Icon = ApplicationIcons.Molecule;
      }

      public override string Id
      {
         get { return "MoleculeParametersReference"; }
      }

      public override object TagAsObject
      {
         get { return null; }
      }
   }
}