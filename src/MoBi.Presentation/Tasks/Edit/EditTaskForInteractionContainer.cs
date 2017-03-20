using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForInteractionContainer : EditTaskFor<InteractionContainer>
   {
      public EditTaskForInteractionContainer(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      public override bool EditEntityModal(InteractionContainer entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         using (var modalPresenter = GetCreateViewFor(entity, commandCollector))
         {
            InitializeSubPresenter(modalPresenter.SubPresenter, buildingBlock, entity);
            ((ICreatePresenter<IContainer>)modalPresenter.SubPresenter).Edit(entity, existingObjectsInParent);
            return modalPresenter.Show();
         }
      }

      protected override IModalPresenter GetCreateViewFor(InteractionContainer entity, ICommandCollector command)
      {
         // Give Type IContainer here explicitly to use EditPresenterFor<IContainer> 
         //Neede as long Interaction Container needs no own presenter
         return _applicationController.GetCreateViewFor<IContainer>(entity, command);
      }
   }
}