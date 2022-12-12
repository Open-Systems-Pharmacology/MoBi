using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterStartValuesPresenter : ISingleStartPresenter<IParameterStartValuesBuildingBlock>
   {
      void ExtendStartValues();
      void AddNewEmptyStartValue();
   }

   public class EditParameterStartValuesPresenter : EditBuildingBlockPresenterBase<IEditParameterStartValuesView, IEditParameterStartValuesPresenter, IParameterStartValuesBuildingBlock, ParameterStartValue>,
                                                    IEditParameterStartValuesPresenter
   {
      private readonly IParameterStartValuesPresenter _parameterStartValuesPresenter;
      private IParameterStartValuesBuildingBlock _parameterStartValues;
      private readonly IEditTaskFor<IParameterStartValuesBuildingBlock> _editTasks;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public EditParameterStartValuesPresenter(IEditParameterStartValuesView view, IParameterStartValuesPresenter parameterStartValuesPresenter,
                                               IFormulaCachePresenter formulaCachePresenter, IEditTaskFor<IParameterStartValuesBuildingBlock> editTasks, IMoBiProjectRetriever projectRetriever)
         : base(view, formulaCachePresenter)
      {
         _parameterStartValuesPresenter = parameterStartValuesPresenter;
         _editTasks = editTasks;
         _projectRetriever = projectRetriever;
         _view.AddParameterView(_parameterStartValuesPresenter.BaseView);
         AddSubPresenters(_parameterStartValuesPresenter);
      }

      public override void Edit(IParameterStartValuesBuildingBlock parameterStartValues)
      {
         _parameterStartValues = parameterStartValues;
         _parameterStartValuesPresenter.Edit(parameterStartValues);
         EditFormulas(parameterStartValues);
         UpdateCaption();
         _view.Display();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.ParameterStartValuesBuildingBlockCaption(_parameterStartValues.Name);
      }

      public override object Subject
      {
         get { return _parameterStartValues; }
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_parameterStartValues, _projectRetriever.Current.All<IParameterStartValuesBuildingBlock>(), _parameterStartValues);
      }

      public void ExtendStartValues()
      {
         _parameterStartValuesPresenter.ExtendStartValues();
      }

      public void AddNewEmptyStartValue()
      {
         _parameterStartValuesPresenter.AddNewEmptyStartValue();
      }
   }
}