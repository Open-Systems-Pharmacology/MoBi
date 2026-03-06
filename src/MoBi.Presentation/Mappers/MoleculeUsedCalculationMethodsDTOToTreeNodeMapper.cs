using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers;

public interface IMoleculeUsedCalculationMethodsDTOToTreeNodeMapper : IMapper<MoleculeUsedCalculationMethodsDTO, ITreeNode>
{

}

public class MoleculeUsedCalculationMethodsDTOToTreeNodeMapper : IMoleculeUsedCalculationMethodsDTOToTreeNodeMapper
{
   public ITreeNode MapFrom(MoleculeUsedCalculationMethodsDTO dto)
   {
      var node = new MoleculeUsedCalculationMethodsTreeNode(dto) { Icon = ApplicationIcons.IconByName(dto.Icon) };
      return node;
   }
}