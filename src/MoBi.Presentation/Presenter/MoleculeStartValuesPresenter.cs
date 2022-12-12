using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeStartValuesPresenter : IStartValuesPresenter<MoleculeStartValueDTO>,
      IEditPresenter<IMoleculeStartValuesBuildingBlock>
   {
      /// <summary>
      ///    Sets the Scale Divisor of the start value to the new value
      /// </summary>
      /// <param name="dto">The dto and start value being modified</param>
      /// <param name="newScaleDivisor">The new value of scale divisor</param>
      void SetScaleDivisor(MoleculeStartValueDTO dto, double newScaleDivisor);

      /// <summary>
      ///    Sets the IsPresent property of the start value <paramref name="dto" /> to <paramref name="isPresent" />
      /// </summary>
      void SetIsPresent(MoleculeStartValueDTO dto, bool isPresent);

      /// <summary>
      ///    Sets the NegativeValuesAllowed property of the start value <paramref name="dto" /> to
      ///    <paramref name="negativeValuesAllowed" />
      /// </summary>
      void SetNegativeValuesAllowed(MoleculeStartValueDTO dto, bool negativeValuesAllowed);
   }

   public class MoleculeStartValuesPresenter : StartValuePresenter<
      IMoleculeStartValuesView,
      IMoleculeStartValuesPresenter,
      IMoleculeStartValuesBuildingBlock, 
      MoleculeStartValueDTO,
      MoleculeStartValue>,
      IMoleculeStartValuesPresenter
   {
      private readonly IMoleculeStartValuesTask _moleculeStartValuesTask;

      public MoleculeStartValuesPresenter(
         IMoleculeStartValuesView view,
         IMoleculeStartValueToMoleculeStartValueDTOMapper startValueMapper,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IRefreshStartValueFromOriginalBuildingBlockPresenter refreshStartValuesPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter,
         IMoleculeStartValuesTask moleculeStartValuesTask,
         IMoleculeStartValuesCreator msvCreator,
         IMoBiContext context,
         ILegendPresenter legendPresenter,
         IDeleteStartValuePresenter deleteStartValuePresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper)
         : base(view, startValueMapper, refreshStartValuesPresenter, moleculeStartValuesTask, msvCreator, context, legendPresenter, deleteStartValuePresenter, formulaToValueFormulaDTOMapper)
      {
         _moleculeStartValuesTask = moleculeStartValuesTask;
         isPresentSelectionPresenter.ApplySelectionAction = performIsPresentAction;
         negativeStartValuesAllowedSelectionPresenter.ApplySelectionAction = performNegativeValuesAllowedAction;
         _view.AddIsPresentSelectionView(isPresentSelectionPresenter.BaseView);
         _view.AddNegativeValuesAllowedSelectionView(negativeStartValuesAllowedSelectionPresenter.BaseView);
      }

      public override void AddNewFormula(MoleculeStartValueDTO moleculeStartValueDTO)
      {
         var startValue = StartValueFrom(moleculeStartValueDTO);
         AddNewFormula(moleculeStartValueDTO, startValue);
      }

      public void SetScaleDivisor(MoleculeStartValueDTO dto, double newScaleDivisor)
      {
         AddCommand(() => _moleculeStartValuesTask.UpdateStartValueScaleDivisor(_buildingBlock, dto.MoleculeStartValue, newScaleDivisor, dto.MoleculeStartValue.ScaleDivisor));
      }

      public void SetIsPresent(MoleculeStartValueDTO dto, bool isPresent)
      {
         setIsPresent(new[] {dto.MoleculeStartValue}, isPresent);
      }

      public void SetNegativeValuesAllowed(MoleculeStartValueDTO dto, bool negativeValuesAllowed)
      {
         setNegativeValuesAllowed(new[] {dto.MoleculeStartValue}, negativeValuesAllowed);
      }

      private void setNegativeValuesAllowed(IEnumerable<MoleculeStartValue> startValuesToUpdate, bool negativeValuesAllowed)
      {
         AddCommand(() => _moleculeStartValuesTask.SetNegativeValuesAllowed(_buildingBlock, startValuesToUpdate, negativeValuesAllowed));
         _view.RefreshData();
      }

      private void setIsPresent(IEnumerable<MoleculeStartValue> startValuesToUpdate, bool isPresent)
      {
         AddCommand(() => _moleculeStartValuesTask.SetIsPresent(_buildingBlock, startValuesToUpdate, isPresent));
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

      private void performSetFlagValueAction(Action<IEnumerable<MoleculeStartValue>, bool> selectionAction, SelectOption option)
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