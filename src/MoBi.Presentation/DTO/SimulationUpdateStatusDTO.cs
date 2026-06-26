using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace MoBi.Presentation.DTO;

public class SimulationUpdateStatusDTO
{
   public string SimulationId { get; init; }
   public string Name { get; init; }
   public RunStatus Status { get; set; }

   public ApplicationIcon StatusIcon => Status?.Id == RunStatusId.Created ? ApplicationIcons.Time : ApplicationIcons.IconFor(Status);

   public string StatusDisplay => Status?.Id switch
   {
      RunStatusId.WaitingToRun => AppConstants.Captions.Waiting,
      RunStatusId.Running => AppConstants.Captions.Updating,
      RunStatusId.Created => AppConstants.Captions.Pending,
      RunStatusId.RanToCompletion => AppConstants.Captions.Updated,
      _ => Status?.DisplayName
   };
}