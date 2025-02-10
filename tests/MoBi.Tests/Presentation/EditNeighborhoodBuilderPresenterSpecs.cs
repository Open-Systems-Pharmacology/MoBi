using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditNeighborhoodBuilderPresenter : ContextSpecification<EditNeighborhoodBuilderPresenter>
   {
      private IEditNeighborhoodBuilderView _view;
      private IEditParametersInContainerPresenter _editParameterInContainerPresenter;
      private ITagsPresenter _tagsPresenter;
      private IMoBiContext _context;
      private IEditTaskForContainer _editTask;
      private INeighborhoodBuilderToNeighborhoodBuilderDTOMapper _neighborhoodBuilderMapper;
      protected IMoBiApplicationController _applicationController;
      protected NeighborhoodBuilder _neighborhoodBuilder;
      protected SpatialStructure _spatialStructure;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<IEditNeighborhoodBuilderView>();
         _editParameterInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _tagsPresenter = A.Fake<ITagsPresenter>();
         _context = A.Fake<IMoBiContext>();
         _editTask = A.Fake<IEditTaskForContainer>();
         _neighborhoodBuilderMapper = A.Fake<INeighborhoodBuilderToNeighborhoodBuilderDTOMapper>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         sut = new EditNeighborhoodBuilderPresenter(_view, _editParameterInContainerPresenter, _tagsPresenter, _context, _editTask, _neighborhoodBuilderMapper, _applicationController);

         _neighborhoodBuilder = new NeighborhoodBuilder();
         _spatialStructure = new SpatialStructure();

         sut.BuildingBlock = _spatialStructure;
         _commandCollector = new MoBiMacroCommand();
         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_setting_the_first_neighbor_path_into_a_neighborhood_builder : concern_for_EditNeighborhoodBuilderPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_neighborhoodBuilder);
      }

      protected override void Because()
      {
         sut.SetFirstNeighborPath("A|B");
      }

      [Observation]
      public void should_ensure_that_the_corresponding_command_is_executed()
      {
         _commandCollector.All().Count().ShouldBeEqualTo(1);
         _commandCollector.All().ElementAt(0).ShouldBeAnInstanceOf<ChangeFirstNeighborPathCommand>();
      }
   }

   public class When_selecting_the_second_neighbor : concern_for_EditNeighborhoodBuilderPresenter
   {
      private ISelectNeighborPathPresenter _selectNeighborPathPresenter;
      private IModalPresenter _modalPresenter;

      protected override void Context()
      {
         base.Context();
         _selectNeighborPathPresenter = A.Fake<ISelectNeighborPathPresenter>();
         _modalPresenter = A.Fake<IModalPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectNeighborPathPresenter>()).Returns(_selectNeighborPathPresenter);
         A.CallTo(() => _applicationController.Start<IModalPresenter>()).Returns(_modalPresenter);
         A.CallTo(() => _modalPresenter.Show(null)).Returns(true);

         A.CallTo(() => _selectNeighborPathPresenter.NeighborPath).Returns(new ObjectPath("A", "B", "C"));
         sut.Edit(_neighborhoodBuilder);
      }

      protected override void Because()
      {
         sut.SelectSecondNeighbor();
      }

      [Observation]
      public void should_ask_the_user_to_select_a_neighborhood_path()
      {
         A.CallTo(() => _selectNeighborPathPresenter.Init(AppConstants.Captions.SecondNeighbor, A<NeighborhoodObjectPathDTO>._)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_second_neighbor_path()
      {
         _neighborhoodBuilder.SecondNeighborPath.ToString().ShouldBeEqualTo("A|B|C");
      }
   }
}