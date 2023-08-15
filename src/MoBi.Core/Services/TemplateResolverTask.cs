using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Repository;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface ITemplateResolverTask
   {
      IBuildingBlock TemplateBuildingBlockFor(IBuildingBlock buildingBlock);
      Module TemplateModuleFor(Module module);
   }

   public class TemplateResolverTask : ITemplateResolverTask
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IMoBiProjectRetriever _moBiProjectRetriever;

      public TemplateResolverTask(IBuildingBlockRepository buildingBlockRepository, IMoBiProjectRetriever moBiProjectRetriever)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _moBiProjectRetriever = moBiProjectRetriever;
      }

      public IBuildingBlock TemplateBuildingBlockFor(IBuildingBlock buildingBlock)
      {
         return _buildingBlockRepository.All().Single(x => x.IsTemplateMatchFor(buildingBlock));
      }

      public Module TemplateModuleFor(Module module)
      {
         return _moBiProjectRetriever.Current.ModuleByName(module.Name);
      }
   }
}