using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditParameterListPresenter : ContextSpecification<IEditParametersInContainerPresenter>
   {
      protected IEditParametersInContainerView _view;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IParameterToParameterDTOMapper _parameterMapper;
      protected IInteractionTasksForParameter _inteactionTasks;
      protected IEditDistributedParameterPresenter _distributeParameterPresenter;
      protected IEditParameterPresenter _parameterPresenter;
      protected IParameter _parameter;
      protected IParameter _advancedParameter;
      protected IQuantityTask _quantityTask;
      protected IInteractionTaskContext _interactionTaskContext;
      private IClipboardManager _clipboardManager;
      private IEditTaskFor<IParameter> _editTask;
      protected ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      protected IFavoriteTask _favoriteTask;

      protected override void Context()
      {
         _view = A.Fake<IEditParametersInContainerView>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterMapper = A.Fake<IParameterToParameterDTOMapper>();
         _inteactionTasks = A.Fake<IInteractionTasksForParameter>();
         _distributeParameterPresenter = A.Fake<IEditDistributedParameterPresenter>();
         _parameterPresenter = A.Fake<IEditParameterPresenter>();
         _parameter = new Parameter().WithId("P").WithName("P");
         _parameter.Visible = true;
         _advancedParameter = new Parameter().WithId("AP").WithName("AP");
         _quantityTask = A.Fake<IQuantityTask>();
         _clipboardManager = A.Fake<IClipboardManager>();
         _advancedParameter.Visible = false;
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<IParameter>>();
         _selectReferencePresenterFactory = A.Fake<ISelectReferencePresenterFactory>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         sut = new EditParametersInContainerPresenter(_view, _formulaMapper, _parameterMapper, _inteactionTasks,
            _distributeParameterPresenter, _parameterPresenter, _quantityTask, _interactionTaskContext, _clipboardManager, _editTask, _selectReferencePresenterFactory, _favoriteTask);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   class When_trying_to_copy_nothing : concern_for_EditParameterListPresenter
   {
      protected override void Because()
      {
         sut.CopyToClipBoard(null);
      }

      [Observation]
      public void should_not_throw_an_exception()
      {
      }
      
   }

   public class When_pasting_from_clipboard : concern_for_EditParameterListPresenter
   {
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         sut.Edit(_container);
      }

      protected override void Because()
      {
         sut.PasteFromClipBoard();
      }

      [Observation]
      public void edit_must_be_called_after_pasting()
      {
         // This test ensures that the edit is called twice 
         A.CallTo(() => _selectReferencePresenterFactory.ReferenceAtParameterFor(_container)).MustHaveHappened(Repeated.Exactly.Times(4));
      }
   }

   public class When_told_to_set_parameter_value_in_a_buildingblock : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private double _newDisplayValue;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _newDisplayValue = 2.4;
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut.BuildingBlock = _buildingBlock;
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, _newDisplayValue, _buildingBlock)).Returns(A.Fake<ICommand>());
      }

      protected override void Because()
      {
         sut.OnParameterValueSet(_parameterDTO, _newDisplayValue);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, _newDisplayValue, _buildingBlock)).MustHaveHappened();
      }
   }

   internal class When_told_to_reset_a_parameter_value : concern_for_EditParameterListPresenter
   {
      private IParameterDTO _parameterDTO;
      private readonly double _setValue = 2.0;
      private readonly double _value = 3.0;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock= A.Fake<IBuildingBlock>();
         _parameter = new Parameter().WithName("p")
            .WithDimension(new Dimension(new BaseDimensionRepresentation(), "Dim", "unit"))
            .WithFormula(new ConstantFormula(_value));
         _parameter.Value = _value;

         _parameter.IsFixedValue = true;
         _parameterDTO = new ParameterDTO(_parameter) {Value = _setValue};
         sut.BuildingBlock = _buildingBlock;
      }

      protected override void Because()
      {
         sut.ResetValueFor(_parameterDTO);
      }

      [Observation]
      public void should_reset_the_dto_value()
      {
         _parameterDTO.Value.ShouldBeEqualTo(_value);
      }

      [Observation]
      public void should_call_parameter_task_for_reset()
      {
         A.CallTo(() => _quantityTask.ResetQuantityValue(_parameter, _buildingBlock)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_value_in_a_simulation : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private double _newDisplayValue;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _newDisplayValue = 2.4;
         sut.BuildingBlock = null;
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(_interactionTaskContext.Context).WithReturnType<IMoBiSimulation>().Returns(_simulation);
         A.CallTo(() => _interactionTaskContext.Active<IMoBiSimulation>()).Returns(_simulation);
      }

      protected override void Because()
      {
         sut.OnParameterValueSet(_parameterDTO, _newDisplayValue);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, _newDisplayValue, _simulation)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_display_unit_in_a_buildingblock : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private IBuildingBlock _buildingBlock;
      private Unit _displayUnit;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _displayUnit = A.Fake<Unit>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut.BuildingBlock = _buildingBlock;
      }

      protected override void Because()
      {
         sut.SetParamterUnit(_parameterDTO, _displayUnit);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_parameter, _displayUnit, _buildingBlock)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_display_unit_in_a_simulation : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private Unit _displayUnit;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _displayUnit = A.Fake<Unit>();
         sut.BuildingBlock = null;
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _interactionTaskContext.Active<IMoBiSimulation>()).Returns(_simulation);
         A.CallTo(_interactionTaskContext.Context).WithReturnType<IMoBiSimulation>().Returns(null);
      }

      protected override void Because()
      {
         sut.SetParamterUnit(_parameterDTO, _displayUnit);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_parameter, _displayUnit, _simulation)).MustHaveHappened();
      }
   }

   public class When_told_to_select_a_parameter : concern_for_EditParameterListPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterMapper.MapFrom(_parameter)).Returns(new ParameterDTO(_parameter));
         sut.Edit(new Container());
      }

      protected override void Because()
      {
         sut.Select(_parameter);
      }

      [Observation]
      public void should_edit_the_selected_paramter()
      {
         A.CallTo(() => _parameterPresenter.Edit(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_select_the_parameter_in_the_view()
      {
         A.CallTo(() => _view.Select(A<ParameterDTO>._)).MustHaveHappened();
      }
   }

   public class When_changing_Show_Advanced_Parameter_Property_to_false : concern_for_EditParameterListPresenter
   {
      private List<ParameterDTO> _boundParameterDTOs;

      protected override void Context()
      {
         base.Context();
         var testContainer = new Container();
         testContainer.Add(_parameter);
         testContainer.Add(_advancedParameter);

         A.CallTo(() => _parameterMapper.MapFrom(_parameter)).Returns(new ParameterDTO(_parameter));
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter)).Returns(new ParameterDTO(_advancedParameter));

         sut.Edit(testContainer);
         A.CallTo(() => _view.BindTo(A<IEnumerable<ParameterDTO>>._))
            .Invokes(x => _boundParameterDTOs = x.GetArgument<IEnumerable<ParameterDTO>>(0).ToList());
      }

      protected override void Because()
      {
         sut.ShowAdvancedParameters = false;
      }

      [Observation]
      public void should_Set_user_setting_show_advanced_parameter_to_new_value()
      {
         _interactionTaskContext.UserSettings.ShowAdvancedParameters.ShouldBeEqualTo(false);
      }

      [Observation]
      public void should_only_map_normal_parameters()
      {
         _boundParameterDTOs.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_changing_Show_Advanced_Parameter_Property_to_true : concern_for_EditParameterListPresenter
   {
      private bool _newValue;

      protected override void Context()
      {
         base.Context();
         var testContainer = new Container();
         A.CallTo(() => _parameterMapper.MapFrom(_parameter)).Returns(new ParameterDTO(_parameter));
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter)).Returns(new ParameterDTO(_advancedParameter));

         testContainer.Add(_parameter);
         testContainer.Add(_advancedParameter);
         sut.Edit(testContainer);
         _newValue = true;
      }

      protected override void Because()
      {
         sut.ShowAdvancedParameters = _newValue;
      }

      [Observation]
      public void should_Set_user_setting_show_advanced_parameter_to_new_value()
      {
         _interactionTaskContext.UserSettings.ShowAdvancedParameters.ShouldBeEqualTo(_newValue);
      }

      [Observation]
      public void should_map_all_parameters()
      {
         A.CallTo(() => _parameterMapper.MapFrom(_parameter)).MustHaveHappened();
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_to_favorite : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut.BuildingBlock = _buildingBlock;
      }

      protected override void Because()
      {
         sut.SetIsFavorite(_parameterDTO, true);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _favoriteTask.SetParameterFavorite(_parameter, true)).MustHaveHappened();
      }
   }
}