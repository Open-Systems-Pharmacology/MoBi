using FakeItEasy;
using FakeItEasy.Core;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectFolderAndIndividualFromProjectPresenter : ContextSpecification<SelectFolderAndIndividualFromProjectPresenter>
   {
      protected IEditTaskForContainer _editTaskForContainer;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectFolderAndIndividualFromProjectView _view;

      protected override void Context()
      {
         _view = A.Fake<ISelectFolderAndIndividualFromProjectView>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _editTaskForContainer = A.Fake<IEditTaskForContainer>();

         sut = new SelectFolderAndIndividualFromProjectPresenter(_view, _buildingBlockRepository, _editTaskForContainer);
      }
   }

   public abstract class When_gathering_the_file_path_and_individual_building_block_from_user : concern_for_SelectFolderAndIndividualFromProjectPresenter
   {
      protected string _filePath;
      protected IndividualBuildingBlock _individualBuildingBlock;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(Cancelled);
         _individualBuildingBlock = new IndividualBuildingBlock();
         _filePath = "FilePath";
         A.CallTo(() => _view.BindTo(A<IndividualAndFilePathDTO>._)).Invokes(x =>
         {
            var dto = x.Arguments.Get<IndividualAndFilePathDTO>(0);
            dto.FilePath = _filePath;
            dto.IndividualBuildingBlock = _individualBuildingBlock;
         });
      }

      public abstract bool Cancelled { get; }

      protected override void Because()
      {
         sut.GetPathAndIndividualForExport("name");
      }
   }

   public class When_the_user_does_not_cancel_the_selection : When_gathering_the_file_path_and_individual_building_block_from_user
   {
      public override bool Cancelled => false;

      [Observation]
      public void the_properties_should_return_selected_values()
      {
         sut.SelectedFilePath.ShouldBeEqualTo(_filePath);
         sut.SelectedIndividual.ShouldBeEqualTo(_individualBuildingBlock);
      }
   }

   public class When_the_user_cancels_the_selection : When_gathering_the_file_path_and_individual_building_block_from_user
   {
      public override bool Cancelled => true;

      [Observation]
      public void the_properties_should_return_null_or_empty()
      {
         sut.SelectedFilePath.ShouldBeEmpty();
         sut.SelectedIndividual.ShouldBeNull();
      }
   }
}
