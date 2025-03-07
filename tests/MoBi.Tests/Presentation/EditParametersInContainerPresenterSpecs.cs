using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditParametersInContainerPresenter : ContextSpecification<EditParametersInContainerPresenter>
   {
      protected IEditParametersInContainerView _view;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IParameterToParameterDTOMapper _parameterMapper;
      protected IInteractionTasksForParameter _interactionTasks;
      protected IEditDistributedParameterPresenter _distributeParameterPresenter;
      protected IEditParameterPresenter _parameterPresenter;
      protected IParameter _parameter;
      protected IParameter _advancedParameter;
      protected IQuantityTask _quantityTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IClipboardManager _clipboardManager;
      protected IEditTaskFor<IParameter> _editTask;
      protected ISelectReferencePresenterFactory _selectReferencePresenterFactory;
      protected IFavoriteTask _favoriteTask;
      protected IObjectTypeResolver _typeResolver;
      protected IEntityPathResolver _entityPathResolver;
      protected IObjectPathFactory _objectPathFactory;
      protected IIndividualParameterToParameterDTOMapper _individualParameterToParameterDTOMapper;
      protected IEditIndividualParameterPresenter _editIndividualParameterPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditParametersInContainerView>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterMapper = A.Fake<IParameterToParameterDTOMapper>();
         _interactionTasks = A.Fake<IInteractionTasksForParameter>();
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
         _typeResolver = A.Fake<IObjectTypeResolver>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _individualParameterToParameterDTOMapper = A.Fake<IIndividualParameterToParameterDTOMapper>();
         _editIndividualParameterPresenter = A.Fake<IEditIndividualParameterPresenter>();

         sut = new EditParametersInContainerPresenter(_view, _formulaMapper, _parameterMapper, _interactionTasks,
            _distributeParameterPresenter, _parameterPresenter, _quantityTask, _interactionTaskContext, _clipboardManager, _editTask,
            _selectReferencePresenterFactory, _favoriteTask, _typeResolver, _entityPathResolver, _objectPathFactory, _individualParameterToParameterDTOMapper, _editIndividualParameterPresenter);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   class When_trying_to_copy_nothing : concern_for_EditParametersInContainerPresenter
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

   public class When_pasting_from_clipboard : concern_for_EditParametersInContainerPresenter
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
         A.CallTo(() => _selectReferencePresenterFactory.ReferenceAtParameterFor(_container)).MustHaveHappenedANumberOfTimesMatching(x => x == 4);
      }
   }

   public class When_told_to_set_parameter_value_in_a_building_block : concern_for_EditParametersInContainerPresenter
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
      public void should_tell_quantity_task_to_set_value()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, _newDisplayValue, _buildingBlock)).MustHaveHappened();
      }
   }

   internal class When_told_to_reset_a_parameter_value : concern_for_EditParametersInContainerPresenter
   {
      private IParameterDTO _parameterDTO;
      private readonly double _setValue = 2.0;
      private readonly double _value = 3.0;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parameter = new Parameter().WithName("p")
            .WithDimension(new Dimension(new BaseDimensionRepresentation(), "Dim", "unit"))
            .WithFormula(new ConstantFormula(_value));
         _parameter.Value = _value;

         _parameter.IsFixedValue = true;
         _parameterDTO = new ParameterDTO(_parameter) { Value = _setValue };
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

   public class When_told_to_set_parameter_value_in_a_simulation : concern_for_EditParametersInContainerPresenter
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
      public void should_tell_quantity_task_to_set_value()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_parameter, _newDisplayValue, _simulation)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_display_unit_in_a_building_block : concern_for_EditParametersInContainerPresenter
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
         sut.SetParameterUnit(_parameterDTO, _displayUnit);
      }

      [Observation]
      public void should_tell_quantity_task_to_set_value()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_parameter, _displayUnit, _buildingBlock)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_display_unit_in_a_simulation : concern_for_EditParametersInContainerPresenter
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
         sut.SetParameterUnit(_parameterDTO, _displayUnit);
      }

      [Observation]
      public void should_tell_quantuity_task_to_set_vlaue()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_parameter, _displayUnit, _simulation)).MustHaveHappened();
      }
   }

   public class When_told_to_select_a_parameter : concern_for_EditParametersInContainerPresenter
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
      public void should_edit_the_selected_parameter()
      {
         A.CallTo(() => _parameterPresenter.Edit(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_select_the_parameter_in_the_view()
      {
         A.CallTo(() => _view.Select(A<ParameterDTO>._)).MustHaveHappened();
      }
   }

   public class When_changing_Show_Advanced_Parameter_Property_to_false : concern_for_EditParametersInContainerPresenter
   {
      private List<ParameterDTO> _boundParameterDTOs;

      protected override void Context()
      {
         base.Context();
         var testContainer = new Container
         {
            _parameter,
            _advancedParameter
         };

         A.CallTo(() => _parameterMapper.MapFrom(_parameter, A<TrackableSimulation>._)).Returns(new ParameterDTO(_parameter));
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter, A<TrackableSimulation>._)).Returns(new ParameterDTO(_advancedParameter));

         sut.Edit(testContainer);
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<ParameterDTO>>._))
            .Invokes(x => _boundParameterDTOs = x.GetArgument<IReadOnlyList<ParameterDTO>>(0).ToList());
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

   public class When_changing_Show_Advanced_Parameter_Property_to_true : concern_for_EditParametersInContainerPresenter
   {
      private bool _newValue;

      protected override void Context()
      {
         base.Context();
         var testContainer = new Container();
         A.CallTo(() => _parameterMapper.MapFrom(_parameter, A<TrackableSimulation>._)).Returns(new ParameterDTO(_parameter));
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter, A<TrackableSimulation>._)).Returns(new ParameterDTO(_advancedParameter));

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
         A.CallTo(() => _parameterMapper.MapFrom(_parameter, A<TrackableSimulation>._)).MustHaveHappened();
         A.CallTo(() => _parameterMapper.MapFrom(_advancedParameter, A<TrackableSimulation>._)).MustHaveHappened();
      }
   }

   public class When_told_to_set_parameter_to_favorite : concern_for_EditParametersInContainerPresenter
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
      public void should_tell_quantity_task_to_set_value()
      {
         A.CallTo(() => _favoriteTask.SetParameterFavorite(_parameter, true)).MustHaveHappened();
      }
   }

   public class When_container_does_not_have_a_name : concern_for_EditParametersInContainerPresenter
   {
      protected IContainer _container;
      private readonly string _containerType = "Container";

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         A.CallTo(() => _typeResolver.TypeFor(_container)).Returns(_containerType);
      }

      protected override void Because()
      {
         sut.Edit(_container);
      }

      [Observation]
      public void label_should_contain_name()
      {
         A.CallTo(_view).Where(x => x.Method.Name.Equals("set_ParentName") && x.Arguments.Get<string>(0).Equals($"New {_containerType}")).MustHaveHappened();
      }
   }

   public class When_container_does_have_a_name : concern_for_EditParametersInContainerPresenter
   {
      protected IContainer _container;
      private readonly string _containerName = "Container Name";

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _container.Name = _containerName;
      }

      protected override void Because()
      {
         sut.Edit(_container);
      }

      [Observation]
      public void label_should_contain_name()
      {
         A.CallTo(_view).Where(x => x.Method.Name.Equals("set_ParentName") && x.Arguments.Get<string>(0).Equals(_containerName)).MustHaveHappened();
      }
   }

   public class When_copying_parameter_path : concern_for_EditParametersInContainerPresenter
   {
      private string _expectedPath;
      private ParameterDTO _parameterDTO;

      protected override void Context()
      {
         base.Context();
         _parameterDTO = new ParameterDTO(_parameter);
         _expectedPath = "Organism|Container|Organ|ADC";
         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns(_expectedPath);
      }

      protected override void Because()
      {
         sut.CopyPathForParameter(_parameterDTO);
      }

      [Observation]
      public void should_copy_resolved_path_to_clipboard()
      {
         A.CallTo(() => _view.CopyToClipBoard(_expectedPath)).MustHaveHappened();
      }
   }

   public class When_selecting_an_individual_parameter_to_view : concern_for_EditParametersInContainerPresenter
   {
      private IndividualParameter _individualParameter;
      private ParameterDTO _dto;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter();
         sut.SelectedIndividual = new IndividualBuildingBlock { _individualParameter };
         _dto = new ParameterDTO(new Parameter().WithName(_individualParameter.Name)) { IsIndividualPreview = true };
         A.CallTo(() => _individualParameterToParameterDTOMapper.MapFrom(sut.SelectedIndividual, _individualParameter)).Returns(_dto);
         
         sut.Edit(new Container());
         sut.UpdatePreview();
      }

      protected override void Because()
      {
         sut.Select(_dto);
      }

      [Observation]
      public void the_editor_is_updated()
      {
         A.CallTo(() => _editIndividualParameterPresenter.Edit(_individualParameter, sut.SelectedIndividual)).MustHaveHappened();
      }

      [Observation]
      public void the_view_is_updated()
      {
         A.CallTo(() => _view.SetEditParameterView(_editIndividualParameterPresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_updating_the_preview_with_a_selected_individual : concern_for_EditParametersInContainerPresenter
   {
      private IContainer _editedContainer;
      private IndividualBuildingBlock _individualBuildingBlock;
      private IndividualParameter _individualParameter;
      private IndividualParameter _excludedIndividualParameter;
      private IReadOnlyList<ParameterDTO> _editableDTOList;

      protected override void Context()
      {
         base.Context();
         _editedContainer = new Container().WithName("last");
         new Container { _editedContainer }.WithName("root");
         _individualBuildingBlock = new IndividualBuildingBlock { Name = "John Doe" };
         _individualParameter = new IndividualParameter { ContainerPath = new ObjectPath("root", "last") }.WithName("individualParameterName");
         _excludedIndividualParameter = new IndividualParameter { ContainerPath = new ObjectPath("root", "somewhere") }.WithName("anothername");
         _individualBuildingBlock.Add(_individualParameter);
         _individualBuildingBlock.Add(_excludedIndividualParameter);
         _editedContainer.Add(new Parameter { Visible = true }.WithName("parameterName"));
         sut.Edit(_editedContainer);
         sut.SelectedIndividual = _individualBuildingBlock;

         A.CallTo(() => _parameterMapper.MapFrom(A<IParameter>._, A<TrackableSimulation>._)).ReturnsLazily(x => new ParameterDTO(x.GetArgument<IParameter>(0)));
         A.CallTo(() => _individualParameterToParameterDTOMapper.MapFrom(A<IndividualBuildingBlock>._, A<IndividualParameter>._)).ReturnsLazily(x => new ParameterDTO(new Parameter().WithName(x.GetArgument<IndividualParameter>(1).Name)) { IsIndividualPreview = true });
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<ParameterDTO>>._)).Invokes(x => _editableDTOList = x.GetArgument<IReadOnlyList<ParameterDTO>>(0));
      }

      protected override void Because()
      {
         sut.UpdatePreview();
      }

      [Observation]
      public void the_list_of_previewed_objects_should_include_an_individual_parameter_and_a_structure_parameter()
      {
         _editableDTOList.Count(x => x.IsIndividualPreview).ShouldBeEqualTo(1);
         _editableDTOList.Count(x => !x.IsIndividualPreview).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_view_should_be_updated()
      {
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<ParameterDTO>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_individual_parameter_from_the_container_should_be_mapped_for_preview()
      {
         A.CallTo(() => _individualParameterToParameterDTOMapper.MapFrom(_individualBuildingBlock, _individualParameter)).MustHaveHappened();
      }

      [Observation]
      public void the_individual_parameter_from_another_container_should_not_be_mapped_for_preview()
      {
         A.CallTo(() => _individualParameterToParameterDTOMapper.MapFrom(_individualBuildingBlock, _excludedIndividualParameter)).MustNotHaveHappened();
      }
   }
}