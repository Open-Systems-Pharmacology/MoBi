﻿using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{

   public interface IEditFormulaPathListView : IView<IEditFormulaPathListPresenter>
   {
      bool ReadOnly { get; set; }
      void BindTo(IReadOnlyList<FormulaUsablePathDTO> formulaUsablePathDTOs);
   }
}