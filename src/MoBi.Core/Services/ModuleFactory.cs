using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IModuleFactory
   {
      Module CreateDedicatedModuleFor(IBuildingBlock buildingBlock);
   }

   public class ModuleFactory : IModuleFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ModuleFactory(IObjectBaseFactory objectBaseFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectBaseFactory = objectBaseFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public Module CreateDedicatedModuleFor(IBuildingBlock buildingBlock)
      {
         var newModule = _objectBaseFactory.Create<Module>().WithName(dedicatedModuleName(buildingBlock));
         newModule.Add(buildingBlock);
         return newModule;
      }

      private string dedicatedModuleName(IBuildingBlock buildingBlock)
      {
         return $"{_objectTypeResolver.TypeFor(buildingBlock).Replace("Building Block", string.Empty)} - {buildingBlock.Name}";
      }
   }
}