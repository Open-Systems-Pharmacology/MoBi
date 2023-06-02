using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionProfileInitialConditionsPresenter : IBuildingBlockWithInitialConditionsPresenter<ExpressionProfileBuildingBlock>
   {
   }

   public class ExpressionProfileInitialConditionsPresenter : BuildingBlockWithInitialConditionsPresenter<IExpressionProfileInitialConditionsView, IExpressionProfileInitialConditionsPresenter, ExpressionProfileBuildingBlock>, IExpressionProfileInitialConditionsPresenter
   {
      public ExpressionProfileInitialConditionsPresenter(
         IExpressionProfileInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper startValueMapper,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter,
         IExpressionProfileInitialConditionsTask initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory) : base(view, startValueMapper, initialConditionsTask, msvCreator, context, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory, isPresentSelectionPresenter, negativeStartValuesAllowedSelectionPresenter)
      {
      }
   }
}