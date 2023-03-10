using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ObjectPathDTO : DxValidatableDTO, IViewItem
   {
      private string _path;

      public virtual string Path
      {
         get => _path;
         set => SetProperty(ref _path, value);
      }

      public ObjectPathDTO()
      {
         Rules.AddRange(AllRules.All);
      }

      private static class AllRules
      {
         private static IBusinessRule notEmptyPathRule { get; } = GenericRules.NonEmptyRule<ObjectPathDTO>(x => x.Path, AppConstants.Validation.EmptyPath);

         public static IReadOnlyList<IBusinessRule> All { get; } = new[] {notEmptyPathRule};
      }
   }
}