using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectCommitTargetView : IModalView<ISelectCommitTargetPresenter>
   {
      void BindModules(IEnumerable<ListItemDTO<Module>> modules, Module selectedModule);
      void BindParameterValues(IEnumerable<ListItemDTO<ParameterValuesBuildingBlock>> parameterValues, ParameterValuesBuildingBlock selectedParameterValues);
      Module SelectedModule { get; }
      ParameterValuesBuildingBlock SelectedParameterValues { get; }
      void SetDescription(string description);
   }
}
