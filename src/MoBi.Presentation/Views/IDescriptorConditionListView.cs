using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IDescriptorConditionListView : IView<IDescriptorConditionListPresenter>
   {
      void BindTo(DescriptorCriteriaDTO descriptorCriteriaDTO);
      string CriteriaDescription { set; }
   }
}