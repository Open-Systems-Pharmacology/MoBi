using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IPathAndValueBuildingBlockPresenter<in TParameterDTO> : IBreadCrumbsPresenter
   {
      void SetValue(TParameterDTO parameterDTO, double? newValue);
      void SetUnit(TParameterDTO parameterDTO, Unit unit);
      void SetFormula(TParameterDTO parameterDTO, IFormula newValueFormula);
      IEnumerable<ValueFormulaDTO> AllFormulas();
      void EditFormula(TParameterDTO parameterDTO);
      IEnumerable<IDimension> DimensionsSortedByName();
      void SetValueOrigin(TParameterDTO parameterDTO, ValueOrigin valueOrigin);
      void EditDistributedParameter(TParameterDTO distributedParameter);
   }

   public abstract class PathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TParameter, TParameterDTO> : AbstractEditPresenter<TView, TPresenter, TBuildingBlock>, IListener<EntitySelectedEvent>
      where TBuildingBlock : IBuildingBlock<TParameter>
      where TPresenter : IPresenter
      where TView : IView<TPresenter>, IPathAndValueEntitiesView
      where TParameter : PathAndValueEntity
      where TParameterDTO : PathAndValueEntityDTO<TParameter>, IWithDisplayUnitDTO
   {
      protected TBuildingBlock _buildingBlock;
      private readonly IInteractionTasksForPathAndValueEntity<TBuildingBlock, TParameter> _interactionTask;
      private readonly IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> _distributedPathAndValuePresenter;

      protected PathAndValueBuildingBlockPresenter(TView view,
         IInteractionTasksForPathAndValueEntity<TBuildingBlock, TParameter> interactionTask,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> distributedPathAndValuePresenter) : base(view)
      {
         _interactionTask = interactionTask;
         _formulaToValueFormulaDTOMapper = formulaToValueFormulaDTOMapper;
         _dimensionFactory = dimensionFactory;
         _distributedPathAndValuePresenter = distributedPathAndValuePresenter;
         _subPresenterManager.Add(_distributedPathAndValuePresenter);
         _view.AddDistributedParameterView(_distributedPathAndValuePresenter.BaseView);
         _distributedPathAndValuePresenter.ParameterModified += UpdateEntityFor;
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _distributedPathAndValuePresenter.ParameterModified -= UpdateEntityFor;
      }

      protected void UpdateEntityFor(ObjectPath objectPath)
      {
         _view.RefreshForUpdatedEntity();
      }

      public override object Subject => _buildingBlock;

      public override void Edit(TBuildingBlock buildngBlock)
      {
         _buildingBlock = buildngBlock;
      }

      public IEnumerable<ValueFormulaDTO> AllFormulas()
      {
         var allFormulas = new List<ValueFormulaDTO> { new EmptyFormulaDTO() };

         allFormulas.AddRange(_buildingBlock.FormulaCache
            .OrderBy(formula => formula.Name)
            .Select(formula => new ValueFormulaDTO(formula)));

         return allFormulas;
      }

      public void SetValueOrigin(TParameterDTO parameterDTO, ValueOrigin newValueOrigin)
      {
         AddCommand(_interactionTask.SetValueOrigin(_buildingBlock, newValueOrigin, parameterDTO.PathWithValueObject));
      }

      protected virtual void RefreshDTO(TParameterDTO parameterDTO, IFormula newFormula, TParameter builder)
      {
         parameterDTO.Formula = _formulaToValueFormulaDTOMapper.MapFrom(newFormula);
      }

      protected void SetFormulaInBuilder(TParameterDTO parameterDTO, IFormula formula, TParameter builder)
      {
         AddCommand(_interactionTask.SetFormula(_buildingBlock, builder, formula));
         RefreshDTO(parameterDTO, formula, builder);
      }

      public void SetUnit(TParameter builder, Unit newUnit)
      {
         AddCommand(_interactionTask.SetUnit(_buildingBlock, builder, newUnit));
      }

      protected void EditFormula(TParameterDTO parameterDTO, TParameter builder)
      {
         AddCommand(_interactionTask.EditFormulaAtBuildingBlock(_buildingBlock, builder));
         RefreshDTO(parameterDTO, builder.Formula, builder);
      }

      public virtual void SetFormula(TParameterDTO parameterDTO, IFormula formula)
      {
         SetFormulaInBuilder(parameterDTO, formula, parameterDTO.PathWithValueObject);
      }

      public virtual void EditFormula(TParameterDTO parameterDTO)
      {
         EditFormula(parameterDTO, parameterDTO.PathWithValueObject);
      }

      public virtual void SetUnit(TParameterDTO parameterDTO, Unit unit)
      {
         SetUnit(parameterDTO.PathWithValueObject, unit);
      }

      public void SetValue(TParameterDTO parameterDTO, double? newValue)
      {
         AddCommand(_interactionTask.SetValue(_buildingBlock, newValue, parameterDTO.PathWithValueObject));
      }

      public IEnumerable<IDimension> DimensionsSortedByName()
      {
         return _dimensionFactory.DimensionsSortedByName;
      }

      public void EditDistributedParameter(TParameterDTO distributedParameter)
      {
         _distributedPathAndValuePresenter.Edit(distributedParameter, _buildingBlock);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (eventToHandle.ObjectBase is TParameter builder && _buildingBlock.Contains(builder)) 
            SelectEntity(DTOForBuilder(builder));
      }

      protected abstract void SelectEntity(TParameterDTO dto);

      protected abstract TParameterDTO DTOForBuilder(TParameter builder);
   }
}