using System;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoBi.Presentation.DTO
{


   public abstract class BreadCrumbsDTO<T> : DxValidatableDTO<T> where T : IValidatable, INotifier
   {
      private ObjectPath _containerPath;
      private IList<string> _elementList;

      protected BreadCrumbsDTO(T underlyingObject) : base(underlyingObject)
      {
         Rules.AddRange(AllRules.All);
      }

      public ObjectPath ContainerPath
      {
         get => _containerPath;
         set
         {
            _containerPath = value;
            _elementList = _containerPath.ToList();
         }
      }

      public string PathElement0 => PathElementByIndex(0);

      public string PathElement1 => PathElementByIndex(1);

      public string PathElement2 => PathElementByIndex(2);

      public string PathElement3 => PathElementByIndex(3);

      public string PathElement4 => PathElementByIndex(4);

      public string PathElement5 => PathElementByIndex(5);

      public string PathElement6 => PathElementByIndex(6);

      public string PathElement7 => PathElementByIndex(7);

      public string PathElement8 => PathElementByIndex(8);

      public string PathElement9 => PathElementByIndex(9);

      public string PathElementByIndex(int index)
      {
         return _elementList.Count > index ? _elementList[index] : string.Empty;
      }

      private static class AllRules
      {
         private static bool elementDoesNotContainWildcardCharacters(string element)
         {
            if (string.IsNullOrEmpty(element))
               return true;

            return !element.Contains(Constants.WILD_CARD);
         }

         private static IBusinessRule elementCannotContainIllegalCharacters(Expression<Func<BreadCrumbsDTO<T>, string>> expression) => CreateRule.For<BreadCrumbsDTO<T>>()
            .Property(expression)
            .WithRule((dto, element) => elementDoesNotContainWildcardCharacters(element))
            .WithError(AppConstants.PathCannotContainWildcardCharacters(new[] { Constants.WILD_CARD }));

         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            elementCannotContainIllegalCharacters(x => x.PathElement0),
            elementCannotContainIllegalCharacters(x => x.PathElement1),
            elementCannotContainIllegalCharacters(x => x.PathElement2),
            elementCannotContainIllegalCharacters(x => x.PathElement3),
            elementCannotContainIllegalCharacters(x => x.PathElement4),
            elementCannotContainIllegalCharacters(x => x.PathElement5),
            elementCannotContainIllegalCharacters(x => x.PathElement6),
            elementCannotContainIllegalCharacters(x => x.PathElement7),
            elementCannotContainIllegalCharacters(x => x.PathElement8),
            elementCannotContainIllegalCharacters(x => x.PathElement9)
         };

      }
   }
}