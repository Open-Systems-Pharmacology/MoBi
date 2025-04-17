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
      SuperToolTip ToolTipFor(ParameterDTO parameterDTO, string title, string value);

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

      public SuperToolTip ToolTipFor(ParameterDTO parameterDTO, string title, string value)
      {
         SuperToolTip toolTip;

         if (string.IsNullOrEmpty(title) || string.Equals(value, parameterDTO.Name))
            toolTip = CreateToolTip(parameterDTO.Description, parameterDTO.Name);
         else
         {
            toolTip = CreateToolTip(value, title);
            addParameterNameToolTip(parameterDTO.Name, parameterDTO.Description, toolTip);
         }

         addParameterReferenceTooltip(parameterDTO, toolTip);

         addFormulaToolTip(parameterDTO.Formula, toolTip, AppConstants.Captions.Formula);
         addFormulaToolTip(parameterDTO.RHSFormula, toolTip, AppConstants.Captions.RHSFormula);

         return toolTip;
      }

      private static void addFormulaToolTip(FormulaBuilderDTO formula, SuperToolTip toolTip, string title)
      {
         if (formula == null || formula.FormulaType != ObjectTypes.ExplicitFormula)
            return;

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
      }

      public SuperToolTip ToolTipFor(UsedCalculationMethodDTO calculationMethod)
      {
         return CreateToolTip(calculationMethod.Description, calculationMethod.CalculationMethodName);
      }

      // Creates a 3-line tooltip with the literal "Parameter" in bold, followed by the parameter-name, followed by the description.
      private void addParameterNameToolTip(string parameterName, string description, SuperToolTip toolTip)
      {
         toolTip.Items.AddSeparator();
         toolTip.WithTitle(ObjectTypes.Parameter);
         var nameItem= toolTip.Items.Add(parameterName);
         nameItem.LeftIndent = 6;
         toolTip.WithText(description);
      }

      private static void addParameterReferenceTooltip(ParameterDTO parameterDTO, SuperToolTip toolTip)
      {
         if (parameterDTO.SourceReference == null)
            return;

         toolTip.Items.AddSeparator();
         toolTip.WithTitle(AppConstants.Captions.Source);
         var sb = new StringBuilder();
         if(!string.IsNullOrEmpty(parameterDTO.ModuleName))
            sb.AppendLine($"{AppConstants.Captions.Module}: {parameterDTO.ModuleName}");

         sb.AppendLine($"{AppConstants.Captions.BuildingBlock}: {parameterDTO.BuildingBlockName}");
         sb.AppendLine($"{AppConstants.Captions.Source}: {parameterDTO.SourceName}");

         toolTip.WithText(sb.ToString());
      }
   }
}