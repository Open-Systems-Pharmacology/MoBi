using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditExpressionProfileBuildingBlockPresenter : ISingleStartPresenter<ExpressionProfileBuildingBlock>
   {

   }

   public class EditExpressionProfileBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditExpressionProfileBuildingBlockView, IEditExpressionProfileBuildingBlockPresenter, ExpressionProfileBuildingBlock, ExpressionParameter>, IEditExpressionProfileBuildingBlockPresenter
   {
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private readonly IExpressionProfileBuildingBlockPresenter _expressionProfileBuildingBlockPresenter;

      public EditExpressionProfileBuildingBlockPresenter(IEditExpressionProfileBuildingBlockView view, IExpressionProfileBuildingBlockPresenter expressionProfileBuildingBlockPresenter, IFormulaCachePresenter formulaCachePresenter) : base(view, formulaCachePresenter)
      {
         AddSubPresenters(expressionProfileBuildingBlockPresenter);
         view.AddExpressionProfileView(expressionProfileBuildingBlockPresenter.BaseView);
         _expressionProfileBuildingBlockPresenter = expressionProfileBuildingBlockPresenter;
      }

      public override void Edit(object subject)
      {
         Edit(subject as ExpressionProfileBuildingBlock);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.ExpressionProfileBuildingBlockCaption(_expressionProfileBuildingBlock.Name);
      }

      public override object Subject => _expressionProfileBuildingBlock;

      public override void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _expressionProfileBuildingBlock = expressionProfileBuildingBlock;
         _expressionProfileBuildingBlockPresenter.Edit(_expressionProfileBuildingBlock);
         UpdateCaption();
         EditFormulas(_expressionProfileBuildingBlock);
         _view.Display();
      }
   }
}
