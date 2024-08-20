using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using FakeItEasy;
using MoBi.UI.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation
{
   public class concern_for_EditSpatialStructureViewSpecs :
      ContextSpecification<EditSpatialStructureView>
   {
      protected EditSpatialStructureView _sut;
      protected SplitContainerControl _splitContainer;

      protected override void Context()
      {
         _sut = new EditSpatialStructureView(A.Fake<IMainView>());
         _splitContainer = new SplitContainerControl();

         var splitHierarchyEditField = typeof(EditSpatialStructureView).GetField("splitHierarchyEdit", BindingFlags.NonPublic | BindingFlags.Instance);
         splitHierarchyEditField.SetValue(_sut, _splitContainer);
         _splitContainer.Panel2.Controls.Add(new Control());
      }
   }

   public class set_edit_view_with_null_value_should_remove_control : concern_for_EditSpatialStructureViewSpecs
   {
      protected override void Because()
      {
         _sut.SetEditView(null);
      }

      [Observation]
      public void should_remove_control()
      {
         _splitContainer.Panel2.Controls.Count.ShouldBeEqualTo(0);
      }
   }

   public class set_edit_view_with_value_should_not_remove_control : concern_for_EditSpatialStructureViewSpecs
   {
      protected override void Because()
      {
         _sut.SetEditView(A.Fake<IView>());
      }

      [Observation]
      public void should_not_remove_control()
      {
         _splitContainer.Panel2.Controls.Count.ShouldBeEqualTo(1);
      }
   }
}