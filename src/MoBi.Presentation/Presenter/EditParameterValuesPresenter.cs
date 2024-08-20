using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterValuesPresenter : ISingleStartPresenter<ParameterValuesBuildingBlock>
   {
      void AddNewParameterValue();
   }

   public class EditParameterValuesPresenter : EditBuildingBlockPresenterBase<IEditParameterValuesView, IEditParameterValuesPresenter, ParameterValuesBuildingBlock, ParameterValue>,
      IEditParameterValuesPresenter
   {
      private readonly IParameterValuesPresenter _parameterValuesPresenter;
      private ParameterValuesBuildingBlock _parameterValues;

      public EditParameterValuesPresenter(IEditParameterValuesView view, IParameterValuesPresenter parameterValuesPresenter,
         IFormulaCachePresenter formulaCachePresenter)
         : base(view, formulaCachePresenter)
      {
         _parameterValuesPresenter = parameterValuesPresenter;
         _view.AddParameterView(_parameterValuesPresenter.BaseView);
         AddSubPresenters(_parameterValuesPresenter);
      }

      public override void Edit(ParameterValuesBuildingBlock parameterValues)
      {
         _parameterValues = parameterValues;
         _parameterValuesPresenter.Edit(parameterValues);
         EditFormulas(parameterValues);
         UpdateCaption();
         _view.Display();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.ParameterValuesBuildingBlockCaption(_parameterValues.DisplayName);
      }

      public override object Subject => _parameterValues;

      public void AddNewParameterValue()
      {
         _parameterValuesPresenter.AddNewParameterValue();
      }
   }
}