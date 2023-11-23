using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> : IPresenter
   {
      void SetParameterUnit(TParameterDTO distributionSubParameterDTO, Unit unit);
      void SetParameterValue(TParameterDTO distributionSubParameterDTO, double? newValueInDisplayUnit);
      void Edit(TParameterDTO distributedParameterDTO, TBuildingBlock pathAndValueEntities);
   }

   public abstract class DistributedPathAndValueEntityPresenter<TView, TPresenter, TParameterDTO, TParameter, TBuildingBlock> : 
      AbstractCommandCollectorPresenter<TView, TPresenter>,
      IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock>
      where TView : IDistributedPathAndValueEntityView<TPresenter, TParameter, TParameterDTO> 
      where TPresenter : IPresenter<TView>
      where TParameterDTO : PathAndValueEntityDTO<TParameter, TParameterDTO> where TParameter : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TParameter>
   {
      private readonly IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter> _interactionTasksForBuildingBlock;
      private TBuildingBlock _buildingBlock;

      protected DistributedPathAndValueEntityPresenter(TView view, 
         IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter> interactionTasksForIndividualBuildingBlock) : base(view)
      {
         _interactionTasksForBuildingBlock = interactionTasksForIndividualBuildingBlock;
      }

      public void SetParameterUnit(TParameterDTO distributionSubParameterDTO, Unit unit)
      {
         AddCommand(_interactionTasksForBuildingBlock.SetUnit(_buildingBlock, distributionSubParameterDTO.PathWithValueObject, unit));
      }

      public void Edit(TParameterDTO distributedParameterDTO, TBuildingBlock buildingBlock)
      {
         _buildingBlock = buildingBlock;
         _view.BindTo(distributedParameterDTO);
      }

      public void SetParameterValue(TParameterDTO distributionSubParameterDTO, double? newValueInDisplayUnit)
      {
         AddCommand(_interactionTasksForBuildingBlock.SetValue(_buildingBlock, newValueInDisplayUnit, distributionSubParameterDTO.PathWithValueObject));
      }
   }
}
