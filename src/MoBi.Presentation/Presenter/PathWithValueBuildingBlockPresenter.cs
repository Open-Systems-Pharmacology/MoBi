using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class PathWithValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TBuilder, TBuilderDTO> : AbstractEditPresenter<TView, TPresenter, TBuildingBlock>
      where TBuildingBlock : IBuildingBlock<TBuilder>
      where TPresenter : IPresenter
      where TView : IView<TPresenter>
      where TBuilder : PathAndValueEntity
      where TBuilderDTO : PathWithValueEntityDTO<TBuilder>, IWithDisplayUnitDTO, IWithFormulaDTO
   {
      protected TBuildingBlock _buildingBlock;
      private readonly IInteractionTasksForPathAndValueEntity<TBuildingBlock, TBuilder> _interactionTask;
      private readonly IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private readonly IDimensionFactory _dimensionFactory;

      protected PathWithValueBuildingBlockPresenter(TView view, IInteractionTasksForPathAndValueEntity<TBuildingBlock, TBuilder> interactionTask, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory) : base(view)
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

      protected virtual void RefreshDTO(TBuilderDTO builderDTO, IFormula newFormula, TBuilder builder)
      {
         builderDTO.Formula = _formulaToValueFormulaDTOMapper.MapFrom(newFormula);
      }

      protected void SetFormulaInBuilder(TBuilderDTO builderDTO, IFormula formula, TBuilder builder)
      {
         AddCommand(_interactionTask.SetFormula(_buildingBlock, builder, formula));
         RefreshDTO(builderDTO, formula, builder);
      }

      public void SetUnit(TBuilder builder, Unit newUnit)
      {
         AddCommand(_interactionTask.SetUnit(_buildingBlock, builder, newUnit));
      }

      protected void AddNewFormula(TBuilderDTO builderDTO, TBuilder builder)
      {
         AddCommand(_interactionTask.AddNewFormulaAtBuildingBlock(_buildingBlock, builder, null));
         RefreshDTO(builderDTO, builder.Formula, builder);
      }

      public virtual void SetFormula(TBuilderDTO expressionParameterDTO, IFormula formula)
      {
         SetFormulaInBuilder(expressionParameterDTO, formula, expressionParameterDTO.PathWithValueObject);
      }

      public virtual void AddNewFormula(TBuilderDTO expressionParameterDTO)
      {
         AddNewFormula(expressionParameterDTO, expressionParameterDTO.PathWithValueObject);
      }

      public virtual void SetUnit(TBuilderDTO expressionParameter, Unit unit)
      {
         SetUnit(expressionParameter.PathWithValueObject, unit);
      }

      public void SetParameterValue(TBuilderDTO parameterDTO, double? newValue)
      {
         AddCommand(_interactionTask.SetValue(_buildingBlock, newValue, parameterDTO.PathWithValueObject));
      }

      public IEnumerable<IDimension> DimensionsSortedByName()
      {
         return _dimensionFactory.DimensionsSortedByName;
      }
   }
}