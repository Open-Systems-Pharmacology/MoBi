using System;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IInitialConditionsBuildingBlockToDataTableMapper : IMapper<InitialConditionsBuildingBlock, DataTable>
   {
   }

   public class InitialConditionsBuildingBlockToDataTableMapper : IInitialConditionsBuildingBlockToDataTableMapper
   {
      private static readonly string _path = AppConstants.Captions.ContainerPath;
      private static readonly string _moleculeName = AppConstants.Captions.MoleculeName;
      private static readonly string _isPresent = AppConstants.Captions.IsPresent;
      private static readonly string _value = AppConstants.Captions.Value;
      private static readonly string _unit = AppConstants.Captions.Unit;
      private static readonly string _scaleDivisor = AppConstants.Captions.ScaleDivisor;
      private static readonly string _negativeValuesAllowed = AppConstants.Captions.NegativeValuesAllowed;

      public DataTable MapFrom(InitialConditionsBuildingBlock input) =>
         parameterValuesToParametersDataTable(input);

      private DataTable parameterValuesToParametersDataTable(InitialConditionsBuildingBlock input)
      {
         var dt = generateEmptyMoleculeParameterDataTable();
         var parameterValues = input.Select(x => x);
         foreach (var parameterValue in parameterValues)
         {
            var row = dt.Rows.Add();
            row[_path] = parameterValue.ContainerPath;
            row[_moleculeName] = parameterValue.Name;
            row[_isPresent] = parameterValue.IsPresent;
            row[_value] = parameterValue.Value != null ? (object)parameterValue.ConvertToDisplayUnit(parameterValue.Value) : DBNull.Value;
            row[_scaleDivisor] = parameterValue.ScaleDivisor;
            row[_unit] = parameterValue.DisplayUnit;
            row[_negativeValuesAllowed] = parameterValue.NegativeValuesAllowed;
         }

         return dt;
      }

      private DataTable generateEmptyMoleculeParameterDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_path);
         dt.AddColumn<string>(_moleculeName);
         dt.AddColumn<bool>(_isPresent);
         dt.AddColumn<double>(_value);
         dt.AddColumn(_scaleDivisor);
         dt.AddColumn(_unit);
         dt.AddColumn<bool>(_negativeValuesAllowed);
         dt.TableName = AppConstants.Captions.ParameterValue;
         return dt;
      }
   }
}