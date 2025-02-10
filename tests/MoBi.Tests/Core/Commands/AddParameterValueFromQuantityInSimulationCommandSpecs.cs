using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterValueFromQuantityInSimulationCommand : ContextSpecification<AddParameterValueFromQuantityInSimulationCommand>
   {
      protected ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      protected ObjectPath _objectPath;
      protected IParameter _parameter;
      protected IMoBiContext _context;
      private IEntityPathResolver _entityPathResolver;
      protected IParameterValuesCreator _parameterValuesCreator;

      protected override void Context()
      {
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock().WithId("PSVBB");
         _objectPath = new ObjectPath("A", "B", "P");
         _parameter = A.Fake<IParameter>().WithName("P").WithId("P");
         sut = new AddParameterValueFromQuantityInSimulationCommand(_parameter, _parameterValuesBuildingBlock);

         _context = A.Fake<IMoBiContext>();
         _parameterValuesCreator = A.Fake<IParameterValuesCreator>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(_entityPathResolver);
         A.CallTo(() => _context.Resolve<IParameterValuesCreator>()).Returns(_parameterValuesCreator);
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         A.CallTo(() => _context.Get<PathAndValueEntityBuildingBlock<ParameterValue>>(_parameterValuesBuildingBlock.Id)).Returns(_parameterValuesBuildingBlock);

         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(_objectPath);
      }
   }

   public class adding_a_parameter_value_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation : concern_for_AddParameterValueFromQuantityInSimulationCommand
   {
      private ParameterValue _parameterValue;

      protected override void Context()
      {
         base.Context();
         _parameterValue = new ParameterValue {Path = _objectPath};
         A.CallTo(() => _parameterValuesCreator.CreateParameterValue(_objectPath, _parameter)).Returns(_parameterValue);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_a_new_parameter_start_value_in_the_given_building_block()
      {
         _parameterValuesBuildingBlock[_objectPath].ShouldBeEqualTo(_parameterValue);
      }
   }

   public class adding_a_parameter_value_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation_that_already_exists : concern_for_AddParameterValueFromQuantityInSimulationCommand
   {
      private ParameterValue _parameterValue;

      protected override void Context()
      {
         base.Context();
         _parameterValue = new ParameterValue {Path = _objectPath};
         _parameterValuesBuildingBlock.Add(_parameterValue);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_add_a_new_parameter_start_value()
      {
         A.CallTo(() => _parameterValuesCreator.CreateParameterValue(_objectPath, _parameter)).MustNotHaveHappened();
      }
   }

   public class reverting_the_add_parameter_value_from_quantity_in_simulation_command : concern_for_AddParameterValueFromQuantityInSimulationCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterValuesCreator.CreateParameterValue(_objectPath, _parameter)).Returns(new ParameterValue {Path = _objectPath});
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_removed_the_added_parameter_start_value()
      {
         _parameterValuesBuildingBlock[_objectPath].ShouldBeNull();
      }
   }
}