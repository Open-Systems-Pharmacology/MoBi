using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ModuleContentDTO : ObjectBaseDTO, IWithProhibitedNames
   {
      public MergeBehavior DefaultMergeBehavior { get; set; }
      public bool WithReaction { get; set; }
      public bool WithEventGroup { get; set; }
      public bool WithSpatialStructure { get; set; }
      public bool WithPassiveTransport { get; set; }
      public bool WithMolecule { get; set; }
      public bool WithObserver { get; set; }

      private bool _withInitialConditions;

      public virtual bool WithInitialConditions
      {
         get => _withInitialConditions;
         set => SetProperty(ref _withInitialConditions, value);
      }

      public virtual bool WithParameterValues { get; set; }

      public bool CanSelectReaction { get; set; } = true;
      public bool CanSelectEventGroup { get; set; } = true;
      public bool CanSelectSpatialStructure { get; set; } = true;
      public bool CanSelectPassiveTransport { get; set; } = true;
      public bool CanSelectMolecule { get; set; } = true;
      public bool CanSelectObserver { get; set; } = true;
      public bool CanSelectInitialConditions { get; set; } = true;
      public bool CanSelectParameterValues { get; set; } = true;
   }
}