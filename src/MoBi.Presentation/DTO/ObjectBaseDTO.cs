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
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ObjectBaseDTO : DxValidatableDTO, IWithName, IWithId, IViewItem
   {
      public IObjectBase ObjectBase { get; }
      private static readonly string _propertyName = MoBiReflectionHelper.PropertyName<IObjectBase>(x => x.Name);

      public string Name { set; get; }
      public string Description { set; get; }
      public string Icon { get; set; }
      public string Id { get; set; }

      protected readonly List<string> _usedNames;

      public ObjectBaseDTO()
      {
         Rules.AddRange(AllRules.All);
         _usedNames = new List<string>();
         AddUsedNames(AppConstants.UnallowedNames);
         //by default, we'll set a random id that might be changed if using a real object underneath the hood
         Id = ShortGuid.NewGuid();
      }

      public ObjectBaseDTO(IObjectBase objectBase) : this()
      {
         ObjectBase = objectBase;
         Id = ObjectBase.Id;
         objectBase.PropertyChanged += HandlePropertyChanged;
      }

      public override string ToString() => Name;

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

      protected virtual void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(_propertyName))
         {
            Name = ObjectBase.Name;
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

         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            notEmptyNameRule,
            nameDoesNotContainIllegalCharacters,
            uniqueNameRule,
         };
      }
   }

   public class BuildingBlockViewItem : ObjectBaseDTO
   {
      public IBuildingBlock BuildingBlock { get; }

      public BuildingBlockViewItem(IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         BuildingBlock = buildingBlock;
      }
   }

   public class ObservedDataViewItem : IViewItem
   {
      public DataRepository Repository { get; }

      public ObservedDataViewItem(DataRepository repository)
      {
         Repository = repository;
      }
   }

   public class SimulationViewItem : ObjectBaseDTO
   {
      public IMoBiSimulation Simulation { get; }

      public SimulationViewItem(IMoBiSimulation simulation) : base(simulation)
      {
         Simulation = simulation;
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
      public IMoBiSimulation Simulation { get; }
      public IBuildingBlockInfo BuildingBlockInfo { get; }

      public BuildingBlockInfoViewItem(IBuildingBlockInfo buildingBlockInfoInfo, IMoBiSimulation simulation)
      {
         BuildingBlockInfo = buildingBlockInfoInfo;
         Simulation = simulation;
      }
   }
}