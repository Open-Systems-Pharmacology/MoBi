using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditReactionBuilderPresenter : IEditPresenter<IReactionBuilder>,
      IPresenterWithContextMenu<IViewItem>,
      ICanEditPropertiesPresenter,
      IPresenterWithFormulaCache,
      ICreatePresenter<IReactionBuilder>,
      IListener<RemovedReactionPartnerEvent>,
      IListener<AddedReactionPartnerEvent>,
      IListener<AddedReactionModifierEvent>,
      IListener<RemovedReactionModifierEvent>
   {
      void SetCreateProcessRateParameter(bool createProcessRate);
      void SelectParameter(IParameter parameter);
      void SetPlotProcessRateParameter(bool plotProcessRate);
      bool HasEductsError { get; }
      bool HasProductsError { get; }
   }

   public class EditReactionBuilderPresenter : AbstractSubPresenterWithFormula<IEditReactionBuilderView, IEditReactionBuilderPresenter>, IEditReactionBuilderPresenter
   {
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IReactionBuilderToReactionBuilderDTOMapper _reactionBuilderToReactionBuilderDTOMapper;
      private IReactionBuilder _reactionBuilder;
      private readonly IEditTaskFor<IReactionBuilder> _editTasks;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaBuidlerMapper;
      private readonly IEditParameterListPresenter _editReactionParametersPresenter;
      private readonly IMoBiContext _context;
      private readonly IDescriptorConditionListPresenter<IReactionBuilder> _containerCriteriaPresenter;
      private IBuildingBlock _buildingBlock;
      private readonly IReactionEductsPresenter _reactionEductPresenter;
      private readonly IReactionProductsPresenter _reactionProductPresenter;
      private readonly IReactionModifiersPresenter _reactionModifiersPresenter;

      public EditReactionBuilderPresenter(IEditReactionBuilderView view, IEditFormulaPresenter editFormulaPresenter,
         ISelectReferenceAtReactionPresenter selectReferencesPresenter, IReactionBuilderToReactionBuilderDTOMapper reactionBuilderToReactionBuilderDTOMapper,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IEditTaskFor<IReactionBuilder> editTasks,
         IFormulaToFormulaBuilderDTOMapper formulaBuilderMapper, IEditParameterListPresenter editReactionParametersPresenter, IMoBiContext context, 
         IDescriptorConditionListPresenter<IReactionBuilder> containerCriteriaPresenter, IReactionEductsPresenter reactionEductPresenter, IReactionProductsPresenter reactionProductPresenter,
         IReactionModifiersPresenter reactionModifiersPresenter)
         : base(view, editFormulaPresenter, selectReferencesPresenter)
      {
         _reactionBuilderToReactionBuilderDTOMapper = reactionBuilderToReactionBuilderDTOMapper;
         _context = context;
         _containerCriteriaPresenter = containerCriteriaPresenter;
         _reactionEductPresenter = reactionEductPresenter;
         _reactionProductPresenter = reactionProductPresenter;
         _reactionModifiersPresenter = reactionModifiersPresenter;
         _editReactionParametersPresenter = editReactionParametersPresenter;
         _view.SetParameterView(editReactionParametersPresenter.BaseView);
         _view.SetContainerCriteriaView(_containerCriteriaPresenter.BaseView);
         _view.SetEductView(reactionEductPresenter.View);
         _view.SetProductView(reactionProductPresenter.View);
         _view.SetModifierView(reactionModifiersPresenter.View);
         _editTasks = editTasks;
         _formulaToDTOFormulaBuidlerMapper = formulaBuilderMapper;
         _editFormulaPresenter.SetDefaultFormulaType<ExplicitFormula>();
         _editFormulaPresenter.RemoveFormulaType<TableFormula>();
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         AddSubPresenters(_editReactionParametersPresenter, _containerCriteriaPresenter, _reactionEductPresenter, _reactionProductPresenter, _reactionModifiersPresenter);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<IReactionBuilder>());
      }

      public void Edit(IReactionBuilder reactionBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _reactionBuilder = reactionBuilder;
         if (_reactionBuilder == null)
         {
            _view.Visible = false;
            return;
         }
         setUpFormulaEditView();
         _editReactionParametersPresenter.Edit(_reactionBuilder);
         _containerCriteriaPresenter.Edit(_reactionBuilder, x => x.ContainerCriteria, BuildingBlock);
         var reactionBuilderDTO = _reactionBuilderToReactionBuilderDTOMapper.MapFrom(_reactionBuilder);
         reactionBuilderDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(reactionBuilder, existingObjectsInParent));
         _reactionEductPresenter.Edit(reactionBuilderDTO, BuildingBlock);
         _reactionProductPresenter.Edit(reactionBuilderDTO, BuildingBlock);
         _reactionModifiersPresenter.Edit(reactionBuilderDTO, BuildingBlock);
         _view.BindTo(reactionBuilderDTO);
         _view.Visible = true;
         _view.PlotProcessRateParameterEnabled = _reactionBuilder.CreateProcessRateParameter;
      }

      public void Edit(IReactionBuilder reactionBuilder)
      {
         Edit(reactionBuilder, Enumerable.Empty<IObjectBase>());
      }

      private void setUpFormulaEditView()
      {
         _editFormulaPresenter.Init(_reactionBuilder, BuildingBlock);
         ((ISelectReferenceAtReactionPresenter) _referencePresenter).Init(null, new[] {_reactionBuilder}, _reactionBuilder);
      }

      public object Subject => _reactionBuilder;

      public virtual void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _reactionBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_reactionBuilder, BuildingBlock);
      }

      public void SetCreateProcessRateParameter(bool createProcessRate)
      {
         AddCommand(new SetCreateProcessRateParameterCommand(createProcessRate, _reactionBuilder, BuildingBlock).Run(_context));
         _view.PlotProcessRateParameterEnabled = createProcessRate;
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editReactionParametersPresenter.Select(parameter);
      }

      public void SetPlotProcessRateParameter(bool plotProcessRate)
      {
         _reactionBuilder.ProcessRateParameterPersistable = plotProcessRate;
      }

      public bool HasEductsError => _reactionEductPresenter.HasError;
      public bool HasProductsError => _reactionProductPresenter.HasError;

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.Select(formula => _formulaToDTOFormulaBuidlerMapper.MapFrom(formula));
      }

      public IBuildingBlock BuildingBlock
      {
         get { return _buildingBlock; }
         set
         {
            _buildingBlock = value;
            _editReactionParametersPresenter.BuildingBlock = value;
         }
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void Handle(AddedReactionPartnerEvent eventToHandle)
      {
         if (!canHandle(eventToHandle.Reaction)) return;
         Edit(_reactionBuilder);
      }

      public void Handle(RemovedReactionPartnerEvent eventToHandle)
      {
         if (!canHandle(eventToHandle.Reaction)) return;
         Edit(_reactionBuilder);
      }

      private bool canHandle(IReactionBuilder reactionBuilder)
      {
         if (_reactionBuilder == null) return false;
         return _reactionBuilder.Equals(reactionBuilder);
      }

      public void Handle(AddedReactionModifierEvent eventToHandle)
      {
         if (!canHandle(eventToHandle.Reaction)) return;
         Edit(_reactionBuilder);
      }

      public void Handle(RemovedReactionModifierEvent eventToHandle)
      {
         if (!canHandle(eventToHandle.Reaction)) return;
         Edit(_reactionBuilder);
      }
   }
}