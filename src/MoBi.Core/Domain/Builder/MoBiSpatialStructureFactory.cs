using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Builder
{
   public interface IMoBiSpatialStructureFactory : ISpatialStructureFactory
   {
      /// <summary>
      ///    Creates the strict minimum default spatial structure
      /// </summary>
      MoBiSpatialStructure CreateDefault(string spatialStructureName = null);
   }

   public class MoBiSpatialStructureFactory : SpatialStructureFactory, IMoBiSpatialStructureFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly IIconRepository _iconRepository;
      private readonly IDiagramManagerFactory _diagramManagerFactory;

      public MoBiSpatialStructureFactory(IObjectBaseFactory objectBaseFactory, IParameterFactory parameterFactory, IIconRepository iconRepository, IDiagramManagerFactory diagramManagerFactory) : base(objectBaseFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _parameterFactory = parameterFactory;
         _iconRepository = iconRepository;
         _diagramManagerFactory = diagramManagerFactory;
      }

      protected override SpatialStructure CreateSpatialStructure()
      {
         var spatialStructure = _objectBaseFactory.Create<MoBiSpatialStructure>();
         spatialStructure.DiagramManager = _diagramManagerFactory.Create<ISpatialStructureDiagramManager>();
         return spatialStructure;
      }

      public MoBiSpatialStructure CreateDefault(string spatialStructureName = null)
      {
         spatialStructureName = spatialStructureName ?? DefaultNames.SpatialStructure;
         
         var spatialStructure = Create().WithName(spatialStructureName).DowncastTo<MoBiSpatialStructure>();
         spatialStructure.DiagramManager = _diagramManagerFactory.Create<ISpatialStructureDiagramManager>();

         return spatialStructure;
      }
   }
}