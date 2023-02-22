using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTransporterMoleculeContainerPresenterSpecs : ContextSpecification<IEditTransporterMoleculeContainerPresenter
      >
   {
      protected IEditActiveTransportBuilderContainerView _view;
      protected IEditTasksForTransporterMoleculeContainer _interactionTasks;
      protected IEditParametersInContainerPresenter _editParameterPresenter;
      protected ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper _transporterMoleculeContainerMapper;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _view = A.Fake<IEditActiveTransportBuilderContainerView>();
         _interactionTasks = A.Fake<IEditTasksForTransporterMoleculeContainer>();
         _editParameterPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _transporterMoleculeContainerMapper = A.Fake<ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper>();
         _context = A.Fake<IMoBiContext>();
         sut = new EditTransporterMoleculeContainerPresenter(_view, _interactionTasks, _editParameterPresenter, _transporterMoleculeContainerMapper, _context);
      }
   }

   internal class When_renaming_the_transporter_alias : concern_for_EditTransporterMoleculeContainerPresenterSpecs
   {
      private IBuildingBlock _buildingBlock;
      private TransporterMoleculeContainer _activeTransportBuilderContainer;
      private TransporterMoleculeContainerDTO _dto;

      protected override void Context()
      {
         base.Context();
         _activeTransportBuilderContainer = new TransporterMoleculeContainer();
         _buildingBlock = A.Fake<IMoleculeBuildingBlock>();
         _dto = new TransporterMoleculeContainerDTO();
         A.CallTo(() => _transporterMoleculeContainerMapper.MapFrom(_activeTransportBuilderContainer)).Returns(_dto);
         sut.BuildingBlock = _buildingBlock;
         sut.Edit(_activeTransportBuilderContainer);
      }

      protected override void Because()
      {
         sut.ChangeTransportName();
      }

      [Observation]
      public void should_start_task_for_rename()
      {
         A.CallTo(() => _interactionTasks.ChangeTransportName(_activeTransportBuilderContainer, _buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_view()
      {
         A.CallTo(() => _view.Show(A < TransporterMoleculeContainerDTO>._)).MustHaveHappened();
      }
   }
}