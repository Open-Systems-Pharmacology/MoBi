using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_ParameterReferencesPresenter : ContextSpecification<ParameterReferencesPresenter>
   {
      protected IParameterReferencesView _view;
      protected IObjectTypeResolver _objectTypeResolver;
      protected IMoBiApplicationController _applicationController;
      protected IMoBiContext _context;
      protected IMoBiSimulation _simulation;
      protected IParameter _parameter;
      protected IReadOnlyList<ParameterReferenceDTO> _boundReferences;
      protected string _breadcrumb;
      protected bool _canGoBack;

      protected override void Context()
      {
         _view = A.Fake<IParameterReferencesView>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _context = A.Fake<IMoBiContext>();

         A.CallTo(() => _view.BindTo(A<IReadOnlyList<ParameterReferenceDTO>>._))
            .Invokes(x => _boundReferences = x.GetArgument<IReadOnlyList<ParameterReferenceDTO>>(0));

         A.CallTo(() => _view.UpdateNavigation(A<string>._, A<bool>._))
            .Invokes(x =>
            {
               _breadcrumb = x.GetArgument<string>(0);
               _canGoBack = x.GetArgument<bool>(1);
            });

         var entityPathResolver = new EntityPathResolver(new ObjectPathFactory(new AliasCreator()));
         sut = new ParameterReferencesPresenter(_view, entityPathResolver, new FormulaUsableToParameterReferenceDTOMapper(entityPathResolver, _objectTypeResolver), _applicationController, _context);
      }
   }

   public abstract class concern_for_ParameterReferencesPresenter_with_a_simulation : concern_for_ParameterReferencesPresenter
   {
      protected IParameter _referencingParameter;

      protected override void Context()
      {
         base.Context();
         var root = new Container { ContainerType = ContainerType.Simulation }.WithName("Sim");
         var organism = new Container().WithName("Organism");
         root.Add(organism);

         _parameter = new Parameter().WithName("P").WithFormula(new ConstantFormula(1));
         organism.Add(_parameter);

         //references the parameter with an absolute path
         var referencingFormula = new ExplicitFormula("P * 2").WithName("F1");
         referencingFormula.AddObjectPath(new FormulaUsablePath("Sim", "Organism", "P").WithAlias("P"));
         _referencingParameter = new Parameter().WithName("P2").WithFormula(referencingFormula);
         organism.Add(_referencingParameter);

         //references the parameter in the RHS formula
         var rhsFormula = new ExplicitFormula("P + 1").WithName("F2");
         rhsFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "P").WithAlias("PRel"));
         var parameterWithRHS = new Parameter().WithName("P3").WithFormula(new ConstantFormula(1));
         parameterWithRHS.RHSFormula = rhsFormula;
         organism.Add(parameterWithRHS);

         //references the referencing parameter (indirect reference to the parameter)
         var indirectFormula = new ExplicitFormula("P2 * 3").WithName("F4");
         indirectFormula.AddObjectPath(new FormulaUsablePath("Sim", "Organism", "P2").WithAlias("P2"));
         organism.Add(new Parameter().WithName("P5").WithFormula(indirectFormula));

         new ReferencesResolver().ResolveReferencesIn(root);

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Model).Returns(new Model { Root = root });
      }
   }

   public class When_showing_the_references_to_a_parameter_used_in_a_simulation : concern_for_ParameterReferencesPresenter_with_a_simulation
   {
      protected override void Because()
      {
         sut.ShowReferencesTo(_parameter, _simulation);
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void should_bind_the_references_to_the_view()
      {
         _boundReferences.Select(x => x.UsedBy).ShouldOnlyContain("Organism|P2", "Organism|P3");
      }

      [Observation]
      public void should_not_navigate_to_any_object()
      {
         A.CallTo(() => _applicationController.Select(A<IObjectBase>._, A<IObjectBase>._, A<OSPSuite.Core.Commands.Core.ICommandCollector>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_show_the_target_as_breadcrumb_and_not_allow_going_back()
      {
         _breadcrumb.ShouldBeEqualTo("P");
         _canGoBack.ShouldBeFalse();
      }
   }

   public class When_drilling_into_the_references_of_a_referencing_object : concern_for_ParameterReferencesPresenter_with_a_simulation
   {
      protected override void Because()
      {
         sut.ShowReferencesTo(_parameter, _simulation);
         sut.FindReferencesFor(_boundReferences.Single(x => string.Equals(x.UsedBy, "Organism|P2")));
      }

      [Observation]
      public void should_show_the_references_to_the_object_of_the_selected_reference()
      {
         _boundReferences.Single().UsedBy.ShouldBeEqualTo("Organism|P5");
      }

      [Observation]
      public void should_update_the_caption_to_the_new_target()
      {
         _view.Caption.ShouldBeEqualTo(AppConstants.Captions.ReferencesTo("Organism|P2"));
      }

      [Observation]
      public void should_show_the_drill_in_trail_as_breadcrumb_and_allow_going_back()
      {
         _breadcrumb.ShouldBeEqualTo("P → P2");
         _canGoBack.ShouldBeTrue();
      }
   }

   public class When_going_back_after_drilling_into_a_reference : concern_for_ParameterReferencesPresenter_with_a_simulation
   {
      protected override void Because()
      {
         sut.ShowReferencesTo(_parameter, _simulation);
         sut.FindReferencesFor(_boundReferences.Single(x => string.Equals(x.UsedBy, "Organism|P2")));
         sut.GoBack();
      }

      [Observation]
      public void should_show_the_references_to_the_previous_target_again()
      {
         _boundReferences.Select(x => x.UsedBy).ShouldOnlyContain("Organism|P2", "Organism|P3");
         _view.Caption.ShouldBeEqualTo(AppConstants.Captions.ReferencesTo("Organism|P"));
      }

      [Observation]
      public void should_show_the_previous_target_as_breadcrumb_and_not_allow_going_back()
      {
         _breadcrumb.ShouldBeEqualTo("P");
         _canGoBack.ShouldBeFalse();
      }
   }

   public class When_going_back_without_having_drilled_into_a_reference : concern_for_ParameterReferencesPresenter_with_a_simulation
   {
      protected override void Because()
      {
         sut.ShowReferencesTo(_parameter, _simulation);
         sut.GoBack();
      }

      [Observation]
      public void should_not_rebind_the_view()
      {
         A.CallTo(() => _view.BindTo(A<IReadOnlyList<ParameterReferenceDTO>>._)).MustHaveHappenedOnceExactly();
      }
   }

   public class When_going_to_the_object_of_a_reference : concern_for_ParameterReferencesPresenter_with_a_simulation
   {
      protected override void Context()
      {
         base.Context();
         //simulates the user pressing the go to button while the modal view is open
         A.CallTo(() => _view.Display())
            .Invokes(x => sut.GoTo(_boundReferences.Single(dto => string.Equals(dto.UsedBy, "Organism|P2"))));
      }

      protected override void Because()
      {
         sut.ShowReferencesTo(_parameter, _simulation);
      }

      [Observation]
      public void should_close_the_view()
      {
         A.CallTo(() => _view.CloseView()).MustHaveHappened();
      }

      [Observation]
      public void should_navigate_to_the_object_in_the_simulation_after_the_view_was_closed()
      {
         A.CallTo(() => _applicationController.Select(_referencingParameter, _simulation, _context.HistoryManager)).MustHaveHappened();
      }
   }

   public class When_checking_if_the_references_of_a_reference_can_be_searched : concern_for_ParameterReferencesPresenter
   {
      [Observation]
      public void should_allow_the_search_for_an_object_that_can_be_referenced_by_formulas()
      {
         sut.CanFindReferencesFor(new ParameterReferenceDTO { UsedByObject = new Parameter() }).ShouldBeTrue();
      }

      [Observation]
      public void should_not_allow_the_search_for_an_object_that_cannot_be_referenced_by_formulas()
      {
         sut.CanFindReferencesFor(new ParameterReferenceDTO { UsedByObject = new EventAssignment() }).ShouldBeFalse();
      }
   }
}
