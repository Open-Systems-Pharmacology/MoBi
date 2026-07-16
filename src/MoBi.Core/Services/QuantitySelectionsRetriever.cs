using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services;

public interface IQuantitySelectionsRetriever
{
   QuantitySelection SelectionFrom(IQuantity quantity);

   void UpdatePersistableOutputsIn(IMoBiSimulation simulation);
}

public class QuantitySelectionsRetriever : IQuantitySelectionsRetriever
{
   private readonly IEntityPathResolver _entityPathResolver;

   public QuantitySelectionsRetriever(IEntityPathResolver entityPathResolver)
   {
      _entityPathResolver = entityPathResolver;
   }

   public QuantitySelection SelectionFrom(IQuantity quantity)
   {
      return new QuantitySelection(_entityPathResolver.PathFor(quantity), quantity.QuantityType);
   }

   public void UpdatePersistableOutputsIn(IMoBiSimulation simulation)
   {
      if (simulation.Settings == null)
         return;

      var allPersistableParameters = simulation.Model.Root.GetAllChildren<IParameter>(x => x.Persistable);
      allPersistableParameters.Each(p => { simulation.OutputSelections.AddOutput(SelectionFrom(p)); });

   }
}