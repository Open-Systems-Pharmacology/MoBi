using System.Collections.Generic;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;

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

      protected override IEnumerable<PathElement> DefaultPathElementsToUse(bool addTopContainerName, PathElements pathElements)
      {
         if (addTopContainerName || chartOptions.SimulationInCurveName)
            yield return PathElement.Simulation;

         //Observed data repository name is stored in PathElement.TopContainer. So if the top container name is to be displayed, we need to add it as well 
         if (addTopContainerName || chartOptions.TopContainerInCurveName)
            yield return PathElement.TopContainer;

         yield return PathElement.Container;
         yield return PathElement.BottomCompartment;
         yield return PathElement.Molecule;
         yield return PathElement.Name;
      }
   }
}