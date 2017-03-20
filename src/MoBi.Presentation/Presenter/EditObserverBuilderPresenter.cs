using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
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
   public interface IEditObserverBuilderPresenter : IPresenter<IEditObserverBuilderView>,
      ICanEditPropertiesPresenter,
      IPresenterWithContextMenu<IViewItem>,
      IEditPresenter<IObserverBuilder>,
      IPresenterWithFormulaCache,
      ICreatePresenter<IObserverBuilder>
   {
      IEnumerable<IDimension> AllDimensions();
      void UpdateDimension(IDimension newDimension);
   }

   public abstract class EditObserverBuilderPresenter<TObserverBuilder> : AbstractSubPresenterWithFormula<IEditObserverBuilderView, IEditObserverBuilderPresenter>,
      IEditObserverBuilderPresenter,
      IListener<AddedEvent<IFormula>>
      where TObserverBuilder : class, IObserverBuilder
   {
      protected TObserverBuilder _observerBuilder;
      protected readonly IMoBiContext _context;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IEditTaskFor<TObserverBuilder> _editTasks;
      private readonly IMoleculeDependentBuilderPresenter _moleculeListPresenter;
      private readonly IDescriptorConditionListPresenter<IObserverBuilder> _descriptorConditionListPresenter;
      private IBuildingBlock _buildingBlock;

      protected EditObserverBuilderPresenter(IEditObserverBuilderView view, IEditFormulaPresenter editFormulaPresenter,
         ISelectReferenceAtObserverPresenter selectReferencePresenter, IMoBiContext context,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IEditTaskFor<TObserverBuilder> editTasks,
         IMoleculeDependentBuilderPresenter moleculeListPresenter, IDescriptorConditionListPresenter<IObserverBuilder> descriptorConditionListPresenter) :
            base(view, editFormulaPresenter, selectReferencePresenter)
      {
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _editTasks = editTasks;
         _moleculeListPresenter = moleculeListPresenter;
         _descriptorConditionListPresenter = descriptorConditionListPresenter;
         _context = context;
         _editFormulaPresenter.RemoveFormulaType<ConstantFormula>();
         _editFormulaPresenter.RemoveFormulaType<TableFormula>();
         _view.AddMoleculeListView(_moleculeListPresenter.View);
         _view.AddDescriptorConditionListView(_descriptorConditionListPresenter.View);
         AddSubPresenters(_moleculeListPresenter, _descriptorConditionListPresenter);
      }

      public void Edit(TObserverBuilder observerBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _observerBuilder = observerBuilder;
         _moleculeListPresenter.Edit(observerBuilder);
         setUpFormulaEditView();
         RefreshView(observerBuilder, existingObjectsInParent);
      }

      public void Edit(IObserverBuilder observerBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         Edit(observerBuilder.DowncastTo<TObserverBuilder>(), existingObjectsInParent);
      }

      public void Edit(TObserverBuilder observerBuilder)
      {
         Edit(observerBuilder, observerBuildingBlock);
      }

      public IBuildingBlock BuildingBlock
      {
         get { return _buildingBlock; }
         set
         {
            _buildingBlock = value;
            _moleculeListPresenter.BuildingBlock = _buildingBlock;
         }
      }

      protected void RefreshView(TObserverBuilder observerBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var observerBuilderDTO = MapFrom(observerBuilder);
         observerBuilderDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(observerBuilder, existingObjectsInParent));
         _view.BindTo(observerBuilderDTO);
         _descriptorConditionListPresenter.Edit(observerBuilder, x => x.ContainerCriteria, _buildingBlock);
         FormulaChanged();
      }

      private IObserverBuildingBlock observerBuildingBlock => BuildingBlock as IObserverBuildingBlock;

      protected override void FormulaChanged()
      {
         _view.FormulaHasError = !_editFormulaPresenter.CanClose;
      }

      protected abstract ObserverBuilderDTO MapFrom(TObserverBuilder observerBuilder);

      public IEnumerable<IDimension> AllDimensions()
      {
         return _context.DimensionFactory.Dimensions;
      }

      public void UpdateDimension(IDimension newDimension)
      {
         AddCommand(new UpdateDimensionInObserverBuilderCommand(_observerBuilder, newDimension, BuildingBlock).Run(_context));
         setUpFormulaEditView();
      }

      public void Edit(IObserverBuilder observerBuilder)
      {
         Edit(observerBuilder.DowncastTo<TObserverBuilder>());
      }

      public object Subject => _observerBuilder;

      private void setUpFormulaEditView()
      {
         _referencePresenter.Init(null, new Collection<IObjectBase>(), _observerBuilder);
         _editFormulaPresenter.Init(_observerBuilder, BuildingBlock);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TObserverBuilder>());
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _observerBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_observerBuilder, _observerBuilder.ParentContainer, BuildingBlock);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         yield break;
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void Handle(AddedEvent<IFormula> eventToHandle)
      {
         if (!canHandle(eventToHandle)) return;

         setUpFormulaEditView();
         RefreshView(_observerBuilder, observerBuildingBlock);
      }

      private bool canHandle(AddedEvent eventToHandle)
      {
         return Equals(eventToHandle.Parent, BuildingBlock);
      }
   }
}