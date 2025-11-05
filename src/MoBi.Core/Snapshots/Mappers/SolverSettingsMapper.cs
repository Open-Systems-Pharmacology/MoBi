using OSPSuite.Core.Domain;

namespace MoBi.Core.Snapshots.Mappers;

public class SolverSettingsMapper : OSPSuite.Core.Snapshots.Mappers.SolverSettingsMapper
{
   private readonly ISolverSettingsFactory _solverSettingsFactory;

   public SolverSettingsMapper(ISolverSettingsFactory solverSettingsFactory) : base(solverSettingsFactory.CreateCVODE())
   {
      _solverSettingsFactory = solverSettingsFactory;
   }

   protected override SolverSettings CreateDefault() => _solverSettingsFactory.CreateCVODE();
}