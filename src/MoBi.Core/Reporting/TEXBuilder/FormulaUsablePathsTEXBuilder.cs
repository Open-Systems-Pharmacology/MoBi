using System.Collections.Generic;
using System.Data;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.TeX;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class FormulaUsablePathsTEXBuilder : TeXChunkBuilder<IEnumerable<IFormulaUsablePath>>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public FormulaUsablePathsTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEnumerable<IFormulaUsablePath> formulaUsablePaths, BuildTracker buildTracker)
      {
         _builderRepository.Report(tableFor(formulaUsablePaths), buildTracker);
      }

      public override string TeXChunk(IEnumerable<IFormulaUsablePath> formulaUsablePaths)
      {
         return _builderRepository.ChunkFor(tableFor(formulaUsablePaths));
      }

      private static SimpleTable tableFor(IEnumerable<IFormulaUsablePath> formulaUsablePaths)
      {
         var dataTable = new DataTable(Constants.REFERENCES);
         dataTable.Columns.Add(Constants.ALIAS, typeof (string));
         dataTable.Columns.Add(Constants.PATH, typeof (string)).SetAlignment(TableWriter.ColumnAlignments.l);
         dataTable.Columns.Add(Constants.DIMENSION, typeof (string));

         dataTable.BeginLoadData();
         foreach (var path in formulaUsablePaths)
         {
            var newRow = dataTable.NewRow();
            newRow[Constants.ALIAS] = path.Alias;
            newRow[Constants.PATH] = path.ToString();
            newRow[Constants.DIMENSION] = path.Dimension.DisplayName;
            dataTable.Rows.Add(newRow);
         }
         dataTable.EndLoadData();
         return new SimpleTable(dataTable.DefaultView);
      }
   }
}