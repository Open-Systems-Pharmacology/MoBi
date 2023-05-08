using System.Linq;
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
   public abstract class concern_for_ParameterValueSynchronizer : ContextSpecification<ParameterValueSynchronizer>
   {
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         sut = new ParameterValueSynchronizer(_entityPathResolver);
      }
   }

   class synchronizing_parameter_values : concern_for_ParameterValueSynchronizer
   {
      private IModelCoreSimulation _simulation;
      private IParameter _parameter;
      private ParameterValue _parameterValue;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         var parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _parameterValue = new ParameterValue()
         {
            Name = "P1",
            ContainerPath = new ObjectPath("Organism"),
            Dimension = A.Fake<IDimension>(),
            DisplayUnit = A.Fake<Unit>(),
            Value = 33
         };
         parameterValuesBuildingBlock.Add(_parameterValue);
         _simulation.Configuration = new SimulationConfiguration();
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module()));
         var moduleConfiguration = _simulation.Configuration.ModuleConfigurations.First();
         moduleConfiguration.Module.Add(parameterValuesBuildingBlock);
         moduleConfiguration.SelectedParameterValues = parameterValuesBuildingBlock;
         _parameter =
            new Parameter().WithName("P1").WithValue(11).WithDimension(A.Fake<IDimension>()).WithDisplayUnit(A.Fake<Unit>());
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(new ObjectPath("Organism", "P1"));
      }

      protected override void Because()
      {
         sut.SynchronizeValue(_simulation, _parameter);
      }


      [Observation]
      public void should_retrieve_path_for_parameter()
      {
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).MustHaveHappened();
      }

      [Observation]
      public void should_have_synchronized_the_parameter_start_value()
      {
         _parameterValue.Value.ShouldBeEqualTo(_parameter.Value);
         _parameterValue.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _parameterValue.DisplayUnit.ShouldBeEqualTo(_parameter.DisplayUnit);
      }
   }
}