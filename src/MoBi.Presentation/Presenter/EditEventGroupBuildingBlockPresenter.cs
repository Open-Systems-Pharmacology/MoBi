using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditEventGroupBuildingBlockPresenter :
      ISingleStartPresenter<EventGroupBuildingBlock>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>

   {
   }

   public class EditEventGroupBuildingBlockPresenter : EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<IEditEventGroupBuildingBlockView, IEditEventGroupBuildingBlockPresenter, EventGroupBuildingBlock, EventGroupBuilder>,
      IEditEventGroupBuildingBlockPresenter
   {
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private readonly IEventGroupListPresenter _eventGroupListPresenter;
      private readonly IEditApplicationBuilderPresenter _editApplicationBuilderPresenter;
      private readonly IEditEventGroupPresenter _editEventGroupPresenter;
      private readonly IEditEventBuilderPresenter _editEventBuilderPresenter;
      private readonly IEditTransportBuilderPresenter _editApplicationTransportBuilderPresenter;
      private readonly IEditContainerPresenter _editContainerPresenter;

      public EditEventGroupBuildingBlockPresenter(IEditEventGroupBuildingBlockView view,
         IEventGroupListPresenter eventGroupListPresenter,
         IFormulaCachePresenter formulaCachePresenter,
         IEditApplicationBuilderPresenter editApplicationBuilderPresenter,
         IEditEventGroupPresenter editEventGroupPresenter,
         IEditEventBuilderPresenter editEventBuilderPresenter,
         IEditTransportBuilderPresenter editApplicationTransportBuilderPresenter,
         IEditContainerPresenter editContainerPresenter,
         IEditFavoritesInEventGroupsPresenter favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter)
         : base(view, formulaCachePresenter, favoritesPresenter, userDefinedParametersPresenter)
      {
         _eventGroupListPresenter = eventGroupListPresenter;
         _editContainerPresenter = editContainerPresenter;
         _editApplicationTransportBuilderPresenter = editApplicationTransportBuilderPresenter;
         _editEventBuilderPresenter = editEventBuilderPresenter;
         _editEventGroupPresenter = editEventGroupPresenter;
         _editApplicationBuilderPresenter = editApplicationBuilderPresenter;

         _view.SetListView(_eventGroupListPresenter.BaseView);
         _view.SetEditView(_favoritesPresenter.BaseView);

         _favoritesPresenter.ShouldHandleRemovedEvent = isShowableType;

         AddSubPresenters(_editApplicationTransportBuilderPresenter, _editContainerPresenter, _editEventBuilderPresenter,
            _editEventGroupPresenter, _eventGroupListPresenter, _editApplicationBuilderPresenter);
      }

      public override void Edit(EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         _eventGroupBuildingBlock = eventGroupBuildingBlock;
         _eventGroupListPresenter.Edit(_eventGroupBuildingBlock);

         allPresenterImplementing<IPresenterWithFormulaCache>()
            .Each(x => x.BuildingBlock = _eventGroupBuildingBlock);

         setupEditPresenterFor(_eventGroupBuildingBlock.FirstOrDefault());
         _favoritesPresenter.Edit(_eventGroupBuildingBlock);
         EditFormulas(_eventGroupBuildingBlock);
         UpdateCaption();
         _view.Display();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.EventsBuildingBlockCaption(_eventGroupBuildingBlock.Caption());
      }

      private void setupEditPresenterFor(IObjectBase objectToEdit, IParameter parameter = null)
      {
         if (objectToEdit == null)
         {
            _view.SetEditView(null);
            return;
         }

         switch (objectToEdit)
         {
            case ApplicationMoleculeBuilder applicationMoleculeBuilder:
               setupEditPresenterFor(applicationMoleculeBuilder.ParentContainer);
               return;
            case ApplicationBuilder applicationBuilder:
               showPresenter(_editApplicationBuilderPresenter, applicationBuilder, parameter);
               return;
            case EventGroupBuilder eventGroupBuilder:
               showPresenter(_editEventGroupPresenter, eventGroupBuilder, parameter);
               return;
            case EventBuilder eventBuilder:
               showPresenter(_editEventBuilderPresenter, eventBuilder, parameter);
               return;
            case TransportBuilder transportBuilder:
               showPresenter(_editApplicationTransportBuilderPresenter, transportBuilder, parameter);
               return;
            case IContainer container:
               showPresenter(_editContainerPresenter, container, parameter);
               return;
            default:
               throw new MoBiException(AppConstants.Exceptions.NoEditPresenterFoundFor(objectToEdit));
         }
      }

      private void showPresenter<T>(IEditPresenterWithParameters<T> presenter, T objectToEdit, IParameter parameter)
      {
         presenter.Edit(objectToEdit);
         ShowView(presenter.BaseView);

         if (parameter != null)
            presenter.SelectParameter(parameter);
      }

      public override object Subject => _eventGroupBuildingBlock;

      private IEnumerable<T> allPresenterImplementing<T>()
      {
         return AllSubPresenters.OfType<T>();
      }

      protected override (bool canHandle, IContainer parentObject) SpecificCanHandle(IObjectBase selectedObject)
      {
         return (shouldShow(selectedObject), null);
      }

      protected override void EnsureItemsVisibility(IContainer parentObject, IParameter parameter = null)
      {
         setupEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(EventGroupBuilder builder)
      {
         setupEditPresenterFor(builder);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         var addedObject = eventToHandle.AddedObject;
         if (!shouldShow(addedObject))
            return;

         setupEditPresenterFor(addedObject);
         _eventGroupListPresenter.Edit(_eventGroupBuildingBlock);
         _eventGroupListPresenter.Select(addedObject);
      }

      private bool eventGroupBuildingBlockContains(IObjectBase objectBase)
      {
         if (_eventGroupBuildingBlock.Equals(objectBase))
            return true;

         var testEntity = objectBase as IEntity;
         if (testEntity != null)
            return eventGroupContainsEntity(testEntity);

         if (!objectBase.IsAnImplementationOf<TransportBuilder>())
            return false;

         return eventGroupContainsTransportBuilder(objectBase);
      }

      private bool eventGroupContainsEntity(IEntity testEntity)
      {
         if (_eventGroupBuildingBlock.Any(eg => eg.Equals(testEntity)))
            return true;

         if (_eventGroupBuildingBlock.Any(eg => eg.GetAllChildren<IEntity>().Contains(testEntity)))
            return true;

         return false;
      }

      private bool eventGroupContainsTransportBuilder(IObjectBase objectBase)
      {
         var transportBuilder = (TransportBuilder)objectBase;
         foreach (var eventGroup in _eventGroupBuildingBlock)
         {
            var applicationBuilder = eventGroup as ApplicationBuilder;
            if (applicationBuilder != null && applicationBuilder.Transports.Contains(transportBuilder))
               return true;

            if (eventGroup.GetAllChildren<ApplicationBuilder>().Any(ab => ab.Transports.Contains(transportBuilder)))
               return true;
         }

         return false;
      }

      private bool shouldShow(IObjectBase addedObject)
      {
         if (_eventGroupBuildingBlock == null)
            return false;

         if (isShowableType(addedObject))
            return eventGroupBuildingBlockContains(addedObject);

         return false;
      }

      private bool isShowableType(IObjectBase addedObject)
      {
         return addedObject.IsAnImplementationOf<EventGroupBuilder>()
                || addedObject.IsAnImplementationOf<EventBuilder>()
                || addedObject.IsAnImplementationOf<ApplicationMoleculeBuilder>()
                || addedObject.IsAnImplementationOf<IContainer>()
                || addedObject.IsAnImplementationOf<TransportBuilder>();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (!eventToHandle.RemovedObjects.Any(isShowableType))
            return;

         //If only a Application Molecule Builder is removed we do not need to update the edit presenter
         if (eventToHandle.RemovedObjects.Count() != 1 ||
             !eventToHandle.RemovedObjects.First().IsAnImplementationOf<ApplicationMoleculeBuilder>())
         {
            setupEditPresenterFor(_eventGroupBuildingBlock.FirstOrDefault());
         }
      }

      internal override (bool canHandle, IContainer parentObject) CanHandle(IObjectBase selectedObject)
      {
         var specificCanHandle = SpecificCanHandle(selectedObject);
         if (specificCanHandle.canHandle)
            return specificCanHandle;

         return base.CanHandle(selectedObject);
      }

      protected override void ShowView(IView viewToShow)
      {
         _view.SetEditView(viewToShow);
      }

      protected override Action<IEditParameterListPresenter> ColumnConfiguration() => x => x.ConfigureForEvent();
   }
}