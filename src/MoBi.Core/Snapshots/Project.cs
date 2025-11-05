using System.ComponentModel.DataAnnotations;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots;
using Classification = OSPSuite.Core.Snapshots.Classification;

namespace MoBi.Core.Snapshots;

public class Project : SnapshotBase
{
   [Required] 
   public int Version { get; set; }

   public string[] PKSimModules { set; get; }

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

   public string[] ExpressionProfileSnapshots { get; set; }

   public string[] IndividualBuildingBlockSnapshots { get; set; }
}