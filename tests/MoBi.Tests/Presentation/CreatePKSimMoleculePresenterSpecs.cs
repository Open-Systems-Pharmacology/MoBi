using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreatePKSimMoleculePresenter : ContextSpecification<ICreatePKSimMoleculePresenter>
   {
      protected ICreatePKSimMoleculeView _view;
      protected IMoBiConfiguration _configuration;
      protected IParameterToParameterDTOMapper _parameterDTOMapper;
      protected IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderMapper;
      protected IQuantityTask _quantityTask;
      protected ISerializationTask _serializationTask;
      protected IMoleculeBuildingBlock _moleculeBuildingBlock;
      protected IEditTaskFor<IMoleculeBuilder> _editTask;

      protected override void Context()
      {
         _view = A.Fake<ICreatePKSimMoleculeView>();
         _configuration = A.Fake<IMoBiConfiguration>();
         _parameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         _moleculeBuilderMapper = A.Fake<IMoleculeBuilderToMoleculeBuilderDTOMapper>();
         _quantityTask = A.Fake<IQuantityTask>();
         _serializationTask = A.Fake<ISerializationTask>();
         _moleculeBuildingBlock = A.Fake<IMoleculeBuildingBlock>();
         _editTask = A.Fake<IEditTaskFor<IMoleculeBuilder>>();

         sut = new CreatePKSimMoleculePresenter(_view, _configuration, _parameterDTOMapper, _moleculeBuilderMapper, _serializationTask, _quantityTask, _editTask);
      }
   }

   public class When_creating_a_molecule_from_template_and_the_user_cancels_the_action : concern_for_CreatePKSimMoleculePresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      [Observation]
      public void should_return_null()
      {
         sut.CreateMolecule(_moleculeBuildingBlock).ShouldBeNull();
      }
   }

   public class When_creating_a_molecule_from_template : concern_for_CreatePKSimMoleculePresenter
   {
      private IMoleculeBuilder _molecule;
      private IMoleculeBuilder _templateMoleculeBuilder;
      private string _templateFileName;
      private MoleculeBuilderDTO _dto;
      private IParameter _templateParameterToEdit;
      private IParameter _templateParameterToHide;
      private ParameterDTO _parameterDTO;
      private IParameter _aFormulaParameterToHide;

      protected override void Context()
      {
         base.Context();
         _templateFileName = "Template";
         _templateParameterToEdit = new Parameter
         {
            Formula = new ConstantFormula(double.NaN),
            Visible = true,
            Name = "P1"
         };

         _templateParameterToHide = new Parameter
         {
            Formula = new ConstantFormula(double.NaN),
            Visible = false,
            Name = "P2"
         };

         _aFormulaParameterToHide = new Parameter
         {
            Visible = true,
            Name = "P3"
         };

         _templateMoleculeBuilder = new MoleculeBuilder {_templateParameterToEdit, _templateParameterToHide, _aFormulaParameterToHide};
         _parameterDTO = new ParameterDTO(_templateParameterToEdit);

         A.CallTo(() => _configuration.StandardMoleculeTemplateFile).Returns(_templateFileName);
         A.CallTo(() => _serializationTask.Load<IMoleculeBuilder>(_templateFileName, true)).Returns(_templateMoleculeBuilder);

         A.CallTo(() => _moleculeBuilderMapper.MapFrom(_templateMoleculeBuilder)).Returns(new MoleculeBuilderDTO());

         A.CallTo(() => _view.BindTo(A<MoleculeBuilderDTO>._))
            .Invokes(x => _dto = x.GetArgument<MoleculeBuilderDTO>(0));

         A.CallTo(() => _parameterDTOMapper.MapFrom(_templateParameterToEdit)).Returns(_parameterDTO);
         A.CallTo(_editTask).WithReturnType<IEnumerable<string>>().Returns(new [] {"A","B"});
      }

      protected override void Because()
      {
         _molecule = sut.CreateMolecule(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_load_the_default_molecule_from_template()
      {
         _molecule.ShouldBeEqualTo(_templateMoleculeBuilder);
      }

      [Observation]
      public void should_only_display_in_the_view_the_parameter_that_need_to_be_edited_by_the_user()
      {
         _dto.Parameters.ShouldOnlyContain(_parameterDTO);
      }

      [Observation]
      public void should_set_the_unallowed_name_into_the_dto()
      {
         _dto.IsNameUnique("A").ShouldBeFalse();
         _dto.IsNameUnique("B").ShouldBeFalse();
         _dto.IsNameUnique("C").ShouldBeTrue();
      }
   }

   public class When_setting_the_parameter_value_of_an_imported_pksim_parameter : concern_for_CreatePKSimMoleculePresenter
   {
      private IParameter _parameter;
      private ParameterDTO _parameterDTO;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _parameterDTO = new ParameterDTO(_parameter);
         sut.CreateMolecule(_moleculeBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetParameterValue(_parameterDTO, 5);
      }

      [Observation]
      public void should_use_the_quantity_task_to_set_the_value()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, 5, _moleculeBuildingBlock)).MustHaveHappened();
      }
   }

   public class When_setting_the_parameter_unit_of_an_imported_pksim_parameter : concern_for_CreatePKSimMoleculePresenter
   {
      private IParameter _parameter;
      private ParameterDTO _parameterDTO;
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _unit = A.Fake<Unit>();
         _parameterDTO = new ParameterDTO(_parameter);
         sut.CreateMolecule(_moleculeBuildingBlock);
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_parameterDTO, _unit);
      }

      [Observation]
      public void should_use_the_quantity_task_to_set_the_unit()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_parameter, _unit, _moleculeBuildingBlock)).MustHaveHappened();
      }
   }
}