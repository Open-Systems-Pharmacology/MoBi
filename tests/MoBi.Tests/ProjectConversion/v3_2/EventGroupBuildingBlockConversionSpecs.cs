using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class concern_for_EventGroupBuildingBlockConversionSpecs : ContextWithLoadedProject
   {
      protected IEventGroupBuildingBlock _eventGroupBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _eventGroupBuildingBlock = LoadPKML<IEventGroupBuildingBlock>("Events");
      }
   }

   public class When_converting_an_event_group_building_block : concern_for_EventGroupBuildingBlockConversionSpecs
   {
      [Observation]
      public void should_move_event_assigments_to_children()
      {
         var eventGroup = _eventGroupBuildingBlock.First();
         var applicationBuilder = eventGroup.GetAllChildren<IApplicationBuilder>().First();
         eventGroup.ShouldNotBeNull();
         applicationBuilder.GetChildren<ITransportBuilder>().Count().ShouldBeEqualTo(1);
         applicationBuilder.GetChildren<IApplicationMoleculeBuilder>().Count().ShouldBeEqualTo(1);
         var domainEvent = eventGroup.GetAllChildren<IEventBuilder>().First();
         domainEvent.GetChildren<IEventAssignmentBuilder>().Count().ShouldBeEqualTo(1);
      }
   }
}