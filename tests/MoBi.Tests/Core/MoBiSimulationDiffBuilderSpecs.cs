using OSPSuite.Utility.Container;
using OSPSuite.Utility.Validation;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Comparison;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Core
{
   public abstract class concern_for_ObjectComparer : ContextForIntegration<IObjectComparer>
   {
      protected IObjectBase _object1;
      protected IObjectBase _object2;
      protected ComparerSettings _comparerSettings;
      protected DiffReport _report;

      protected override void Context()
      {
         sut = IoC.Resolve<IObjectComparer>();
         _comparerSettings = new ComparerSettings();
      }

      protected override void Because()
      {
         _report = sut.Compare(_object1, _object2, _comparerSettings);
      }
   }

   class When_comparing_two_similar_simulations : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var m1 = new Model().WithName("S1");
         var m2 = new Model().WithName("S1");

         var bc1 = A.Fake<IMoBiBuildConfiguration>();
         var bc2 = A.Fake<IMoBiBuildConfiguration>();

         var sim1 = new MoBiSimulation{BuildConfiguration = bc1, Model = m1}.WithName("S1");
         var sim2 = new MoBiSimulation{BuildConfiguration = bc2, Model = m2}.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }


      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   class When_comparing_two_simulations_build_with_different_Reaction_building_blocks : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var m1 = new Model().WithName("S1");
         var m2 = new Model().WithName("S1");

         var bc1 = A.Fake<IMoBiBuildConfiguration>();
         var bc2 = A.Fake<IMoBiBuildConfiguration>();

         var reactionInfo1 = new ReactionBuildingBlockInfo
         {
            BuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1"),
            TemplateBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1")
         };
         bc1.ReactionsInfo = reactionInfo1;

         var reactionInfo2 = new ReactionBuildingBlockInfo
         {
            BuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R2"),
            TemplateBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R2")
         };
         bc2.ReactionsInfo = reactionInfo2;

         var sim1 = new MoBiSimulation { BuildConfiguration = bc1, Model = m1 }.WithName("S1");
         var sim2 = new MoBiSimulation { BuildConfiguration = bc2, Model = m2 }.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }


      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   class When_comparing_two_simulations_build_with_different_Reaction_building_block_versions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var m1 = new Model().WithName("S1");
         var m2 = new Model().WithName("S1");

         var bc1 = A.Fake<IMoBiBuildConfiguration>();
         var bc2 = A.Fake<IMoBiBuildConfiguration>();

         var reactionInfo1 = new ReactionBuildingBlockInfo
         {
            BuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1"),
            TemplateBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1")
         };
         reactionInfo1.BuildingBlock.Version = 1;
         bc1.ReactionsInfo = reactionInfo1;

         var reactionInfo2 = new ReactionBuildingBlockInfo
         {
            BuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1"),
            TemplateBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithName("R1")
         };
         reactionInfo2.BuildingBlock.Version = 3;
         bc2.ReactionsInfo = reactionInfo2;

         var sim1 = new MoBiSimulation { BuildConfiguration = bc1, Model = m1 }.WithName("S1");
         var sim2 = new MoBiSimulation { BuildConfiguration = bc2, Model = m2 }.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }


      [Observation]
      public void should_report_the_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}	