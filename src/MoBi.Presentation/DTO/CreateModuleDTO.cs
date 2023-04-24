using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class CreateModuleDTO : ValidatableDTO, IWithProhibitedNames
   {
      private IReadOnlyList<string> _prohibitedNames;

      public CreateModuleDTO()
      {
         Rules.Add(GenericRules.UniqueName);
         Rules.Add(GenericRules.NotEmptyName);
      }

      public void AddForbiddenNames(IReadOnlyList<string> prohibitedNames)
      {
         _prohibitedNames = prohibitedNames.Select(x => x.Trim().ToLower()).ToList();
      }

      public string Name { get; set; }

      public bool IsNameUnique(string newName)
      {
         if (_prohibitedNames == null)
            return true;

         return !_prohibitedNames.Contains(newName.Trim().ToLower());
      }

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