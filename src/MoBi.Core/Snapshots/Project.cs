using OSPSuite.Core.Domain;
using System.ComponentModel.DataAnnotations;

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
}