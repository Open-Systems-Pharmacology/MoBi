using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IObserverBuilderListPresenter : IPresenterWithContextMenu<IViewItem>, 
      IEditPresenter<IObserverBuildingBlock>, 
      IListener<AddedEvent>, 
      IListener<RemovedEvent>, 
      IListener<EntitySelectedEvent>,
      IPresenterWithFormulaCache
   {
      void SetFormula(ObserverBuilderDTO dtoObserverBuilder, FormulaBuilderDTO newValue, FormulaBuilderDTO oldValue);
      IReadOnlyList<IDimension> GetDimensions();
      void SetPropertyValueFromViewFor<T>(ObjectBaseDTO dtoObjectBase, string propertyName, T newValue, T oldValue);
      IEditObserverBuildingBlockPresenter Parent { get; set; }
      void Select(ObserverBuilderDTO dto);
   }

   public abstract class ObserverBuilderListPresenterBase : AbstractEditPresenter<IObserverListView, IObserverBuilderListPresenter, IObserverBuildingBlock>, IObserverBuilderListPresenter
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private IObserverBuildingBlock _buildingBlock;
      private readonly IMoBiDimensionFactory _dimensionFactory;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IMoBiContext _context;

      protected ObserverBuilderListPresenterBase(IObserverListView view, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IMoBiDimensionFactory dimensionFactory, IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context) : base(view)
      {
         _context = context;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _dimensionFactory = dimensionFactory;
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         if (objectRequestingPopup == null)
         {
            objectRequestingPopup = GetRootItem();
         }
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      protected abstract IViewItem GetRootItem();

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return _buildingBlock.FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public IBuildingBlock BuildingBlock { get; set; }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void SetFormula(ObserverBuilderDTO dtoObserverBuilder, FormulaBuilderDTO newValue, FormulaBuilderDTO oldValue)
      {
         var newFormula = _buildingBlock.FormulaCache[newValue.Id];
         var oldFormula = _buildingBlock.FormulaCache[oldValue.Id];
         var observerBuilder = _context.Get<IObserverBuilder>(dtoObserverBuilder.Id);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand("Formula", newFormula, oldFormula, observerBuilder, BuildingBlock).Run(_context)); //<IFormula>
      }

      public IReadOnlyList<IDimension> GetDimensions() => _dimensionFactory.DimensionsSortedByName;

      public void SetPropertyValueFromViewFor<T>(ObjectBaseDTO dtoObserverBuilder, string propertyName, T newValue, T oldValue)
      {
         var observerBuilder = _context.Get<IObserverBuilder>(dtoObserverBuilder.Id);
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, observerBuilder, BuildingBlock).Run(_context)); // <T>
      }

      public void Select(ObserverBuilderDTO dto)
      {
         Parent.Select(dto);
      }

      public override void Edit(IObserverBuildingBlock objectToEdit)
      {
         _buildingBlock = objectToEdit;
         BuildingBlock = _buildingBlock;
         _view.Show(GetListToShow(objectToEdit).ToList());
      }

      protected abstract IEnumerable<ObserverBuilderDTO> GetListToShow(IObserverBuildingBlock observerBuildingBlock);

      public override object Subject
      {
         get { return _buildingBlock; }
      }

      public IEditObserverBuildingBlockPresenter Parent { get; set; }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_buildingBlock == null) return;
         this.DoWithinExceptionHandler(() =>
         {
            if (ShouldShow(eventToHandle.AddedObject))
            {
               var dtos = GetListToShow(_buildingBlock);
               _view.Show(dtos);
               var newDTO = (from dto in dtos
                  where dto.Id.Equals(eventToHandle.AddedObject.Id)
                  select dto).SingleOrDefault();
               if (newDTO != null)
               {
                  Select(newDTO);
               }
            }
         });
      }

      protected abstract bool ShouldShow(IObjectBase addedObject);

      /// <summary>
      ///    Handles the specified event to handle.
      /// </summary>
      /// <param name="eventToHandle"> The event to handle. </param>
      public void Handle(RemovedEvent eventToHandle)
      {
         if (_buildingBlock == null) return;
         if (eventToHandle.RemovedObjects.Any(ShouldShow))
         {
            _view.Show(GetListToShow(_buildingBlock));
         }
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (_buildingBlock == null) return;
         var selectedObserver = eventToHandle.ObjectBase as IObserverBuilder;
         if (selectedObserver != null && ShouldShow(selectedObserver))
         {
            SelectObserver(selectedObserver);
         }
      }

      protected abstract void SelectObserver(IObserverBuilder selectedObserver);
   }

   public interface IAmountObserverBuilderListPresenter : IObserverBuilderListPresenter
   {
   }

   public class AmountObserverBuilderListPresenter : ObserverBuilderListPresenterBase, IAmountObserverBuilderListPresenter
   {
      private readonly IObserverBuilderToDTOObserverBuilderMapper _amountObserverBuilderToDTOAmountObserverBuilderMapper;

      public AmountObserverBuilderListPresenter(IObserverListView view, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper, IMoBiDimensionFactory dimensionFactory, IObserverBuilderToDTOObserverBuilderMapper amountObserverBuilderToDTOAmountObserverBuilderMapper, IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context)
         : base(view, formulaToDTOFormulaMapper, dimensionFactory, viewItemContextMenuFactory, context)
      {
         _amountObserverBuilderToDTOAmountObserverBuilderMapper = amountObserverBuilderToDTOAmountObserverBuilderMapper;
      }

      protected override IViewItem GetRootItem()
      {
         return new AmountObserverBuilderRootItem();
      }

      protected override IEnumerable<ObserverBuilderDTO> GetListToShow(IObserverBuildingBlock observerBuildingBlock)
      {
         return observerBuildingBlock.AmountObserverBuilders.MapAllUsing(_amountObserverBuilderToDTOAmountObserverBuilderMapper);
      }

      protected override bool ShouldShow(IObjectBase addedObject)
      {
         return addedObject.IsAnImplementationOf<IAmountObserverBuilder>();
      }

      protected override void SelectObserver(IObserverBuilder selectedObserver)
      {
         var amountObserver = (IAmountObserverBuilder) selectedObserver;
         if (amountObserver == null) return;
         Select(_amountObserverBuilderToDTOAmountObserverBuilderMapper.MapFrom(amountObserver));
      }
   }

   public class AmountObserverBuilderRootItem : IRootViewItem<IAmountObserverBuilder>
   {
   }

   public interface IContainerObserverBuilderListPresenter : IObserverBuilderListPresenter
   {
   }

   public class ContainerObserverBuilderListPresenter : ObserverBuilderListPresenterBase, IContainerObserverBuilderListPresenter
   {
      private readonly IObserverBuilderToDTOObserverBuilderMapper _containerObserverBuilderToDTOContainerObserverBuilderMapper;

      public ContainerObserverBuilderListPresenter(IObserverListView view, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper, IMoBiDimensionFactory dimensionFactory, IObserverBuilderToDTOObserverBuilderMapper containerObserverBuilderToDTOContainerObserverBuilderMapper, IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context)
         : base(view, formulaToDTOFormulaMapper, dimensionFactory, viewItemContextMenuFactory, context)
      {
         _containerObserverBuilderToDTOContainerObserverBuilderMapper = containerObserverBuilderToDTOContainerObserverBuilderMapper;
      }

      protected override IViewItem GetRootItem()
      {
         return new ContainerObserverBuilderRootItem();
      }

      protected override IEnumerable<ObserverBuilderDTO> GetListToShow(IObserverBuildingBlock observerBuildingBlock)
      {
         return observerBuildingBlock.ContainerObserverBuilders.MapAllUsing(_containerObserverBuilderToDTOContainerObserverBuilderMapper);
      }

      protected override bool ShouldShow(IObjectBase addedObject)
      {
         return addedObject.IsAnImplementationOf<IContainerObserverBuilder>();
      }

      protected override void SelectObserver(IObserverBuilder selectedObserver)
      {
         var containerObserver = selectedObserver as ContainerObserverBuilder;
         if (containerObserver == null) return;
         Select(_containerObserverBuilderToDTOContainerObserverBuilderMapper.MapFrom(containerObserver));
      }
   }

   public class ContainerObserverBuilderRootItem : IRootViewItem<IContainerObserverBuilder>
   {
   }

   public class NeighborhoodBuilderRootItem : IRootViewItem<IAmountObserverBuilder>
   {
   }
}