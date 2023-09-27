using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class PathWithValueBuildingBlockPresenter<TView, TPresenter, TParent, TBuildingBlock, TParameter, TParameterDTO> : AbstractEditPresenter<TView, TPresenter, TBuildingBlock>
      where TBuildingBlock : IBuildingBlock<TParameter>
      where TPresenter : IPresenter
      where TView : IView<TPresenter>
      where TParameter : PathAndValueEntity
      where TParameterDTO : PathAndValueEntityDTO<TParameter>, IWithDisplayUnitDTO, IWithFormulaDTO
   {
      protected TBuildingBlock _buildingBlock;
      private readonly IInteractionTasksForPathAndValueEntity<TParent, TBuildingBlock, TParameter> _interactionTask;
      private readonly IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private readonly IDimensionFactory _dimensionFactory;

      protected PathWithValueBuildingBlockPresenter(TView view, IInteractionTasksForPathAndValueEntity<TParent, TBuildingBlock, TParameter> interactionTask, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory) : base(view)
      {
         _interactionTask = interactionTask;
         _formulaToValueFormulaDTOMapper = formulaToValueFormulaDTOMapper;
         _dimensionFactory = dimensionFactory;
      }

      public IEnumerable<ValueFormulaDTO> AllFormulas()
      {
         var allFormulas = new List<ValueFormulaDTO> { new EmptyFormulaDTO() };

         allFormulas.AddRange(_buildingBlock.FormulaCache.OfType<ExplicitFormula>()
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

      protected void AddNewFormula(TParameterDTO parameterDTO, TParameter builder)
      {
         AddCommand(_interactionTask.AddNewFormulaAtBuildingBlock(_buildingBlock, builder, null));
         RefreshDTO(parameterDTO, builder.Formula, builder);
      }

      public virtual void SetFormula(TParameterDTO parameterDTO, IFormula formula)
      {
         SetFormulaInBuilder(parameterDTO, formula, parameterDTO.PathWithValueObject);
      }

      public virtual void AddNewFormula(TParameterDTO parameterDTO)
      {
         AddNewFormula(parameterDTO, parameterDTO.PathWithValueObject);
      }

      public virtual void SetUnit(TParameterDTO parameterDTO, Unit unit)
      {
         SetUnit(parameterDTO.PathWithValueObject, unit);
      }

      public void SetParameterValue(TParameterDTO parameterDTO, double? newValue)
      {
         AddCommand(_interactionTask.SetValue(_buildingBlock, newValue, parameterDTO.PathWithValueObject));
      }

      public IEnumerable<IDimension> DimensionsSortedByName()
      {
         return _dimensionFactory.DimensionsSortedByName;
      }
   }
}