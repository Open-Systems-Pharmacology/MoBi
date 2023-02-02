using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForParameter : EditTaskFor<IParameter>
   {
      private readonly ISelectReferencePresenterFactory _selectReferencePresenterFactory;

      public EditTasksForParameter(IInteractionTaskContext interactionTaskContext, ISelectReferencePresenterFactory selectReferencePresenterFactory) : base(interactionTaskContext)
      {
         _selectReferencePresenterFactory = selectReferencePresenterFactory;
      }

      protected override void InitializeSubPresenter(IPresenter subPresenter, IBuildingBlock buildingBlock, IParameter parameter)
      {
         base.InitializeSubPresenter(subPresenter, buildingBlock, parameter);
         var parent = parameter.ParentContainer;
         var editParameterPresenter = subPresenter.DowncastTo<IEditParameterPresenter>();
         editParameterPresenter.ValueReferencesPresenter = _selectReferencePresenterFactory.ReferenceAtParameterFor(parent);
         editParameterPresenter.RhsReferencesPresenter = _selectReferencePresenterFactory.ReferenceAtParameterFor(parent);

         editParameterPresenter.CanSetBuildMode = parent.CanSetBuildModeForParameters();
         editParameterPresenter.ParameterBuildModes = parent.AvailableBuildModeForParameters();
         editParameterPresenter.WarnOnBuildModeChange = false;
      }

      protected override IModalPresenter GetCreateViewFor(IParameter entity, ICommandCollector command)
      {
         return _applicationController.GetCreateParameterViewFor(entity, entity.ParentContainer, command);
      }
   }
}