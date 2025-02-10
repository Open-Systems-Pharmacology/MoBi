using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForMoleculeBuilder : ContextSpecification<InteractionTasksForMoleculeBuilder>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      private IEditTaskFor<MoleculeBuilder> _editTasks;
      private ICoreCalculationMethodRepository _calculation;
      protected IReactionDimensionRetriever _dimensionRetriever;
      protected IParameterFactory _parameterFactory;
      protected IInteractionTask _interactionTask;
      protected IMoBiContext _context;
      protected IMoBiFormulaTask _formulaTask;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTasks = A.Fake<IEditTaskFor<MoleculeBuilder>>();
         _calculation = A.Fake<ICoreCalculationMethodRepository>();
         _dimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _parameterFactory = A.Fake<IParameterFactory>();
         _formulaTask= A.Fake<IMoBiFormulaTask>();
         sut = new InteractionTasksForMoleculeBuilder(_interactionTaskContext, _editTasks, _dimensionRetriever, _parameterFactory, _calculation,_formulaTask);

         _interactionTask = A.Fake<IInteractionTask>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
      }
   }

   public class When_creating_a_new_default_molecule_builder : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuilder _builder;
      private ConstantFormula _constantFormula;

      protected override void Context()
      {
         base.Context();
         _constantFormula = new ConstantFormula(1.0);
         A.CallTo(() => _formulaTask.CreateNewFormula<ConstantFormula>(_dimensionRetriever.MoleculeDimension)).Returns(_constantFormula);
      }

      protected override void Because()
      {
         _builder = sut.CreateDefault("moleculeName");
      }

      [Observation]
      public void the_default_dimension_should_be_set()
      {
         _builder.Dimension.ShouldBeEqualTo(_dimensionRetriever.MoleculeDimension);
      }

      [Observation]
      public void the_default_start_formula_should_be_set()
      {
         _builder.DefaultStartFormula.ShouldBeEqualTo(_constantFormula);
      }

      [Observation]
      public void the_builder_name_should_be_set()
      {
         _builder.Name.ShouldBeEqualTo("moleculeName");
      }
   }


   public class When_creating_a_new_molecule_builder : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuildingBlock _moleculeBuilderBuildingBlock;
      private MoleculeBuilder _moleculeBuilder;
      private IDimension _amountDimension;
      private Unit _displayUnit;
      private IModalPresenter _modalPresenter;
      private IEditPresenter<MoleculeBuilder> _editMoleculeBuilderPresenter;
      private ConstantFormula _defaultStartFormula;

      protected override void Context()
      {
         base.Context();
         _modalPresenter = A.Fake<IModalPresenter>();
         _amountDimension = A.Fake<IDimension>();
         _displayUnit = A.Fake<Unit>();
         _editMoleculeBuilderPresenter = A.Fake<IEditPresenter<MoleculeBuilder>>();
         _moleculeBuilderBuildingBlock = new MoleculeBuildingBlock();

         A.CallTo(() => _context.Create<MoleculeBuilder>()).Returns(new MoleculeBuilder());
         A.CallTo(() => _dimensionRetriever.MoleculeDimension).Returns(_amountDimension);
         A.CallTo(_interactionTaskContext.ApplicationController).WithReturnType<IModalPresenter>().Returns(_modalPresenter);
         A.CallTo(_interactionTaskContext).WithReturnType<Unit>().Returns(_displayUnit);
         A.CallTo(() => _modalPresenter.SubPresenter).Returns(_editMoleculeBuilderPresenter);
         A.CallTo(() => _parameterFactory.CreateConcentrationParameter(_moleculeBuilderBuildingBlock.FormulaCache)).Returns(new Parameter {Name = Constants.Parameters.CONCENTRATION });

         _defaultStartFormula = new ConstantFormula();
         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(_defaultStartFormula);
      }

      protected override void Because()
      {
         sut.AddNew(_moleculeBuilderBuildingBlock, _moleculeBuilderBuildingBlock);
         _moleculeBuilder = _moleculeBuilderBuildingBlock.First();
      }

      [Observation]
      public void should_set_the_dimension_to_amount()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(_amountDimension);
      }

      [Observation]
      public void should_update_the_display_unit_to_use_the_default_unit_for_the_molecule_builder_dimension()
      {
         _moleculeBuilder.DisplayUnit.ShouldBeEqualTo(_displayUnit);
      }

      [Observation]
      public void should_add_the_readonly_concentration_parameter_to_the_molecule_builder()
      {
         var concentrationParameter = _moleculeBuilder.Parameters.FindByName(Constants.Parameters.CONCENTRATION);
         concentrationParameter.ShouldNotBeNull();
      }

      [Observation]
      public void should_set_the_default_start_formula()
      {
         _moleculeBuilder.DefaultStartFormula.ShouldBeEqualTo(_defaultStartFormula);
      }
   }

   public class When_adding_a_pksim_molecule_to_a_molecule_building_block_ : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private ICreatePKSimMoleculePresenter _createMoleculePresenter;
      private MoleculeBuilder _molecule;
      private ICommand _command;
      private ConstantFormula _defaultStartFormula;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _createMoleculePresenter = A.Fake<ICreatePKSimMoleculePresenter>();
         _molecule = new MoleculeBuilder();
         _molecule.DefaultStartFormula = A.Fake<IFormula>();
         A.CallTo(() => _createMoleculePresenter.CreateMolecule(_moleculeBuildingBlock)).Returns(_molecule);
         A.CallTo(() => _interactionTaskContext.ApplicationController.Start<ICreatePKSimMoleculePresenter>()).Returns(_createMoleculePresenter);
         _defaultStartFormula = new ConstantFormula();
         A.CallTo(_formulaTask).WithReturnType<ConstantFormula>().Returns(_defaultStartFormula);

         A.CallTo(() => _context.AddToHistory(A<ICommand>._))
            .Invokes(x => _command = x.GetArgument<ICommand>(0));
      }

      protected override void Because()
      {
         sut.AddPKSimMoleculeTo(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_add_the_resulting_command_to_the_history()
      {
         _command.ShouldNotBeNull();
      }

      [Observation]
      public void should_reset_the_default_start_value()
      {
         _molecule.DefaultStartFormula.ShouldBeEqualTo(_defaultStartFormula);
      }
   }

   public class When_adding_a_molecule_to_a_molecule_building_block : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoleculeBuilder _moleculeBuilderToAdd;
      private IMoBiCommand _command;
      private AddedEvent<MoleculeBuilder> _addEvent;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>();
         _moleculeBuilderToAdd = new MoleculeBuilder();
         A.CallTo(() => _interactionTask.AdjustFormula(_moleculeBuilderToAdd, _moleculeBuildingBlock, A<IMoBiMacroCommand>._)).Returns(true);
         A.CallTo(() => _interactionTask.CorrectName(_moleculeBuilderToAdd, A<IEnumerable<string>>._)).Returns(true);
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<MoleculeBuilder>>._))
            .Invokes(x => _addEvent = x.GetArgument<AddedEvent<MoleculeBuilder>>(0));
      }

      protected override void Because()
      {
         _command = sut.AddTo(_moleculeBuilderToAdd, _moleculeBuildingBlock, _moleculeBuildingBlock);
      }

      [Observation]
      public void should_add_the_molecule_to_the_building_block_using_a_silent_add_command()
      {
         var macroCommand = _command.DowncastTo<IMoBiMacroCommand>();
         var addCommand = macroCommand.All().First() as ISilentCommand;
         addCommand.Silent.ShouldBeTrue();
      }

      [Observation]
      public void should_notify_the_added_event_command()
      {
         _addEvent.AddedObject.ShouldBeEqualTo(_moleculeBuilderToAdd);
      }
   }

   public class When_adding_a_molecule_to_a_molecule_building_block_already_containing_a_molecule_with_the_same_name_and_the_user_cancels_the_action : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoleculeBuilder _moleculeBuilderToAdd;
      private IMoBiCommand _command;
      private AddedEvent<MoleculeBuilder> _addEvent;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>();
         _moleculeBuilderToAdd = new MoleculeBuilder();
         A.CallTo(() => _interactionTask.CorrectName(_moleculeBuilderToAdd, A<IEnumerable<string>>._)).Returns(false);
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<MoleculeBuilder>>._))
            .Invokes(x => _addEvent = x.GetArgument<AddedEvent<MoleculeBuilder>>(0));
      }

      protected override void Because()
      {
         _command = sut.AddTo(_moleculeBuilderToAdd, _moleculeBuildingBlock, _moleculeBuildingBlock);
      }

      [Observation]
      public void should_not_add_the_molecule_to_the_molecule_building_block()
      {
         _command.IsEmpty().ShouldBeTrue();
      }

      [Observation]
      public void should_not_notify_the_added_event_command()
      {
         _addEvent.ShouldBeNull();
      }
   }

   public class When_adding_a_molecule_to_a_molecule_building_block_already_containing_a_similar_formula_and_the_adjustement_is_canceled : concern_for_InteractionTasksForMoleculeBuilder
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoleculeBuilder _moleculeBuilderToAdd;
      private IMoBiCommand _command;
      private AddedEvent<MoleculeBuilder> _addEvent;
      private IMoBiCommand _cancelCommand;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>();
         _moleculeBuilderToAdd = new MoleculeBuilder();
         A.CallTo(() => _interactionTask.CorrectName(_moleculeBuilderToAdd, A<IEnumerable<string>>._)).Returns(true);
         A.CallTo(() => _interactionTask.AdjustFormula(_moleculeBuilderToAdd, _moleculeBuildingBlock, A<IMoBiMacroCommand>._)).Returns(false);
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<MoleculeBuilder>>._))
            .Invokes(x => _addEvent = x.GetArgument<AddedEvent<MoleculeBuilder>>(0));

         _cancelCommand= A.Fake<IMoBiCommand>();
         A.CallTo(() => _interactionTaskContext.CancelCommand(A<IMoBiCommand>._)).Returns(_cancelCommand);
      }

      protected override void Because()
      {
         _command = sut.AddTo(_moleculeBuilderToAdd, _moleculeBuildingBlock, _moleculeBuildingBlock);
      }

      [Observation]
      public void should_cancel_the_addition_of_the_molecule_to_the_molecule_building_block()
      {
         _command.ShouldBeEqualTo(_cancelCommand);
      }

      [Observation]
      public void should_not_notify_the_added_event_command()
      {
         _addEvent.ShouldBeNull();
      }
   }
}