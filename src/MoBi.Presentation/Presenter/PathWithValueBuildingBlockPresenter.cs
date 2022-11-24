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
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.Presenter
{
   public abstract class PathWithValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, TBuilder, TBuilderDTO> : AbstractEditPresenter<TView, TPresenter, TBuildingBlock>
      where TBuildingBlock : IBuildingBlock<TBuilder>
      where TPresenter : IPresenter 
      where TView : IView<TPresenter>
      where TBuilder : class, IObjectBase, IValidatable, IUsingFormula
      where TBuilderDTO: BreadCrumbsDTO<TBuilder>, IWithDisplayUnitDTO, IWithFormulaDTO
   {
      protected TBuildingBlock _buildingBlock;
      private readonly IInteractionTaskForPathAndValueEntity<TBuildingBlock, TBuilder> _interactionTask;
      private readonly IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;

      protected PathWithValueBuildingBlockPresenter(TView view, IInteractionTaskForPathAndValueEntity<TBuildingBlock, TBuilder> interactionTask, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper) : base(view)
      {
         _interactionTask = interactionTask;
         _formulaToValueFormulaDTOMapper = formulaToValueFormulaDTOMapper;
      }

      public IEnumerable<ValueFormulaDTO> AllFormulas()
      {
         var allFormulas = new List<ValueFormulaDTO> {new EmptyFormulaDTO()};

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
   }
}