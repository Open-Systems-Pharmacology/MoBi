using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditEventGroupBuildingBlockPresenter : ISingleStartPresenter<IEventGroupBuildingBlock>,
      IListener<EntitySelectedEvent>, IListener<AddedEvent>, IListener<RemovedEvent>, IListener<FavoritesSelectedEvent>

   {
   }

   public class EditEventGroupBuildingBlockPresenter :
      EditBuildingBlockPresenterBase
         <IEditEventGroupBuildingBlockView, IEditEventGroupBuildingBlockPresenter, IEventGroupBuildingBlock,
            IEventGroupBuilder>,
      IEditEventGroupBuildingBlockPresenter
   {
      private IEventGroupBuildingBlock _eventGroupBuildingBlock;
      private readonly IEventGroupListPresenter _eventGroupListPresenter;
      private readonly IEditApplicationBuilderPresenter _editApplicationBuilderPresenter;
      private readonly IEditEventGroupPresenter _editEventGroupPresenter;
      private readonly IEditEventBuilderPresenter _editEventBuilderPresenter;
      private readonly IEditTransportBuilderPresenter _editApplicationTransportBuilderPresenter;
      private readonly IEditContainerPresenter _editContainerPresenter;
      private readonly IEditFavoritesInEventGroupsPresenter _editFavoritesPresenter;

      public EditEventGroupBuildingBlockPresenter(IEditEventGroupBuildingBlockView view,
         IEventGroupListPresenter eventGroupListPresenter,
         IFormulaCachePresenter formulaCachePresenter,
         IEditApplicationBuilderPresenter editApplicationBuilderPresenter,
         IEditEventGroupPresenter editEventGroupPresenter,
         IEditEventBuilderPresenter editEventBuilderPresenter,
         IEditTransportBuilderPresenter editApplicationTransportBuilderPresenter,
         IEditContainerPresenter editContainerPresenter, IEditFavoritesInEventGroupsPresenter editFavoritesPresenter)
         : base(view, formulaCachePresenter)
      {
         _eventGroupListPresenter = eventGroupListPresenter;
         _editContainerPresenter = editContainerPresenter;
         _editFavoritesPresenter = editFavoritesPresenter;
         _editApplicationTransportBuilderPresenter = editApplicationTransportBuilderPresenter;
         _editEventBuilderPresenter = editEventBuilderPresenter;
         _editEventGroupPresenter = editEventGroupPresenter;
         _editApplicationBuilderPresenter = editApplicationBuilderPresenter;
         _view.SetListView(_eventGroupListPresenter.BaseView);
         _view.SetEditView(_editFavoritesPresenter.BaseView);
         _editFavoritesPresenter.ShouldHandleRemovedEvent = isShowableType;
         AddSubPresenters(_editApplicationTransportBuilderPresenter, _editContainerPresenter, _editEventBuilderPresenter,
            _editEventGroupPresenter, _eventGroupListPresenter, _editApplicationBuilderPresenter, _editFavoritesPresenter);
      }

      public override void Edit(IEventGroupBuildingBlock eventToEdit)
      {
         _eventGroupBuildingBlock = eventToEdit;
         _eventGroupListPresenter.Edit(eventToEdit);
         allPresenterImplementing<IPresenterWithFormulaCache>()
            .Each(x => x.BuildingBlock = _eventGroupBuildingBlock);
         setUpEditPresenterFor(eventToEdit.FirstOrDefault());
         _editFavoritesPresenter.Edit(eventToEdit);
         EditFormulas(eventToEdit);
         UpdateCaption();
         _view.Display();
      }


      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.EventsBuildingBlockCaption(_eventGroupBuildingBlock.Name);
      }

      private void setUpEditPresenterFor(IObjectBase objectToEdit)
      {
         setUpEditPresenterFor(objectToEdit, null);
      }

      private void setUpEditPresenterFor(IObjectBase objectToEdit, IParameter parameter)
      {
         if (objectToEdit == null)
         {
            _view.SetEditView(null);
            return;
         }

         if (objectToEdit.IsAnImplementationOf<IApplicationMoleculeBuilder>())
         {
            setUpEditPresenterFor(((IApplicationMoleculeBuilder) objectToEdit).ParentContainer);
            return;
         }
         IEditPresenterWithParameters presenter = null;
         if (objectToEdit.IsAnImplementationOf<IApplicationBuilder>())
         {
            presenter = _editApplicationBuilderPresenter;
            _editApplicationBuilderPresenter.Edit((IApplicationBuilder) objectToEdit);
         }
         else if (objectToEdit.IsAnImplementationOf<IEventGroupBuilder>())
         {
            presenter = _editEventGroupPresenter;
            _editEventGroupPresenter.Edit((IEventGroupBuilder) objectToEdit);
         }
         else if (objectToEdit.IsAnImplementationOf<IEventBuilder>())
         {
            presenter = _editEventBuilderPresenter;
            _editEventBuilderPresenter.Edit((IEventBuilder) objectToEdit);
         }
         else if (objectToEdit.IsAnImplementationOf<ITransportBuilder>())
         {
            presenter = _editApplicationTransportBuilderPresenter;
            var transportBuilder = (ITransportBuilder) objectToEdit;
            _editApplicationTransportBuilderPresenter.Edit(transportBuilder);
         }
         else if (objectToEdit.IsAnImplementationOf<IContainer>())
         {
            presenter = _editContainerPresenter;
            _editContainerPresenter.Edit(objectToEdit);
         }

         if (presenter == null)
            throw new MoBiException(AppConstants.Exceptions.NoEditPresenterFoundFor(objectToEdit));
         _view.SetEditView(presenter.BaseView);

         if(parameter!=null)
            presenter.SelectParameter(parameter);
      }

      private IApplicationBuilder getApplicationBuilder(ITransportBuilder transportBuilder)
      {
         foreach (var eventGroup in _eventGroupBuildingBlock)
         {
            var applicationBuilder = eventGroup
               .GetAllContainersAndSelf<IApplicationBuilder>()
               .FirstOrDefault(ab => ab.Transports.Contains(transportBuilder));
            if (applicationBuilder != null)
            {
               return applicationBuilder;
            }
         }
         return null;
      }

      public override object Subject
      {
         get { return _eventGroupBuildingBlock; }
      }

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
         setUpEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(IEventGroupBuilder builder)
      {
         setUpEditPresenterFor(builder);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         var addedObject = eventToHandle.AddedObject;
         if (shouldShow(addedObject))
         {
            setUpEditPresenterFor(addedObject);
            _eventGroupListPresenter.Edit(_eventGroupBuildingBlock);
            _eventGroupListPresenter.Select(addedObject);
         }
      }

      private bool eventGroupBuildingBlockContains(IObjectBase objectBase)
      {
         if (_eventGroupBuildingBlock.Equals(objectBase)) return true;
         var testEntity = objectBase as IEntity;
         if (testEntity != null)
         {
            return eventGroupContainsEntity(testEntity);
         }
         if (!objectBase.IsAnImplementationOf<ITransportBuilder>()) return false;
         return eventGroupContainesTranportBuilder(objectBase);
      }

      private bool eventGroupContainsEntity(IEntity testEntity)
      {
         if (_eventGroupBuildingBlock.Any(eg => eg.Equals(testEntity))) return true;
         if (_eventGroupBuildingBlock.Any(eg => eg.GetAllChildren<IEntity>().Contains(testEntity))) return true;
         return false;
      }

      private bool eventGroupContainesTranportBuilder(IObjectBase objectBase)
      {
         var tranportBuilder = (ITransportBuilder) objectBase;
         foreach (var eventGroup in _eventGroupBuildingBlock)
         {
            var applicationBuilder = eventGroup as IApplicationBuilder;
            if (applicationBuilder != null && applicationBuilder.Transports.Contains(tranportBuilder)) return true;
            if (eventGroup.GetAllChildren<IApplicationBuilder>().Any(ab => ab.Transports.Contains(tranportBuilder)))
               return true;
         }
         return false;
      }

      private bool shouldShow(IObjectBase addedObject)
      {
         if (_eventGroupBuildingBlock == null) return false;
         if (isShowableType(addedObject))
         {
            return eventGroupBuildingBlockContains(addedObject);
         }
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
         if (eventToHandle.RemovedObjects.Any(isShowableType))
         {
            //If only a Application Molecule Builder is removed we do not need to update the edit presenter
            if (eventToHandle.RemovedObjects.Count() != 1 ||
                !eventToHandle.RemovedObjects.First().IsAnImplementationOf<IApplicationMoleculeBuilder>())
            {
               setUpEditPresenterFor(_eventGroupBuildingBlock.FirstOrDefault());
            }
         }
      }

      internal override Tuple<bool, IObjectBase> CanHandle(IObjectBase selectedObject)
      {
         var specificCanHandle = SpecificCanHandle(selectedObject);
         if (specificCanHandle.Item1)
            return specificCanHandle;

         return base.CanHandle(selectedObject);
      }

      public void Handle(FavoritesSelectedEvent eventToHandle)
      {
         if (eventToHandle.ObjectBase.Equals(_eventGroupBuildingBlock))
            _view.SetEditView(_editFavoritesPresenter.BaseView);
      }
   }
}