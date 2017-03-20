using System.Collections.ObjectModel;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;


namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_MoleculeBuilderDTOToTreeNodeMapperSpecs : ContextSpecification<IMoleculeBuilderDTOToTreeNodeMapper>
   {
      protected ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper _transportMoleculeMapper;
      protected IInteractionContainerDTOToMoleculeTreeMapper _interactionContainerMapper;
      protected override void Context()
      {
         _transportMoleculeMapper = A.Fake<ITransporterMoleculeContainerDTOToMoleculeTreeNodeMapper>();
         _interactionContainerMapper = A.Fake<IInteractionContainerDTOToMoleculeTreeMapper>();
         sut = new MoleculeBuilderDTOToTreeNodeMapper(_transportMoleculeMapper, _interactionContainerMapper);
      }
   }

   class When_mapping_a_molecule_builder_dto_to_a_tree_node : concern_for_MoleculeBuilderDTOToTreeNodeMapperSpecs
   {
      private MoleculeBuilderDTO _moleculeBuilderDTO;
      private TransporterMoleculeContainerDTO _transporterMolecule;
      private InteractionContainerDTO _interactionContainer;
      private IMoleculeTreeNode _treeNode;
      

      protected override void Context()
      {
         base.Context();
         _moleculeBuilderDTO = new MoleculeBuilderDTO();
         _transporterMolecule = new TransporterMoleculeContainerDTO().WithName("aa").WithId("bb");
         _interactionContainer = new InteractionContainerDTO().WithName("cc").WithId("dd");
         _moleculeBuilderDTO.TransporterMolecules = new []{_transporterMolecule};
         _moleculeBuilderDTO.InteractionContainerCollection = new[] {_interactionContainer};
      }

      protected override void Because()
      {
         _treeNode = sut.MapFrom(_moleculeBuilderDTO);
      }

      [Observation]
      public void should_create_a_vailde_treeNode()
      {
         _treeNode.ShouldNotBeNull();
      }

      [Observation]
      public void should_map_children()
      {
         A.CallTo(() => _transportMoleculeMapper.MapFrom(_transporterMolecule)).MustHaveHappened();
         A.CallTo(() => _interactionContainerMapper.MapFrom(_interactionContainer)).MustHaveHappened();
         
      }
   }
}	