using System.Data;
using MoBi.Assets;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class ImportedQuantityDTO : IWithDisplayUnit
   {
      private Warning _warning;
      public Unit DisplayUnit { get; set; }
      public bool IsPresent { get; set; }
      public double QuantityInBaseUnit { get; set; }
      public ObjectPath ContainerPath { get; set; }
      public string Name { get; set; }
      public IDimension Dimension { get; set; }
      public double ScaleDivisor { get; set; }
      public bool NegativeValuesAllowed { get; set; }
      public bool IsScaleDivisorSpecified { get; set; }

      public bool IsQuantitySpecified { get; set; }

      /// <summary>
      ///    Set this value to true to skip the import step.
      ///    The DTO should be kept in case of a warning message
      /// </summary>
      public bool SkipImport { set; get; }

      public ObjectPath Path => ContainerPath.Clone<ObjectPath>().AndAdd(Name);

      public void SetWarning(DataRow row, int rowIndex, string suggestion)
      {
         _warning = new Warning {Row = row, RowIndex = rowIndex, Suggestion = suggestion};
      }

      public bool HasWarning()
      {
         return _warning != null;
      }

      public string GetWarning()
      {
         return HasWarning() ? _warning.ToString() : string.Empty;
      }

      private class Warning
      {
         public DataRow Row { private get; set; }
         public int RowIndex { private get; set; }
         public string Suggestion { private get; set; }

         public override string ToString()
         {
            return AppConstants.Warnings.FormatAsStartValueImportWarning(RowIndex, Row.ToNiceString(), Suggestion);
         }
      }
   }
}