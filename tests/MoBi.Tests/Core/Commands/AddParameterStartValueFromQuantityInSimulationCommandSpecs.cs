using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterStartValueFromQuantityInSimulationCommand : ContextSpecification<AddParameterStartValueFromQuantityInSimulationCommand>
   {
      protected IParameterStartValuesBuildingBlock _parameterStartValueBuildingBlock;
      protected IObjectPath _objectPath;
      protected IParameter _parameter;
      protected IMoBiContext _context;
      private IEntityPathResolver _entityPathResolver;
      protected IParameterStartValuesCreator _parameterStartValuesCreator;

      protected override void Context()
      {
         _parameterStartValueBuildingBlock = new ParameterStartValuesBuildingBlock().WithId("PSVBB");
         _objectPath = new ObjectPath("A", "B", "P");
         _parameter = A.Fake<IParameter>().WithName("P").WithId("P");
         sut = new AddParameterStartValueFromQuantityInSimulationCommand(_parameter, _parameterStartValueBuildingBlock);

         _context = A.Fake<IMoBiContext>();
         _parameterStartValuesCreator = A.Fake<IParameterStartValuesCreator>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(_entityPathResolver);
         A.CallTo(() => _context.Resolve<IParameterStartValuesCreator>()).Returns(_parameterStartValuesCreator);
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         A.CallTo(() => _context.Get<IStartValuesBuildingBlock<ParameterStartValue>>(_parameterStartValueBuildingBlock.Id)).Returns(_parameterStartValueBuildingBlock);

         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(_objectPath);
      }
   }

   public class When_adding_a_parameter_start_value_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation : concern_for_AddParameterStartValueFromQuantityInSimulationCommand
   {
      private ParameterStartValue _parameterStartValue;

      protected override void Context()
      {
         base.Context();
         _parameterStartValue = new ParameterStartValue {Path = _objectPath};
         A.CallTo(() => _parameterStartValuesCreator.CreateParameterStartValue(_objectPath, _parameter)).Returns(_parameterStartValue);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_a_new_parameter_start_value_in_the_given_building_block()
      {
         _parameterStartValueBuildingBlock[_objectPath].ShouldBeEqualTo(_parameterStartValue);
      }
   }

   public class When_adding_a_parameter_start_value_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation_that_already_exists : concern_for_AddParameterStartValueFromQuantityInSimulationCommand
   {
      private ParameterStartValue _parameterStartValue;

      protected override void Context()
      {
         base.Context();
         _parameterStartValue = new ParameterStartValue {Path = _objectPath};
         _parameterStartValueBuildingBlock.Add(_parameterStartValue);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_add_a_new_parameter_start_value()
      {
         A.CallTo(() => _parameterStartValuesCreator.CreateParameterStartValue(_objectPath, _parameter)).MustNotHaveHappened();
      }
   }

   public class When_reverting_the_add_parameter_start_value_from_quantity_in_simulation_command : concern_for_AddParameterStartValueFromQuantityInSimulationCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterStartValuesCreator.CreateParameterStartValue(_objectPath, _parameter)).Returns(new ParameterStartValue {Path = _objectPath});
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_removed_the_added_parameter_start_value()
      {
         _parameterStartValueBuildingBlock[_objectPath].ShouldBeNull();
      }
   }
}