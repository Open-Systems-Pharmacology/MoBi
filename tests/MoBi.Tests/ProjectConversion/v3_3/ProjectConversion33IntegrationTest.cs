using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.Core.Serialization.Converter.v3_3;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.ProjectConversion.v3_3
{
   public abstract class concern_for_ProjectConversionIntegrationTest : ContextWithLoadedProject
   {
      protected SimulationTransfer _simulationTransfer;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationTransfer = LoadPKML<SimulationTransfer>("523_Export_S1");
      }
   }

   internal class When_converting_project_523_Export_S1 : concern_for_ProjectConversionIntegrationTest
   {
      protected IEventGroupBuildingBlock _eventGroupBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuildingBlock = _simulationTransfer.Simulation.BuildConfiguration.EventGroups;
      }

      [Observation]
      public void should_have_changed_GallbladderEmptyin_Transport()
      {
         var passiveTransportBuildingBlock = _simulationTransfer.Simulation.BuildConfiguration.PassiveTransports;
         var gallbladderEmptying = passiveTransportBuildingBlock.FirstOrDefault(pt => pt.Name.Equals("GallbladderEmptying"));
         gallbladderEmptying.ShouldNotBeNull();
         var kinetic = ((ExplicitFormula) gallbladderEmptying.Formula);
         kinetic.FormulaString.ShouldBeEqualTo("EHC_Active ? ln(2) / EHC_Halftime * M * EHC_EjectionFraction : 0");
         var alias = kinetic.ObjectPaths.Select(op => op.Alias);
         alias.ShouldOnlyContain("EHC_Active", "EHC_Halftime", "M", "EHC_EjectionFraction");
      }

      [Observation]
      public void should_change_EHC_Start_event_to_assign_Gallbladder_emptying_active()
      {
         var eventGroup = _eventGroupBuildingBlock.First(eg => eg.Name.Equals("EHC"));
         eventGroup = eventGroup.GetSingleChildByName<EventGroupBuilder>("EHC_1");
         var ehcStartEvent = eventGroup.GetSingleChildByName<IEventBuilder>(Converter321To331.EHCStartEvent);
         var assingment = ehcStartEvent.Assignments.FirstOrDefault(a => a.ObjectPath.PathAsString.Equals("Organism|Gallbladder|Gallbladder emptying active"));
         assingment.ShouldNotBeNull();
         assingment.UseAsValue.ShouldBeFalse();
         assingment.Formula.IsConstant().ShouldBeTrue();
         ((ConstantFormula) assingment.Formula).Value.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_change_EHC_Stop_event_to_assign_Gallbladder_emptying_active()
      {
         var eventGroup = _eventGroupBuildingBlock.First(eg => eg.Name.Equals("EHC"));
         eventGroup = eventGroup.GetSingleChildByName<EventGroupBuilder>("EHC_1");
         var ehcStopEvent = eventGroup.GetSingleChildByName<IEventBuilder>(Converter321To331.EHCStopEvent);
         var assingment = ehcStopEvent.Assignments.FirstOrDefault(a => a.ObjectPath.PathAsString.Equals("Organism|Gallbladder|Gallbladder emptying active"));
         assingment.ShouldNotBeNull();
         assingment.UseAsValue.ShouldBeTrue();
         assingment.Formula.IsConstant().ShouldBeTrue();
         ((ConstantFormula) assingment.Formula).Value.ShouldBeEqualTo(0);
      }
   }
}