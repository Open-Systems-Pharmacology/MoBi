using System.Drawing;
using System.Text;
using OSPSuite.UI.Extensions;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

namespace MoBi.UI
{
   public partial class AboutView : BaseModalView, IAboutView
   {
      private IAboutPresenter _presenter;
      public string VersionInformation { private get; set; }
      public string Product { private get; set; }
      public string LicenseAgreementFilePath { get; set; }

      public AboutView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IAboutPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void Display()
      {
         _labelInfo.Text = versionDescription();
         ShowDialog();
      }

      private string versionDescription()
      {
         var sb = new StringBuilder($"<B>{Product}</B>");
         sb.AppendFormat("\t{0}\n", VersionInformation);
         sb.AppendLine();
         return sb.ToString();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         licenseAgreementLink.OpenLink += (o, e) => OnEvent(openLicense, e);
      }

      private void openLicense(OpenLinkEventArgs e)
      {
         e.EditValue = LicenseAgreementFilePath;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.AboutProduct;
         _labelInfo.BackColor = Color.Transparent;
         _labelInfo.AsDescription();
         _websiteLink.Text = AppConstants.Website;
         licenseAgreementLink.Text = Captions.ReadLicenseAgreement;
         CancelVisible = false;
      }

      protected override void SetActiveControl()
      {
         ActiveControl = ButtonOk;
      }
   }
}