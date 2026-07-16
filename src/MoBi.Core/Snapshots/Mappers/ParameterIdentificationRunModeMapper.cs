using OSPSuite.Core.Domain.ParameterIdentifications;

namespace MoBi.Core.Snapshots.Mappers;

public class ParameterIdentificationRunModeMapper : OSPSuite.Core.Snapshots.Mappers.ParameterIdentificationRunModeMapper
{
   protected override ParameterIdentificationRunMode RunModeFrom(OSPSuite.Core.Snapshots.ParameterIdentificationRunMode snapshot) => 
      new StandardParameterIdentificationRunMode();
}