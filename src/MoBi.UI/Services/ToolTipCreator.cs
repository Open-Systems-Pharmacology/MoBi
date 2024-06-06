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
      SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO, string cellValue = null);
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


      public SuperToolTip ToolTipFor(IMoBiParameterDTO parameterDTO, string cellValue = null)
      {
         var toolTip = CreateToolTip();

         toolTip = addToolTip(createCellValueToolTip(cellValue), toolTip);
         toolTip = addToolTip(CreateToolTip(parameterDTO.Description, parameterDTO.Name), toolTip);
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
         var toolTip = CreateToolTip(calculationMethod.Description, calculationMethod.CalculationMethodName);
         return toolTip;
      }

      private SuperToolTip createCellValueToolTip(string cellValue)
      {
         if (cellValue == string.Empty)
         {
            return CreateToolTip();
         }

         var newToolTip = CreateToolTip(string.Empty, cellValue);
         newToolTip.Items.AddSeparator();
         return newToolTip;
      }

      private SuperToolTip addToolTip(SuperToolTip toolTipToAdd, SuperToolTip toolTip)
      {
         foreach (BaseToolTipItem item in toolTipToAdd.Items)
         {
            toolTip.Items.Add(item);
         }

         return toolTip;
      }
   }
}