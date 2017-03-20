using System;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.DTO
{
   public class SolverSettingsDTO
   {
      public string Name { get; set; }
      public IEnumerable<ISolverOptionDTO> SolverOptions { get; set; }
   }

   public interface ISolverOptionDTO
   {
      string Name { get; }
      string Description { get; }
      string Value { set; get; }
      Type Type { get; }
   }

   public class SolverOptionDTO<T> : ISolverOptionDTO
   {
      private T _value;
      public string Name { get; private set; }
      public string Description { get; private set; }

      public SolverOptionDTO(string name, T value, string description)
      {
         _value = value;
         Name = name;
         Description = description;
      }

      public string Value
      {
         get { return _value.ConvertedTo<string>(); }
         set { _value = value.ConvertedTo<T>(); }
      }

      public Type Type
      {
         get { return typeof(T); }
      }
   }
}
