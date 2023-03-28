using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Core.Service
{
   public abstract class concern_for_ParameterStartValueSynchronizer : ContextSpecification<IParameterStartValueSynchronizer>
   {
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         sut = new ParameterStartValueSynchronizer(_entityPathResolver);
      }
   }

   class When_synchronizing_parameter_start_values : concern_for_ParameterStartValueSynchronizer
   {
      private IModelCoreSimulation _simulation;
      private IParameter _parameter;
      private ParameterStartValue _parameterStartValue;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         var parameterStartValues = new ParameterStartValuesBuildingBlock();
         _parameterStartValue = new ParameterStartValue()
         {
            Name = "P1",
            ContainerPath = new ObjectPath("Organism"),
            Dimension = A.Fake<IDimension>(),
            DisplayUnit = A.Fake<Unit>(),
            StartValue = 33
         };
         parameterStartValues.Add(_parameterStartValue);
         _simulation.Configuration = new SimulationConfiguration { Module = new Module() };
         _simulation.Configuration.Module.AddParameterStartValueBlock(parameterStartValues);
         _parameter =
            new Parameter().WithName("P1").WithValue(11).WithDimension(A.Fake<IDimension>()).WithDisplayUnit(A.Fake<Unit>());
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(new ObjectPath("Organism","P1"));
      }

      protected override void Because()
      {
         sut.SynchronizeValue(_simulation,_parameter);
      }


      [Observation]
      public void should_retrieve_path_for_parameter()
      {
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).MustHaveHappened();
      }

      [Observation]
      public void should_have_synchronized_the_parameter_start_value()
      {
         _parameterStartValue.Value.ShouldBeEqualTo(_parameter.Value);
         _parameterStartValue.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _parameterStartValue.DisplayUnit.ShouldBeEqualTo(_parameter.DisplayUnit);
      }
   }
}	