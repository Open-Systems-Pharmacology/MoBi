using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation.DTO
{
   public class QuantityImporterDTO : ObjectBaseDTO
   {
      private readonly List<string> _log;

      /// <summary>
      ///    The Quantity Values that were imported
      /// </summary>
      public NotifyList<ImportedQuantityDTO> QuantityDTOs { get; }

      public QuantityImporterDTO()
      {
         _log = new List<string>();
         QuantityDTOs = new NotifyList<ImportedQuantityDTO>();
         QuantityDTOs.CollectionChanged += (o, e) => OnPropertyChanged(() => Count);
      }

      public int Count => QuantityDTOs.Count;

      public void AddToLog(string message)
      {
         _log.Add(message);
      }

      /// <summary>
      ///    Log of the messages resulting from importing quantity values
      /// </summary>
      public IReadOnlyList<string> Log => _log;
   }
}