using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectAndEditMoleculesStartValuesPresenter : ContextSpecification<ISelectAndEditMoleculesStartValuesPresenter>
   {
      protected ISelectAndEditContainerView _view;
      protected IMoleculeStartValuesPresenter _editStartValuesPresenter;
      protected IMoleculeStartValuesTask _moleculeStartValuesTask;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<ISelectAndEditContainerView>();
         _editStartValuesPresenter = A.Fake<IMoleculeStartValuesPresenter>();
         _moleculeStartValuesTask = A.Fake<IMoleculeStartValuesTask>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new SelectAndEditMoleculesStartValuesPresenter(_view, _moleculeStartValuesTask, _cloneManager, new ObjectTypeResolver(), _editStartValuesPresenter, A.Fake<ILegendPresenter>());
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_starting_the_seleact_and_edit_molecule_start_values_presenter : concern_for_SelectAndEditMoleculesStartValuesPresenter
   {
      [Observation]
      public void should_not_allow_the_creation_of_formula()
      {
         A.CallTo(_editStartValuesPresenter).Where(x => x.Method.Name.Equals("set_CanCreateNewFormula")).WhenArgumentsMatch(x => x.Get<bool>(0) == false).MustHaveHappened();
      }
   }

   internal class When_Add_to_Project_is_called_at_SelectAndEditMoleculesStartValuesPresenter : concern_for_SelectAndEditMoleculesStartValuesPresenter
   {
      protected override void Because()
      {
         sut.AddToProject();
      }

      [Observation]
      public void should_generate_add_command_for_msv()
      {
         A.CallTo(() => _moleculeStartValuesTask.AddToProject(A<MoleculeStartValuesBuildingBlock>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_cloned_the_msv()
      {
         A.CallTo(() => _cloneManager.CloneBuildingBlock(A<MoleculeStartValuesBuildingBlock>._)).MustHaveHappened();
      }
   }
}