
using System.ComponentModel.DataAnnotations;

namespace MoBi.Core.Snapshots;

public class ScaleDivisor
{
   [Required] public string Path { get; set; }

   [Required] public double Value { set; get; }
}