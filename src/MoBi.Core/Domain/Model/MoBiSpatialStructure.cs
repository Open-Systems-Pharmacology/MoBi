using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiSpatialStructure : ISpatialStructure, IWithDiagramFor<IMoBiSpatialStructure>
   {
      /// <summary>
      ///    Return the list of neighborhood connected to this neighbor;
      /// </summary>
      IReadOnlyList<NeighborhoodBuilder> GetConnectingNeighborhoods(IReadOnlyList<IContainer> neighbors, IObjectPathFactory objectPathFactory);
   }

   public class MoBiSpatialStructure : SpatialStructure, IMoBiSpatialStructure
   {
      public IDiagramModel DiagramModel { get; set; }
      public IDiagramManager<IMoBiSpatialStructure> DiagramManager { get; set; }

      public IReadOnlyList<NeighborhoodBuilder> GetConnectingNeighborhoods(IReadOnlyList<IContainer> neighbors, IObjectPathFactory objectPathFactory)
      {
         var allImportedContainers = neighbors
            .SelectMany(cont => cont.GetAllContainersAndSelf<IContainer>(x => !x.IsAnImplementationOf<IParameter>()))
            .ToList();

         var neighborhoods = new List<NeighborhoodBuilder>();
         foreach (var neighborhood in Neighborhoods)
         {
            var firstFound = false;
            var secondFound = false;
            foreach (var cont in allImportedContainers)
            {
               var contObjectPath = objectPathFactory.CreateAbsoluteObjectPath(cont);
               if (Equals(neighborhood.FirstNeighborPath, contObjectPath))
                  firstFound = true;

               if (Equals(neighborhood.SecondNeighborPath, contObjectPath))
                  secondFound = true;
            }

            if (firstFound && secondFound)
               neighborhoods.Add(neighborhood);
         }

         return neighborhoods;
      }

      public override void UpdatePropertiesFrom(IUpdatable sourceObject, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(sourceObject, cloneManager);
         var sourceSpatialStructure = sourceObject as IMoBiSpatialStructure;
         if (sourceSpatialStructure == null) return;

         this.UpdateDiagramFrom(sourceSpatialStructure);
      }
   }
}