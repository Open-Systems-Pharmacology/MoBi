using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.IntegrationTests
{

   public class When_loading_a_reaction_building_block_from_pkml_file : ContextWithLoadedProject
   {
      [Observation]
      public void should_have_a_diagram_manager_initialized()
      {
         var reactionBuildingBlock = LoadPKML<IMoBiReactionBuildingBlock>("warnings");
         reactionBuildingBlock.ShouldNotBeNull();
         reactionBuildingBlock.DiagramManager.ShouldNotBeNull();
      }
   }
}	