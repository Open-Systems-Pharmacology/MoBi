using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class PathAndValueBuildingBlockWithDistributionPresenter<TView, TPresenter, TParent, TBuildingBlock, TParameter, TParameterDTO> :
      PathAndValueBuildingBlockPresenter<TView, TPresenter, TParent, TBuildingBlock, TParameter, TParameterDTO>
      where TBuildingBlock : IBuildingBlock<TParameter>
      where TPresenter : IPresenter
      where TView : IView<TPresenter>, IWithDistributedPathAndValueGridView
      where TParameter : PathAndValueEntity
      where TParameterDTO : PathAndValueEntityDTO<TParameter>, IWithDisplayUnitDTO
   {
      private readonly IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> _distributedPathAndValuePresenter;

      protected PathAndValueBuildingBlockWithDistributionPresenter(TView view, 
         IInteractionTasksForPathAndValueEntity<TParent, TBuildingBlock, TParameter> interactionTask, 
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, 
         IDimensionFactory dimensionFactory, 
         IDistributedPathAndValueEntityPresenter<TParameterDTO, TBuildingBlock> distributedPathAndValuePresenter) :  base(view, interactionTask, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _distributedPathAndValuePresenter = distributedPathAndValuePresenter;
         _subPresenterManager.Add(_distributedPathAndValuePresenter);
         _view.AddDistributedParameterView(_distributedPathAndValuePresenter.BaseView);
      }

      public void EditDistributedParameter(TParameterDTO distributedParameter)
      {
         _distributedPathAndValuePresenter.Edit(distributedParameter, _buildingBlock);
      }
   }
}