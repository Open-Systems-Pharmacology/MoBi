using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Serialization.Converter.v6_1
{
   public class Converter60To61 : IMoBiObjectConverter,
      IVisitor<IPassiveTransportBuildingBlock>,
      IVisitor<IBuildConfiguration>,
      IVisitor<IModelCoreSimulation>
   {
      private readonly OSPSuite.Core.Converter.v6_1.Converter60To61 _coreConverter60To61;

      public Converter60To61(OSPSuite.Core.Converter.v6_1.Converter60To61 coreConverter60To61)
      {
         _coreConverter60To61 = coreConverter60To61;
      }

      public bool IsSatisfiedBy(int item)
      {
         return item == ProjectVersions.V6_0_1;
      }

      public int Convert(object objectToUpdate, IMoBiProject project)
      {
         _coreConverter60To61.Convert(objectToUpdate);
         this.Visit(objectToUpdate);
         return ProjectVersions.V6_1_1;
      }

      public int ConvertXml(XElement element, IMoBiProject project)
      {
         convertPreviousMissedPerTimeDimension(element);
         return ProjectVersions.V6_1_1;
      }

      private void convertPreviousMissedPerTimeDimension(XElement element)
      {
         //retrieve all elements with an attribute dimension
         var allDimensionAttributes = from child in element.DescendantsAndSelf()
            where child.HasAttributes
            //Account for all possible attribute names containing a dimension!
            let attr = child.Attribute(Constants.Serialization.Attribute.Dimension) ?? child.Attribute("dimension") ?? child.Attribute("s")
            where attr != null
            select attr;

         foreach (var attribute in allDimensionAttributes)
         {
            string attributeValue = attribute.Value;
            if (attributeValue.IsOneOf("MassAmount per Time", "MassAmount per time"))
               attribute.SetValue("Amount per time");
         }
      }

      private void convertPreviousProblemWithPassiveTransportParametersGlobals(ITransportBuilder passiveTransportBuilder)
      {
         passiveTransportBuilder.Parameters.Each(p => p.BuildMode = ParameterBuildMode.Local);
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         passiveTransportBuildingBlock.Each(convertPreviousProblemWithPassiveTransportParametersGlobals);
      }

      public void Visit(IBuildConfiguration buildConfiguration)
      {
         Visit(buildConfiguration.PassiveTransports);
      }

      public void Visit(IModelCoreSimulation modelCoreSimulation)
      {
         Visit(modelCoreSimulation.BuildConfiguration);
      }
   }
}