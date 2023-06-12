using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditIndividualBuildingBlockPresenter : ISingleStartPresenter<IndividualBuildingBlock>
   {
   }

   public class EditIndividualBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditIndividualBuildingBlockView, IEditIndividualBuildingBlockPresenter, IndividualBuildingBlock, IndividualParameter>, IEditIndividualBuildingBlockPresenter
   {
      private IndividualBuildingBlock _individualBuildingBlock;
      private readonly IIndividualBuildingBlockPresenter _individualBuildingBlockPresenter;

      public EditIndividualBuildingBlockPresenter(IEditIndividualBuildingBlockView view, IIndividualBuildingBlockPresenter individualBuildingBlockPresenter, IFormulaCachePresenter formulaCachePresenter) : base(view, formulaCachePresenter)
      {
         AddSubPresenters(individualBuildingBlockPresenter);
         view.AddIndividualView(individualBuildingBlockPresenter.BaseView);
         _individualBuildingBlockPresenter = individualBuildingBlockPresenter;
      }

      public override void Edit(object subject)
      {
         Edit(subject as IndividualBuildingBlock);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.IndividualBuildingBlockCaption(_individualBuildingBlock.Caption());
      }

      public override object Subject => _individualBuildingBlock;

      public override void Edit(IndividualBuildingBlock individualBuildingBlock)
      {
         _individualBuildingBlock = individualBuildingBlock;
         _individualBuildingBlockPresenter.Edit(_individualBuildingBlock);
         UpdateCaption();
         EditFormulas(_individualBuildingBlock);
         _view.Display();
      }
   }
}