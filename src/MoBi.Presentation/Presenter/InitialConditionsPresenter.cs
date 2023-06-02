using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IInitialConditionsPresenter : IBuildingBlockWithInitialConditionsPresenter<InitialConditionsBuildingBlock>
   {

   }

   public class InitialConditionsPresenter : BuildingBlockWithInitialConditionsPresenter<
         IInitialConditionsView,
         IInitialConditionsPresenter,
         InitialConditionsBuildingBlock>,
      IInitialConditionsPresenter
   {

      public InitialConditionsPresenter(
         IInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper startValueMapper,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter,
         IInitialConditionsTask initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, startValueMapper, initialConditionsTask, msvCreator, context, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory, isPresentSelectionPresenter, negativeStartValuesAllowedSelectionPresenter)
      {

      }
   }
}