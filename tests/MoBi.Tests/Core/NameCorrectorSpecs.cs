using FakeItEasy;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_NameCorrector : ContextSpecification<INameCorrector>
   {
      protected IObjectBaseNamingTask _namingTask;
      protected IObjectBase _renameObject;
      protected string[] _alreadyUsedNames;

      protected override void Context()
      {
         _alreadyUsedNames = new[] { "a", "b" };
         _renameObject = A.Fake<IObjectBase>();
         _namingTask = A.Fake<IObjectBaseNamingTask>();

         sut = new NameCorrector(_namingTask, new ObjectTypeResolver(), new ContainerTask(A.Fake<IObjectBaseFactory>(), A.Fake<IEntityPathResolver>(), new ObjectPathFactoryForSpecs()));
      }
   }

   internal class When_auto_correcting_a_name_and_the_base_name_is_not_in_the_used_names : concern_for_NameCorrector
   {
      protected override void Context()
      {
         base.Context();
         _renameObject.Name = "c";
      }

      protected override void Because()
      {
         sut.AutoCorrectName(_alreadyUsedNames, _renameObject);
      }

      [Observation]
      public void the_original_name_should_be_kept()
      {
         _renameObject.Name.ShouldBeEqualTo("c");
      }
   }

   internal class When_correcting_a_valid_name : concern_for_NameCorrector
   {
      private bool _result;
      private string _name;

      protected override void Context()
      {
         base.Context();
         _name = "Name";
         _renameObject.Name = _name;
      }

      protected override void Because()
      {
         _result = sut.CorrectName(_alreadyUsedNames, _renameObject);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_leave_the_name_unchanged()
      {
         _renameObject.Name.ShouldBeEqualTo(_name);
      }
   }

   internal class When_correcting_a_invalid_name_and_a_valid_Name_is_provided : concern_for_NameCorrector
   {
      private bool _result;
      private string _name;

      protected override void Context()
      {
         base.Context();
         _name = "Name";
         _renameObject.Name = _alreadyUsedNames[0];
         A.CallTo(_namingTask).WithReturnType<string>().Returns(_name);
      }

      protected override void Because()
      {
         _result = sut.CorrectName(_alreadyUsedNames, _renameObject);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_have_change_the_name()
      {
         _renameObject.Name.ShouldBeEqualTo(_name);
      }
   }

   internal class When_correcting_a_invalid_name_and_the_action_is_cannceld : concern_for_NameCorrector
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _renameObject.Name = _alreadyUsedNames[0];
         A.CallTo(_namingTask).WithReturnType<string>().Returns(string.Empty);
      }

      protected override void Because()
      {
         _result = sut.CorrectName(_alreadyUsedNames, _renameObject);
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      [Observation]
      public void should_leave_the_name_unchanged()
      {
         _renameObject.Name.ShouldBeEqualTo(_alreadyUsedNames[0]);
      }
   }
}