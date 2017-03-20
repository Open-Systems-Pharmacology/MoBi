using OSPSuite.Core.Commands.Core;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;


namespace MoBi.Presentation
{
   public abstract class concern_for_ISelectAndEditParameterStartValuesPresenterSpecs : ContextSpecification<ISelectAndEditParameterStartValuesPresenter>
   {
      protected ISelectAndEditContainerView _view;
      protected IParameterStartValuesPresenter _editStartValuesPresenter;
      protected IParameterStartValuesTask _moleculeStartValuesTask;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<ISelectAndEditContainerView>();
         _editStartValuesPresenter = A.Fake<IParameterStartValuesPresenter>();
         _moleculeStartValuesTask = A.Fake<IParameterStartValuesTask>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new SelectAndEditParameterStartValuesPresenter(_view, _moleculeStartValuesTask, _cloneManager,new ObjectTypeResolver(), _editStartValuesPresenter, A.Fake<ILegendPresenter>());
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   class When_Add_to_Project_is_called_at_SelectAndEditParameterStartValuesPresenter : concern_for_ISelectAndEditParameterStartValuesPresenterSpecs
   {
      protected override void Because()
      {
         sut.AddToProject();
      }

      [Observation]
      public void should_generate_add_command_for_psv()
      {
         A.CallTo(() => _moleculeStartValuesTask.AddToProject(A<IParameterStartValuesBuildingBlock>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_cloned_the_psv()
      {
         A.CallTo(() => _cloneManager.CloneBuildingBlock(A<IParameterStartValuesBuildingBlock>._)).MustHaveHappened();
      }
   }
}	