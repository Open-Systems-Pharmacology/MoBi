using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderDTOToTreeNodeMapper : IMapper<MoleculeBuilderDTO, IMoleculeTreeNode>
   {
   }

   internal class MoleculeBuilderDTOToTreeNodeMapper : IMoleculeBuilderDTOToTreeNodeMapper
   {
      private readonly ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper _activeTransportBuilderContainerToMoleculeTreeNodeMapper;
      private readonly IInteractionContainerDTOToMoleculeTreeMapper _interactionContainerMapper;

      public MoleculeBuilderDTOToTreeNodeMapper(ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper activeTransportBuilderContainerToMoleculeTreeNodeMapper, IInteractionContainerDTOToMoleculeTreeMapper interactionContainerMapper)
      {
         _activeTransportBuilderContainerToMoleculeTreeNodeMapper = activeTransportBuilderContainerToMoleculeTreeNodeMapper;
         _interactionContainerMapper = interactionContainerMapper;
      }

      public IMoleculeTreeNode MapFrom(MoleculeBuilderDTO moleculeBuilderDTO)
      {
         var node = new MoleculeTreeNode(moleculeBuilderDTO) {Text = moleculeBuilderDTO.Name, Icon = ApplicationIcons.IconByName(moleculeBuilderDTO.Icon)};
         var children = moleculeBuilderDTO.TransporterMolecules.MapAllUsing(_activeTransportBuilderContainerToMoleculeTreeNodeMapper).ToList();
         children = children.Union(moleculeBuilderDTO.InteractionContainerCollection.MapAllUsing(_interactionContainerMapper)).ToList();
         children.Each(node.AddChild);
         return node;
      }
   }

   public interface ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper : IMapper<TransporterMoleculeContainerDTO, IMoleculeTreeNode>
   {
   }

   internal class TransporterMoleculeContainerDTOToMoleculeTreeNodeMapper : ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper
   {
      private readonly ITransportBuilderDTOToMoleculeTreeNodeMapper _activeTransportRealizationsBuilderToMoleculeTreeNodeMapper;

      public TransporterMoleculeContainerDTOToMoleculeTreeNodeMapper(ITransportBuilderDTOToMoleculeTreeNodeMapper activeTransportRealizationsBuilderToMoleculeTreeNodeMapper)
      {
         _activeTransportRealizationsBuilderToMoleculeTreeNodeMapper = activeTransportRealizationsBuilderToMoleculeTreeNodeMapper;
      }

      public IMoleculeTreeNode MapFrom(TransporterMoleculeContainerDTO input)
      {
         var node = new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
         var children = input.Realizations.MapAllUsing(_activeTransportRealizationsBuilderToMoleculeTreeNodeMapper);
         children.Each(node.AddChild);
         return node;
      }
   }

   internal interface ITransportBuilderDTOToMoleculeTreeNodeMapper : IMapper<TransportBuilderDTO, IMoleculeTreeNode>
   {
   }

   internal class TransportBuilderDTOToMoleculeTreeNodeMapper : ITransportBuilderDTOToMoleculeTreeNodeMapper
   {
      public IMoleculeTreeNode MapFrom(TransportBuilderDTO input)
      {
         return new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
      }
   }

   public interface IInteractionContainerDTOToMoleculeTreeMapper : IMapper<InteractionContainerDTO, IMoleculeTreeNode>
   {
   }

   public class InteractionContainerDTOToMoleculeTreeMapper : IInteractionContainerDTOToMoleculeTreeMapper
   {
      public IMoleculeTreeNode MapFrom(InteractionContainerDTO interactionContainerDTO)
      {
         return new MoleculeTreeNode(interactionContainerDTO)
         {
            Text = interactionContainerDTO.Name, Icon = ApplicationIcons.IconByName(interactionContainerDTO.Icon)
         };
      }
   }
}