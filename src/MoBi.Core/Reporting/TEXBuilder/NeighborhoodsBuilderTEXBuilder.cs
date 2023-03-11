using System.Collections.Generic;
using System.Data;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class NeighborhoodsBuilderTEXBuilder : OSPSuiteTeXBuilder<IEnumerable<NeighborhoodBuilder>>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly IObjectPathFactory _objectPathFactory;

      public NeighborhoodsBuilderTEXBuilder(ITeXBuilderRepository builderRepository, IObjectPathFactory objectPathFactory)
      {
         _builderRepository = builderRepository;
         _objectPathFactory = objectPathFactory;
      }

      public override void Build(IEnumerable<NeighborhoodBuilder> neighborhoodBuilders, OSPSuiteTracker tracker)
      {
         _builderRepository.Report(tableFor(neighborhoodBuilders), tracker);
      }

      private DataTable tableFor(IEnumerable<NeighborhoodBuilder> neighborhoodBuilders)
      {
         var dataTable = new DataTable(Constants.NEIGHBORHOODS);

         dataTable.Columns.Add(Constants.FIRST_NEIGHBOR, typeof(string));
         dataTable.Columns.Add(Constants.SECOND_NEIGHBOR, typeof(string));

         dataTable.BeginLoadData();
         foreach (var neighborhood in neighborhoodBuilders)
         {
            var newRow = dataTable.NewRow();
            newRow[Constants.FIRST_NEIGHBOR] = _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.FirstNeighbor).PathAsString;
            newRow[Constants.SECOND_NEIGHBOR] = _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.SecondNeighbor).PathAsString;
            dataTable.Rows.Add(newRow);
         }
         dataTable.EndLoadData();

         return dataTable;
      }
   }
}
