using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_ParameterStartValuesTask : ContextSpecification<ParameterStartValuesTask>
   {
      protected IParameterStartValuesCreator _parameterStartValuesCreator;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected ParameterStartValuesBuildingBlock _parameterStartValueBuildingBlock;
      protected IInteractionTaskContext _context;
      private IEditTasksForBuildingBlock<ParameterStartValuesBuildingBlock> _editTasks;
      protected IParameterResolver _parameterResolver;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTasks = A.Fake<IEditTasksForBuildingBlock<ParameterStartValuesBuildingBlock>>();
         _parameterStartValuesCreator = A.Fake<IParameterStartValuesCreator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _parameterStartValueBuildingBlock = new ParameterStartValuesBuildingBlock();
         _parameterResolver = A.Fake<IParameterResolver>();

         sut = new ParameterStartValuesTask(_context, _editTasks,
            _parameterStartValuesCreator,
            _cloneManagerForBuildingBlock,
            new ImportedQuantityToParameterStartValueMapper(_parameterStartValuesCreator), _parameterResolver, A.Fake<IParameterStartValueBuildingBlockExtendManager>(),
            A.Fake<IMoBiFormulaTask>(), A.Fake<IMoBiSpatialStructureFactory>(), new ParameterStartValuePathTask(A.Fake<IFormulaTask>(), _context.Context));
      }
   }

   public class When_updating_parameter_start_values_from_template : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValuesBuildingBlock _templateStartValues;
      private ObjectPath _containerPath;
      private ParameterStartValue _parameterStartValue;
      private ParameterStartValue _clonedStartValue;

      protected override void Context()
      {
         base.Context();
         _templateStartValues = new ParameterStartValuesBuildingBlock();

         _containerPath = new ObjectPath("the", "container", "path");

         _parameterStartValueBuildingBlock.Add(
            new ParameterStartValue {StartValue = 0.1, ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "ConstantStartValue"});

         _parameterStartValue = new ParameterStartValue {ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "FormulaStartValue", StartValue = 4};
         _clonedStartValue = new ParameterStartValue {ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "FormulaStartValue", StartValue = 4};

         _templateStartValues.Add(_parameterStartValue);
         _templateStartValues.Add(new ParameterStartValue {StartValue = 0.4, ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "ConstantStartValue"});

         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_parameterStartValue, A<IFormulaCache>.Ignored)).Returns(_clonedStartValue);
      }

      protected override void Because()
      {
         sut.UpdateValuesFromTemplate(_parameterStartValueBuildingBlock, _templateStartValues);
      }

      [Observation]
      public void constant_value_should_be_updated_from_template_building_block()
      {
         _parameterStartValueBuildingBlock[_containerPath.AndAdd("ConstantStartValue")].Value.ShouldBeEqualTo(0.4);
      }

      [Observation]
      public void start_value_should_be_added_to_building_block_from_template_building_block()
      {
         _parameterStartValueBuildingBlock[_containerPath.AndAdd("FormulaStartValue")].Value.ShouldBeEqualTo(4);
      }
   }

   public class When_retrieving_default_dimension_for_Parameters : concern_for_ParameterStartValuesTask
   {
      private IDimension _result;

      protected override void Because()
      {
         _result = sut.GetDefaultDimension();
      }

      [Observation]
      public void result_should_be_amount_dimension()
      {
         _result.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
      }
   }

   public class When_comparing_start_value_to_original_parameter : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _parameterStartValue;
      private const string _name = "Name";
      protected bool _result;
      private IDimension _dimension;
      protected IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);
         _parameterStartValue = DomainHelperForSpecs.ParameterStartValue;
         _parameterStartValue.Dimension = _dimension;

         _parameter = new Parameter {Dimension = _dimension, Name = _name, Value = 1.0};
      }

      protected override void Because()
      {
         _result = sut.IsEquivalentToOriginal(_parameterStartValue, _parameterStartValueBuildingBlock);
      }
   }

   public class When_original_parameter_cannot_be_found : When_comparing_start_value_to_original_parameter
   {
      [Observation]
      public void should_test_as_not_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_original_parameter_can_be_found : When_comparing_start_value_to_original_parameter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterResolver.Resolve(A<ObjectPath>._, A<string>._, A<SpatialStructure>._, A<MoleculeBuildingBlock>._)).Returns(_parameter);
      }

      [Observation]
      public void parameterStartValue_and_builder_are_equivalent()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_original_parameter_can_be_found_but_its_formula_cannot_be_evaluated : When_comparing_start_value_to_original_parameter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterResolver.Resolve(A<ObjectPath>._, A<string>._, A<SpatialStructure>._, A<MoleculeBuildingBlock>._)).Returns(_parameter);
         _parameter.Formula = new ExplicitFormula("1+ Exp");
      }

      [Observation]
      public void parameterStartValue_and_builder_are_not_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_importing_multiple_parameter_start_values : concern_for_ParameterStartValuesTask
   {
      private IList<ImportedQuantityDTO> _parameterStartValues;
      private ParameterStartValue _firstStartValueRef;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         var unit = new Unit("Dimensionless", 1.0, 0.0);
         _firstStartValueRef = new ParameterStartValue {ContainerPath = new ObjectPath("this", "path"), Name = "Name", StartValue = -1.0, DisplayUnit = unit};
         _parameterStartValues = new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"this", "path"}), QuantityInBaseUnit = 1.0, DisplayUnit = unit},
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"that", "path"}), QuantityInBaseUnit = 2.0, DisplayUnit = unit},
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"the", "path"}), QuantityInBaseUnit = 3.0, DisplayUnit = unit}
         };

         _parameterStartValueBuildingBlock.Add(_firstStartValueRef);

         for (var i = 1; i < 3; i++)
         {
            var dto = _parameterStartValues[i];
            A.CallTo(() => _parameterStartValuesCreator.CreateParameterStartValue(dto.Path, dto.QuantityInBaseUnit, A<IDimension>._, A<Unit>._, A<ValueOrigin>._, A<bool>._)).Returns(
               new ParameterStartValue
               {
                  ContainerPath = dto.ContainerPath,
                  Name = dto.Name,
                  Dimension = dto.Dimension,
                  DisplayUnit = dto.DisplayUnit,
                  StartValue = dto.QuantityInBaseUnit
               });
         }
      }

      protected override void Because()
      {
         _result = sut.ImportStartValuesToBuildingBlock(_parameterStartValueBuildingBlock, _parameterStartValues);
      }

      [Observation]
      public void resulting_command_should_have_correct_attributes()
      {
         _result.CommandType.ShouldBeEqualTo(AppConstants.Commands.ImportCommand);
         _result.Description.ShouldBeEqualTo(AppConstants.Commands.ImportParameterStartValues);
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.ParameterValue);
      }

      [Observation]
      public void building_block_should_contain_3_start_values()
      {
         _parameterStartValueBuildingBlock.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void value_of_start_values_should_match_commands_issued()
      {
         _parameterStartValueBuildingBlock[_parameterStartValues[0].Path].Value.ShouldBeEqualTo(1.0);
         _parameterStartValueBuildingBlock[_parameterStartValues[1].Path].Value.ShouldBeEqualTo(2.0);
         _parameterStartValueBuildingBlock[_parameterStartValues[2].Path].Value.ShouldBeEqualTo(3.0);
      }

      [Observation]
      public void reference_in_building_block_should_not_have_changed_for_updated_start_value()
      {
         _parameterStartValueBuildingBlock[_parameterStartValues[0].Path].ShouldBeEqualTo(_firstStartValueRef);
      }
   }

   public class When_removing_an_element_of_parameter_start_value : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterStartValue {ContainerPath = new ObjectPath("A", "B"), Name = "C"};
         _parameterStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_parameterStartValueBuildingBlock, _startValue, 0, "");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("B|C");
      }
   }

   public class When_appending_an_element_of_parameter_start_value : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterStartValue {ContainerPath = new ObjectPath("A", "B")};
         _parameterStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_parameterStartValueBuildingBlock, _startValue, 2, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_replacing_an_element_of_parameter_start_value : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterStartValue {ContainerPath = new ObjectPath("A", "B"), Name = "D"};
         _parameterStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_parameterStartValueBuildingBlock, _startValue, 0, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("C|B|D");
      }
   }

   public class When_replacing_an_element_outside_parameter_start_value_path_range : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterStartValue {ContainerPath = new ObjectPath("A", "B")};
         _parameterStartValueBuildingBlock.Add(_startValue);
         sut.EditStartValueContainerPath(_parameterStartValueBuildingBlock, _startValue, 5, "C");
      }

      [Observation]
      public void should_not_affect_start_value_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("A|B|");
      }
   }

   public class When_updating_the_value_description_of_a_start_value : concern_for_ParameterStartValuesTask
   {
      private ParameterStartValue _startValue;
      private ValueOrigin _valueOrigin;

      protected override void Because()
      {
         _startValue = new ParameterStartValue();
         _parameterStartValueBuildingBlock.Add(_startValue);
         _valueOrigin = new ValueOrigin
         {
            Method = ValueOriginDeterminationMethods.Assumption,
            Description = "hello"
         };

         sut.SetValueOrigin(_parameterStartValueBuildingBlock, _valueOrigin, _startValue);
      }

      [Observation]
      public void should_update_the_start_value_description()
      {
         _startValue.ValueOrigin.Description.ShouldBeEqualTo(_valueOrigin.Description);
         _startValue.ValueOrigin.Method.ShouldBeEqualTo(_valueOrigin.Method);
      }
   }
}