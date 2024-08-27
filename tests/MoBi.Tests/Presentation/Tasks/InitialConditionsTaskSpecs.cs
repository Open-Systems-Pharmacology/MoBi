using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InitialConditionsTask : ContextSpecification<InitialConditionsTask<InitialConditionsBuildingBlock>>
   {
      protected IInitialConditionsCreator _initialConditionsCreator;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private IEditTasksForBuildingBlock<InitialConditionsBuildingBlock> _editTask;
      protected IInteractionTaskContext _context;
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IInteractionTasksForMoleculeBuilder _moleculeBuilderTask;
      protected IParameterFactory _parameterFactory;
      protected INameCorrector _nameCorrector;
      protected IFormulaTask _formulaTask;
      private IObjectTypeResolver _objectTypeResolver;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForBuildingBlock<InitialConditionsBuildingBlock>>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _nameCorrector = A.Fake<INameCorrector>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock
         {
            Module = new Module()
         };
         _parameterFactory = A.Fake<IParameterFactory>();
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _moleculeBuilderTask = A.Fake<IInteractionTasksForMoleculeBuilder>();
         _formulaTask = A.Fake<IFormulaTask>();
         sut = new InitialConditionsTask<InitialConditionsBuildingBlock>(_context, _editTask, A.Fake<IInitialConditionsBuildingBlockExtendManager>(), _cloneManagerForBuildingBlock, A.Fake<IMoBiFormulaTask>(), A.Fake<IMoBiSpatialStructureFactory>(),
            new ImportedQuantityToInitialConditionMapper(_initialConditionsCreator),
            new InitialConditionPathTask(_formulaTask, _context.Context), _reactionDimensionRetriever, _initialConditionsCreator, _parameterFactory, _objectTypeResolver, _nameCorrector, A.Fake<IExportDataTableToExcelTask>());
      }
   }

   /// <summary>
   ///    Making sure that the execute is actually called as part of the specs. Command is actually tested elsewhere
   /// </summary>
   public class When_updating_scale_divisor : concern_for_InitialConditionsTask
   {
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _initialCondition = new InitialCondition { ScaleDivisor = 1 };
      }

      protected override void Because()
      {
         sut.UpdateInitialConditionScaleDivisor(_initialConditionsBuildingBlock, _initialCondition, 65, _initialCondition.ScaleDivisor);
      }

      [Observation]
      public void must_update_scale_divisor()
      {
         _initialCondition.ScaleDivisor.ShouldBeEqualTo(65);
      }
   }

   public class When_retrieving_default_dimension : concern_for_InitialConditionsTask
   {
      private IDimension _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.MoleculeDimension).Returns(DomainHelperForSpecs.AmountDimension);
      }

      protected override void Because()
      {
         _result = sut.GetDefaultDimension();
      }

      [Observation]
      public void result_should_be_amount_dimension()
      {
         _result.ShouldBeEqualTo(DomainHelperForSpecs.AmountDimension);
      }
   }

   public class When_removing_building_block_referenced_by_simulation : concern_for_InitialConditionsTask
   {
      private MoBiProject _project;
      private IMoBiSimulation _simulation;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _simulation = A.Fake<IMoBiSimulation>();
         _project.AddSimulation(_simulation);
         _module = new Module { _initialConditionsBuildingBlock };
         A.CallTo(() => _simulation.Uses(_initialConditionsBuildingBlock)).Returns(true);
         A.CallTo(() => _context.Context.CurrentProject).Returns(_project);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Remove(_initialConditionsBuildingBlock, _module, null, false)).ShouldThrowAn<MoBiException>();
      }
   }

   public class When_importing_multiple_molecule_start_values : concern_for_InitialConditionsTask
   {
      private IList<ImportedQuantityDTO> _initialConditions;
      private InitialCondition _firstStartValueRef;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         _firstStartValueRef = new InitialCondition { Path = new ObjectPath("this", "path", "C1"), Value = -1.0, IsPresent = true };
         IDimension d = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);

         _initialConditions = new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO
            {
               Name = "C1",
               ContainerPath = new ObjectPath("this", "path"),
               QuantityInBaseUnit = 1.0,
               IsPresent = false,
               Dimension = d,
               IsQuantitySpecified = true
            },
            new ImportedQuantityDTO { Name = "C1", ContainerPath = new ObjectPath("that", "path"), QuantityInBaseUnit = 2.0, Dimension = d },
            new ImportedQuantityDTO { Name = "C1", ContainerPath = new ObjectPath("the", "path"), QuantityInBaseUnit = 3.0, Dimension = d }
         };

         _initialConditionsBuildingBlock.Add(_firstStartValueRef);

         for (var i = 1; i < 3; i++)
         {
            var dto = _initialConditions[i];
            A.CallTo(() => _initialConditionsCreator.CreateInitialCondition(dto.ContainerPath, dto.Name, A<IDimension>._, A<Unit>._, A<ValueOrigin>._)).Returns(
               new InitialCondition
               {
                  ContainerPath = dto.ContainerPath,
                  Name = dto.Name,
                  IsPresent = dto.IsPresent,
                  Dimension = dto.Dimension,
                  DisplayUnit = dto.DisplayUnit,
                  Value = dto.QuantityInBaseUnit
               });
         }
      }

      protected override void Because()
      {
         _result = sut.ImportPathAndValueEntitiesToBuildingBlock(_initialConditionsBuildingBlock, _initialConditions);
      }

      [Observation]
      public void resulting_command_should_have_correct_attributes()
      {
         _result.CommandType.ShouldBeEqualTo(AppConstants.Commands.ImportCommand);
         _result.Description.ShouldBeEqualTo(AppConstants.Commands.ImportInitialConditions);
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.InitialCondition);
      }

      [Observation]
      public void building_block_should_contain_3_start_values()
      {
         _initialConditionsBuildingBlock.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void value_of_start_values_should_match_commands_issued()
      {
         _initialConditionsBuildingBlock[_initialConditions[0].Path].Value.ShouldBeEqualTo(1.0);
         _initialConditionsBuildingBlock[_initialConditions[1].Path].Value.ShouldBeEqualTo(2.0);
         _initialConditionsBuildingBlock[_initialConditions[2].Path].Value.ShouldBeEqualTo(3.0);
      }

      [Observation]
      public void reference_in_building_block_should_not_have_changed_for_updated_start_value()
      {
         _initialConditionsBuildingBlock[_initialConditions[0].Path].ShouldBeEqualTo(_firstStartValueRef);
      }
   }

   public class When_setting_the_is_present_flag_for_a_set_of_molecule_start_values : concern_for_InitialConditionsTask
   {
      private List<InitialCondition> _startValues;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _startValues = new List<InitialCondition>
         {
            new InitialCondition { Name = "M1", IsPresent = true },
            new InitialCondition { Name = "M2", IsPresent = false },
            new InitialCondition { Name = "M3", IsPresent = true },
            new InitialCondition { Name = "M4", IsPresent = false }
         };
         _startValues.Each(_initialConditionsBuildingBlock.Add);
      }

      protected override void Because()
      {
         _command = sut.SetIsPresent(_initialConditionsBuildingBlock, _startValues, isPresent: true);
      }

      [Observation]
      public void should_return_a_macro_command_containing_one_command_for_each_molecule_start_value_for_which_the_flag_value_has_changed()
      {
         _command.ShouldBeAnInstanceOf<IMoBiMacroCommand>();
         var macroCommand = _command.DowncastTo<IMoBiMacroCommand>();
         macroCommand.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_setting_the_negative_start_values_flag_for_a_set_of_molecule_start_values : concern_for_InitialConditionsTask
   {
      private List<InitialCondition> _startValues;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _startValues = new List<InitialCondition>
         {
            new InitialCondition { Name = "M1", NegativeValuesAllowed = true },
            new InitialCondition { Name = "M2", NegativeValuesAllowed = false },
            new InitialCondition { Name = "M3", NegativeValuesAllowed = true },
            new InitialCondition { Name = "M4", NegativeValuesAllowed = false }
         };
         _startValues.Each(_initialConditionsBuildingBlock.Add);
      }

      protected override void Because()
      {
         _command = sut.SetNegativeValuesAllowed(_initialConditionsBuildingBlock, _startValues, negativeValuesAllowed: true);
      }

      [Observation]
      public void should_return_a_macro_command_containing_one_command_for_each_molecule_start_value_for_which_the_flag_value_has_changed()
      {
         _command.ShouldBeAnInstanceOf<IMoBiMacroCommand>();
         var macroCommand = _command.DowncastTo<IMoBiMacroCommand>();
         macroCommand.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_removing_an_element_of_start_value : concern_for_InitialConditionsTask
   {
      private InitialCondition _startValue;

      protected override void Because()
      {
         _startValue = new InitialCondition { ContainerPath = new ObjectPath("A", "B") };
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_initialConditionsBuildingBlock, _startValue, 0, "");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("B");
      }
   }

   public class When_appending_an_element_of_start_value : concern_for_InitialConditionsTask
   {
      private InitialCondition _startValue;

      protected override void Because()
      {
         _startValue = new InitialCondition { ContainerPath = new ObjectPath("A", "B") };
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_initialConditionsBuildingBlock, _startValue, 2, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_replacing_an_element_of_start_value : concern_for_InitialConditionsTask
   {
      private InitialCondition _startValue;

      protected override void Because()
      {
         _startValue = new InitialCondition { ContainerPath = new ObjectPath("A", "B") };
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_initialConditionsBuildingBlock, _startValue, 0, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("C|B");
      }
   }

   public class When_replacing_an_element_outside_start_value_path_range : concern_for_InitialConditionsTask
   {
      private InitialCondition _startValue;

      protected override void Because()
      {
         _startValue = new InitialCondition { ContainerPath = new ObjectPath("A", "B") };
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_initialConditionsBuildingBlock, _startValue, 5, "C");
      }

      [Observation]
      public void should_not_affect_start_value_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("A|B");
      }
   }

   public class When_updating_a_molecule_start_value_with_new_display_unit : concern_for_InitialConditionsTask
   {
      private InitialCondition _startValue;
      private IDimension _dim;
      private const double TARGET_BASE_VALUE = 1000000;
      private const double TARGET_DISPLAY_VALUE = 1000;

      protected override void Context()
      {
         base.Context();
         _dim = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);

         _startValue = new InitialCondition { Dimension = _dim, Value = TARGET_DISPLAY_VALUE, DisplayUnit = _dim.Unit("g") };
      }

      protected override void Because()
      {
         var targetUnit = _dim.Unit("kg");

         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         sut.SetDisplayValueWithUnit(_startValue, _startValue.ConvertToDisplayUnit(_startValue.Value.Value), targetUnit, A.Fake<InitialConditionsBuildingBlock>());
      }

      [Observation]
      public void display_amount_should_not_change()
      {
         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         _startValue.ConvertToDisplayUnit(_startValue.Value.Value).ShouldBeEqualTo(TARGET_DISPLAY_VALUE);
      }

      [Observation]
      public void base_amount_must_change_to_new_amount()
      {
         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         _startValue.Value.Value.ShouldBeEqualTo(TARGET_BASE_VALUE);
      }
   }

   public abstract class When_cloning_a_molecule_start_values_building_block : concern_for_InitialConditionsTask
   {
      protected InitialConditionsBuildingBlock _buildingBlockToClone;
      protected Module _module;
      protected ICommand _result;

      protected override void Context()
      {
         base.Context();
         _buildingBlockToClone = new InitialConditionsBuildingBlock();
         _module = new Module { _buildingBlockToClone };
         A.CallTo(() => _context.NamingTask.NewName(A<string>._, A<string>._, A<string>._, A<IEnumerable<string>>._, A<IEnumerable<string>>._, A<string>._)).Returns(CloneName());
         A.CallTo(() => _context.InteractionTask.CorrectName(A<InitialConditionsBuildingBlock>._, A<IEnumerable<string>>._)).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.CloneAndAddToParent(_buildingBlockToClone, _module);
      }

      protected abstract string CloneName();
   }

   public class When_a_clone_of_an_existing_building_block_is_canceled : When_cloning_a_molecule_start_values_building_block
   {
      protected override string CloneName()
      {
         return string.Empty;
      }

      [Observation]
      public void the_resulting_command_should_be_empty()
      {
         _result.ShouldBeAnInstanceOf<MoBiEmptyCommand>();
      }

      [Observation]
      public void the_clone_should_not_be_made()
      {
         _module.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_a_clone_of_an_existing_building_block_is_made : When_cloning_a_molecule_start_values_building_block
   {
      [Observation]
      public void the_cloned_building_block_must_belong_to_the_module()
      {
         _module.InitialConditionsCollection.Count.ShouldBeEqualTo(2);
      }

      protected override string CloneName()
      {
         return "name of clone";
      }
   }

   public class concern_for_extending_initial_conditions : concern_for_InitialConditionsTask
   {
      protected override void Context()
      {
         base.Context();
         ConfigureBuildingBlockRepository();
      }

      protected virtual void ConfigureBuildingBlockRepository()
      {
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { new MoBiSpatialStructure(), new MoBiSpatialStructure() });
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { new MoleculeBuildingBlock(), new MoleculeBuildingBlock() });
      }
   }

   public class When_extending_initial_conditions_and_not_exactly_one_of_each_type_is_available : concern_for_extending_initial_conditions
   {
      private ISelectSpatialStructureAndMoleculesPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISelectSpatialStructureAndMoleculesPresenter>();
         A.CallTo(() => _context.Context.Resolve<ISelectSpatialStructureAndMoleculesPresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.ExtendPathAndValueEntityBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_spatial_structure_and_molecule_selection_presenter_is_used_to_select_the_building_blocks()
      {
         A.CallTo(() => _presenter.SelectBuildingBlocksForExtend(null, null)).MustHaveHappened();
      }
   }

   public class When_extending_initial_conditions_and_exactly_one_spatial_structure_and_one_molecule_is_available : concern_for_extending_initial_conditions
   {
      private MoBiSpatialStructure _moBiSpatialStructure;
      private ISelectSpatialStructureAndMoleculesPresenter _selectionPresenter;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void ConfigureBuildingBlockRepository()
      {
         _selectionPresenter = A.Fake<ISelectSpatialStructureAndMoleculesPresenter>();
         _moBiSpatialStructure = new MoBiSpatialStructure();
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { _moBiSpatialStructure });
         _moleculeBuildingBlock = new MoleculeBuildingBlock
         {
            new MoleculeBuilder { Name = "M1" }
         };
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { _moleculeBuildingBlock });
         A.CallTo(() => _context.Context.Resolve<ISelectSpatialStructureAndMoleculesPresenter>()).Returns(_selectionPresenter);

         A.CallTo(() => _selectionPresenter.SelectedMolecules).Returns(_moleculeBuildingBlock.ToList());
         A.CallTo(() => _selectionPresenter.SelectedSpatialStructure).Returns(_moBiSpatialStructure);
      }

      protected override void Because()
      {
         sut.ExtendPathAndValueEntityBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_spatial_structure_and_molecule_selection_presenter_is_not_used_to_select_the_building_blocks()
      {
         A.CallTo(() => _context.Context.Resolve<ISelectSpatialStructureAndMoleculesPresenter>()).MustNotHaveHappened();
      }

      [Observation]
      public void the_only_valid_values_should_be_used_to_create_new_initial_conditions()
      {
         A.CallTo(() => _initialConditionsCreator.CreateFrom(_moBiSpatialStructure, A<IReadOnlyList<MoleculeBuilder>>._)).MustHaveHappened();
      }
   }

   public class When_extending_initial_conditions_and_the_building_blocks_are_not_selected : concern_for_extending_initial_conditions
   {
      private ISelectSpatialStructureAndMoleculesPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISelectSpatialStructureAndMoleculesPresenter>();
         A.CallTo(() => _context.Context.Resolve<ISelectSpatialStructureAndMoleculesPresenter>()).Returns(_presenter);
         A.CallTo(() => _presenter.SelectedMolecules).Returns(new List<MoleculeBuilder>());
      }

      protected override void Because()
      {
         sut.ExtendPathAndValueEntityBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_initial_conditions_creator_is_not_used_to_create_initial_conditions()
      {
         A.CallTo(() => _initialConditionsCreator.CreateFrom(A<MoBiSpatialStructure>._, A<IReadOnlyList<MoleculeBuilder>>._)).MustNotHaveHappened();
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_NaN_and_molecule_start_value_NaN : concern_for_InitialConditionsTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         var molecule = new MoleculeBuilder { Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ConstantFormula(double.NaN) };
         moleculeBuildingBlock.Add(molecule);
         var nanStartValue = new InitialCondition { Formula = new ConstantFormula(double.NaN), Name = molecule.Name, Value = double.NaN, Dimension = Constants.Dimension.NO_DIMENSION };
         _initialConditionsBuildingBlock.Add(nanStartValue);
      }

      protected override void Because()
      {
         _command = sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void should_not_create_an_entry_in_the_history()
      {
         _command.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public class When_updating_a_molecule_start_value_from_original_building_block_When_the_start_value_has_changed : concern_for_InitialConditionsTask
   {
      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         var builder = new MoleculeBuilder { Name = "molecule", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ExplicitFormula("50") };
         moleculeBuildingBlock.Add(builder);
         var startValue = new InitialCondition { Name = builder.Name, Value = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = null };
         _initialConditionsBuildingBlock.Add(startValue);
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(builder.DefaultStartFormula, _initialConditionsBuildingBlock.FormulaCache)).Returns(new ExplicitFormula("M/V"));
      }

      protected override void Because()
      {
         sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void formula_must_be_set()
      {
         _initialConditionsBuildingBlock.Each(startValue => startValue.Formula.IsExplicit().ShouldBeTrue());
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_NaN_and_molecule_start_value_NULL : concern_for_InitialConditionsTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         var molecule = new MoleculeBuilder { Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION };
         moleculeBuildingBlock.Add(molecule);
         var nanStartValue = new InitialCondition { Name = molecule.Name, Value = null, Dimension = Constants.Dimension.NO_DIMENSION };
         _initialConditionsBuildingBlock.Add(nanStartValue);
      }

      protected override void Because()
      {
         _command = sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void should_not_create_an_entry_in_the_history()
      {
         _command.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_null_and_molecule_start_value_NULL : concern_for_InitialConditionsTask
   {
      private IMoBiCommand _command;
      private InitialCondition _nullStartValue;

      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         var molecule = new MoleculeBuilder { Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION };
         moleculeBuildingBlock.Add(molecule);
         _nullStartValue = new InitialCondition { Name = molecule.Name, Value = 1, Dimension = Constants.Dimension.NO_DIMENSION };
         _initialConditionsBuildingBlock.Add(_nullStartValue);
         A.CallTo(_context.Context).WithReturnType<MoleculeBuildingBlock>().Returns(moleculeBuildingBlock);
      }

      protected override void Because()
      {
         _command = sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void should_create_an_entry_in_the_history()
      {
         _command.IsEmptyMacro().ShouldBeFalse();
      }

      [Observation]
      public void should_set_the_value_to_null()
      {
         _nullStartValue.Value.ShouldBeNull();
      }
   }

   public abstract class concern_for_initialConditions_With_formulas_for_Renaming : concern_for_InitialConditionsTask
   {
      protected BlackBoxFormula _replacedFormula;
      protected ExplicitFormula _refreshedFormula;
      protected AddedEvent<IFormula> _addEvent;
      protected MoleculeBuilder _builder;

      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         _builder = new MoleculeBuilder { Name = "molecule", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ExplicitFormula("50").WithId("50") };
         moleculeBuildingBlock.Add(_builder);
         _replacedFormula = new BlackBoxFormula().WithId("blackbox");
         var startValue = new InitialCondition { Name = _builder.Name, Value = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = _replacedFormula };
         _initialConditionsBuildingBlock.Add(startValue);
         _initialConditionsBuildingBlock.FormulaCache.Add(startValue.Formula);
         _refreshedFormula = new ExplicitFormula("M/V").WithId("m/v").WithName("FormulaName");
      }
   }

   public class When_updating_a_molecule_start_value_from_original_building_block_and_all_formulas_will_be_replaced : concern_for_initialConditions_With_formulas_for_Renaming
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_builder.DefaultStartFormula, _initialConditionsBuildingBlock.FormulaCache))
            .Invokes(x => _initialConditionsBuildingBlock.FormulaCache.Add(_refreshedFormula)).Returns(_refreshedFormula);
         A.CallTo(() => _context.Context.PublishEvent(A<AddedEvent<IFormula>>._))
            .Invokes(x => _addEvent = x.GetArgument<AddedEvent<IFormula>>(0));
      }

      protected override void Because()
      {
         sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void the_formula_that_was_refreshed_is_in_the_cache()
      {
         _initialConditionsBuildingBlock.FormulaCache.Contains(_refreshedFormula).ShouldBeTrue();
      }

      [Observation]
      public void the_added_expression_should_be_renamed()
      {
         _addEvent.AddedObject.ShouldBeEqualTo(_refreshedFormula);
      }

      [Observation]
      public void the_formula_that_was_replaced_is_removed_from_cache()
      {
         _initialConditionsBuildingBlock.FormulaCache.Contains(_replacedFormula).ShouldBeFalse();
      }
   }

   public class When_updating_a_molecule_start_value_from_original_building_block_and_all_formulas_will_be_replaced_and_added_event_should_not_be_called : concern_for_initialConditions_With_formulas_for_Renaming
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_builder.DefaultStartFormula, _initialConditionsBuildingBlock.FormulaCache)).Returns(_refreshedFormula);

         A.CallTo(() => _context.Context.PublishEvent(A<AddedEvent<IFormula>>._))
            .Invokes(x => _addEvent = x.GetArgument<AddedEvent<IFormula>>(0));
      }

      protected override void Because()
      {
         sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsBuildingBlock.ToList());
      }

      [Observation]
      public void the_formula_that_was_replaced_is_removed_from_cache()
      {
         _initialConditionsBuildingBlock.FormulaCache.Contains(_replacedFormula).ShouldBeFalse();
      }

      [Observation]
      public void should_publish_event_not_be_called()
      {
         A.CallTo(() => _context.Context.PublishEvent(A<AddedEvent<IFormula>>._)).MustNotHaveHappened();
      }
   }

   public class When_updating_a_molecule_start_value_from_original_building_block_and_not_all_formulas_will_be_replaced : concern_for_InitialConditionsTask
   {
      private IFormula _replacedFormula;
      private IFormula _refreshedFormula;
      private List<InitialCondition> _initialConditionsToRefresh;

      protected override void Context()
      {
         base.Context();
         var spatialStructure = A.Fake<MoBiSpatialStructure>();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new[] { moleculeBuildingBlock });
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         var builder = new MoleculeBuilder { Name = "molecule", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ExplicitFormula("50").WithId("50") };
         moleculeBuildingBlock.Add(builder);
         _replacedFormula = new BlackBoxFormula().WithId("blackbox").WithName("M/V");
         var startValue = new InitialCondition { Name = builder.Name, Value = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = _replacedFormula };
         var thirdStartValue = new InitialCondition { Name = "yet another name", Value = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = _replacedFormula };

         _initialConditionsBuildingBlock.Add(startValue);
         _initialConditionsBuildingBlock.Add(thirdStartValue);
         var secondStartValue = new InitialCondition { Name = "anothername", Value = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = _replacedFormula };
         _initialConditionsBuildingBlock.Add(secondStartValue);

         _initialConditionsBuildingBlock.FormulaCache.Add(startValue.Formula);
         _refreshedFormula = new ExplicitFormula("M/V").WithId("m/v").WithName("M/V");
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(builder.DefaultStartFormula, _initialConditionsBuildingBlock.FormulaCache)).Invokes(x => _initialConditionsBuildingBlock.FormulaCache.Add(_refreshedFormula)).Returns(_refreshedFormula);

         A.CallTo(() => _formulaTask.FormulasAreTheSame(startValue.Formula, secondStartValue.Formula)).Returns(true);
         _initialConditionsToRefresh = new List<InitialCondition> { startValue, thirdStartValue };
      }

      protected override void Because()
      {
         sut.RefreshInitialConditionsFromBuildingBlocks(_initialConditionsBuildingBlock, _initialConditionsToRefresh);
      }

      [Observation]
      public void the_formula_that_was_refreshed_is_in_the_cache()
      {
         _initialConditionsBuildingBlock.FormulaCache.Contains(_refreshedFormula).ShouldBeTrue();
      }

      [Observation]
      public void the_formula_that_was_replaced_is_not_removed_from_cache()
      {
         _initialConditionsBuildingBlock.FormulaCache.Contains(_replacedFormula).ShouldBeTrue();
      }

      [Observation]
      public void the_formula_that_was_added_is_renamed()
      {
         A.CallTo(() => _nameCorrector.AutoCorrectName(A<IEnumerable<string>>._, _refreshedFormula)).MustHaveHappenedOnceExactly();
      }
   }
}