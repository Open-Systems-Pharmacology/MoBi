using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Commands.Core;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddInitialConditionFromQuantityInSimulationCommand : ContextSpecification<AddInitialConditionFromQuantityInSimulationCommand>
   {
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      protected ObjectPath _objectPath;
      protected MoleculeAmount _moleculeAmount;
      protected IMoBiContext _context;
      private IEntityPathResolver _entityPathResolver;
      protected IInitialConditionsCreator _initialConditionsCreator;
      protected IMoBiSimulation _simulation;
      protected ISimulationEntitySourceUpdater _entitySourceUpdater;
      private MoBiProject _project;

      protected override void Context()
      {
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock().WithId("ICBB");
         _objectPath = new ObjectPath("A", "B", "P");
         _moleculeAmount = A.Fake<MoleculeAmount>().WithName("P").WithId("P");
         _simulation = new MoBiSimulation();
         _project = new MoBiProject();
         _simulation.Model = new Model
         {
            Root = new Container
            {
               _moleculeAmount
            }
         };
         
         sut = new AddInitialConditionFromQuantityInSimulationCommand(_moleculeAmount, _initialConditionsBuildingBlock, _simulation);
         _project.AddSimulation(_simulation);

         _context = A.Fake<IMoBiContext>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _entitySourceUpdater = A.Fake<ISimulationEntitySourceUpdater>();
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(_entityPathResolver);
         A.CallTo(() => _context.Resolve<ISimulationEntitySourceUpdater>()).Returns(_entitySourceUpdater);
         A.CallTo(() => _context.Resolve<IInitialConditionsCreator>()).Returns(_initialConditionsCreator);
         A.CallTo(() => _context.Get<MoleculeAmount>(_moleculeAmount.Id)).Returns(_moleculeAmount);
         A.CallTo(() => _context.Get<PathAndValueEntityBuildingBlock<InitialCondition>>(_initialConditionsBuildingBlock.Id)).Returns(_initialConditionsBuildingBlock);
         A.CallTo(() => _context.CurrentProject).Returns(_project);

         A.CallTo(() => _entityPathResolver.ObjectPathFor(_moleculeAmount, false)).Returns(_objectPath);
      }
   }

   public class adding_a_initial_condition_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation : concern_for_AddInitialConditionFromQuantityInSimulationCommand
   {
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _initialCondition = new InitialCondition { Path = _objectPath };
         A.CallTo(() => _initialConditionsCreator.CreateInitialCondition(_objectPath, _moleculeAmount)).Returns(_initialCondition);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_a_new_initial_condition_in_the_given_building_block()
      {
         _initialConditionsBuildingBlock[_objectPath].ShouldBeEqualTo(_initialCondition);
      }

      [Observation]
      public void the_source_must_be_updated_in_the_simulation()
      {
         A.CallTo(() => _entitySourceUpdater.UpdateSourcesForNewPathAndValueEntity(_initialConditionsBuildingBlock, _objectPath, _simulation)).MustHaveHappened();
      }
   }

   public class adding_a_initial_condition_based_on_a_simulation_parameter_to_a_building_block_defined_in_a_simulation_that_already_exists : concern_for_AddInitialConditionFromQuantityInSimulationCommand
   {
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _initialCondition = new InitialCondition { Path = _objectPath };
         _initialConditionsBuildingBlock.Add(_initialCondition);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_add_a_new_initial_condition()
      {
         A.CallTo(() => _initialConditionsCreator.CreateInitialCondition(_objectPath, _moleculeAmount)).MustNotHaveHappened();
      }
   }

   public class reverting_the_add_initial_condition_from_quantity_in_simulation_command : concern_for_AddInitialConditionFromQuantityInSimulationCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _initialConditionsCreator.CreateInitialCondition(_objectPath, _moleculeAmount)).Returns(new InitialCondition { Path = _objectPath });
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_removed_the_added_initial_condition()
      {
         _initialConditionsBuildingBlock[_objectPath].ShouldBeNull();
      }
   }

   public class reverting_and_reverting_again_the_add_initial_condition_from_quantity_in_simulation_command : concern_for_AddInitialConditionFromQuantityInSimulationCommand
   {
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _initialCondition = new InitialCondition { Path = _objectPath };
         A.CallTo(() => _initialConditionsCreator.CreateInitialCondition(_objectPath, _moleculeAmount)).Returns(_initialCondition);
      }

      protected override void Because()
      {
         var inverse = sut.ExecuteAndInvokeInverse(_context) as IReversibleCommand<IMoBiContext>;
         inverse.InvokeInverse(_context);
      }

      [Observation]
      public void should_add_a_new_initial_condition_in_the_given_building_block()
      {
         _initialConditionsBuildingBlock[_objectPath].ShouldBeEqualTo(_initialCondition);
      }
   }
}