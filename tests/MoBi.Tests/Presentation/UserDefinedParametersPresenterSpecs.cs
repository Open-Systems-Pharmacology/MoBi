using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_UserDefinedParametersPresenter : ContextSpecification<IUserDefinedParametersPresenter>
   {
      protected IEditParameterListView _view;
      protected IQuantityTask _quantityTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IInteractionTasksForParameter _parameterTask;
      protected IFavoriteTask _favoriteTask;
      protected IParameterToParameterDTOMapper _parameterDTOMapper;
      protected IViewItemContextMenuFactory _viewContextMenuItemFactory;

      protected IContainer _container1;
      protected IContainer _container2;
      protected IParameter _parameter1;
      protected IParameter _parameter2;
      protected IParameter _parameter3;
      protected ParameterDTO _parameterDTO1;
      protected ParameterDTO _parameterDTO2;
      protected ParameterDTO _parameterDTO3;
      protected List<ParameterDTO> _allDTOs;

      protected override void Context()
      {
         _view = A.Fake<IEditParameterListView>();
         _quantityTask = A.Fake<IQuantityTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterTask = A.Fake<IInteractionTasksForParameter>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         _parameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         _viewContextMenuItemFactory = A.Fake<IViewItemContextMenuFactory>();

         sut = new UserDefinedParametersPresenter(_view, _quantityTask, _interactionTaskContext, _formulaMapper, _parameterTask, _favoriteTask, _parameterDTOMapper, _viewContextMenuItemFactory);

         _parameter1 = new Parameter().WithName("P1");
         _parameter2 = new Parameter().WithName("P2");
         _parameter3 = new Parameter().WithName("P3");

         _container1 = new Container {_parameter1, _parameter2};
         _container2 = new Container {_parameter3};

         _parameter1.IsDefault = true;
         _parameter2.IsDefault = false;
         _parameter3.IsDefault = false;

         _parameterDTO1 = new ParameterDTO(_parameter1);
         _parameterDTO2 = new ParameterDTO(_parameter2);
         _parameterDTO3 = new ParameterDTO(_parameter3);

         A.CallTo(() => _parameterDTOMapper.MapFrom(_parameter1)).Returns(_parameterDTO1);
         A.CallTo(() => _parameterDTOMapper.MapFrom(_parameter2)).Returns(_parameterDTO2);
         A.CallTo(() => _parameterDTOMapper.MapFrom(_parameter3)).Returns(_parameterDTO3);

         A.CallTo(() => _view.BindTo(A<IEnumerable<ParameterDTO>>._))
            .Invokes(x => _allDTOs = x.GetArgument<IEnumerable<ParameterDTO>>(0).ToList());
      }
   }

   public class When_updating_the_list_of_user_defined_parameters_defined_in_some_containers : concern_for_UserDefinedParametersPresenter
   {
      protected override void Because()
      {
         sut.ShowUserDefinedParametersIn(new[] {_container1, _container2});
      }

      [Observation]
      public void should_retrieve_all_parameters_with_that_are_not_in_the_default_state_and_display_them_in_the_view()
      {
         _allDTOs.ShouldOnlyContain(_parameterDTO2, _parameterDTO3);
      }
   }

   public class When_updating_the_list_of_user_defined_parameters_defined_in_one_containes : concern_for_UserDefinedParametersPresenter
   {
      protected override void Because()
      {
         sut.ShowUserDefinedParametersIn(_container1);
      }

      [Observation]
      public void should_retrieve_all_parameters_with_that_are_not_in_the_default_state_and_display_them_in_the_view()
      {
         _allDTOs.ShouldOnlyContain(_parameterDTO2);
      }
   }

   public class When_the_user_defined_parameters_presenter_is_navigating_to_a_given_parameter : concern_for_UserDefinedParametersPresenter
   {
      private EntitySelectedEvent _event;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.Context.PublishEvent(A<EntitySelectedEvent>._))
            .Invokes(x => _event = x.GetArgument<EntitySelectedEvent>(0));
      }

      protected override void Because()
      {
         sut.GoTo(_parameterDTO2);
      }

      [Observation]
      public void should_raise_the_entity_selected_event_with_the_given_parameter()
      {
         _event.ObjectBase.ShouldBeEqualTo(_parameterDTO2.Parameter);
      }
   }
}