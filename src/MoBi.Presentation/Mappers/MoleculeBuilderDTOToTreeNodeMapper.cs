using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderDTOToTreeNodeMapper : IMapper<MoleculeBuilderDTO, IMoleculeTreeNode>
   {
   }


   internal class MoleculeBuilderDTOToTreeNodeMapper : IMoleculeBuilderDTOToTreeNodeMapper
   {
      private readonly ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper _activeTransportBuilderContaienrToMolceculeTreeNodeMapper;
      private readonly IInteractionContainerDTOToMoleculeTreeMapper _interactionContainerMapper;

      public MoleculeBuilderDTOToTreeNodeMapper(ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper activeTransportBuilderContaienrToMolceculeTreeNodeMapper, IInteractionContainerDTOToMoleculeTreeMapper interactionContainerMapper)
      {
         _activeTransportBuilderContaienrToMolceculeTreeNodeMapper = activeTransportBuilderContaienrToMolceculeTreeNodeMapper;
         _interactionContainerMapper = interactionContainerMapper;
      }

      public IMoleculeTreeNode MapFrom(MoleculeBuilderDTO input)
      {
         var node = new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
         var children = input.TransporterMolecules.MapAllUsing(_activeTransportBuilderContaienrToMolceculeTreeNodeMapper).ToList();
         children = children.Union(input.InteractionContainerCollection.MapAllUsing(_interactionContainerMapper)).ToList();
         children.Each(node.AddChild);
         return node;
      }
   }


   public interface ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper : IMapper<TransporterMoleculeContainerDTO, IMoleculeTreeNode>
   {
   }

   internal class TransporterMoleculeContainerDTOToMoleculeTreeNodeMapper : ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper
   {
      private readonly ITranpsortBuilderDTOToMoleculeTreeNodeMapper _activeTransportRealizationsBuilderToMolceculeTreeNodeMapper;

      public TransporterMoleculeContainerDTOToMoleculeTreeNodeMapper(ITranpsortBuilderDTOToMoleculeTreeNodeMapper activeTransportRealizationsBuilderToMolceculeTreeNodeMapper)
      {
         _activeTransportRealizationsBuilderToMolceculeTreeNodeMapper = activeTransportRealizationsBuilderToMolceculeTreeNodeMapper;
      }

      public IMoleculeTreeNode MapFrom(TransporterMoleculeContainerDTO input)
      {
         var node = new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
         var children = input.Realizations.MapAllUsing(_activeTransportRealizationsBuilderToMolceculeTreeNodeMapper);
         children.Each(node.AddChild);
         return node;
      }
   }

   internal interface ITranpsortBuilderDTOToMoleculeTreeNodeMapper : IMapper<TransportBuilderDTO, IMoleculeTreeNode>
   {
   }

   internal class TranpsortBuilderDTOToMoleculeTreeNodeMapper : ITranpsortBuilderDTOToMoleculeTreeNodeMapper
   {
      public IMoleculeTreeNode MapFrom(TransportBuilderDTO input)
      {
         return new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
      }
   }

   public interface IInteractionContainerDTOToMoleculeTreeMapper:IMapper<InteractionContainerDTO,IMoleculeTreeNode>
   {
       
   }

   class InteractionContainerDTOToMoleculeTreeMapper : IInteractionContainerDTOToMoleculeTreeMapper
   {
      public IMoleculeTreeNode MapFrom(InteractionContainerDTO input)
      {
         return new MoleculeTreeNode(input) {Text = input.Name, Icon = ApplicationIcons.IconByName(input.Icon)};
      }
   }
}