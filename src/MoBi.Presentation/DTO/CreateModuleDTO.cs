using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class CreateModuleDTO : ObjectBaseDTO, IWithProhibitedNames
   {
      public bool WithReaction { get; set; }
      public bool WithEventGroup { get; set; }
      public bool WithSpatialStructure { get; set; }
      public bool WithPassiveTransport { get; set; }
      public bool WithMolecule { get; set; }
      public bool WithObserver { get; set; }
      public bool WithMoleculeStartValues { get; set; }
      public bool WithParameterStartValues { get; set; }
   }
}