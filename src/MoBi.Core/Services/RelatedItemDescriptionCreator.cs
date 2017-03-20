using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Journal;

namespace MoBi.Core.Services
{
   public class RelatedItemDescriptionCreator : IRelatedItemDescriptionCreator,
      IVisitor<IMoBiSimulation>,
      IVisitor<IBuildingBlock>
   {
      private string _report;

      public string DescriptionFor<T>(T relatedObject)
      {
         try
         {
            this.Visit(relatedObject);
            return _report;
         }
         finally
         {
            _report = string.Empty;
         }
      }

      public void Visit(IMoBiSimulation simulation)
      {
         updateReport(simulation);
      }

      private void updateReport(IWithCreationMetaData withCreationMetaData)
      {
         _report = withCreationMetaData.Creation.ToDisplayString();
      }

      public void Visit(IBuildingBlock buildingBlock)
      {
         updateReport(buildingBlock);
      }
   }
}