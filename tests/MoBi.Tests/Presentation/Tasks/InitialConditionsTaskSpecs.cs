﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;
using OSPSuite.Core.Services;

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
      protected IMoleculeResolver _moleculeResolver;
      protected IInteractionTasksForMoleculeBuilder _moleculeBuilderTask;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForBuildingBlock<InitialConditionsBuildingBlock>>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _moleculeResolver = A.Fake<IMoleculeResolver>();
         _moleculeBuilderTask = A.Fake<IInteractionTasksForMoleculeBuilder>();

         sut = new InitialConditionsTask<InitialConditionsBuildingBlock>(_context, _editTask, A.Fake<IInitialConditionsBuildingBlockExtendManager>(), _cloneManagerForBuildingBlock, A.Fake<IMoBiFormulaTask>(), A.Fake<IMoBiSpatialStructureFactory>(), new ImportedQuantityToInitialConditionMapper(_initialConditionsCreator),
            new InitialConditionPathTask(A.Fake<IFormulaTask>(), _context.Context), _reactionDimensionRetriever, _initialConditionsCreator);
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
         _initialCondition = new InitialCondition {ScaleDivisor = 1};
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
         _firstStartValueRef = new InitialCondition {Path = new ObjectPath("this", "path", "C1"), Value = -1.0, IsPresent = true};
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
            new ImportedQuantityDTO {Name = "C1", ContainerPath = new ObjectPath("that", "path"), QuantityInBaseUnit = 2.0, Dimension = d},
            new ImportedQuantityDTO {Name = "C1", ContainerPath = new ObjectPath("the", "path"), QuantityInBaseUnit = 3.0, Dimension = d}
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
            new InitialCondition {Name = "M1", IsPresent = true},
            new InitialCondition {Name = "M2", IsPresent = false},
            new InitialCondition {Name = "M3", IsPresent = true},
            new InitialCondition {Name = "M4", IsPresent = false}
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
            new InitialCondition {Name = "M1", NegativeValuesAllowed = true},
            new InitialCondition {Name = "M2", NegativeValuesAllowed = false},
            new InitialCondition {Name = "M3", NegativeValuesAllowed = true},
            new InitialCondition {Name = "M4", NegativeValuesAllowed = false}
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
         _startValue = new InitialCondition {ContainerPath = new ObjectPath("A", "B")};
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
         _startValue = new InitialCondition {ContainerPath = new ObjectPath("A", "B")};
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
         _startValue = new InitialCondition {ContainerPath = new ObjectPath("A", "B")};
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
         _startValue = new InitialCondition {ContainerPath = new ObjectPath("A", "B")};
         _initialConditionsBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_initialConditionsBuildingBlock, _startValue, 5, "C");
      }

      [Observation]
      public void should_not_affect_start_value_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("A|B|");
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

         _startValue = new InitialCondition {Dimension = _dim, Value = TARGET_DISPLAY_VALUE, DisplayUnit = _dim.Unit("g")};
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

   public class When_extending_initial_conditions_and_not_exactly_one_of_each_type_is_available : concern_for_InitialConditionsTask
   {
      private ISelectBuildingBlocksForExtendPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISelectBuildingBlocksForExtendPresenter>();
         A.CallTo(() => _context.Context.Resolve<ISelectBuildingBlocksForExtendPresenter>()).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.ExtendStartValueBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_spatial_structure_and_molecule_selection_presenter_is_used_to_select_the_building_blocks()
      {
         A.CallTo(() => _presenter.SelectBuildingBlocksForExtend(true)).MustHaveHappened();
      }
   }

   public class When_extending_initial_conditions_and_exactly_one_of_each_type_is_available : concern_for_InitialConditionsTask
   {
      private MoBiSpatialStructure _moBiSpatialStructure;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moBiSpatialStructure = new MoBiSpatialStructure();
         A.CallTo(() => _context.BuildingBlockRepository.SpatialStructureCollection).Returns(new List<MoBiSpatialStructure> { _moBiSpatialStructure });
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(() => _context.BuildingBlockRepository.MoleculeBlockCollection).Returns(new List<MoleculeBuildingBlock> { _moleculeBuildingBlock });
         
      }

      protected override void Because()
      {
         sut.ExtendStartValueBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_spatial_structure_and_molecule_selection_presenter_is_not_used_to_select_the_building_blocks()
      {
         A.CallTo(() => _context.Context.Resolve<ISelectBuildingBlocksForExtendPresenter>()).MustNotHaveHappened();
      }

      [Observation]
      public void the_only_valid_values_should_be_used_to_create_new_initial_conditions()
      {
         A.CallTo(() => _initialConditionsCreator.CreateFrom(_moBiSpatialStructure, A<IReadOnlyList<MoleculeBuilder>>._)).MustHaveHappened();
      }
   }

   public class When_extending_initial_conditions_and_the_building_blocks_are_not_selected : concern_for_InitialConditionsTask
   {
      private ISelectBuildingBlocksForExtendPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ISelectBuildingBlocksForExtendPresenter>();
         A.CallTo(() => _context.Context.Resolve<ISelectBuildingBlocksForExtendPresenter>()).Returns(_presenter);
         A.CallTo(() => _presenter.SelectedMoleculeBuildingBlock).Returns(null);
      }

      protected override void Because()
      {
         sut.ExtendStartValueBuildingBlock(_initialConditionsBuildingBlock);
      }

      [Observation]
      public void the_initial_conditions_creator_is_not_used_to_create_initial_conditions()
      {
         A.CallTo(() => _initialConditionsCreator.CreateFrom(A<MoBiSpatialStructure>._, A<IReadOnlyList<MoleculeBuilder>>._)).MustNotHaveHappened();
      }
   }
}