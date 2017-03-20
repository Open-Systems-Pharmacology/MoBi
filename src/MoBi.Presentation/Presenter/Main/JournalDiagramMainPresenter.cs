﻿using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Regions;

namespace MoBi.Presentation.Presenter.Main
{
   public class JournalDiagramMainPresenter : OSPSuite.Presentation.Presenters.Journal.JournalDiagramMainPresenter
   {
      public JournalDiagramMainPresenter(OSPSuite.Presentation.Views.Journal.IJournalDiagramMainView view, IJournalDiagramPresenter journaliDiagramPresenter, IRegionResolver regionResolver)
         : base(view, journaliDiagramPresenter, regionResolver, RegionNames.JournalDiagram)
      {
      }
   }
}
