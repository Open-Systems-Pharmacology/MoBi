using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class IdentificationParameterMapper : OSPSuite.Core.Snapshots.Mappers.IdentificationParameterMapper
{
   public IdentificationParameterMapper(ParameterMapper parameterMapper, IIdentificationParameterFactory identificationParameterFactory, IIdentificationParameterTask identificationParameterTask, IOSPSuiteLogger logger) : base(parameterMapper, identificationParameterFactory, identificationParameterTask, logger)
   {
   }

   protected override bool ShouldExportToSnapshot(IParameter parameter) => true;

   protected override IModelCoreSimulation SimulationByName(ParameterIdentificationContext parameterIdentificationContext, string simulationName) => 
      parameterIdentificationContext.MoBiProject().Simulations.FindByName(simulationName);
}