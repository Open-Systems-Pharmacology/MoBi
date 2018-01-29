using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
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
      ISingleStartPresenter<IEventGroupBuildingBlock>,
      IListener<EntitySelectedEvent>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IListener<FavoritesSelectedEvent>,
      IListener<UserDefinedSelectedEvent>

   {
   }

   public class EditEventGroupBuildingBlockPresenter : EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<IEditEventGroupBuildingBlockView, IEditEventGroupBuildingBlockPresenter, IEventGroupBuildingBlock, IEventGroupBuilder>,
      IEditEventGroupBuildingBlockPresenter
   {
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
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
            _editEventGroupPresenter, _eventGroupListPresenter, _editApplicationBuilderPresenter, _favoritesPresenter);
      }

      public override void Edit(IEventGroupBuildingBlock eventToEdit)
      {
         _eventGroupBuildingBlock = eventToEdit;
         _eventGroupListPresenter.Edit(eventToEdit);
         allPresenterImplementing<IPresenterWithFormulaCache>()
            .Each(x => x.BuildingBlock = _eventGroupBuildingBlock);
         setupEditPresenterFor(eventToEdit.FirstOrDefault());
         _favoritesPresenter.Edit(eventToEdit);
         EditFormulas(eventToEdit);
         UpdateCaption();
         _view.Display();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.EventsBuildingBlockCaption(_eventGroupBuildingBlock.Name);
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
            case IApplicationMoleculeBuilder applicationMoleculeBuilder:
               setupEditPresenterFor(applicationMoleculeBuilder.ParentContainer);
               return;
            case IApplicationBuilder applicationBuilder:
               showPresenter(_editApplicationBuilderPresenter, applicationBuilder, parameter);
               return;
            case IEventGroupBuilder eventGroupBuilder:
               showPresenter(_editEventGroupPresenter, eventGroupBuilder, parameter);
               return;
            case IEventBuilder eventBuilder:
               showPresenter(_editEventBuilderPresenter, eventBuilder, parameter);
               return;
            case ITransportBuilder transportBuilder:
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
         _view.SetEditView(presenter.BaseView);

         if (parameter != null)
            presenter.SelectParameter(parameter);
      }

      public override object Subject => _eventGroupBuildingBlock;

      private IEnumerable<T> allPresenterImplementing<T>()
      {
         return AllSubPresenters.OfType<T>();
      }

      protected override Tuple<bool, IObjectBase> SpecificCanHandle(IObjectBase selectedObject)
      {
         return new Tuple<bool, IObjectBase>(shouldShow(selectedObject), selectedObject);
      }

      protected override void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         setupEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(IEventGroupBuilder builder)
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

         if (!objectBase.IsAnImplementationOf<ITransportBuilder>())
            return false;

         return eventGroupContainesTranportBuilder(objectBase);
      }

      private bool eventGroupContainsEntity(IEntity testEntity)
      {
         if (_eventGroupBuildingBlock.Any(eg => eg.Equals(testEntity)))
            return true;

         if (_eventGroupBuildingBlock.Any(eg => eg.GetAllChildren<IEntity>().Contains(testEntity)))
            return true;

         return false;
      }

      private bool eventGroupContainesTranportBuilder(IObjectBase objectBase)
      {
         var tranportBuilder = (ITransportBuilder) objectBase;
         foreach (var eventGroup in _eventGroupBuildingBlock)
         {
            var applicationBuilder = eventGroup as IApplicationBuilder;
            if (applicationBuilder != null && applicationBuilder.Transports.Contains(tranportBuilder))
               return true;

            if (eventGroup.GetAllChildren<IApplicationBuilder>().Any(ab => ab.Transports.Contains(tranportBuilder)))
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
         return addedObject.IsAnImplementationOf<IEventGroupBuilder>()
                || addedObject.IsAnImplementationOf<IEventBuilder>()
                || addedObject.IsAnImplementationOf<IApplicationMoleculeBuilder>()
                || addedObject.IsAnImplementationOf<IContainer>()
                || addedObject.IsAnImplementationOf<ITransportBuilder>();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (!eventToHandle.RemovedObjects.Any(isShowableType))
            return;

         //If only a Application Molecule Builder is removed we do not need to update the edit presenter
         if (eventToHandle.RemovedObjects.Count() != 1 ||
             !eventToHandle.RemovedObjects.First().IsAnImplementationOf<IApplicationMoleculeBuilder>())
         {
            setupEditPresenterFor(_eventGroupBuildingBlock.FirstOrDefault());
         }
      }

      internal override Tuple<bool, IObjectBase> CanHandle(IObjectBase selectedObject)
      {
         var specificCanHandle = SpecificCanHandle(selectedObject);
         if (specificCanHandle.Item1)
            return specificCanHandle;

         return base.CanHandle(selectedObject);
      }

      protected override void ShowView(IView viewToShow)
      {
         _view.SetEditView(viewToShow);
      }
   }
}