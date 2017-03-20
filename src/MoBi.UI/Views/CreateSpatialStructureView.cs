using MoBi.Presentation.Views;
using OSPSuite.Presentation;

namespace MoBi.UI.Views
{
   public class CreateSpatialStructureView : ModalForm, ICreateSpatialStructureView
   {
      protected override int TopicId => HelpId.MoBi_ModelBuilding_CreatingSpatialStructure;
   }
}