using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class ObjectBaseSelectionDTO<TObjectBase> : ValidatableDTO
   {
      public ObjectBaseSelectionDTO()
      {
         Rules.AddRange(AllRules.All());
      }

      private TObjectBase _selectedObject;

      public TObjectBase SelectedObject
      {
         get => _selectedObject;
         set
         {
            _selectedObject = value;
            OnPropertyChanged(() => SelectedObject);
         }
      }

      private static class AllRules
      {
         private static IBusinessRule buildingBlockNotNull
         {
            get
            {
               return CreateRule.For<ObjectBaseSelectionDTO<TObjectBase>>()
                  .Property(item => item.SelectedObject)
                  .WithRule((dto, block) => block != null)
                  .WithError((dto, block) => AppConstants.Exceptions.NoBuildingBlockAvailable);
            }
         }

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return buildingBlockNotNull;
         }
      }
   }

   public class ModuleSelectionDTO : ObjectBaseSelectionDTO<Module>
   {

   }
   
   public class BuildingBlockSelectionDTO : ObjectBaseSelectionDTO<IBuildingBlock>
   {

   }
}