using System;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Repository;

using OSPSuite.Core.Domain;


namespace MoBi.Core
{
   public abstract class concern_for_GroupRepository : ContextSpecification<IGroupRepository>
   {
      protected override void Context()
      {
         sut = new GroupRepository();
      }
   }

   class When_Assking_for_a_unknown_id : concern_for_GroupRepository
   {
      private IGroup _group;

      protected override void Because()
      {
         _group = sut.GroupById("11");
      }

      [Observation]
      public void should_return_group_DummyGroup()
      {
         _group.ShouldNotBeNull();
         _group.Name.ShouldBeEqualTo(Constants.Groups.MOBI);
      }
   }
   class When_Assking_for_a_unknown_name : concern_for_GroupRepository
   {
      private IGroup _group;

      protected override void Because()
      {
         _group = sut.GroupByName("ToTo");
      }

      [Observation]
      public void should_return_group_DummyGroup()
      {
         _group.ShouldNotBeNull();
         _group.Name.ShouldBeEqualTo(Constants.Groups.MOBI);
      }
   }
   class When_Assking_for_a_known_name : concern_for_GroupRepository
   {
      private IGroup _group;

      protected override void Because()
      {
         _group = sut.GroupByName(Constants.Groups.MOBI);
      }

      [Observation]
      public void should_return_group_undefiend()
      {
         _group.ShouldNotBeNull();
         _group.Name.ShouldBeEqualTo(Constants.Groups.MOBI);
      }
   }
}	