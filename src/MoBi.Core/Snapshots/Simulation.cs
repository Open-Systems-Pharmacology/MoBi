using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots;
using System.Collections.Generic;
using System.Linq;
using OutputMapping = OSPSuite.Core.Snapshots.OutputMapping;
using OutputSelections = OSPSuite.Core.Snapshots.OutputSelections;

namespace MoBi.Core.Snapshots;

public class Simulation : SnapshotBase
{
   public OutputMapping[] OutputMappings { get; set; }

   public CurveChart Chart { get; set; }
   public SimulationConfiguration Configuration { get; set; }
   public OutputSelections OutputSelections { get; set; }

   public string ParameterIdentificationWorkingDirectory { get; set; }
   public SimulationPredictedVsObservedChart SimulationPredictedVsObservedChart { get; set; }
   public CurveChart SimulationResidualVsTimeChart { get; set; }

   public LocalizedParameter[] Parameters { get; set; }

   public LocalizedParameter ParameterByPath(string parameterPath) =>
      Parameters?.Find(x => string.Equals(x.Path, parameterPath));

   public void AddOrUpdate(LocalizedParameter parameter)
   {
      var existingParameter = ParameterByPath(parameter.Path);
      var localizedParameters = new List<LocalizedParameter>(Parameters ?? Enumerable.Empty<LocalizedParameter>());
      if (existingParameter != null)
         localizedParameters.Remove(existingParameter);

      localizedParameters.Add(parameter);

      Parameters = localizedParameters.ToArray();
   }
}