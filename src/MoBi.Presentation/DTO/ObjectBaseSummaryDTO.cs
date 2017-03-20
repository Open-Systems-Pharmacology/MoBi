using System.Collections.Generic;
using OSPSuite.Assets;

namespace MoBi.Presentation.DTO
{
   public class ObjectBaseSummaryDTO
   {
      private readonly IDictionary<string, string> _dictionary;

      /// <summary>
      ///    The Icon to be used to represent the entity type
      /// </summary>
      public ApplicationIcon ApplicationIcon { get; set; }

      /// <summary>
      ///    The name of this entity
      /// </summary>
      public string EntityName { get; set; }

      /// <summary>
      ///    A collection of key-value pairs indicating a parameter and a value used to describe this entity
      /// </summary>
      public IEnumerable<KeyValuePair<string, string>> Dictionary
      {
         get { return _dictionary; }
      }

      public ObjectBaseSummaryDTO()
      {
         _dictionary = new Dictionary<string, string>();
      }

      /// <summary>
      ///    Adds key-value pairs to the Dictionary
      /// </summary>
      /// <param name="key">The string used as key</param>
      /// <param name="value">The string used as value</param>
      public void AddToDictionary(string key, string value)
      {
         _dictionary.Add(key, value);
      }
   }
}