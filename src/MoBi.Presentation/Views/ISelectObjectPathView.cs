﻿using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectObjectPathView : IModalView<ISelectObjectPathPresenter>
   {
      void BindTo(IEnumerable<ObjectBaseDTO> dtos);
      ObjectBaseDTO Selected { get; }
      ITreeNode GetNode(string id);
   }
}