using System.ComponentModel.DataAnnotations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots;
using Classification = OSPSuite.Core.Snapshots.Classification;

namespace MoBi.Core.Snapshots;

public class Project : IWithDescription, IWithName
{
   [Required]
   public int Version { get; set; }
      
   public string Description { get; set; }
   public string Name { get; set; }

   public string[] ExtensionModules { set; get; }
      
   public string[] ExpressionProfileBuildingBlocks { set; get; }
      
   public string[] IndividualBuildingBlocks { set; get; }

   public Classification[] ObservedDataClassifications { get; set; }
   
   public Classification[] SimulationClassifications { get; set; }
   
   public Classification[] ParameterIdentificationClassifications { get; set; }
   
   public Classification[] ModuleClassifications { get; set; }
   
   public DataRepository[] ObservedData { get; set; }
   
   public ParameterIdentification[] ParameterIdentifications { get; set; }
}