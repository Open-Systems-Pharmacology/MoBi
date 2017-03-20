using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IBuildingBlockMergeView : IView<IBuildingBlockMergePresenter>
   {
      string SimulationFile {  set; }
      void BindTo(IEnumerable<BuildingBlockMappingDTO> allBuildingBlockMappings);
   }
}