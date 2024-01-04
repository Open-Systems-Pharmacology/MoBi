using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
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
   public abstract class concern_for_ParameterValuesTask : ContextSpecification<ParameterValuesTask>
   {
      protected IParameterValuesCreator _parameterValuesCreator;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected ParameterValuesBuildingBlock _parameterValueBuildingBlock;
      protected IInteractionTaskContext _context;
      private IEditTasksForBuildingBlock<ParameterValuesBuildingBlock> _editTasks;
      protected IParameterResolver _parameterResolver;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTasks = A.Fake<IEditTasksForBuildingBlock<ParameterValuesBuildingBlock>>();
         _parameterValuesCreator = A.Fake<IParameterValuesCreator>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _parameterValueBuildingBlock = new ParameterValuesBuildingBlock();
         _parameterResolver = A.Fake<IParameterResolver>();

         sut = new ParameterValuesTask(_context, _editTasks,
            _cloneManagerForBuildingBlock,
            new ImportedQuantityToParameterValueMapper(_parameterValuesCreator), A.Fake<IParameterValueBuildingBlockExtendManager>(),
            A.Fake<IMoBiFormulaTask>(), A.Fake<IMoBiSpatialStructureFactory>(), new ParameterValuePathTask(A.Fake<IFormulaTask>(), _context.Context), _parameterValuesCreator);
      }
   }

   public class updating_parameter_values_from_template : concern_for_ParameterValuesTask
   {
      private ParameterValuesBuildingBlock _templateParameterValues;
      private ObjectPath _containerPath;
      private ParameterValue _parameterValue;
      private ParameterValue _clonedParameterValue;

      protected override void Context()
      {
         base.Context();
         _templateParameterValues = new ParameterValuesBuildingBlock();

         _containerPath = new ObjectPath("the", "container", "path");

         _parameterValueBuildingBlock.Add(
            new ParameterValue { Value = 0.1, ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "ConstantStartValue"});

         _parameterValue = new ParameterValue {ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "FormulaStartValue", Value = 4};
         _clonedParameterValue = new ParameterValue {ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "FormulaStartValue", Value = 4};

         _templateParameterValues.Add(_parameterValue);
         _templateParameterValues.Add(new ParameterValue { Value = 0.4, ContainerPath = _containerPath.Clone<ObjectPath>(), Name = "ConstantStartValue"});

         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_parameterValue, A<IFormulaCache>.Ignored)).Returns(_clonedParameterValue);
      }

      protected override void Because()
      {
         sut.UpdateValuesFromTemplate(_parameterValueBuildingBlock, _templateParameterValues);
      }

      [Observation]
      public void constant_value_should_be_updated_from_template_building_block()
      {
         _parameterValueBuildingBlock[_containerPath.AndAdd("ConstantStartValue")].Value.ShouldBeEqualTo(0.4);
      }

      [Observation]
      public void start_value_should_be_added_to_building_block_from_template_building_block()
      {
         _parameterValueBuildingBlock[_containerPath.AndAdd("FormulaStartValue")].Value.ShouldBeEqualTo(4);
      }
   }

   public class When_retrieving_default_dimension_for_Parameters : concern_for_ParameterValuesTask
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

   public class importing_multiple_parameter_values : concern_for_ParameterValuesTask
   {
      private IList<ImportedQuantityDTO> _parameterValue;
      private ParameterValue _firstStartValueRef;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         var unit = new Unit("Dimensionless", 1.0, 0.0);
         _firstStartValueRef = new ParameterValue {ContainerPath = new ObjectPath("this", "path"), Name = "Name", Value = -1.0, DisplayUnit = unit};
         _parameterValue = new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"this", "path"}), QuantityInBaseUnit = 1.0, DisplayUnit = unit},
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"that", "path"}), QuantityInBaseUnit = 2.0, DisplayUnit = unit},
            new ImportedQuantityDTO {Name = "Name", ContainerPath = new ObjectPath(new[] {"the", "path"}), QuantityInBaseUnit = 3.0, DisplayUnit = unit}
         };

         _parameterValueBuildingBlock.Add(_firstStartValueRef);

         for (var i = 1; i < 3; i++)
         {
            var dto = _parameterValue[i];
            A.CallTo(() => _parameterValuesCreator.CreateParameterValue(dto.Path, dto.QuantityInBaseUnit, A<IDimension>._, A<Unit>._, A<ValueOrigin>._, A<bool>._)).Returns(
               new ParameterValue
               {
                  ContainerPath = dto.ContainerPath,
                  Name = dto.Name,
                  Dimension = dto.Dimension,
                  DisplayUnit = dto.DisplayUnit,
                  Value = dto.QuantityInBaseUnit
               });
         }
      }

      protected override void Because()
      {
         _result = sut.ImportPathAndValueEntitiesToBuildingBlock(_parameterValueBuildingBlock, _parameterValue);
      }

      [Observation]
      public void resulting_command_should_have_correct_attributes()
      {
         _result.CommandType.ShouldBeEqualTo(AppConstants.Commands.ImportCommand);
         _result.Description.ShouldBeEqualTo(AppConstants.Commands.ImportParameterValues);
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.ParameterValue);
      }

      [Observation]
      public void building_block_should_contain_3_start_values()
      {
         _parameterValueBuildingBlock.Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void value_of_start_values_should_match_commands_issued()
      {
         _parameterValueBuildingBlock[_parameterValue[0].Path].Value.ShouldBeEqualTo(1.0);
         _parameterValueBuildingBlock[_parameterValue[1].Path].Value.ShouldBeEqualTo(2.0);
         _parameterValueBuildingBlock[_parameterValue[2].Path].Value.ShouldBeEqualTo(3.0);
      }

      [Observation]
      public void reference_in_building_block_should_not_have_changed_for_updated_start_value()
      {
         _parameterValueBuildingBlock[_parameterValue[0].Path].ShouldBeEqualTo(_firstStartValueRef);
      }
   }

   public class removing_an_element_of_parameter_value : concern_for_ParameterValuesTask
   {
      private ParameterValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterValue {ContainerPath = new ObjectPath("A", "B"), Name = "C"};
         _parameterValueBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_parameterValueBuildingBlock, _startValue, 0, "");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("B|C");
      }
   }

   public class appending_an_element_of_parameter_value : concern_for_ParameterValuesTask
   {
      private ParameterValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterValue {ContainerPath = new ObjectPath("A", "B")};
         _parameterValueBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_parameterValueBuildingBlock, _startValue, 2, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.ContainerPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class replacing_an_element_of_parameter_value : concern_for_ParameterValuesTask
   {
      private ParameterValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterValue {ContainerPath = new ObjectPath("A", "B"), Name = "D"};
         _parameterValueBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_parameterValueBuildingBlock, _startValue, 0, "C");
      }

      [Observation]
      public void task_must_have_properly_configured_new_replacement_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("C|B|D");
      }
   }

   public class replacing_an_element_outside_parameter_value_path_range : concern_for_ParameterValuesTask
   {
      private ParameterValue _startValue;

      protected override void Because()
      {
         _startValue = new ParameterValue {ContainerPath = new ObjectPath("A", "B")};
         _parameterValueBuildingBlock.Add(_startValue);
         sut.EditPathAndValueEntityContainerPath(_parameterValueBuildingBlock, _startValue, 5, "C");
      }

      [Observation]
      public void should_not_affect_start_value_path()
      {
         _startValue.Path.PathAsString.ShouldBeEqualTo("A|B|");
      }
   }

   public class updating_the_value_description_of_a_value : concern_for_ParameterValuesTask
   {
      private ParameterValue _startValue;
      private ValueOrigin _valueOrigin;

      protected override void Because()
      {
         _startValue = new ParameterValue();
         _parameterValueBuildingBlock.Add(_startValue);
         _valueOrigin = new ValueOrigin
         {
            Method = ValueOriginDeterminationMethods.Assumption,
            Description = "hello"
         };

         sut.SetValueOrigin(_parameterValueBuildingBlock, _valueOrigin, _startValue);
      }

      [Observation]
      public void should_update_the_start_value_description()
      {
         _startValue.ValueOrigin.Description.ShouldBeEqualTo(_valueOrigin.Description);
         _startValue.ValueOrigin.Method.ShouldBeEqualTo(_valueOrigin.Method);
      }
   }
}