using System;
using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IMoleculeStartValuesBuildingBlockToParameterDataTableMapper
   {
      /// <summary>
      /// Maps all molecule start values in a building block to a datatable.
      /// </summary>
      /// <param name="moleculeStartValues">The molecule start values being mapped</param>
      /// <param name="moleculeBuilders">The original builder building block. This may be used to look up information not contained in the start value, but needed for the data table</param>
      /// <returns>The data table</returns>
      DataTable MapFrom(IEnumerable<IMoleculeStartValue> moleculeStartValues, IEnumerable<IMoleculeBuilder> moleculeBuilders);
   }

   public class MoleculeStartValuesBuildingBlockToParameterDataTableMapper : IMoleculeStartValuesBuildingBlockToParameterDataTableMapper
   {
      public DataTable MapFrom(IEnumerable<IMoleculeStartValue> moleculeStartValues, IEnumerable<IMoleculeBuilder> moleculeBuilders)
      {
         var dt = generateEmptyMoleculeParameterDataTable();
         moleculeStartValues.Each(moleculeStartValue => moleculeStartValueToParametersDataTable(moleculeStartValue, dt, moleculeBuilders.FindDescriptionForStartValueFromBuilder(moleculeStartValue.Name)));
         return dt;
      }

      private static readonly string _name = AppConstants.Captions.Name;
      private static readonly string _path = AppConstants.Captions.Path;
      private static readonly string _initialValue = AppConstants.Captions.InitialValue;
      private static readonly string _formula = AppConstants.Captions.Formula;
      private static readonly string _unit = AppConstants.Captions.Unit;
      private static readonly string _scaleDivisor = AppConstants.Captions.ScaleDivisor;
      private const string _description = AppConstants.Captions.Description;

      private void moleculeStartValueToParametersDataTable(IMoleculeStartValue moleculeStartValue, DataTable dt, string description)
      {
         var row = dt.Rows.Add();
         row[_name] = moleculeStartValue.Name;
         row[_path] = moleculeStartValue.ContainerPath;
         row[_initialValue] = moleculeStartValue.StartValue != null ? (object) moleculeStartValue.ConvertToDisplayUnit(moleculeStartValue.StartValue) : DBNull.Value; 
         row[_formula] = moleculeStartValue.Formula ?? (object) String.Empty; 
         row[_unit] = moleculeStartValue.DisplayUnit;
         row[_scaleDivisor] = moleculeStartValue.ScaleDivisor;
         row[_description] = moleculeStartValue.Description.IsNullOrEmpty() ? description : moleculeStartValue.Description;
      }

      private DataTable generateEmptyMoleculeParameterDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn<string>(_name);

         dt.AddColumn(_path);
         dt.AddColumn<double>(_initialValue);
         dt.AddColumn(_formula);
         dt.AddColumn(_unit);
         dt.AddColumn<double>(_scaleDivisor);
         dt.AddColumn(_description);
         dt.TableName = AppConstants.Captions.MoleculeStartValues;
         return dt;
      }
   }
}