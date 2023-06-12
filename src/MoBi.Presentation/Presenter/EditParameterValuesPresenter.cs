using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterValuesPresenter : ISingleStartPresenter<ParameterValuesBuildingBlock>
   {
      void ExtendParameterValues();
      void AddNewEmptyParameterValue();
   }

   public class EditParameterValuesPresenter : EditBuildingBlockPresenterBase<IEditParameterValuesView, IEditParameterValuesPresenter, ParameterValuesBuildingBlock, ParameterValue>,
      IEditParameterValuesPresenter
   {
      private readonly IParameterValuesPresenter _parameterValuesPresenter;
      private ParameterValuesBuildingBlock _parameterValues;
      private readonly IEditTaskFor<ParameterValuesBuildingBlock> _editTasks;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public EditParameterValuesPresenter(IEditParameterValuesView view, IParameterValuesPresenter parameterValuesPresenter,
         IFormulaCachePresenter formulaCachePresenter, IEditTaskFor<ParameterValuesBuildingBlock> editTasks, IMoBiProjectRetriever projectRetriever)
         : base(view, formulaCachePresenter)
      {
         _parameterValuesPresenter = parameterValuesPresenter;
         _editTasks = editTasks;
         _projectRetriever = projectRetriever;
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
         _view.Caption = AppConstants.Captions.ParameterValuesBuildingBlockCaption(_parameterValues.Caption());
      }

      public override object Subject => _parameterValues;

      public void RenameSubject()
      {
         _editTasks.Rename(_parameterValues, _projectRetriever.Current.All<ParameterValuesBuildingBlock>(), _parameterValues);
      }

      public void ExtendParameterValues()
      {
         _parameterValuesPresenter.ExtendStartValues();
      }

      public void AddNewEmptyParameterValue()
      {
         _parameterValuesPresenter.AddNewEmptyStartValue();
      }
   }
}