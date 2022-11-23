using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
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

      protected PathWithValueBuildingBlockPresenter(TView view, IInteractionTaskForPathAndValueEntity<TBuildingBlock, TBuilder> interactionTask) : base(view)
      {
         _interactionTask = interactionTask;
      }

      public IEnumerable<ValueFormulaDTO> AllFormulas()
      {
         var allFormulas = new List<ValueFormulaDTO> {new EmptyFormulaDTO()};

         allFormulas.AddRange(_buildingBlock.FormulaCache.OfType<ExplicitFormula>()
            .OrderBy(formula => formula.Name)
            .Select(formula => new ValueFormulaDTO(formula)));

         return allFormulas;
      }

      protected virtual void RefreshDTO(TBuilderDTO startValueDTO, IFormula newValue, TBuilder startValue)
      {
         if (newValue != null)
         {
            var explicitFormula = newValue as ExplicitFormula;
            if (explicitFormula != null)
               startValueDTO.Formula = new ValueFormulaDTO(explicitFormula);
         }
         else
         {
            startValueDTO.Formula = new EmptyFormulaDTO();
         }
      }

      protected void SetFormulaInBuilder(TBuilderDTO startValueDTO, IFormula formula, TBuilder startValue)
      {
         AddCommand(_interactionTask.SetFormula(_buildingBlock, startValue, formula));
         RefreshDTO(startValueDTO, formula, startValue);
      }

      public void SetUnit(TBuilder startValue, Unit newUnit)
      {
         AddCommand(_interactionTask.SetUnit(_buildingBlock, startValue, newUnit));
      }

      protected void AddNewFormula(TBuilderDTO moleculeStartValueDTO, TBuilder startValue)
      {
         AddCommand(_interactionTask.AddNewFormulaAtBuildingBlock(_buildingBlock, startValue, null));
         RefreshDTO(moleculeStartValueDTO, startValue.Formula, startValue);
      }
   }
}