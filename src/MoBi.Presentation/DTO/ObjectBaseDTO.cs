using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ObjectBaseDTO : DxValidatableDTO, IWithId, IWithName, IViewItem
   {
      private static readonly string _propertyName = MoBiReflectionHelper.PropertyName<IObjectBase>(x => x.Name);

      public string Name { set; get; }
      public string Description { set; get; }
      public string Id { get; set; }
      public string Icon { get; set; }
      protected readonly List<string> _usedNames;

      public ObjectBaseDTO()
      {
         Rules.AddRange(AllRules.All());
         _usedNames = new List<string>();
         AddUsedNames(AppConstants.UnallowedNames);
      }

      public override string ToString()
      {
         return Name;
      }

      public virtual bool IsNameUnique(string newName)
      {
         if (string.IsNullOrEmpty(newName))
            return true;

         return !_usedNames.Contains(newName.ToLower().Trim());
      }

      public bool IsNameDefined(string name)
      {
         if (string.IsNullOrEmpty(name))
            return false;

         return !name.Trim().IsNullOrEmpty();
      }

      public void AddUsedNames(IEnumerable<string> usedNames)
      {
         _usedNames.AddRange(usedNames.Select(x => x.ToLower()));
      }

      public virtual void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(_propertyName))
         {
            Name = ((IObjectBase)sender).Name;
         }

         RaisePropertyChanged(e.PropertyName);
      }

      private static class AllRules
      {
         private static bool nameDoesNotContainerIllegalCharacters(string name)
         {
            if (string.IsNullOrEmpty(name))
               return true;

            return !Constants.ILLEGAL_CHARACTERS.Any(name.Contains);
         }

         private static IBusinessRule notEmptyNameRule { get; } = CreateRule.For<ObjectBaseDTO>()
            .Property(x => x.Name)
            .WithRule((dto, name) => dto.IsNameDefined(name))
            .WithError(AppConstants.Validation.EmptyName);

         private static IBusinessRule uniqueNameRule { get; } = CreateRule.For<ObjectBaseDTO>()
            .Property(x => x.Name)
            .WithRule((dto, name) => dto.IsNameUnique(name))
            .WithError(AppConstants.Validation.NameAlreadyUsed);

         private static IBusinessRule nameDoesNotContainIllegalCharacters { get; } = CreateRule.For<ObjectBaseDTO>()
            .Property(item => item.Name)
            .WithRule((dto, name) => nameDoesNotContainerIllegalCharacters(name))
            .WithError(Error.NameCannotContainIllegalCharacters(Constants.ILLEGAL_CHARACTERS));

         public static IEnumerable<IBusinessRule> All()
         {
            yield return notEmptyNameRule;
            yield return nameDoesNotContainIllegalCharacters;
            yield return uniqueNameRule;
         }
      }
   }

   public class BuildingBlockViewItem : ObjectBaseDTO
   {
      public IBuildingBlock BuildingBlock { get; private set; }

      public BuildingBlockViewItem(IBuildingBlock buildingBlock)
      {
         BuildingBlock = buildingBlock;
         Id = buildingBlock.Id;
      }
   }

   public class ObservedDataViewItem : IViewItem
   {
      public DataRepository Repository { get; private set; }

      public ObservedDataViewItem(DataRepository repository)
      {
         Repository = repository;
      }
   }

   public class SimulationViewItem : ObjectBaseDTO
   {
      public IMoBiSimulation Simulation { get; private set; }

      public SimulationViewItem(IMoBiSimulation simulation)
      {
         Simulation = simulation;
         Id = simulation.Id;
      }
   }

   public class FavoritesNodeViewItem : ObjectBaseDTO
   {
   }

   public class UserDefinedNodeViewItem : ObjectBaseDTO
   {
   }

   public class BuildingBlockInfoViewItem : ObjectBaseDTO
   {
      public IMoBiSimulation Simulation { get; private set; }
      public IBuildingBlockInfo BuildingBlockInfo { get; private set; }

      public BuildingBlockInfoViewItem(IBuildingBlockInfo buildingBlockInfoInfo, IMoBiSimulation simulation)
      {
         BuildingBlockInfo = buildingBlockInfoInfo;
         Simulation = simulation;
      }
   }

   public class ModuleViewItem : ObjectBaseDTO
   {
      public Module Module { get; private set; }

      public ModuleViewItem(Module module)
      {
         Module = module;
      }
   }
}