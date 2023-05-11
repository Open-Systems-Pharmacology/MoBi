using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class AddBuildingBlocksToModuleDTO : ModuleContentDTO
   {
      private readonly List<string> _parameterValuesNames;
      private readonly List<string> _initialConditionsNames;

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

         _parameterValuesNames = new List<string>();
         _initialConditionsNames = new List<string>();

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
         _initialConditionsNames.AddRange(allNames.Select(x => x.ToLower().Trim()));
      }

      public void AddUsedParameterValuesNames(IReadOnlyList<string> allNames)
      {
         _parameterValuesNames.AddRange(allNames.Select(x => x.ToLower().Trim()));
      }

      private static class AllRules
      {
         private static IBusinessRule createUniqueNameRule(Expression<Func<AddBuildingBlocksToModuleDTO, string>> propertyToCheck,
            Func<AddBuildingBlocksToModuleDTO, bool> skipValidation,
            Func<AddBuildingBlocksToModuleDTO, List<string>> prohibitedNamesRetriever)
         {
            return CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(propertyToCheck)
               .WithRule((dto, name) => skipValidation(dto) || dto.nameIsNotInProhibitedNames(name, prohibitedNamesRetriever(dto)))
               .WithError(AppConstants.Validation.NameAlreadyUsed);
         }

         private static IBusinessRule createNonEmptyRule(Expression<Func<AddBuildingBlocksToModuleDTO, string>> propertyToCheck, Func<AddBuildingBlocksToModuleDTO, bool> skipValidation)
         {
            return CreateRule.For<AddBuildingBlocksToModuleDTO>()
               .Property(propertyToCheck)
               .WithRule((dto, name) => skipValidation(dto) || name.StringIsNotEmpty())
               .WithError(Validation.ValueIsRequired);
         }

         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            createNonEmptyRule(x => x.InitialConditionsName, dto => !dto.WithInitialConditions),
            createNonEmptyRule(x => x.ParameterValuesName, dto => !dto.WithParameterValues),
            createUniqueNameRule(x => x.ParameterValuesName, dto => !dto.WithParameterValues, dto => dto._parameterValuesNames),
            createUniqueNameRule(x => x.InitialConditionsName, dto => !dto.WithInitialConditions, dto => dto._initialConditionsNames),
         };
      }

      private bool nameIsNotInProhibitedNames(string name, List<string> prohibitedNames)
      {
         if (string.IsNullOrEmpty(name))
            return true;

         return !prohibitedNames.Contains(name.ToLower().Trim());
      }
   }
}