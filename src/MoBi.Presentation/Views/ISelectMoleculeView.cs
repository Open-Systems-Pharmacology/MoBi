using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectMoleculesView : IView<ISelectMoleculesPresenter>
   {
      void BindTo(IReadOnlyList<MoleculeSelectionDTO> dtoMolecules);
   }
}