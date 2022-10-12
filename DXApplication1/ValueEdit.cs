using System;
using System.Windows.Forms;

namespace DXApplication1
{
   public partial class ValueEdit : UserControl
   {


      /// <summary>
      ///    Event is raised whenever a value is being changed in the user control
      /// </summary>
      public event Action Changing = delegate { };

      /// <summary>
      ///    Event is raised whenever a value has changed
      /// </summary>
      public event Action Changed = delegate { };

      public ValueEdit()
      {
         InitializeComponent();
         InitializeBinding();
      }

      public void InitializeBinding()
      {
        

         tbValue.EnterMoveNextControl = true;
      }


      public string ToolTip
      {
         set => tbValue.ToolTip = value;
         get => tbValue.ToolTip;
      }
      public virtual string Caption
      {
         get => this.Text;
         set
         {
            this.Text = value;

         }
      }
   }
}