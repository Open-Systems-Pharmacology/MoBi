using System.Collections.Generic;
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
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_MoleculeStartValuesTask : ContextSpecification<IMoleculeStartValuesTask>
   {
      protected IMoleculeStartValuesCreator _moleculeStartValuesCreator;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected MoleculeStartValuesBuildingBlock _moleculeStartValueBuildingBlock;
      private IEditTasksForBuildingBlock<MoleculeStartValuesBuildingBlock> _editTask;
      protected IInteractionTaskContext _context;
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IMoleculeResolver _moleculeResolver;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForBuildingBlock<MoleculeStartValuesBuildingBlock>>();
         _moleculeStartValuesCreator = A.Fake<IMoleculeStartValuesCreator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _moleculeStartValueBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _moleculeResolver = A.Fake<IMoleculeResolver>();

         sut = new MoleculeStartValuesTask(_context, _editTask, _moleculeStartValuesCreator,
            new ImportedQuantityToMoleculeStartValueMapper(_moleculeStartValuesCreator), A.Fake<IMoleculeStartValueBuildingBlockExtendManager>(), _cloneManagerForBuildingBlock, _reactionDimensionRetriever, A.Fake<IMoBiFormulaTask>(), A.Fake<IMoBiSpatialStructureFactory>(), new MoleculeStartValuePathTask(A.Fake<IFormulaTask>(), _context.Context), _moleculeResolver);
      }
   }

   /// <summary>
   ///    Making sure that the execute is actually called as part of the specs. Command is actually tested elsewhere
   /// </summary>
   public class When_updating_scale_divisor : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _moleculeStartValue;

      protected override void Context()
      {
         base.Context();
         _moleculeStartValue = new MoleculeStartValue {ScaleDivisor = 1};
      }

      protected override void Because()
      {
         sut.UpdateStartValueScaleDivisor(_moleculeStartValueBuildingBlock, _moleculeStartValue, 65, _moleculeStartValue.ScaleDivisor);
      }

      [Observation]
      public void must_update_scale_divisor()
      {
         _moleculeStartValue.ScaleDivisor.ShouldBeEqualTo(65);
      }
   }

   public class When_retrieving_default_dimension : concern_for_MoleculeStartValuesTask
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

   public class When_removing_building_block_referenced_by_simulation : concern_for_MoleculeStartValuesTask
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
         _module = new Module { _moleculeStartValueBuildingBlock };
         A.CallTo(() => _simulation.IsCreatedBy(_moleculeStartValueBuildingBlock)).Returns(true);
         A.CallTo(() => _context.Context.CurrentProject).Returns(_project);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Remove(_moleculeStartValueBuildingBlock, _module, null)).ShouldThrowAn<MoBiException>();
      }
   }

   public class When_returning_the_list_of_predefined_container_in_the_spatial_structure_referenced_by_a_molecule_start_value : concern_for_MoleculeStartValuesTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Context.Get<SpatialStructure>(_moleculeStartValueBuildingBlock.SpatialStructureId)).Returns(null);
         A.CallTo(() => _context.Context.Create<SpatialStructure>(_moleculeStartValueBuildingBlock.SpatialStructureId)).Returns(new SpatialStructure());
      }

      [Observation]
      public void should_return_an_empty_list_of_the_spatial_structure_was_not_defined()
      {
         sut.GetContainerPathItemsForBuildingBlock(_moleculeStartValueBuildingBlock).ShouldBeEmpty();
      }
   }

   public class When_comparing_start_value_to_original_builder : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _moleculeStartValue;
      private const string _name = "Name";
      protected bool _result;
      private IDimension _dimension;
      protected MoleculeBuilder _builder;

      protected override void Context()
      {
         base.Context();
         _dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);
         _moleculeStartValue = new MoleculeStartValue {Dimension = _dimension, Name = _name, Formula = null};

         _builder = new MoleculeBuilder
         {
            Name = _name,
            Dimension = _dimension
         };
      }

      protected override void Because()
      {
         _result = sut.IsEquivalentToOriginal(_moleculeStartValue, _moleculeStartValueBuildingBlock);
      }
   }

   public class When_original_builder_cannot_be_found : When_comparing_start_value_to_original_builder
   {
      [Observation]
      public void should_test_as_not_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_original_builder_can_be_found : When_comparing_start_value_to_original_builder
   {
      protected override void Context()
      {
         base.Context();
         if (_builder != null) A.CallTo(_moleculeResolver).WithReturnType<MoleculeBuilder>().Returns(_builder);
      }

      [Observation]
      public void moleculeStartValue_and_builder_are_equivalent()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_importing_multiple_molecule_start_values : concern_for_MoleculeStartValuesTask
   {
      private IList<ImportedQuantityDTO> _moleculeStartValues;
      private MoleculeStartValue _firstStartValueRef;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         _firstStartValueRef = new MoleculeStartValue {Path = new ObjectPath("this", "path", "C1"), StartValue = -1.0, IsPresent = true};
         IDimension d = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);

         _moleculeStartValues = new List<ImportedQuantityDTO>
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

         _moleculeStartValueBuildingBlock.Add(_firstStartValueRef);

         for (var i = 1; i < 3; i++)
         {
            var dto = _moleculeStartValues[i];
            A.CallTo(() => _moleculeStartValuesCreator.CreateMoleculeStartValue(dto.ContainerPath, dto.Name, A<IDimension>._, A<Unit>._, A<ValueOrigin>._)).Returns(
               new MoleculeStartValue
               {
                  ContainerPath = dto.ContainerPath,
                  Name = dto.Name,
                  IsPresent = dto.IsPresent,
                  Dimension = dto.Dimension,
                  DisplayUnit = dto.DisplayUnit,
                  StartValue = dto.QuantityInBaseUnit
               });
         }
      }

      protected override void Because()
      {
         _result = sut.ImportStartValuesToBuildingBlock(_moleculeStartValueBuildingBlock, _moleculeStartValues);
      }

      [Observation]
      public void resulting_command_should_have_correct_attributes()
      {
         _result.CommandType.ShouldBeEqualTo(AppConstants.Commands.ImportCommand);
         _result.Description.ShouldBeEqualTo(AppConstants.Commands.ImportMoleculeStartValues);
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.MoleculeStartValue);
      }

      [Observation]
      public void building_block_should_contain_3_start_values()
      {
         _moleculeStartValueBuildingBlock.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void value_of_start_values_should_match_commands_issued()
      {
         _moleculeStartValueBuildingBlock[_moleculeStartValues[0].Path].Value.ShouldBeEqualTo(1.0);
         _moleculeStartValueBuildingBlock[_moleculeStartValues[1].Path].Value.ShouldBeEqualTo(2.0);
         _moleculeStartValueBuildingBlock[_moleculeStartValues[2].Path].Value.ShouldBeEqualTo(3.0);
      }

      [Observation]
      public void reference_in_building_block_should_not_have_changed_for_updated_start_value()
      {
         _moleculeStartValueBuildingBlock[_moleculeStartValues[0].Path].ShouldBeEqualTo(_firstStartValueRef);
      }
   }

   public class When_setting_the_is_present_flag_for_a_set_of_molecule_start_values : concern_for_MoleculeStartValuesTask
   {
      private List<MoleculeStartValue> _startValues;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _startValues = new List<MoleculeStartValue>
         {
            new MoleculeStartValue {Name = "M1", IsPresent = true},
            new MoleculeStartValue {Name = "M2", IsPresent = false},
            new MoleculeStartValue {Name = "M3", IsPresent = true},
            new MoleculeStartValue {Name = "M4", IsPresent = false}
         };
         _startValues.Each(_moleculeStartValueBuildingBlock.Add);
      }

      protected override void Because()
      {
         _command = sut.SetIsPresent(_moleculeStartValueBuildingBlock, _startValues, isPresent: true);
      }

      [Observation]
      public void should_return_a_macro_command_containing_one_command_for_each_molecule_start_value_for_which_the_flag_value_has_changed()
      {
         _command.ShouldBeAnInstanceOf<IMoBiMacroCommand>();
         var macrocommand = _command.DowncastTo<IMoBiMacroCommand>();
         macrocommand.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_setting_the_negatiave_start_values_flag_for_a_set_of_molecule_start_values : concern_for_MoleculeStartValuesTask
   {
      private List<MoleculeStartValue> _startValues;
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         _startValues = new List<MoleculeStartValue>
         {
            new MoleculeStartValue {Name = "M1", NegativeValuesAllowed = true},
            new MoleculeStartValue {Name = "M2", NegativeValuesAllowed = false},
            new MoleculeStartValue {Name = "M3", NegativeValuesAllowed = true},
            new MoleculeStartValue {Name = "M4", NegativeValuesAllowed = false}
         };
         _startValues.Each(_moleculeStartValueBuildingBlock.Add);
      }

      protected override void Because()
      {
         _command = sut.SetNegativeValuesAllowed(_moleculeStartValueBuildingBlock, _startValues, negativeValuesAllowed: true);
      }

      [Observation]
      public void should_return_a_macro_command_containing_one_command_for_each_molecule_start_value_for_which_the_flag_value_has_changed()
      {
         _command.ShouldBeAnInstanceOf<IMoBiMacroCommand>();
         var macrocommand = _command.DowncastTo<IMoBiMacroCommand>();
         macrocommand.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_removing_an_element_of_start_value : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _startValue;

      protected override void Because()
      {
         _startValue = new MoleculeStartValue {ContainerPath = new ObjectPath("A", "B")};
         _moleculeStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_moleculeStartValueBuildingBlock, _startValue, 0, "");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("B");
      }
   }

   public class When_appending_an_element_of_start_value : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _startValue;

      protected override void Because()
      {
         _startValue = new MoleculeStartValue {ContainerPath = new ObjectPath("A", "B")};
         _moleculeStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_moleculeStartValueBuildingBlock, _startValue, 2, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_replacing_an_element_of_start_value : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _startValue;

      protected override void Because()
      {
         _startValue = new MoleculeStartValue {ContainerPath = new ObjectPath("A", "B")};
         _moleculeStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_moleculeStartValueBuildingBlock, _startValue, 0, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("C|B");
      }
   }

   public class When_replacing_an_element_outside_start_value_path_range : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _startValue;

      protected override void Because()
      {
         _startValue = new MoleculeStartValue {ContainerPath = new ObjectPath("A", "B")};
         _moleculeStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_moleculeStartValueBuildingBlock, _startValue, 5, "C");
      }

      [Observation]
      public void should_not_affect_start_value_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("A|B|");
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_NaN_and_molecule_start_value_NaN : concern_for_MoleculeStartValuesTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         var molecule = new MoleculeBuilder {Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ConstantFormula(double.NaN)};
         var nanStartValue = new MoleculeStartValue {Formula = new ConstantFormula(double.NaN), Name = molecule.Name, StartValue = double.NaN, Dimension = Constants.Dimension.NO_DIMENSION};
         _moleculeStartValueBuildingBlock.Add(nanStartValue);
         A.CallTo(_moleculeResolver).WithReturnType<MoleculeBuilder>().Returns(molecule);
      }

      protected override void Because()
      {
         _command = sut.RefreshStartValuesFromBuildingBlocks(_moleculeStartValueBuildingBlock, _moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_not_create_an_entry_in_the_history()
      {
         _command.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public class When_updating_a_molecule_start_value_with_new_display_unit : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValue _startValue;
      private IDimension _dim;
      private const double _targetBaseValue = 1000000;
      private const double _targetDisplayValue = 1000;

      protected override void Context()
      {
         base.Context();
         _dim = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);

         _startValue = new MoleculeStartValue {Dimension = _dim, StartValue = _targetDisplayValue, DisplayUnit = _dim.Unit("g")};
      }

      protected override void Because()
      {
         var targetUnit = _dim.Unit("kg");

         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         sut.SetDisplayValueWithUnit(_startValue, _startValue.ConvertToDisplayUnit(_startValue.Value.Value), targetUnit, A.Fake<MoleculeStartValuesBuildingBlock>());
      }

      [Observation]
      public void display_amount_should_not_change()
      {
         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         _startValue.ConvertToDisplayUnit(_startValue.Value.Value).ShouldBeEqualTo(_targetDisplayValue);
      }

      [Observation]
      public void base_amount_must_change_to_new_amount()
      {
         // ReSharper disable once PossibleInvalidOperationException - suppress the warning. We want the exception if it's thrown
         _startValue.Value.Value.ShouldBeEqualTo(_targetBaseValue);
      }
   }

   public class When_updating_a_molecule_start_value_from_original_building_block_When_the_start_value_has_changed : concern_for_MoleculeStartValuesTask
   {
      protected override void Context()
      {
         base.Context();
         var builder = new MoleculeBuilder {Name = "molecule", Dimension = Constants.Dimension.NO_DIMENSION, DefaultStartFormula = new ExplicitFormula("50")};
         var startValue = new MoleculeStartValue {Name = builder.Name, StartValue = 45, Dimension = Constants.Dimension.NO_DIMENSION, Formula = null};
         _moleculeStartValueBuildingBlock.Add(startValue);
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(builder.DefaultStartFormula, _moleculeStartValueBuildingBlock.FormulaCache)).Returns(new ExplicitFormula("M/V"));
         A.CallTo(_moleculeResolver).WithReturnType<MoleculeBuilder>().Returns(builder);
      }

      protected override void Because()
      {
         sut.RefreshStartValuesFromBuildingBlocks(_moleculeStartValueBuildingBlock, _moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void formula_must_be_set()
      {
         _moleculeStartValueBuildingBlock.Each(startValue => startValue.Formula.IsExplicit().ShouldBeTrue());
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_NaN_and_molecule_start_value_NULL : concern_for_MoleculeStartValuesTask
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         var molecule = new MoleculeBuilder {Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION};
         var nanStartValue = new MoleculeStartValue {Name = molecule.Name, StartValue = null, Dimension = Constants.Dimension.NO_DIMENSION};
         _moleculeStartValueBuildingBlock.Add(nanStartValue);
         A.CallTo(_moleculeResolver).WithReturnType<MoleculeBuilder>().Returns(molecule);
      }

      protected override void Because()
      {
         _command = sut.RefreshStartValuesFromBuildingBlocks(_moleculeStartValueBuildingBlock, _moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_not_create_an_entry_in_the_history()
      {
         _command.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public class When_updating_a_molecule_start_value_building_block_with_original_value_null_and_molecule_start_value_NULL : concern_for_MoleculeStartValuesTask
   {
      private IMoBiCommand _command;
      private MoleculeStartValue _nullStartValue;

      protected override void Context()
      {
         base.Context();
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         var molecule = new MoleculeBuilder {Name = "Mol", Dimension = Constants.Dimension.NO_DIMENSION};
         moleculeBuildingBlock.Add(molecule);
         _nullStartValue = new MoleculeStartValue {Name = molecule.Name, StartValue = 1, Dimension = Constants.Dimension.NO_DIMENSION};
         _moleculeStartValueBuildingBlock.Add(_nullStartValue);
         A.CallTo(_context.Context).WithReturnType<MoleculeBuildingBlock>().Returns(moleculeBuildingBlock);
         A.CallTo(() => _moleculeResolver.Resolve(_nullStartValue.ContainerPath, _nullStartValue.MoleculeName, A<SpatialStructure>._, A<MoleculeBuildingBlock>._)).Returns(molecule);
      }

      protected override void Because()
      {
         _command = sut.RefreshStartValuesFromBuildingBlocks(_moleculeStartValueBuildingBlock, _moleculeStartValueBuildingBlock);
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

   public class When_creating_a_molecule_start_value_for_simulation_based_on_a_selected_template : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValuesBuildingBlock _result;
      private SimulationConfiguration _simulationConfiguration;
      private MoleculeStartValuesBuildingBlock _templateStartValuesBuildingBlock;
      private MoleculeStartValuesBuildingBlock _newMoleculeStartValues;
      private MoleculeStartValue _newEndogenousValue;
      private MoleculeStartValue _existingEndogenousValue;
      private MoleculeStartValue _existingTemplateEndogenousValue;
      private MoleculeStartValue _otherStartValues;

      protected override void Context()
      {
         base.Context();
         _templateStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _simulationConfiguration = new SimulationConfiguration();
         var module = new Module
         {
            new SpatialStructure(),
            new MoleculeBuildingBlock()
         };
         var moduleConfiguration = new ModuleConfiguration(module);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         moduleConfiguration.Module.Add(_templateStartValuesBuildingBlock);
         moduleConfiguration.SelectedMoleculeStartValues = _templateStartValuesBuildingBlock;

         _newMoleculeStartValues = new MoleculeStartValuesBuildingBlock();

         // _buildConfiguration.MoleculeStartValuesInfo = new MoleculeStartValuesBuildingBlockInfo {BuildingBlock = _templateStartValuesBuildingBlock, TemplateBuildingBlock = _templateStartValuesBuildingBlock};

         A.CallTo(_moleculeStartValuesCreator).WithReturnType<MoleculeStartValuesBuildingBlock>().Returns(_newMoleculeStartValues);

         _newEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Plasma"), Name = "M", IsPresent = true};
         _existingEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Cell"), Name = "M", IsPresent = true};
         _otherStartValues = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", "Liver", "Cell"), Name = "M", IsPresent = true};
         _existingTemplateEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Cell"), Name = "M", IsPresent = true};

         _templateStartValuesBuildingBlock.Add(_existingTemplateEndogenousValue);
         _newMoleculeStartValues.Add(_newEndogenousValue);
         _newMoleculeStartValues.Add(_existingEndogenousValue);
         _newMoleculeStartValues.Add(_otherStartValues);
      }

      protected override void Because()
      {
         _result = sut.CreateStartValuesForSimulation(_simulationConfiguration);
      }

      [Observation]
      public void should_ensure_that_molecule_defined_in_endogenous_igg_compartments_are_not_present_by_default()
      {
         _newEndogenousValue.IsPresent.ShouldBeFalse();
      }

      [Observation]
      public void should_let_previous_molecule_present_in_endogenous_igg_compartment_to_present()
      {
         _existingEndogenousValue.IsPresent.ShouldBeTrue();
      }

      [Observation]
      public void should_mark_the_added_molecule_start_values_as_present_in_all_other_organs()
      {
         _otherStartValues.IsPresent.ShouldBeTrue();
      }
   }

   public class When_extending_a_given_molecule_start_value_with_building_block_based_on_used_templates : concern_for_MoleculeStartValuesTask
   {
      private MoleculeStartValuesBuildingBlock _templateMoleculeStartValues;
      private MoleculeStartValue _newEndogenousValue;
      private MoleculeStartValue _existingEndogenousValue;
      private MoleculeStartValue _existingTemplateEndogenousValue;

      protected override void Context()
      {
         base.Context();
         _templateMoleculeStartValues = new MoleculeStartValuesBuildingBlock();
         A.CallTo(_context.Context.ObjectRepository).WithReturnType<bool>().Returns(true);
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         A.CallTo(_context.Context).WithReturnType<MoleculeBuildingBlock>().Returns(moleculeBuildingBlock);

         var spatialStructure = new SpatialStructure();
         A.CallTo(_context.Context).WithReturnType<SpatialStructure>().Returns(spatialStructure);
         A.CallTo(() => _moleculeStartValuesCreator.CreateFrom(spatialStructure, moleculeBuildingBlock)).Returns(_templateMoleculeStartValues);

         _newEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Plasma"), Name = "M", IsPresent = true};
         _existingEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Cell"), Name = "M", IsPresent = true};
         _existingTemplateEndogenousValue = new MoleculeStartValue {ContainerPath = new ObjectPath("Organism", AppConstants.Organs.ENDOGENOUS_IGG, "Cell"), Name = "M", IsPresent = true};

         _templateMoleculeStartValues.Add(_newEndogenousValue);
         _templateMoleculeStartValues.Add(_existingTemplateEndogenousValue);

         _moleculeStartValueBuildingBlock.Add(_existingEndogenousValue);
      }

      protected override void Because()
      {
         sut.ExtendStartValues(_moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_set_new_entries_for_moelcule_in_endogenous_container_to_non_present()
      {
         _newEndogenousValue.IsPresent.ShouldBeFalse();
      }

      [Observation]
      public void should_not_change_existing_entries_for_moelcule_in_endogenous_container()
      {
         _existingTemplateEndogenousValue.IsPresent.ShouldBeTrue();
      }
   }
}