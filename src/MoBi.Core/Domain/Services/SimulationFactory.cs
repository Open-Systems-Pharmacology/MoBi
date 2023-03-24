using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Domain.Services
{
   public interface ISimulationFactory
   {
      /// <summary>
      ///    Creates and returns a new <see cref="IMoBiSimulation" /> using the <paramref name="simulationConfiguration" /> and
      ///    <paramref name="model" />
      /// </summary>
      IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model);

      /// <summary>
      ///    Creates and returns a new <see cref="IMoBiSimulation" /> with an empty <see cref="IMoBiBuildConfiguration" />
      /// </summary>
      IMoBiSimulation Create();
   }

   public class SimulationFactory : ISimulationFactory
   {
      private readonly IIdGenerator _idGenerator;
      private readonly ICreationMetaDataFactory _creationMetaDataFactory;
      private readonly ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;
      private readonly IDiagramManagerFactory _diagramManagerFactory;

      public SimulationFactory(IIdGenerator idGenerator, ICreationMetaDataFactory creationMetaDataFactory, ISimulationParameterOriginIdUpdater simulationParameterOriginIdUpdater, IDiagramManagerFactory diagramManagerFactory)
      {
         _idGenerator = idGenerator;
         _creationMetaDataFactory = creationMetaDataFactory;
         _simulationParameterOriginIdUpdater = simulationParameterOriginIdUpdater;
         _diagramManagerFactory = diagramManagerFactory;
      }

      public IMoBiSimulation CreateFrom(SimulationConfiguration simulationConfiguration, IModel model)
      {
         var moBiSimulation = new MoBiSimulation()
         {
            DiagramManager = _diagramManagerFactory.Create<ISimulationDiagramManager>(),
            Configuration = simulationConfiguration,
            Model = model,
            Creation = _creationMetaDataFactory.Create(),
            HasChanged = true,
            Id = _idGenerator.NewId(),
         };

         _simulationParameterOriginIdUpdater.UpdateSimulationId(moBiSimulation);

         return moBiSimulation;
      }

      public IMoBiSimulation Create()
      {
         return CreateFrom(new SimulationConfiguration(), null);
      }
   }
}