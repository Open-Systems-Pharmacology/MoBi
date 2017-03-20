using System.Linq;
using System.Text;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;

namespace MoBi.UI.Services
{
   public interface IToolTipCreator : OSPSuite.UI.Services.IToolTipCreator
   {
      SuperToolTip ToolTipFor(NotificationMessage notificationMessage);
      SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO);
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


      public SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO)
      {
         var toolTip = CreateToolTip(parameterDTO.Description, parameterDTO.Name);
         toolTip = addFormulaToolTip(parameterDTO.Formula, toolTip, AppConstants.Captions.Formula);
         return addFormulaToolTip(parameterDTO.RHSFormula, toolTip, AppConstants.Captions.RHSFormula);
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
            sb.AppendLine(string.Format("<I>{0}</I> is defined as: {1}", objectPath.Alias, objectPath.Path));
         }

         toolTip.WithText(sb.ToString());

         return toolTip;
      }

      public SuperToolTip ToolTipFor(UsedCalculationMethodDTO calculationMethod)
      {
         var toolTip = CreateToolTip(calculationMethod.Description, calculationMethod.CalculationMethodName);
         return toolTip;
      }
   }
}