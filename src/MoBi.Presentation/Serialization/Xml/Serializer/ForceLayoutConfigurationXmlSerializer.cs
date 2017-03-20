using System.Xml.Linq;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class ForceLayoutConfigurationXmlSerializer : XmlSerializer<IForceLayoutConfiguration, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(config => config.MaxIterations);
         Map(config => config.Epsilon);
         Map(config => config.InfinityDistance);

         Map(config => config.ArrangementSpacingWidth);
         Map(config => config.ArrangementSpacingHeight);
         Map(config => config.LogPositions);

         Map(config => config.BaseElectricalCharge);
         Map(config => config.BaseGravitationalMass);
         Map(config => config.BaseSpringLength);
         Map(config => config.BaseSpringStiffness);


         Map(config => config.RelativeElectricalChargeLinkless);
         Map(config => config.RelativeElectricalChargeContainer);
         Map(config => config.RelativeElectricalChargeNeighborhood);
         Map(config => config.RelativeElectricalChargeNeighborPort);
         Map(config => config.RelativeElectricalChargeMolecule);
         Map(config => config.RelativeElectricalChargeObserver);
         Map(config => config.RelativeElectricalChargeReaction);

         Map(config => config.RelativeGravitationalMassLinkless);
         Map(config => config.RelativeGravitationalMassContainer);
         Map(config => config.RelativeGravitationalMassNeighborhood);
         Map(config => config.RelativeGravitationalMassNeighborPort);
         Map(config => config.RelativeGravitationalMassMolecule);
         Map(config => config.RelativeGravitationalMassObserver);
         Map(config => config.RelativeGravitationalMassReaction);

         Map(config => config.RelativeSpringLengthContainerContainer);
         Map(config => config.RelativeSpringLengthContainerNeighborhood);
         Map(config => config.RelativeSpringLengthNeighborPortNeighborhood);
         Map(config => config.RelativeSpringLengthMoleculeNeighborPort);
         Map(config => config.RelativeSpringLengthObserverMolecule);
         Map(config => config.RelativeSpringLengthReactionMolecule);

         Map(config => config.RelativeSpringStiffnessContainerContainer);
         Map(config => config.RelativeSpringStiffnessContainerNeighborhood);
         Map(config => config.RelativeSpringStiffnessNeighborPortNeighborhood);
         Map(config => config.RelativeSpringStiffnessMoleculeNeighborPort);
         Map(config => config.RelativeSpringStiffnessObserverMolecule);
         Map(config => config.RelativeSpringStiffnessReactionMolecule);

         Map(config => config.RelativeElectricalChargeRemote);
         Map(config => config.RelativeGravitationalMassRemote);
         Map(config => config.RelativeSpringLengthContainerRemote);
         Map(config => config.RelativeSpringStiffnessContainerRemote);
      }

      public override IForceLayoutConfiguration CreateObject(XElement element, SerializationContext serializationContext)
      {
         return IoC.Resolve<IForceLayoutConfiguration>();
      }
   }
}