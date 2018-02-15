using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_UserDefinedParametersPresenter : ContextSpecification<IUserDefinedParametersPresenter>
   {
      protected IUserDefinedParametersView _view;

      protected IContainer _container1;
      protected IContainer _container2;
      protected IParameter _parameter1;
      protected IParameter _parameter2;
      protected IParameter _parameter3;
      private IEditParameterListPresenter _editParameterListPresenter;
      protected List<IParameter> _editedParameters;

      protected override void Context()
      {
         _view = A.Fake<IUserDefinedParametersView>();
         _editParameterListPresenter = A.Fake<IEditParameterListPresenter>();
         sut = new UserDefinedParametersPresenter(_view, _editParameterListPresenter);

         _parameter1 = new Parameter().WithName("P1");
         _parameter2 = new Parameter().WithName("P2");
         _parameter3 = new Parameter().WithName("P3");

         _container1 = new Container {_parameter1, _parameter2};
         _container2 = new Container {_parameter3};

         _parameter1.IsDefault = true;
         _parameter2.IsDefault = false;
         _parameter3.IsDefault = false;

         A.CallTo(() => _editParameterListPresenter.Edit(A<IEnumerable<IParameter>>._))
            .Invokes(x => _editedParameters = x.GetArgument<IEnumerable<IParameter>>(0).ToList());
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
         _editedParameters.ShouldOnlyContain(_parameter2, _parameter3);
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
         _editedParameters.ShouldOnlyContain(_parameter2);
      }
   }
}