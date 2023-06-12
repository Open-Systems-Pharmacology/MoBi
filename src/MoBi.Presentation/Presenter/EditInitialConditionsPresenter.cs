using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditInitialConditionsPresenter : ISingleStartPresenter<InitialConditionsBuildingBlock>, IListener<EntitySelectedEvent>
   {
      void ExtendInitialConditions();
      void AddNewEmptyInitialCondition();
   }

   public class EditInitialConditionsPresenter : EditBuildingBlockPresenterBase<IEditInitialConditionsView, IEditInitialConditionsPresenter, InitialConditionsBuildingBlock, InitialCondition>,
      IEditInitialConditionsPresenter
   {
      private readonly IInitialConditionsPresenter _initialConditionsPresenter;
      private InitialConditionsBuildingBlock _initialConditions;

      public EditInitialConditionsPresenter(IEditInitialConditionsView view, IInitialConditionsPresenter initialConditionsPresenter, IFormulaCachePresenter formulaCachePresenter) :
         base(view, formulaCachePresenter)
      {
         _initialConditionsPresenter = initialConditionsPresenter;
         AddSubPresenters(initialConditionsPresenter);
         view.AddInitialConditionsView(_initialConditionsPresenter.BaseView);
      }

      public override void Edit(InitialConditionsBuildingBlock initialConditions)
      {
         if (initialConditions == null) return;
         _initialConditions = initialConditions;
         _initialConditionsPresenter.Edit(_initialConditions);
         EditFormulas(initialConditions);
         UpdateCaption();

         _view.Display();
      }

      public void ExtendInitialConditions()
      {
         _initialConditionsPresenter.ExtendStartValues();
      }

      public void AddNewEmptyInitialCondition()
      {
         _initialConditionsPresenter.AddNewEmptyStartValue();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.InitialConditionsBuildingBlockCaption(_initialConditions.Caption());
      }

      public override object Subject => _initialConditions;
   }
}