using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class SpatialStructureReporter : OSPSuiteTeXReporter<IMoBiSpatialStructure>
   {
      private readonly IDiagramModelToImageTask _diagramModelToImageTask;

      public SpatialStructureReporter(IDiagramModelToImageTask diagramModelToImageTask)
      {
         _diagramModelToImageTask = diagramModelToImageTask;
      }

      public override IReadOnlyCollection<object> Report(IMoBiSpatialStructure spatialStructure, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Section(string.Format(Constants.BUILDING_BLOCK_PROMPT_FORMAT, Constants.SPATIAL_STRUCTURE, spatialStructure.Name)));
         listToReport.AddRange(this.ReportDescription(spatialStructure, buildTracker));

         if (buildTracker.Settings.Verbose)
         {
            var fileName = String.Concat(spatialStructure.Name, "_", Guid.NewGuid(), ".png");
            var figure = Figure.ForCreation(String.Format("Diagram of spatial structure  {0}", spatialStructure.Name),
                                            fileName, buildTracker);
            listToReport.Add(new Text("The {0} shows the diagram of the spatial structure.", new Reference(figure)));
            listToReport.Add(figure);
            _diagramModelToImageTask.ExportTo(spatialStructure, figure.FullPath);
         }

         if (spatialStructure.GlobalMoleculeDependentProperties != null)
         {
            listToReport.Add(new SubSection(Constants.GLOBAL_MOLECULE_DEPENDENT_PROPERTIES));
            listToReport.Add(spatialStructure.GlobalMoleculeDependentProperties);
         }

         if (spatialStructure.Neighborhoods.Any())
         {
            listToReport.Add(new SubSection(Constants.NEIGHBORHOODS));
            listToReport.Add(spatialStructure.Neighborhoods.ToList());
            listToReport.AddRange(spatialStructure.Neighborhoods);
         }

         if (spatialStructure.TopContainers.Any())
         {
            listToReport.Add(new SubSection(Constants.TOP_CONTAINERS));
            listToReport.AddRange(spatialStructure.TopContainers);
         }

         if (spatialStructure.FormulaCache.Any())
         {
            listToReport.Add(new SubSection(Constants.FORMULA));
            listToReport.AddRange(spatialStructure.FormulaCache.OrderBy(f => f.Name));
         }

         return listToReport;
      }
   }
}