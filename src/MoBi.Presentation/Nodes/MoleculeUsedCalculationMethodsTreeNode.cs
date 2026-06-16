using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation.Nodes;

public class MoleculeUsedCalculationMethodsTreeNode : AbstractNode
{
   public MoleculeUsedCalculationMethodsTreeNode(MoleculeUsedCalculationMethodsDTO dto)
   {
      TagAsObject = dto;
      Id = dto.Name;
      Text = dto.Name;
   }

   public override string Id { get; }
   public override object TagAsObject { get; }
}