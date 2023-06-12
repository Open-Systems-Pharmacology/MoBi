using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditObserverBuildingBlockPresenter : ISingleStartPresenter<ObserverBuildingBlock>, IListener<RemovedEvent>
   {
      void Select(ObserverBuilderDTO dto);
      void Select(ObserverType observerType);
   }

   public class EditObserverBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditObserverBuildingBlockView, IEditObserverBuildingBlockPresenter, ObserverBuildingBlock, ObserverBuilder>, IEditObserverBuildingBlockPresenter, IListener<EntitySelectedEvent>
   {
      private ObserverBuildingBlock _subject;
      private readonly IAmountObserverBuilderListPresenter _amountObserverBuilderListPresenter;
      private readonly IContainerObserverBuilderListPresenter _containerObserverBuilderListPresenter;
      private IEditObserverBuilderPresenter _editObserverPresenter;
      private readonly IMoBiContext _context;
      private readonly IEditAmountObserverBuilderPresenter _editAmountObserverPresenter;
      private readonly IEditContainerObserverBuilderPresenter _editContainerObserverPresenter;

      public EditObserverBuildingBlockPresenter(IEditObserverBuildingBlockView view, IAmountObserverBuilderListPresenter amountObserverBuilderListPresenter,
         IContainerObserverBuilderListPresenter containerObserverBuilderListPresenter,
         IFormulaCachePresenter formulaCachePresenter, IMoBiContext context,
         IEditAmountObserverBuilderPresenter editAmountObserverPresenter,
         IEditContainerObserverBuilderPresenter editContainerObserverPresenter) :
            base(view, formulaCachePresenter)
      {
         _context = context;
         _editContainerObserverPresenter = editContainerObserverPresenter;
         _editAmountObserverPresenter = editAmountObserverPresenter;
         _containerObserverBuilderListPresenter = containerObserverBuilderListPresenter;
         _amountObserverBuilderListPresenter = amountObserverBuilderListPresenter;
         _view.SetAmountObserverView(_amountObserverBuilderListPresenter.BaseView);
         _view.SetContainerObserverView(_containerObserverBuilderListPresenter.BaseView);
         _amountObserverBuilderListPresenter.Parent = this;
         _containerObserverBuilderListPresenter.Parent = this;
         AddSubPresenters(_editContainerObserverPresenter, _editAmountObserverPresenter,
            _containerObserverBuilderListPresenter, _amountObserverBuilderListPresenter);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.ObserverBuildingBlockCaption(_subject.Caption());
      }

      public override void Edit(ObserverBuildingBlock objectToEdit)
      {
         _subject = objectToEdit;
         _amountObserverBuilderListPresenter.Edit(objectToEdit);
         _containerObserverBuilderListPresenter.Edit(objectToEdit);
         EditFormulas(objectToEdit);
         Select(ObserverType.AmountObserver);
         UpdateCaption();
         _view.Display();
      }

      public override object Subject
      {
         get { return _subject; }
      }

      public void Select(ObserverBuilderDTO dto)
      {
         var selectedObserver = _context.Get<ObserverBuilder>(dto.Id);
         _editObserverPresenter = setUpEditObserverPresenter(selectedObserver);
         _view.SetEditObserverBuilderView(_editObserverPresenter.View);
         _editObserverPresenter.BuildingBlock = _subject;
         _editObserverPresenter.Edit(selectedObserver);
      }

      public void Select(ObserverType observerType)
      {
         var observer = observerBuilderFrom(observerType);
         if (observer == null)
         {
            _view.SetEditObserverBuilderView(null);
            return;
         }

         _editObserverPresenter = setUpEditObserverPresenter(observer);
         _view.SetEditObserverBuilderView(_editObserverPresenter.View);
         _editObserverPresenter.BuildingBlock = _subject;
         _editObserverPresenter.Edit(observer);
      }

      private ObserverBuilder observerBuilderFrom(ObserverType observerType)
      {
         switch (observerType)
         {
            case ObserverType.AmountObserver:
               return _subject.AmountObserverBuilders.FirstOrDefault();
            case ObserverType.ContainerObserver:
               return _subject.ContainerObserverBuilders.FirstOrDefault();
            default:
               throw new ArgumentOutOfRangeException("observerType");
         }
      }

      private IEditObserverBuilderPresenter setUpEditObserverPresenter(ObserverBuilder selectedObserver)
      {
         if (selectedObserver.IsAnImplementationOf<AmountObserverBuilder>())
            return _editAmountObserverPresenter;

         return _editContainerObserverPresenter;
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_editObserverPresenter != null)
         {
            if (eventToHandle.RemovedObjects.Select(objects => objects.Id).Contains(((ObserverBuilder) _editObserverPresenter.Subject).Id))
            {
               Edit(_subject);
            }
         }

         if (eventToHandle.RemovedObjects.Select(objects => objects.Id).Contains(_subject.Id))
         {
            _view.CloseView();
         }
      }
   }
}