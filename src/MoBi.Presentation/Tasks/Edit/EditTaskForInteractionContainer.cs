using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForInteractionContainer : EditTaskFor<InteractionContainer>
   {
      public EditTaskForInteractionContainer(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
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
         // Needed as long Interaction Container needs no own presenter
         return _applicationController.GetCreateViewFor<IContainer>(entity, command);
      }
   }
}