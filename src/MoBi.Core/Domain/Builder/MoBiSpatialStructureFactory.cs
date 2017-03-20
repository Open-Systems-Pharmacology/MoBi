using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
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
      /// <returns></returns>
      IMoBiSpatialStructure CreateDefault(string spatialStructureName);
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

      protected override ISpatialStructure CreateSpatialStructure()
      {
         var spatialStructure = _objectBaseFactory.Create<IMoBiSpatialStructure>();
         spatialStructure.DiagramManager = _diagramManagerFactory.Create<ISpatialStructureDiagramManager>();
         return spatialStructure;
      }

      public IMoBiSpatialStructure CreateDefault(string spatialStructureName)
      {
         var topContainer = _objectBaseFactory.Create<IContainer>()
            .WithName(spatialStructureName)
            .WithMode(ContainerMode.Physical)
            .WithContainerType(ContainerType.Organism);
         updateIcon(topContainer);

         topContainer.AddChildren(_parameterFactory.CreateVolumeParameter());

         var moleculeProperties = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Other)
            .WithParentContainer(topContainer);
         updateIcon(moleculeProperties);


         var spatialStructure = Create()
            .WithName(spatialStructureName)
            .WithTopContainer(topContainer).DowncastTo<IMoBiSpatialStructure>();

         spatialStructure.DiagramManager = _diagramManagerFactory.Create<ISpatialStructureDiagramManager>();
         spatialStructure.DiagramManager.AddObjectBase(topContainer);
         spatialStructure.DiagramManager.AddObjectBase(moleculeProperties);

         return spatialStructure;
      }

      private void updateIcon(IObjectBase objectWithIcon)
      {
         objectWithIcon.Icon = _iconRepository.IconFor(objectWithIcon);
      }
   }
}