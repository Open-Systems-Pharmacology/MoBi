using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.TeXReporting.Data;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX;
using OSPSuite.TeXReporting.TeX.Converter;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class ReactionBuildingBlockReporter : BuildingBlockReporter<MoBiReactionBuildingBlock, ReactionBuilder>
   {
      private readonly IDiagramModelToImageTask _diagramModelToImageTask;
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;

      public ReactionBuildingBlockReporter(IDiagramModelToImageTask diagramModelToImageTask, IStoichiometricStringCreator stoichiometricStringCreator, IReactionDimensionRetriever reactionDimensionRetriever)
         : base(Constants.REACTION_BUILDING_BLOCK, Constants.REACTION_BUILDING_BLOCKS)
      {
         _diagramModelToImageTask = diagramModelToImageTask;
         _stoichiometricStringCreator = stoichiometricStringCreator;
         _reactionDimensionRetriever = reactionDimensionRetriever;
      }

      protected override void AddBuildersReport(MoBiReactionBuildingBlock buildingBlock, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         listToReport.Add(string.Format("All reactions are {0} based.", getSelectedDimensionString()));

         if (buildTracker.Settings.Verbose)
         {
            var fileName = String.Concat(buildingBlock.Name, "_", Guid.NewGuid(), ".png");
            var figure = Figure.ForCreation(String.Format("Diagram of reaction  {0}", buildingBlock.Name),
                                            fileName, buildTracker);
            listToReport.Add(new Text("The {0} shows the diagram of the reaction.", new Reference(figure)));
            listToReport.Add(figure);
            _diagramModelToImageTask.ExportTo(buildingBlock, figure.FullPath);
         }

         //create table as overview
         listToReport.Add(new InLandscape(new List<object> {tableFor(buildingBlock)}));

         base.AddBuildersReport(buildingBlock, listToReport, buildTracker);
      }

      private Table tableFor(MoBiReactionBuildingBlock buildingBlock)
      {
         var table = new DataTable(getTableCaption());
         table.Columns.Add(Constants.NAME, typeof(string));
         table.Columns.Add(Constants.STOICHIOMETRY, typeof(string)).SetAlignment(TableWriter.ColumnAlignments.l);
         table.Columns.Add(Constants.KINETIC, typeof(string)).SetAlignment(TableWriter.ColumnAlignments.l);

         table.BeginLoadData();
         foreach (var reaction in buildingBlock.OrderBy(b => b.Name))
         {
            var row = table.NewRow();
            row[Constants.NAME] =  DefaultConverter.Instance.StringToTeX(reaction.Name);
            row[Constants.STOICHIOMETRY] = FormulaConverter.Instance.StringToTeX(_stoichiometricStringCreator.CreateFrom(reaction.Educts, reaction.Products));
            row[Constants.KINETIC] = FormulaConverter.Instance.StringToTeX(reaction.Formula.ToString());
            table.Rows.Add(row);
         }
         table.EndLoadData();

         return new Table(table.DefaultView, table.TableName) {Converter = NoConverter.Instance};
      }

      private string getSelectedDimensionString()
      {
         switch (_reactionDimensionRetriever.SelectedDimensionMode)
         {
            case ReactionDimensionMode.AmountBased:
               return "amount";
            case ReactionDimensionMode.ConcentrationBased:
               return "concentration";
         }
         return string.Empty;
      }

      private string getTableCaption()
      {
         var selectedDimension = _reactionDimensionRetriever.SelectedDimensionMode;

         switch (selectedDimension)
         {
            case ReactionDimensionMode.AmountBased:
               return "Amount based reactions.";
            case ReactionDimensionMode.ConcentrationBased:
               return "Concentration based reactions.";
         }

         return Constants.REACTION_BUILDING_BLOCKS;
      }

   }
}