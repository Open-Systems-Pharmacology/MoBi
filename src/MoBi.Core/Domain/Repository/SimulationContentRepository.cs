using OSPSuite.Utility.Collections;

namespace MoBi.Core.Domain.Repository
{
   /// <summary>
   ///    Holds the compressed serialized content of simulations that were loaded as shells (without a
   ///    constructed model). The content is kept in memory, keyed by simulation Id, so that the model
   ///    can be deserialized on demand the first time <see cref="OSPSuite.Core.Domain.IWithModel.Model" />
   ///    is accessed. The content is intentionally NOT stored on the simulation itself (it is not part of
   ///    the application domain) and is cleared whenever the project changes.
   /// </summary>
   public interface ISimulationContentRepository
   {
      /// <summary>
      ///    Stores the compressed serialized <paramref name="content" /> for the simulation with the given
      ///    <paramref name="simulationId" />.
      /// </summary>
      void Register(string simulationId, byte[] content);

      /// <summary>
      ///    Returns <c>true</c> if deferred content is available for the given <paramref name="simulationId" />.
      /// </summary>
      bool Contains(string simulationId);

      /// <summary>
      ///    Returns the compressed serialized content for the given <paramref name="simulationId" />, or
      ///    <c>null</c> if none is available.
      /// </summary>
      byte[] ContentFor(string simulationId);

      void Clear();
   }

   public class SimulationContentRepository : ISimulationContentRepository
   {
      private readonly Cache<string, byte[]> _contentById = new Cache<string, byte[]>(onMissingKey: _ => null);

      public void Register(string simulationId, byte[] content) => _contentById[simulationId] = content;

      public bool Contains(string simulationId) => _contentById.Contains(simulationId);

      public byte[] ContentFor(string simulationId) => _contentById[simulationId];

      public void Clear() => _contentById.Clear();
   }
}
