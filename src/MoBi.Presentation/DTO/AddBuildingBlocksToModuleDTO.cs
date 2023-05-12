using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class AddBuildingBlocksToModuleDTO : ModuleContentDTO
   {
      private readonly List<string> _existingParameterValuesNames;
      private readonly List<string> _existingInitialConditionsNames;

      public AddBuildingBlocksToModuleDTO(Module module)
      {
         Name = module.Name;

         CanSelectMolecule = module.Molecules == null;
         WithMolecule = !CanSelectMolecule;

         CanSelectReaction = module.Reactions == null;
         WithReaction = !CanSelectReaction;

         CanSelectSpatialStructure = module.SpatialStructure == null;
         WithSpatialStructure = !CanSelectSpatialStructure;

         CanSelectPassiveTransport = module.PassiveTransports == null;
         WithPassiveTransport = !CanSelectPassiveTransport;

         CanSelectEventGroup = module.EventGroups == null;
         WithEventGroup = !CanSelectEventGroup;

         CanSelectObserver = module.Observers == null;
         WithObserver = !CanSelectObserver;

         _existingParameterValuesNames = new List<string>();
         _existingInitialConditionsNames = new List<string>();

         Rules.AddRange(AllRules.All);
      }

      public override bool WithParameterValues
      {
         get => base.WithParameterValues;
         set
         {
            base.WithParameterValues = value;
            OnPropertyChanged(() => ParameterValuesName);
         }
      }

      public override bool WithInitialConditions
      {
         get => base.WithInitialConditions;
         set
         {
            base.WithInitialConditions = value;
            OnPropertyChanged(() => InitialConditionsName);
         }
      }

      public string ParameterValuesName { get; set; }
      public string InitialConditionsName { get; set; }

      public bool CreateMolecule => WithMolecule && CanSelectMolecule;
      public bool CreateReaction => WithReaction && CanSelectReaction;
      public bool CreateSpatialStructure => WithSpatialStructure && CanSelectSpatialStructure;
      public bool CreatePassiveTransport => WithPassiveTransport && CanSelectPassiveTransport;
      public bool CreateEventGroup => WithEventGroup && CanSelectEventGroup;
      public bool CreateObserver => WithObserver && CanSelectObserver;

      public void AddUsedInitialConditionsNames(IReadOnlyList<string> allNames)
      {
         _existingInitialConditionsNames.AddRange(lowerCaseAndTrim(allNames));
      }

      private static IEnumerable<string> lowerCaseAndTrim(IReadOnlyList<string> stringsToConvert)
      {
         return stringsToConvert.Select(lowerCaseAndTrim);
      }

      private static string lowerCaseAndTrim(string stringToConvert)
      {
         return stringToConvert.ToLower().Trim();
      }

      public void AddUsedParameterValuesNames(IReadOnlyList<string> allNames)
      {
         _existingParameterValuesNames.AddRange(lowerCaseAndTrim(allNames));
      }

      private static class AllRules
      {
         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(x => x.InitialConditionsName)
               .WithRule((dto, name) => dto.isInitialConditionsCreatedAndNotEmptyName(name))
               .WithError(Validation.ValueIsRequired),

            CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(x => x.ParameterValuesName)
               .WithRule((dto, name) => dto.isParameterValuesCreatedAndNotEmptyName(name))
               .WithError(Validation.ValueIsRequired),

            CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(x => x.InitialConditionsName)
               .WithRule((dto, name) => dto.isInitialConditionsCreatedAndNameIsNotInProhibitedNames(name))
               .WithError(AppConstants.Validation.NameAlreadyUsed),

            CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(x => x.ParameterValuesName)
               .WithRule((dto, name) => dto.isParameterValuesCreatedAndNameIsNotInProhibitedNames(name))
               .WithError(AppConstants.Validation.NameAlreadyUsed)
         };
      }

      private bool isParameterValuesCreatedAndNameIsNotInProhibitedNames(string name)
      {
         return !WithParameterValues || nameIsNotInProhibitedNames(name, _existingParameterValuesNames);
      }

      private bool isInitialConditionsCreatedAndNameIsNotInProhibitedNames(string name)
      {
         return !WithInitialConditions || nameIsNotInProhibitedNames(name, _existingInitialConditionsNames);
      }

      private bool isParameterValuesCreatedAndNotEmptyName(string name)
      {
         return !WithParameterValues || name.StringIsNotEmpty();
      }

      private bool isInitialConditionsCreatedAndNotEmptyName(string name)
      {
         return !WithInitialConditions || name.StringIsNotEmpty();
      }

      private bool nameIsNotInProhibitedNames(string name, List<string> prohibitedNames)
      {
         if (string.IsNullOrEmpty(name))
            return true;

         return !prohibitedNames.Contains(lowerCaseAndTrim(name));
      }
   }
}