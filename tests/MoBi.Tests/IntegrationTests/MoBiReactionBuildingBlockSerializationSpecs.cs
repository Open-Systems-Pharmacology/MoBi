using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public class When_serializing_and_deserializing_a_reaction_building_block : ContextForIntegration<IXmlSerializationService>
   {
      private MoBiReactionBuildingBlock _deserializedBuildingBlock;

      protected override void Because()
      {
         var reactionBuildingBlock = IoC.Resolve<IReactionBuildingBlockFactory>().Create().WithName("Reactions");
         _deserializedBuildingBlock = sut.Deserialize<MoBiReactionBuildingBlock>(sut.SerializeAsString(reactionBuildingBlock), new MoBiProject());
      }

      [Observation]
      public void should_create_the_diagram_model_with_the_reaction_diagram_model_factory()
      {
         _deserializedBuildingBlock.DiagramModel.ShouldBeAnInstanceOf<DiagramModel>();
      }

      [Observation]
      public void should_initialize_the_diagram_manager()
      {
         _deserializedBuildingBlock.DiagramManager.ShouldNotBeNull();
      }
   }
}
