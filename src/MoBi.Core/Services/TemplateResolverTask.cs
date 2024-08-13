using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Repository;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface ITemplateResolverTask
   {
      TBuildingBlock TemplateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock;
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

      public TBuildingBlock TemplateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         if (buildingBlock == null)
            return null;

         try
         {
            return _buildingBlockRepository.All().Single(x => x.IsTemplateMatchFor(buildingBlock)) as TBuildingBlock;
         }
         catch
         {
            return null;
         }
      }
      
      public Module TemplateModuleFor(Module module) => _moBiProjectRetriever.Current.ModuleByName(module.Name);
   }
}