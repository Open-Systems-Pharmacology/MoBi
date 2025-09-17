using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Events;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditIndividualParameterPresenter : IPresenter<IEditIndividualParameterView>
   {
      void Edit(IndividualParameter objectToEdit, IndividualBuildingBlock selectedIndividual);
      void NavigateToParameter();
   }

   public class EditIndividualParameterPresenter : AbstractCommandCollectorPresenter<IEditIndividualParameterView, IEditIndividualParameterPresenter>, IEditIndividualParameterPresenter
   {
      private readonly IIndividualParameterToIndividualParameterDTOMapper _individualParameterToIndividualParameterDTOMapper;
      private IndividualParameterDTO _individualParameterDTO;
      private IndividualBuildingBlock _individualBuildingBlock;
      private IndividualParameter _individualParameter;
      private readonly IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;
      private readonly IEditTaskFor<IndividualBuildingBlock> _editTask;
      private readonly IEventPublisher _eventPublisher;

      public EditIndividualParameterPresenter(IEditIndividualParameterView view,
         IIndividualParameterToIndividualParameterDTOMapper individualParameterToIndividualParameterDTOMapper,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper,
         IEditTaskFor<IndividualBuildingBlock> editTask,
         IEventPublisher eventPublisher) : base(view)
      {
         _individualParameterToIndividualParameterDTOMapper = individualParameterToIndividualParameterDTOMapper;
         _pathAndValueEntityToDistributedParameterMapper = pathAndValueEntityToDistributedParameterMapper;
         _editTask = editTask;
         _eventPublisher = eventPublisher;
      }

      public void Edit(IndividualParameter individualParameter, IndividualBuildingBlock buildingBlock)
      {
         _individualBuildingBlock = buildingBlock;
         _individualParameter = individualParameter;
         _individualParameterDTO = _individualParameterToIndividualParameterDTOMapper.MapFrom(individualParameter);
         createDistributionValue();
         _view.BindTo(_individualParameterDTO);
         _view.ShowWarningFor(buildingBlock?.Name);
      }
      
      private void createDistributionValue()
      {
         if (_individualParameter.DistributionType.HasValue)
            _individualParameterDTO.DistributionValue = _pathAndValueEntityToDistributedParameterMapper.MapFrom(_individualParameter, _individualParameter.DistributionType.Value, subParametersFrom(_individualBuildingBlock, _individualParameter)).Value;
      }

      private IReadOnlyList<IndividualParameter> subParametersFrom(IndividualBuildingBlock buildingBlock, IndividualParameter individualParameter)
      {
         return buildingBlock.Where(x => x.ContainerPath.Equals(individualParameter.Path)).ToList();
      }

      public void NavigateToParameter()
      {
         _editTask.Edit(_individualBuildingBlock);
         _eventPublisher.PublishEvent(new EntitySelectedEvent(_individualParameter, this));
      }
   }
}