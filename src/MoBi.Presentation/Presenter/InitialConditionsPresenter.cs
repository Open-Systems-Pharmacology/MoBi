using System;
using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IInitialConditionsPresenter : IStartValuesPresenter<InitialConditionDTO>,
      IEditPresenter<InitialConditionsBuildingBlock>
   {
      /// <summary>
      ///    Sets the Scale Divisor of the start value to the new value
      /// </summary>
      /// <param name="dto">The dto and start value being modified</param>
      /// <param name="newScaleDivisor">The new value of scale divisor</param>
      void SetScaleDivisor(InitialConditionDTO dto, double newScaleDivisor);

      /// <summary>
      ///    Sets the IsPresent property of the start value <paramref name="dto" /> to <paramref name="isPresent" />
      /// </summary>
      void SetIsPresent(InitialConditionDTO dto, bool isPresent);

      /// <summary>
      ///    Sets the NegativeValuesAllowed property of the start value <paramref name="dto" /> to
      ///    <paramref name="negativeValuesAllowed" />
      /// </summary>
      void SetNegativeValuesAllowed(InitialConditionDTO dto, bool negativeValuesAllowed);
   }

   public class InitialConditionsPresenter : PathAndValueBuildingBlockPresenter<
         IInitialConditionsView,
         IInitialConditionsPresenter,
         InitialConditionsBuildingBlock,
         InitialConditionDTO,
         InitialCondition>,
      IInitialConditionsPresenter
   {
      private readonly IInitialConditionsTask _initialConditionsTask;

      public InitialConditionsPresenter(
         IInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper startValueMapper,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IRefreshStartValueFromOriginalBuildingBlockPresenter refreshStartValuesPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter,
         IInitialConditionsTask initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         ILegendPresenter legendPresenter,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory)
         : base(view, startValueMapper, refreshStartValuesPresenter, initialConditionsTask, msvCreator, context, legendPresenter, deleteStartValuePresenter, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _initialConditionsTask = initialConditionsTask;
         isPresentSelectionPresenter.ApplySelectionAction = performIsPresentAction;
         negativeStartValuesAllowedSelectionPresenter.ApplySelectionAction = performNegativeValuesAllowedAction;
         _view.AddIsPresentSelectionView(isPresentSelectionPresenter.BaseView);
         _view.AddNegativeValuesAllowedSelectionView(negativeStartValuesAllowedSelectionPresenter.BaseView);
      }

      protected override string RemoveCommandDescription()
      {
         return AppConstants.Commands.RemoveMultipleInitialConditions;
      }

      public override void AddNewFormula(InitialConditionDTO initialConditionDTO)
      {
         var pathAndValueEntity = StartValueFrom(initialConditionDTO);
         AddNewFormula(initialConditionDTO, pathAndValueEntity);
      }

      public void SetScaleDivisor(InitialConditionDTO dto, double newScaleDivisor)
      {
         AddCommand(() => _initialConditionsTask.UpdateInitialConditionScaleDivisor(_buildingBlock, dto.InitialCondition, newScaleDivisor, dto.InitialCondition.ScaleDivisor));
      }

      public void SetIsPresent(InitialConditionDTO dto, bool isPresent)
      {
         setIsPresent(new[] { dto.InitialCondition }, isPresent);
      }

      public void SetNegativeValuesAllowed(InitialConditionDTO dto, bool negativeValuesAllowed)
      {
         setNegativeValuesAllowed(new[] { dto.InitialCondition }, negativeValuesAllowed);
      }

      private void setNegativeValuesAllowed(IEnumerable<InitialCondition> startValuesToUpdate, bool negativeValuesAllowed)
      {
         AddCommand(() => _initialConditionsTask.SetNegativeValuesAllowed(_buildingBlock, startValuesToUpdate, negativeValuesAllowed));
         _view.RefreshData();
      }

      private void setIsPresent(IEnumerable<InitialCondition> startValuesToUpdate, bool isPresent)
      {
         AddCommand(() => _initialConditionsTask.SetIsPresent(_buildingBlock, startValuesToUpdate, isPresent));
         _view.RefreshData();
      }

      private void performIsPresentAction(SelectOption option)
      {
         performSetFlagValueAction(setIsPresent, option);
      }

      private void performNegativeValuesAllowedAction(SelectOption option)
      {
         performSetFlagValueAction(setNegativeValuesAllowed, option);
      }

      private void performSetFlagValueAction(Action<IEnumerable<InitialCondition>, bool> selectionAction, SelectOption option)
      {
         if (option.IsOneOf(SelectOption.AllPresent, SelectOption.AllNegativeValuesAllowed))
            selectionAction(VisibleStartValues, true);

         else if (option.IsOneOf(SelectOption.AllNotPresent, SelectOption.AllNegativeValuesNotAllowed))
            selectionAction(VisibleStartValues, false);

         else if (option.IsOneOf(SelectOption.SelectedPresent, SelectOption.SelectedNegativeValuesAllowed))
            selectionAction(SelectedStartValues, true);

         else if (option.IsOneOf(SelectOption.SelectedNotPresent, SelectOption.SelectedNegativeValuesNotAllowed))
            selectionAction(SelectedStartValues, false);
      }
   }
}