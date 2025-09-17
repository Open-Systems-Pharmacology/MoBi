using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IBuildingBlockWithInitialConditionsPresenter : IExtendablePathAndValueBuildingBlockPresenter<InitialConditionDTO>
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

      void Refresh(IReadOnlyList<InitialConditionDTO> initialConditionDTOs);
      void AsPresent(IReadOnlyList<InitialConditionDTO> initialConditionDTOs);
      void AsNotPresent(IReadOnlyList<InitialConditionDTO> initialConditionDTOs);
      void AllowNegativeValues(IReadOnlyList<InitialConditionDTO> initialConditionDTOs);
      void DoNotAllowNegativeValues(IReadOnlyList<InitialConditionDTO> initialConditionDTOs);
   }

   public interface IBuildingBlockWithInitialConditionsPresenter<TBuildingBlock> : IBuildingBlockWithInitialConditionsPresenter, IEditPresenter<TBuildingBlock>
   {
   }

   public abstract class BuildingBlockWithInitialConditionsPresenter<TView, TPresenter, TBuildingBlock> :
      ExtendablePathAndValueBuildingBlockPresenter<TView, TPresenter, TBuildingBlock, InitialConditionDTO, InitialCondition>, IBuildingBlockWithInitialConditionsPresenter<TBuildingBlock>
      where TView : IInitialConditionsView, IPathAndValueEntitiesView<InitialConditionDTO>, IView<TPresenter>
      where TPresenter : IPresenter
      where TBuildingBlock : class, IBuildingBlock<InitialCondition>
   {
      private readonly IInitialConditionsTask<TBuildingBlock> _initialConditionsTask;

      protected BuildingBlockWithInitialConditionsPresenter(TView view,
         IInitialConditionToInitialConditionDTOMapper dtoMapper,
         IInitialConditionsTask<TBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IDistributedPathAndValueEntityPresenter<InitialConditionDTO, TBuildingBlock> distributedParameterPresenter) :
         base(view, dtoMapper, initialConditionsTask, msvCreator, context, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _initialConditionsTask = initialConditionsTask;
      }

      protected override string RemoveCommandDescription() => AppConstants.Commands.RemoveMultipleInitialConditions;

      public void SetScaleDivisor(InitialConditionDTO dto, double newScaleDivisor) => AddCommand(() => _initialConditionsTask.UpdateInitialConditionScaleDivisor(_buildingBlock, dto.InitialCondition, newScaleDivisor, dto.InitialCondition.ScaleDivisor));

      public void SetIsPresent(InitialConditionDTO dto, bool isPresent) => setIsPresent(new[] { dto.InitialCondition }, isPresent);

      private void setIsPresent(IEnumerable<InitialCondition> startValuesToUpdate, bool isPresent)
      {
         AddCommand(() => _initialConditionsTask.SetIsPresent(_buildingBlock, startValuesToUpdate, isPresent));
         _view.RefreshData();
      }

      public void SetNegativeValuesAllowed(InitialConditionDTO dto, bool negativeValuesAllowed) => setNegativeValuesAllowed(new[] { dto.InitialCondition }, negativeValuesAllowed);

      public void Refresh(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         AddCommand(
            _initialConditionsTask.RefreshInitialConditionsFromBuildingBlocks(
               _buildingBlock,
               initialConditionsFromDTO(initialConditionDTOs)));
      }

      private static List<InitialCondition> initialConditionsFromDTO(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         return initialConditionDTOs.Select(x => x.InitialCondition).ToList();
      }

      public void AsPresent(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         setIsPresent(initialConditionsFromDTO(initialConditionDTOs), isPresent: true);
      }

      public void AsNotPresent(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         setIsPresent(initialConditionsFromDTO(initialConditionDTOs), isPresent: false);
      }

      public void AllowNegativeValues(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         setNegativeValuesAllowed(initialConditionsFromDTO(initialConditionDTOs), negativeValuesAllowed: true);
      }

      public void DoNotAllowNegativeValues(IReadOnlyList<InitialConditionDTO> initialConditionDTOs)
      {
         setNegativeValuesAllowed(initialConditionsFromDTO(initialConditionDTOs), negativeValuesAllowed: false);
      }

      protected void DisablePathColumns() => _view.DisablePathColumns();

      private void setNegativeValuesAllowed(IEnumerable<InitialCondition> startValuesToUpdate, bool negativeValuesAllowed)
      {
         AddCommand(() => _initialConditionsTask.SetNegativeValuesAllowed(_buildingBlock, startValuesToUpdate, negativeValuesAllowed));
         _view.RefreshData();
      }
   }
}