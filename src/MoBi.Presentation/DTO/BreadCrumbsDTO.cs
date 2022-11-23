using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public interface IBreadCrumbsDTO
   {
      string PathElement0 { get; }
      string PathElement1 { get; }
      string PathElement2 { get; }
      string PathElement3 { get; }
      string PathElement4 { get; }
      string PathElement5 { get; }
      string PathElement6 { get; }
      string PathElement7 { get; }
      string PathElement8 { get; }
      string PathElement9 { get; }
      string PathElementByIndex(int index);
   }

   public abstract class BreadCrumbsDTO<T> : DxValidatableDTO<T>, IBreadCrumbsDTO where T : IValidatable, INotifier
   {
      private IObjectPath _containerPath;
      private IList<string> _elementList;

      protected BreadCrumbsDTO(T underlyingObject) : base(underlyingObject)
      {
         
      }

      public IObjectPath ContainerPath
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
   }
}