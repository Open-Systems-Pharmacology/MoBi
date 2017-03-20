using OSPSuite.Utility.Format;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.Formatters
{
   public static class FormatterExtensions
   {
      public static IFormatter<double?> MoleculeStartValueFormatter(this MoleculeStartValueDTO moleculeStartValueDTO)
      {
         return new StartValueFormatter(moleculeStartValueDTO);
      }

      public static IFormatter<double?> ParameterStartValueFormatter(this ParameterStartValueDTO parameterStartValueDTO)
      {
         return new StartValueFormatter(parameterStartValueDTO);
      }

      public static IFormatter<double> ValuePointXFormatter(this DTOValuePoint dtoValuePoint)
      {
         return new ValuePointFormatter(dtoValuePoint.X);
      }

      public static IFormatter<double> ValuePointYFormatter(this DTOValuePoint dtoValuePoint)
      {
         return new ValuePointFormatter(dtoValuePoint.Y);
      }
   }
}