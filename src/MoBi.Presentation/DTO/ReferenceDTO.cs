using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ReferenceDTO
   {
      public IFormulaUsablePath Path { get; set; }
      public ParameterBuildMode BuildMode { get; set; }

      public ReferenceDTO()
      {
         BuildMode = ParameterBuildMode.Local;
      }
   }
}