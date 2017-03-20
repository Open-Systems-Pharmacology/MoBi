using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class ParametersTEXBuilder : OSPSuiteTeXBuilder<IEnumerable<IParameter>>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly ReportingHelper _reportingHelper;

      public ParametersTEXBuilder(ITeXBuilderRepository builderRepository, IDisplayUnitRetriever displayUnitRetriever)
      {
         _builderRepository = builderRepository;
         _reportingHelper = new ReportingHelper(displayUnitRetriever);
      }

      public override void Build(IEnumerable<IParameter> parameters, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         var sortedParameters = parameters.OrderBy(x => x.Name).ToList();
         var parameterConstantsTable = tableForConstants(sortedParameters, buildTracker.Settings.Verbose);

         if (parameterConstantsTable.Rows.Count > 0)
         {
            listToReport.Add(new SubParagraph(Constants.CONSTANTS));
            listToReport.Add(parameterConstantsTable);
         }

         if (sortedParameters.Count > parameterConstantsTable.Rows.Count)
         {
            listToReport.Add(new SubParagraph(Constants.PARAMETERS));
            foreach (var parameter in sortedParameters)
            {
               var constant = parameter.Formula as ConstantFormula;
               if (constant != null) continue;

               listToReport.Add(new SubParagraph(parameter.Name));

               var distributedParameter = parameter as DistributedParameter;
               if (distributedParameter != null)
               {
                  listToReport.Add(new Par());
                  listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.BUILD_MODE, distributedParameter.BuildMode));
                  listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.PERCENTILE, distributedParameter.Percentile));
                  listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.CAN_BE_VARIED, distributedParameter.CanBeVaried));
                  listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.VISIBLE, distributedParameter.Visible));
                  listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.VALUE,
                                                 String.Format("{0} {1}",
                                                               _reportingHelper.ConvertToDisplayUnit(
                                                                  distributedParameter.DisplayUnit,
                                                                  distributedParameter.Dimension,
                                                                  (float) distributedParameter.Value),
                                                               _reportingHelper.GetDisplayUnitFor(distributedParameter).Name)));
               }

               listToReport.Add(parameter.Formula);
               
               listToReport.Add(new Par());
               listToReport.Add(parameter.RHSFormula);

               if (parameter.Tags.Any())
               {
                  listToReport.Add(new Par());
                  listToReport.Add(parameter.Tags);
               }
            }
         }

         _builderRepository.Report(listToReport, buildTracker);
      }

      private DataTable tableForConstants(IEnumerable<IParameter> parameters, bool verbose)
      {
         var name = String.Format("{0} as {1}", Constants.PARAMETERS, Constants.CONSTANTS);
         var parameterTable = new DataTable(name);

         parameterTable.AddColumn(Constants.PARAMETER);
         parameterTable.AddColumn<double>(Constants.VALUE);
         parameterTable.AddColumn(Constants.UNIT);
         parameterTable.AddColumn(Constants.DIMENSION);
         parameterTable.AddColumn(Constants.TAGS);

         var descriptionTable = new DataTable();
         descriptionTable.AddColumn(Constants.PARAMETER);
         descriptionTable.AddColumn(Constants.DESCRIPTION);

         parameterTable.BeginLoadData();
         descriptionTable.BeginLoadData();
         foreach (var parameter in parameters)
         {
            var constantFormula = parameter.Formula as ConstantFormula;
            if (constantFormula == null) continue;

            var newParameterRow = parameterTable.NewRow();
            newParameterRow[Constants.PARAMETER] = parameter.Name;
            newParameterRow[Constants.VALUE] = _reportingHelper.ConvertToDisplayUnit(parameter.DisplayUnit, parameter.Dimension, (float)constantFormula.Value);
            newParameterRow[Constants.UNIT] = _reportingHelper.GetDisplayUnitFor(parameter);
            newParameterRow[Constants.DIMENSION] = parameter.Dimension.DisplayName;
            newParameterRow[Constants.TAGS] = parameter.Tags.Select(x=>x.Value).ToString(", ");
            parameterTable.Rows.Add(newParameterRow);

            if (string.IsNullOrEmpty(parameter.Description)) continue;

            var newDescriptionRow = descriptionTable.NewRow();
            newDescriptionRow[Constants.PARAMETER] = parameter.Name;
            newDescriptionRow[Constants.DESCRIPTION] = parameter.Description;
            descriptionTable.Rows.Add(newDescriptionRow);
         }
         descriptionTable.EndLoadData();
         parameterTable.EndLoadData();

         if (verbose)
         {
            var ds = new DataSet();
            ds.Tables.Add(parameterTable);
            ds.Tables.Add(descriptionTable);
            ds.Relations.Add(parameterTable.Columns[Constants.PARAMETER], descriptionTable.Columns[Constants.PARAMETER]);
         }

         return parameterTable;
      }

   }
}
