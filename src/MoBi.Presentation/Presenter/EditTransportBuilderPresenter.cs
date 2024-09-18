using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
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
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTransportBuilderPresenter : IEditPresenterWithParameters<TransportBuilder>,
      ICanEditPropertiesPresenter, IPresenterWithContextMenu<IViewItem>,
      IPresenterWithFormulaCache
   {
      void SetCreateProcessRateParameter(bool createProcessRate);
      void SetPlotProcessRateParameter(bool plotProcessRate);
   }

   public class EditTransportBuilderPresenter : AbstractSubPresenterWithFormula<IEditTransportBuilderView, IEditTransportBuilderPresenter>, IEditTransportBuilderPresenter
   {
      protected readonly ITransportBuilderToTransportBuilderDTOMapper _transportBuilderToDTOTransportBuilderMapper;
      private TransportBuilder _transportBuilder;
      protected readonly IEditTaskFor<TransportBuilder> _editTasks;
      protected readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaBuilderMapper;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private readonly ISelectReferenceAtTransportPresenter _selectReferencePresenter;
      private readonly IMoBiContext _context;
      private TransportBuilderDTO _transportBuilderDTO;
      private IBuildingBlock _buildingBlock;
      private readonly IMoleculeDependentBuilderPresenter _moleculeListPresenter;
      private readonly IDescriptorConditionListPresenter<TransportBuilder> _sourceCriteriaPresenter;
      private readonly IDescriptorConditionListPresenter<TransportBuilder> _targetCriteriaPresenter;

      public virtual IBuildingBlock BuildingBlock
      {
         get => _buildingBlock;
         set
         {
            _buildingBlock = value;
            _moleculeListPresenter.BuildingBlock = value;
         }
      }

      public EditTransportBuilderPresenter(IEditTransportBuilderView view, ITransportBuilderToTransportBuilderDTOMapper transportBuilderToDTOTransportBuilderMapper,
         IEditTaskFor<TransportBuilder> editTasks, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaBuilderMapper, IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IEditFormulaInContainerPresenter editFormulaPresenter, ISelectReferenceAtTransportPresenter selectReferencePresenter, IMoBiContext context,
         IMoleculeDependentBuilderPresenter moleculeListPresenter, IDescriptorConditionListPresenter<TransportBuilder> sourceCriteriaPresenter,
         IDescriptorConditionListPresenter<TransportBuilder> targetCriteriaPresenter)
         : base(view, editFormulaPresenter, selectReferencePresenter)
      {
         _transportBuilderToDTOTransportBuilderMapper = transportBuilderToDTOTransportBuilderMapper;
         _context = context;

         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _formulaToDTOFormulaBuilderMapper = formulaToDTOFormulaBuilderMapper;
         _editTasks = editTasks;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _selectReferencePresenter = selectReferencePresenter;
         editFormulaPresenter.ReferencePresenter = _selectReferencePresenter;
         editFormulaPresenter.RemoveFormulaType<TableFormula>();
         editFormulaPresenter.RemoveFormulaType<TableFormulaWithOffset>();
         editFormulaPresenter.RemoveFormulaType<SumFormula>();
         editFormulaPresenter.SetDefaultFormulaType<ExplicitFormula>();
         View.SetParameterView(_editParametersInContainerPresenter.BaseView);
         View.SetFormulaView(_editFormulaPresenter.BaseView);
         _moleculeListPresenter = moleculeListPresenter;
         _sourceCriteriaPresenter = sourceCriteriaPresenter;
         _targetCriteriaPresenter = targetCriteriaPresenter;
         AddSubPresenters(moleculeListPresenter, _sourceCriteriaPresenter, _targetCriteriaPresenter, editParametersInContainerPresenter);
         _view.AddMoleculeSelectionView(_moleculeListPresenter.View);
         _view.AddSourceCriteriaView(_sourceCriteriaPresenter.View);
         _view.AddTargetCriteriaView(_targetCriteriaPresenter.View);
      }

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<TransportBuilder>());
      }

      protected override void FormulaChanged()
      {
         _view.FormulaHasError = !_editFormulaPresenter.CanClose;
      }

      public virtual void Edit(TransportBuilder transportBuilder)
      {
         Edit(transportBuilder, transportBuilder.ParentContainer?.Children);
      }

      public void Edit(TransportBuilder transportBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _transportBuilder = transportBuilder;
         _transportBuilderDTO = _transportBuilderToDTOTransportBuilderMapper.MapFrom(transportBuilder);
         _transportBuilderDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(transportBuilder, existingObjectsInParent));
         setUpFormulaEditView();
         setUpEditParameterListView();
         setUpReferenceView();
         View.Show(_transportBuilderDTO);
         _view.EnableDisablePlotProcessRateParameter(_transportBuilder.CreateProcessRateParameter);
         FormulaChanged();
         _view.ShowMoleculeList = transportIsInPassiveTransportBuildingBlock(transportBuilder);
         _moleculeListPresenter.Edit(transportBuilder);
         _sourceCriteriaPresenter.Edit(_transportBuilder, x => x.SourceCriteria, _buildingBlock);
         _targetCriteriaPresenter.Edit(_transportBuilder, x => x.TargetCriteria, _buildingBlock);
      }

      private static bool transportIsInPassiveTransportBuildingBlock(TransportBuilder transportBuilder)
      {
         return transportBuilder.ParentContainer == null;
      }

      private void setUpReferenceView()
      {
         _selectReferencePresenter.Init(null, new[] { _transportBuilder }, _transportBuilder);
      }

      public object Subject => _transportBuilder;

      private void setUpEditParameterListView()
      {
         _editParametersInContainerPresenter.BuildingBlock = BuildingBlock;
         _editParametersInContainerPresenter.Edit(_transportBuilder);
      }

      private void setUpFormulaEditView()
      {
         _editFormulaPresenter.Init(_transportBuilder, BuildingBlock);
      }

      public void SetPropertyValueFromView<TProperty>(string propertyName, TProperty newValue, TProperty oldValue)
      {
         var editPropertyCommand = new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _transportBuilder, BuildingBlock); //<TProperty>
         AddCommand(editPropertyCommand.RunCommand(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_transportBuilder, BuildingBlock);
      }

      public void SetCreateProcessRateParameter(bool createProcessRate)
      {
         AddCommand(new SetCreateProcessRateParameterCommand(createProcessRate, _transportBuilder, BuildingBlock).RunCommand(_context));
         _view.EnableDisablePlotProcessRateParameter(createProcessRate);
      }

      public void SetPlotProcessRateParameter(bool plotProcessRate)
      {
         _transportBuilder.ProcessRateParameterPersistable = plotProcessRate;
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editParametersInContainerPresenter.Select(parameter);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(View, popupLocation);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.Select(formula => _formulaToDTOFormulaBuilderMapper.MapFrom(formula));
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;
   }
}