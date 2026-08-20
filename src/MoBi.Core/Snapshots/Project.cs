using System.ComponentModel.DataAnnotations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots;
using Classification = OSPSuite.Core.Snapshots.Classification;

namespace MoBi.Core.Snapshots;

public class Project : SnapshotBase
{
   [Required]
   public int Version { get; set; }

   public string ApplicationName { get; set; }

   public object[] PKSimModules { set; get; }

   /// <summary>
   ///    The MoBi project names of the modules in <see cref="PKSimModules" />, in the same order. Each PK-Sim snapshot
   ///    carries the name it had in PK-Sim, but the MoBi module may have been renamed on import to avoid a collision (for
   ///    example when importing two simulations that share the same individual). Preserving the MoBi name keeps the module
   ///    resolvable by simulations after the snapshot is loaded. Null for snapshots written before this was introduced.
   /// </summary>
   public string[] PKSimModuleNames { set; get; }

   public string[] ExtensionModules { set; get; }

   public string[] ExpressionProfileBuildingBlocks { set; get; }

   public string[] IndividualBuildingBlocks { set; get; }

   public Classification[] ObservedDataClassifications { get; set; }

   public Classification[] SimulationClassifications { get; set; }

   public Classification[] ParameterIdentificationClassifications { get; set; }

   public Classification[] ModuleClassifications { get; set; }

   public DataRepository[] ObservedData { get; set; }

   public ParameterIdentification[] ParameterIdentifications { get; set; }

   public Simulation[] Simulations { get; set; }

   public ReactionDimensionMode ReactionDimensionMode { get; set; }

   public ExpressionProfileSnapshot[] ExpressionProfileSnapshots { get; set; }

   public IndividualSnapshot[] IndividualBuildingBlockSnapshots { get; set; }
}