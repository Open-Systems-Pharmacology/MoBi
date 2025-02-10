using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> : IPresenter
   {
      void Edit(TParameterDTO distributedParameterDTO, TBuildingBlock pathAndValueEntities);
      void ConvertToConstantFormula();

      event ParameterModifiedDelegate ParameterModified;
   }

   public delegate void ParameterModifiedDelegate(ObjectPath objectPath);

   public abstract class DistributedPathAndValueEntityPresenter<TView, TPresenter, TParameterDTO, TParameter, TBuildingBlock> : 
      AbstractCommandCollectorPresenter<TView, TPresenter>,
      IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock>
      where TView : IDistributedPathAndValueEntityView<TPresenter, TParameter, TParameterDTO> 
      where TPresenter : IPresenter<TView>
      where TParameterDTO : PathAndValueEntityDTO<TParameter, TParameterDTO> where TParameter : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TParameter>
   {
      private readonly IInteractionTasksForPathAndValueEntity<TBuildingBlock, TParameter> _interactionTasks;
      private TParameterDTO _distributedParameterDTO;
      private TBuildingBlock _buildingBlock;
      

      public event ParameterModifiedDelegate ParameterModified = path => { };

      protected DistributedPathAndValueEntityPresenter(TView view, IInteractionTasksForPathAndValueEntity<TBuildingBlock, TParameter> interactionTasks) : base(view)
      {
         _interactionTasks = interactionTasks;
      }

      public void Edit(TParameterDTO distributedParameterDTO, TBuildingBlock buildingBlock)
      {
         _distributedParameterDTO = distributedParameterDTO;
         _buildingBlock = buildingBlock;
         _view.BindTo(distributedParameterDTO);
      }

      public void ConvertToConstantFormula()
      {
         AddCommand(_interactionTasks.ConvertDistributedParameterToConstantParameter(_distributedParameterDTO.PathWithValueObject, _buildingBlock, _distributedParameterDTO.SubParameters.Select(x => x.PathWithValueObject).ToList()));
         ParameterModified(_distributedParameterDTO.Path);
      }
   }
}
