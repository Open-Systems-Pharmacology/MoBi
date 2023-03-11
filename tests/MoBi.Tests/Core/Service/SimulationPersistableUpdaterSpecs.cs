using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;
using SimulationPersistableUpdater = MoBi.Core.Services.SimulationPersistableUpdater;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationPersistableUpdater : ContextSpecification<ISimulationPersistableUpdater>
   {
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      private IContainerTask _containerTask;
      private IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      protected override void Context()
      {
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _containerTask = new ContainerTask(A.Fake<IObjectBaseFactory>(), new EntityPathResolverForSpecs(), new ObjectPathFactoryForSpecs());
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         sut = new SimulationPersistableUpdater(_entitiesInSimulationRetriever, _containerTask, _reactionDimensionRetriever);
      }
   }

   public class When_checking_if_a_quantity_is_selectable : concern_for_SimulationPersistableUpdater
   {
      [Observation]
      public void should_return_true_if_the_quantity_is_an_observer()
      {
         sut.QuantityIsSelectable(new Observer()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_quantity_is_a_parameter_that_is_persistable()
      {
         sut.QuantityIsSelectable(new Parameter {Persistable = true}).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_quantity_is_a_parameter_that_is_not_persistable()
      {
         sut.QuantityIsSelectable(new Parameter {Persistable = false}).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_quantity_is_a_molecule_amount_in_amount_based_network()
      {
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
         sut.QuantityIsSelectable(new MoleculeAmount()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_quantity_is_a_molecule_amount_in_concentration_based_network()
      {
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         sut.QuantityIsSelectable(new MoleculeAmount()).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_quantity_is_a_molecule_amount_in_concentration_based_network_but_the_force_flag_is_set_to_true()
      {
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         sut.QuantityIsSelectable(new MoleculeAmount(), true).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_quantity_is_a_molecule_concentration_parameter_in_concentration_based_network()
      {
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         var parameter = new Parameter().WithName(Constants.Parameters.CONCENTRATION)
            .WithParentContainer(new MoleculeAmount());

         sut.QuantityIsSelectable(parameter).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_quantity_is_a_molecule_concentration_parameter_in_amount_based_network()
      {
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
         var parameter = new Parameter().WithName(Constants.Parameters.CONCENTRATION)
            .WithParentContainer(new MoleculeAmount());

         sut.QuantityIsSelectable(parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.QuantityIsSelectable(A.Fake<IQuantity>()).ShouldBeFalse();
      }
   }

   public class When_resetting_all_persistable_object_defined_in_a_given_simulation : concern_for_SimulationPersistableUpdater
   {
      private IMoBiSimulation _simulation;
      private IMoleculeAmount _moleculeAmount;
      private Parameter _concentrationParameter;
      private Observer _observer;
      private Parameter _anotherParameter;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _simulation = A.Fake<IMoBiSimulation>();
         _moleculeAmount = new MoleculeAmount().WithName("M");
         _moleculeAmount.Persistable = true;
         _concentrationParameter = new Parameter().WithName(Constants.Parameters.CONCENTRATION).WithParentContainer(_moleculeAmount);
         _concentrationParameter.Persistable = true;
         _observer = new Observer();
         _observer.Persistable = true;
         _anotherParameter = new Parameter().WithName("Another Parameter");
         _anotherParameter.Persistable = true;
         var rootContainer = new Container();
         rootContainer.Add(_moleculeAmount);
         rootContainer.Add(_observer);
         _simulation.Model.Root = rootContainer;
      }

      protected override void Because()
      {
         sut.ResetPersistable(_simulation);
      }

      [Observation]
      public void should_set_all_persistable_quantities_to_not_persistable()
      {
         _moleculeAmount.Persistable.ShouldBeFalse();
         _concentrationParameter.Persistable.ShouldBeFalse();
         _observer.Persistable.ShouldBeFalse();
      }

      [Observation]
      public void should_not_reset_special_parameters_that_were_set_to_persistalbe()
      {
         _anotherParameter.Persistable.ShouldBeTrue();
      }
   }
}