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
      public NotifyList<ImportedQuantityDTO> QuantitDTOs { private set; get; }

      public QuantityImporterDTO()
      {
         _log = new List<string>();
         QuantitDTOs = new NotifyList<ImportedQuantityDTO>();
         QuantitDTOs.CollectionChanged += (o, e) => OnPropertyChanged(() => Count);
      }

      public int Count
      {
         get { return QuantitDTOs.Count; }
      }

      public void AddToLog(string message)
      {
         _log.Add(message);
      }

      /// <summary>
      ///    Log of the messages resulting from importing quantity values
      /// </summary>
      public IReadOnlyList<string> Log
      {
         get { return _log; }
      }
   }
}