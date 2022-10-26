using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core.Serialization.Services
{
   public interface ISerializationContextFactory
   {
      SerializationContext Create(Type type = null, SerializationContext parentSerializationContext = null);
   }

   public class SerializationContextFactory : ISerializationContextFactory
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IContainer _container;

      public SerializationContextFactory(
         IDimensionFactory dimensionFactory,
         IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel,
         IContainer container
      )
      {
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
         _container = container;
      }

      public SerializationContext Create(Type type = null, SerializationContext parentSerializationContext = null)
      {
         var projectRetriever = _container.Resolve<IMoBiProjectRetriever>();
         var project = projectRetriever.Current;

         var idRepository = new WithIdRepository();
         var allRepositories = new List<DataRepository>();

         if (parentSerializationContext != null)
         {
            allRepositories.AddRange(parentSerializationContext.Repositories);
            parentSerializationContext.IdRepository.All().Each(idRepository.Register);
         }

         //We only registers existing simulation if we are not deserializing a whole Simulation.
         //Otherwise we may get conflicts when loading an existing simulation again
         var shouldRegisterSimulations = type != typeof(SimulationTransfer);

         //if project is defined, retrieved all available results from existing simulation. Required to ensure correct deserialization
         if (project != null && shouldRegisterSimulations)
         {
            var allSimulations = project.Simulations;
            var allSimulationResults = allSimulations.Where(s => s.HasResults).Select(s => s.ResultsDataRepository);
            allRepositories.AddRange(allSimulationResults.Union(project.AllObservedData));

            //Also register simulations to ensure that they are available as well for deserialization
            allSimulations.Each(idRepository.Register);
         }

         allRepositories.Each(idRepository.Register);


         return new SerializationContext(_dimensionFactory, _objectBaseFactory, idRepository, allRepositories, _cloneManagerForModel, _container);
      }
   }
}