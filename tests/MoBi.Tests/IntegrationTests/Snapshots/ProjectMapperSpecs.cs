using System.Linq;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Container;
using SnapshotProject = MoBi.Core.Snapshots.Project;

namespace MoBi.IntegrationTests.Snapshots
{
   internal class concern_for_ProjectMapper : ContextForIntegration<ProjectMapper>
   {
      protected MoBiProject _project;
      private IXmlSerializationService _xmlSerializationService;
      private ICreationMetaDataFactory _creationMetaDataFactory;

      protected override void Context()
      {
         _xmlSerializationService = IoC.Resolve<IXmlSerializationService>();
         _creationMetaDataFactory = IoC.Resolve<ICreationMetaDataFactory>();

         _project = new MoBiProject();
         sut = new ProjectMapper(_xmlSerializationService, _creationMetaDataFactory);
         
         _project.AddModule(new Module());
         var expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock
         {
            Type = ExpressionTypes.MetabolizingEnzyme
         };
         
         _project.AddExpressionProfileBuildingBlock(expressionProfileBuildingBlock);
         _project.AddIndividualBuildingBlock(new IndividualBuildingBlock());
      }
   }

   internal class When_mapping_snapshot_to_project : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;
      private MoBiProject _result;

      protected override void Context()
      {
         base.Context();
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      protected override void Because()
      {
         _result = sut.MapToModel(_snapshot, new ProjectContext(runSimulations:false)).Result;
      }

      [Observation]
      public void the_snapshot_should_contain_extension_modules_snapshots_for_each_extension_module()
      {
          _result.Modules.Count(x => !x.IsPKSimModule).ShouldBeEqualTo(_snapshot.ExtensionModules.Length);
      }

      [Observation]
      public void the_snapshot_should_contain_expression_snapshots_for_each_expression()
      {
         _result.ExpressionProfileCollection.Count.ShouldBeEqualTo(_snapshot.ExpressionProfileBuildingBlocks.Length);
      }

      [Observation]
      public void the_snapshot_should_contain_individual_snapshots_for_each_individual()
      {
         _result.IndividualsCollection.Count.ShouldBeEqualTo(_snapshot.IndividualBuildingBlocks.Length);
      }
   }

   internal class When_mapping_project_to_snapshot : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;

      protected override void Because()
      {
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      [Observation]
      public void the_snapshot_should_contain_extension_modules_snapshots_for_each_extension_module()
      {
         _snapshot.ExtensionModules.Length.ShouldBeEqualTo(_project.Modules.Count(x => !x.IsPKSimModule));
      }

      [Observation]
      public void the_snapshot_should_contain_expression_snapshots_for_each_expression()
      {
         _snapshot.ExpressionProfileBuildingBlocks.Length.ShouldBeEqualTo(_project.ExpressionProfileCollection.Count);
      }

      [Observation]
      public void the_snapshot_should_contain_individual_snapshots_for_each_individual()
      {
         _snapshot.IndividualBuildingBlocks.Length.ShouldBeEqualTo(_project.IndividualsCollection.Count);
      }

      [Observation]
      public void the_snapshot_version_should_be_project_latest()
      {
         _snapshot.Version.ShouldBeEqualTo(ProjectVersions.Current);
      }
   }
}
