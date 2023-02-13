using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class CreateModuleDTO : ValidatableDTO
   {
      private IReadOnlyList<string> _prohibitedNames;

      public CreateModuleDTO()
      {
         Rules.Add(ModuleCreationRules.UniqueName);
         Rules.Add(ModuleCreationRules.NotEmptyName);
      }

      public void AddForbiddenNames(IReadOnlyList<string> prohibitedNames)
      {
         _prohibitedNames = prohibitedNames;
      }

      public string Name { get; set; }

      public bool WithReaction { get; set; }
      public bool WithEventGroup { get; set; }
      public bool WithSpatialStructure { get; set; }
      public bool WithPassiveTransport { get; set; }
      public bool WithMolecule { get; set; }
      public bool WithObserver { get; set; }
      public bool WithMoleculeStartValues { get; set; }
      public bool WithParameterStartValues { get; set; }

      private bool isNameUnique(string newName)
      {
         if (_prohibitedNames == null)
            return true;

         return !_prohibitedNames.Contains(newName.Trim());
      }
      
      private static class ModuleCreationRules
      {
         public static IBusinessRule NotEmptyName { get; } = CreateRule.For<CreateModuleDTO>()
            .Property(item => item.Name)
            .WithRule((dto, newName) => !string.IsNullOrEmpty(newName))
            .WithError((dto, newName) => AppConstants.Validation.ModuleNameCannotBeEmpty);
         
         public static IBusinessRule UniqueName { get; } = CreateRule.For<CreateModuleDTO>()
            .Property(item => item.Name)
            .WithRule((dto, newName) => dto.isNameUnique(newName))
            .WithError((dto, newName) => Error.NameAlreadyExists(newName));
      }
   }
}
