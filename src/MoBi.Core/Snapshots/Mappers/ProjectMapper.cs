using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots.Mappers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using ModelProject = MoBi.Core.Domain.Model.MoBiProject;
using SnapshotProject = MoBi.Core.Snapshots.Project;

namespace MoBi.Core.Snapshots.Mappers;

public class ProjectMapper : SnapshotMapperBase<ModelProject, SnapshotProject, ProjectContext>
{
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly ICreationMetaDataFactory _creationMetaDataFactory;

   public ProjectMapper(IXmlSerializationService xmlSerializationService, ICreationMetaDataFactory creationMetaDataFactory)
   {
      _xmlSerializationService = xmlSerializationService;
      _creationMetaDataFactory = creationMetaDataFactory;
   }
      
   public override Task<ModelProject> MapToModel(SnapshotProject snapshot, ProjectContext context)
   {
      var project = new ModelProject
      {
         Name = snapshot.Name,
         Description = snapshot.Description,
         Creation = _creationMetaDataFactory.Create()
      };
         
      snapshot.ExtensionModules?.Each(x => project.AddModule((deserializeFromBase64PKML<Module>(x, project))));
      
      snapshot.ExpressionProfileBuildingBlocks?.Each(x => project.AddExpressionProfileBuildingBlock(deserializeFromBase64PKML<ExpressionProfileBuildingBlock>(x, project)));
      
      snapshot.IndividualBuildingBlocks?.Each(x => project.AddIndividualBuildingBlock(deserializeFromBase64PKML<IndividualBuildingBlock>(x, project)));

      return Task.FromResult(project);
   }

   public override async Task<SnapshotProject> MapToSnapshot(ModelProject project)
   {
      var snapshot = await SnapshotFrom(project, x =>
      {
         x.Version = ProjectVersions.Current;
         x.Description = SnapshotValueFor(project.Description);
      });

      snapshot.ExtensionModules = await mapExtensionModules(project);
      snapshot.ExpressionProfileBuildingBlocks = await mapExpressionProfilesBuildingBlocks(project);
      snapshot.IndividualBuildingBlocks = await mapIndividualBuildingBlocks(project);

      return snapshot;
   }



   private Task<string[]> mapExtensionModules(ModelProject project) => Task.FromResult(project.Modules.Where(x => !x.IsPKSimModule).Select(serializeToBase64PKML).ToArray());
   private Task<string[]> mapExpressionProfilesBuildingBlocks(ModelProject project) => Task.FromResult(project.ExpressionProfileCollection.Select(serializeToBase64PKML).ToArray());
   private Task<string[]> mapIndividualBuildingBlocks(ModelProject project) => Task.FromResult(project.IndividualsCollection.Select(serializeToBase64PKML).ToArray());

   private T deserializeFromBase64PKML<T>(string encodedModule, ModelProject project) => _xmlSerializationService.Deserialize<T>(fromBase64String(encodedModule), project);
   
   private string serializeToBase64PKML<T>(T elementToSerialize) => toBase64String(_xmlSerializationService.SerializeAsString(elementToSerialize));

   private string fromBase64String(string encodedElement) => Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedElement));

   private static string toBase64String(string serializeAsString) => System.Convert.ToBase64String(Encoding.UTF8.GetBytes(serializeAsString));
}