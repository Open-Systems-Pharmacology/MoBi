using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Services
{
   public interface IOutputSelectionsRetriever
   {
      /// <summary>
      /// Retrieves a new <see cref="OutputSelections"/>  based on the output available in the given <paramref name="simulation"/>. Returns null if the operation was canceled.
      /// </summary>
      /// <remarks>A new instance is returned. The <see cref="OutputSelections"/>  defined in the <paramref name="simulation "/> will not be updated</remarks>
      OutputSelections OutputSelectionsFor(IMoBiSimulation simulation);

      QuantitySelection SelectionFrom(IQuantity quantity);
   }
}