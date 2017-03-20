using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;
using MoBi.Core.Reporting.Items;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class FormulaTextBoxTEXBuilder : OSPSuiteTeXBuilder<FormulaTextBox>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public FormulaTextBoxTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(FormulaTextBox formulaTextBox, OSPSuiteTracker buildTracker)
      {
         var formula = formulaTextBox.Formula;
         if (String.IsNullOrEmpty(formula.Name)) return;

         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(formula, buildTracker));

         var formulaText = new Text(formula.ToString()) { Converter = FormulaConverter.Instance };
         formulaText = new Text("{0} = {1}", formula.Name, formulaText) { Alignment = Text.Alignments.flushleft };
         var content = new Text("{0}{1}", formulaText, formula.ObjectPaths);
         listToReport.Add(new TextBox(formulaTextBox.Caption, content));

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}
