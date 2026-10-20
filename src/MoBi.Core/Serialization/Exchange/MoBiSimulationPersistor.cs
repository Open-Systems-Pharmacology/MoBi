using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core.Serialization.Exchange
{
   public interface IMoBiSimulationPersistor
   {
      /// <summary>
      ///    Saves the <paramref name="simulation" /> to the pkml file <paramref name="fileName" />.
      ///    The simulation is saved the same way as with the <see cref="ISimulationPersistor" /> so that the file can still be
      ///    used everywhere a pkml can be used. On top of that, the changes tracked in the simulation are saved so that they are
      ///    restored when the file is loaded back into MoBi.
      /// </summary>
      void Save(IMoBiSimulation simulation, string fileName);
   }

   public class MoBiSimulationPersistor : IMoBiSimulationPersistor
   {
      private const string _true = "1";
      private readonly IOSPSuiteXmlSerializerRepository _coreSerializerRepository;
      private readonly IMoBiXmlSerializerRepository _moBiSerializerRepository;
      private readonly IContainer _container;

      public MoBiSimulationPersistor(IOSPSuiteXmlSerializerRepository coreSerializerRepository, IMoBiXmlSerializerRepository moBiSerializerRepository, IContainer container)
      {
         _coreSerializerRepository = coreSerializerRepository;
         _moBiSerializerRepository = moBiSerializerRepository;
         _container = container;
      }

      public void Save(IMoBiSimulation simulation, string fileName)
      {
         var simulationTransfer = new SimulationTransfer { Simulation = simulation };
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var simulationTransferElement = _coreSerializerRepository.SerializerFor(simulationTransfer).Serialize(simulationTransfer, serializationContext);
            addTrackedChanges(simulation, simulationTransferElement.Element(Constants.Serialization.SIMULATION), serializationContext);
            simulationTransferElement.PermissiveSave(fileName);
         }
      }

      private void addTrackedChanges(IMoBiSimulation simulation, XElement simulationElement, SerializationContext serializationContext)
      {
         if (simulation.HasUntraceableChanges)
            simulationElement.AddAttribute(SerializationConstants.HasUntraceableChanges, _true);

         if (!simulation.OriginalQuantityValues.Any())
            return;

         var originalQuantityValueSerializer = _moBiSerializerRepository.SerializerFor<OriginalQuantityValue>();
         var originalQuantityValuesElement = _moBiSerializerRepository.CreateElement(SerializationConstants.OriginalQuantityValues);
         simulation.OriginalQuantityValues.Each(x => originalQuantityValuesElement.Add(originalQuantityValueSerializer.Serialize(x, serializationContext)));
         simulationElement.Add(originalQuantityValuesElement);
      }
   }
}
