using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots
{
   public class concern_for_ModuleConfigurationMapper : ContextSpecificationAsync<ModuleConfigurationMapper>
   {
      protected override Task Context()
      {
         sut = new ModuleConfigurationMapper();
         return Task.FromResult(sut);
      }
   }

   public class When_mapping_snapshot_to_module : concern_for_ModuleConfigurationMapper
   {
      private SimulationContext _context;
      private ModuleConfiguration _snapshot;
      private OSPSuite.Core.Domain.ModuleConfiguration _result;
      private MoBiProject _project;

      protected override async Task Context()
      {
         await base.Context();
         _project = new MoBiProject();
         var module = new Module().WithName("module");
         _project.AddModule(module);
         module.Add(new InitialConditionsBuildingBlock().WithName("ic"));
         module.Add(new ParameterValuesBuildingBlock().WithName("pv"));
         _context = new SimulationContext(false, new SnapshotContext(_project, SnapshotVersions.Current));
         _snapshot = new ModuleConfiguration
         {
            Module = "module",
            SelectedInitialConditions = "ic",
            SelectedParameterValues = "pv"
         };
      }

      protected override async Task Because()
      {
         _result = await sut.MapToModel(_snapshot, _context);
      }

      [Observation]
      public void the_module_configuration_should_use_the_project_module()
      {
         _result.Module.ShouldBeEqualTo(_project.ModuleByName("module"));
      }

      [Observation]
      public void the_module_configuration_should_use_the_building_blocks()
      {
         _result.SelectedInitialConditions.ShouldBeEqualTo(_project.Modules.First().InitialConditionsCollection.FindByName("ic"));
         _result.SelectedParameterValues.ShouldBeEqualTo(_project.Modules.First().ParameterValuesCollection.FindByName("pv"));
      }
   }

   public class When_mapping_module_configuration_to_snapshot : concern_for_ModuleConfigurationMapper
   {
      private OSPSuite.Core.Domain.ModuleConfiguration _moduleConfiguration;
      private ModuleConfiguration _result;
      private Module _module;

      protected override async Task Context()
      {
         await base.Context();
         _module = new Module().WithName("module");
         _module.Add(new InitialConditionsBuildingBlock().WithName("ic"));
         _module.Add(new ParameterValuesBuildingBlock().WithName("pv"));
         _moduleConfiguration = new OSPSuite.Core.Domain.ModuleConfiguration(_module)
         {
            SelectedInitialConditions = _module.InitialConditionsCollection.First(),
            SelectedParameterValues = _module.ParameterValuesCollection.First()
         };
      }

      protected override async Task Because()
      {
         _result = await sut.MapToSnapshot(_moduleConfiguration);
      }

      [Observation]
      public void all_fields_should_be_mapped()
      {
         _result.Module.ShouldBeEqualTo("module");
         _result.SelectedInitialConditions.ShouldBeEqualTo("ic");
         _result.SelectedParameterValues.ShouldBeEqualTo("pv");
      }
   }

   public class When_mapping_module_configuration_to_snapshot_without_ic_and_pv : concern_for_ModuleConfigurationMapper
   {
      private OSPSuite.Core.Domain.ModuleConfiguration _moduleConfiguration;
      private ModuleConfiguration _result;
      private Module _module;

      protected override async Task Context()
      {
         await base.Context();
         _module = new Module().WithName("module");
         _moduleConfiguration = new OSPSuite.Core.Domain.ModuleConfiguration(_module);
      }

      protected override async Task Because()
      {
         _result = await sut.MapToSnapshot(_moduleConfiguration);
      }

      [Observation]
      public void pv_and_ic_should_not_be_mapped()
      {
         _result.Module.ShouldBeEqualTo("module");
         _result.SelectedInitialConditions.ShouldBeNull();
         _result.SelectedParameterValues.ShouldBeNull();
      }
   }
}