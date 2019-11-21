using System;
using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.TeXReporting;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_Reporting : ContextWithLoadedProject
   {
      protected IReportingTask _reportingTask;
      protected ReportConfiguration _reportConfiguration;
      protected IMoBiProject _reportingProject;
      private DirectoryInfo _reportsDir;
      protected string _projectName;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _reportingProject = LoadProject(_projectName);
         _reportingTask = IoC.Resolve<IReportingTask>();

         _reportsDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", _projectName));
         if (!_reportsDir.Exists)
            _reportsDir.Create();

         _reportConfiguration = new ReportConfiguration
         {
            Title = "Testing Reports",
            Author = "Unit Tests Engine",
            SubTitle = "SubTitle",
            DeleteWorkingDir = true,
            ColorStyle = ReportColorStyles.Color
         };

         _reportConfiguration.Template = new ReportTemplate {Path = TEXTemplateFolder()};
      }

      private static string TEXTemplateFolder()
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TeXTemplates", "StandardTemplate");
      }

      public void CreateReportAndValidate(IObjectBase objectBase)
      {
         CreateReportAndValidate(objectBase, objectBase.Name);
      }

      public void CreateReportAndValidate(object objectToReport, string reportName)
      {
         _reportConfiguration.ReportFile = Path.Combine(_reportsDir.FullName, $"{reportName}.pdf");
         _reportConfiguration.SubTitle = reportName;
         _reportingTask.CreateReportAsync(objectToReport, _reportConfiguration).Wait();
         FileHelper.FileExists(_reportConfiguration.ReportFile).ShouldBeTrue();
      }
   }

   public abstract class When_creating_a_report : concern_for_Reporting
   {
      protected When_creating_a_report(string projectName)
      {
         _projectName = projectName;
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_spatial_structure()
      {
         CreateReportAndValidate(_reportingProject.SpatialStructureCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_spatial_structures()
      {
         CreateReportAndValidate(_reportingProject.SpatialStructureCollection.ToList(), "Spatial Structures");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_molecule()
      {
         CreateReportAndValidate(_reportingProject.MoleculeBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_molecules()
      {
         CreateReportAndValidate(_reportingProject.MoleculeBlockCollection.ToList(), "Molecules");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_reaction()
      {
         CreateReportAndValidate(_reportingProject.ReactionBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_reactions()
      {
         CreateReportAndValidate(_reportingProject.ReactionBlockCollection.ToList(), "Reactions");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_passive_transport()
      {
         CreateReportAndValidate(_reportingProject.PassiveTransportCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_passive_transports()
      {
         CreateReportAndValidate(_reportingProject.PassiveTransportCollection.ToList(), "Passive Transports");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_observer()
      {
         CreateReportAndValidate(_reportingProject.ObserverBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_observers()
      {
         CreateReportAndValidate(_reportingProject.ObserverBlockCollection.ToList(), "Observers");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_simulation_setting()
      {
         CreateReportAndValidate(_reportingProject.SimulationSettingsCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_simulation_settings()
      {
         CreateReportAndValidate(_reportingProject.SimulationSettingsCollection.ToList(), "Simulation Settings");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_event()
      {
         CreateReportAndValidate(_reportingProject.EventBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_events()
      {
         CreateReportAndValidate(_reportingProject.EventBlockCollection.ToList(), "Events");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_msv()
      {
         CreateReportAndValidate(_reportingProject.MoleculeStartValueBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_msvs()
      {
         CreateReportAndValidate(_reportingProject.MoleculeStartValueBlockCollection.ToList(), "Molecule Start Values");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_psv()
      {
         CreateReportAndValidate(_reportingProject.ParametersStartValueBlockCollection.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_psvs()
      {
         CreateReportAndValidate(_reportingProject.ParametersStartValueBlockCollection.ToList(), "Parameter Start Values");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_observed_data()
      {
         if (_reportingProject.AllObservedData.Any())
            CreateReportAndValidate(_reportingProject.AllObservedData.First(), "Data Repository");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_observed_datas()
      {
         if (_reportingProject.AllObservedData.Any())
            CreateReportAndValidate(_reportingProject.AllObservedData.ToList(), "Observed Data");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_simulation()
      {
         CreateReportAndValidate(_reportingProject.Simulations.First());
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_simulations()
      {
         CreateReportAndValidate(_reportingProject.Simulations.ToList(), "Simulations");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_chart()
      {
         if (_reportingProject.Charts.Any())
            CreateReportAndValidate(_reportingProject.Charts.First(), "Chart");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_charts()
      {
         if (_reportingProject.Charts.Any())
            CreateReportAndValidate(_reportingProject.Charts.ToList(), "Charts");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_project()
      {
         CreateReportAndValidate(_reportingProject, "Project");
      }

      [Observation]
      public void should_have_created_the_pdf_report_for_history_manager()
      {
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
         context.LoadFrom(_reportingProject);
         CreateReportAndValidate(context.HistoryManager, "History");
      }
   }

   public class Testing_project_ManualModel_Sim : When_creating_a_report
   {
      public Testing_project_ManualModel_Sim()
         : base("ManualModel_Sim")
      {
      }
   }
}