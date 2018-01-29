using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
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
   public interface IEditTransportBuilderPresenter : IEditPresenterWithParameters<ITransportBuilder>,
      ICanEditPropertiesPresenter, IPresenterWithContextMenu<IViewItem>,
      IPresenterWithFormulaCache
   {
      void SetCreateProcessRateParameter(bool createProccessRate);
      void SetPlotProcessRateParameter(bool plotProcessRate);
   }

   public class EditTransportBuilderPresenter : AbstractSubPresenterWithFormula<IEditTransportBuilderView, IEditTransportBuilderPresenter>, IEditTransportBuilderPresenter
   {
      protected readonly ITransportBuilderToTransportBuilderDTOMapper _transportBuilderToDTOTransportBuilderMapper;
      private ITransportBuilder _transportBuilder;
      protected readonly IEditTaskFor<ITransportBuilder> _editTasks;
      protected readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaBuidlerMapper;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private readonly ISelectReferenceAtTransportPresenter _selectReferencePresenter;
      private readonly IMoBiContext _context;
      private TransportBuilderDTO _transportBuilderDTO;
      private IBuildingBlock _buildingBlock;
      private readonly IMoleculeDependentBuilderPresenter _moleculeListPresenter;
      private readonly IDescriptorConditionListPresenter<ITransportBuilder> _sourceCriteriaPresenter;
      private readonly IDescriptorConditionListPresenter<ITransportBuilder> _targetCriteriaPresenter;

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
         IEditTaskFor<ITransportBuilder> editTasks, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaBuidlerMapper, IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IEditFormulaPresenter editFormulaPresenter, ISelectReferenceAtTransportPresenter selectReferencePresenter, IMoBiContext context,
         IMoleculeDependentBuilderPresenter moleculeListPresenter, IDescriptorConditionListPresenter<ITransportBuilder> sourceCriteriaPresenter,
         IDescriptorConditionListPresenter<ITransportBuilder> targetCriteriaPresenter)
         : base(view, editFormulaPresenter, selectReferencePresenter)
      {
         _transportBuilderToDTOTransportBuilderMapper = transportBuilderToDTOTransportBuilderMapper;
         _context = context;

         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _formulaToDTOFormulaBuidlerMapper = formulaToDTOFormulaBuidlerMapper;
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
         Edit(objectToEdit.DowncastTo<ITransportBuilder>());
      }

      protected override void FormulaChanged()
      {
         _view.FormulaHasError = !_editFormulaPresenter.CanClose;
      }

      public virtual void Edit(ITransportBuilder transportBuilder)
      {
         Edit(transportBuilder, transportBuilder.ParentContainer);
      }

      public void Edit(ITransportBuilder transportBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
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
         _sourceCriteriaPresenter.Edit(_transportBuilder,  x => x.SourceCriteria, _buildingBlock);
         _targetCriteriaPresenter.Edit(_transportBuilder, x => x.TargetCriteria, _buildingBlock);
      }

      private static bool transportIsInPassiveTransportBuildingBlock(ITransportBuilder transportBuilder)
      {
         return transportBuilder.ParentContainer == null;
      }

      private void setUpReferenceView()
      {
         _selectReferencePresenter.Init(null, new[] {_transportBuilder}, _transportBuilder);
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
         AddCommand(editPropertyCommand.Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_transportBuilder, BuildingBlock);
      }

      public void SetCreateProcessRateParameter(bool createProcessRate)
      {
         AddCommand(new SetCreateProcessRateParameterCommand(createProcessRate, _transportBuilder, BuildingBlock).Run(_context));
         _view.EnableDisablePlotProcessRateParameter(createProcessRate);
      }

      public void SetPlotProcessRateParameter(bool plotProcessRate)
      {
         _transportBuilder.ProcessRateParameterPersistable = plotProcessRate;
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParamters();
         _editParametersInContainerPresenter.Select(parameter);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(View, popupLocation);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.Select(formula => _formulaToDTOFormulaBuidlerMapper.MapFrom(formula));
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;
   }
}