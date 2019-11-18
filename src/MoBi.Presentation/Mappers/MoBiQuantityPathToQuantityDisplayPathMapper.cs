using System.Collections.Generic;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;

namespace MoBi.Presentation.Mappers
{
   public class MoBiQuantityPathToQuantityDisplayPathMapper : QuantityPathToQuantityDisplayPathMapper
   {
      private readonly IUserSettings _userSettings;
      private ChartOptions chartOptions => _userSettings.ChartOptions;

      public MoBiQuantityPathToQuantityDisplayPathMapper(IObjectPathFactory objectPathFactory, IPathToPathElementsMapper pathToPathElementsMapper, IDataColumnToPathElementsMapper dataColumnToPathElementsMapper, IUserSettings userSettings) :
         base(objectPathFactory, pathToPathElementsMapper, dataColumnToPathElementsMapper)
      {
         _userSettings = userSettings;
      }

      protected override IEnumerable<PathElementId> DefaultPathElementsToUse(bool addTopContainerName, PathElements pathElements)
      {
         if (addTopContainerName || chartOptions.SimulationInCurveName)
            yield return PathElementId.Simulation;

         //Observed data repository name is stored in PathElementId.TopContainer. So if the top container name is to be displayed, we need to add it as well 
         if (addTopContainerName || chartOptions.TopContainerInCurveName)
            yield return PathElementId.TopContainer;

         yield return PathElementId.Container;
         yield return PathElementId.BottomCompartment;
         yield return PathElementId.Molecule;
         yield return PathElementId.Name;
      }
   }
}