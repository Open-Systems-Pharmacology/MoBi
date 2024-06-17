using System.Linq;
using System.Text;
using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Services
{
   public interface IToolTipCreator : OSPSuite.UI.Services.IToolTipCreator
   {
      SuperToolTip ToolTipFor(NotificationMessage notificationMessage);

      /// <summary>
      ///    Creates a tooltip for the <paramref name="parameterDTO" />. If given, the <paramref name="title" /> and
      ///    <paramref name="value" /> will be displayed first in the tooltip
      /// </summary>
      SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO, string title, string value);

      SuperToolTip ToolTipFor(UsedCalculationMethodDTO calculationMethod);
   }

   public class ToolTipCreator : OSPSuite.UI.Services.ToolTipCreator, IToolTipCreator
   {
      public SuperToolTip ToolTipFor(NotificationMessage notificationMessage)
      {
         var toolTip = CreateToolTip(notificationMessage.Message, notificationMessage.Type.ToString(),
            notificationMessage.Image);
         if (!notificationMessage.Details.Any())
            return toolTip;

         toolTip.Items.AddSeparator();
         toolTip.WithTitle(AppConstants.Captions.Details);
         notificationMessage.Details.Each(s => toolTip.WithText(s));
         return toolTip;
      }

      public SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO, string title, string value)
      {
         SuperToolTip toolTip;

         if (string.IsNullOrEmpty(title))
            toolTip = CreateToolTip(parameterDTO.Description, parameterDTO.Name);
         else
         {
            toolTip = CreateToolTip(value, title);
            toolTip.Items.AddSeparator();
            toolTip = addParameterNameToolTip(parameterDTO.Name, parameterDTO.Description, toolTip);
         }

         toolTip = addFormulaToolTip(parameterDTO.Formula, toolTip, AppConstants.Captions.Formula);
         toolTip = addFormulaToolTip(parameterDTO.RHSFormula, toolTip, AppConstants.Captions.RHSFormula);
         return toolTip;
      }

      private static SuperToolTip addFormulaToolTip(FormulaBuilderDTO formula, SuperToolTip toolTip, string title)
      {
         if (formula == null || formula.FormulaType != ObjectTypes.ExplicitFormula)
            return toolTip;

         toolTip.Items.AddSeparator();
         toolTip.WithTitle(title);
         toolTip.WithText(formula.FormulaString);

         toolTip.Items.AddSeparator();
         toolTip.WithTitle(AppConstants.Captions.References);

         var sb = new StringBuilder();
         foreach (var objectPath in formula.ObjectPaths)
         {
            sb.AppendLine($"<I>{objectPath.Alias}</I> is defined as: {objectPath.Path}");
         }

         toolTip.WithText(sb.ToString());

         return toolTip;
      }

      public SuperToolTip ToolTipFor(UsedCalculationMethodDTO calculationMethod)
      {
         return CreateToolTip(calculationMethod.Description, calculationMethod.CalculationMethodName);
      }

      private SuperToolTip addToolTip(SuperToolTip toolTipToAdd, SuperToolTip toolTip)
      {
         toolTipToAdd.Items.Cast<BaseToolTipItem>().ToList().ForEach(x => toolTip.Items.Add(x));
         return toolTip;
      }

      // Creates a 3-line tooltip with the literal "Parameter" in bold, followed by the parameter-name, followed by the description.
      private SuperToolTip addParameterNameToolTip(string name, string description, SuperToolTip toolTip)
      {
         var tt = CreateToolTip(name, ObjectTypes.Parameter);
         tt.Items.Add(description);
         return addToolTip(tt, toolTip);
      }
   }
}