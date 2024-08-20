using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IParameterValueBuildingBlockToParameterValuesDataTableMapper : IMapper<ParameterValuesBuildingBlock, List<DataTable>>
   {
   }

   public class ParameterValueBuildingBlockToParameterValuesDataTableMapper : IParameterValueBuildingBlockToParameterValuesDataTableMapper
   {
      private static readonly string _name = AppConstants.Captions.ParameterName;
      private static readonly string _path = AppConstants.Captions.ContainerPath;
      private static readonly string _value = AppConstants.Captions.Value;
      private static readonly string _unit = AppConstants.Captions.Unit;

      public List<DataTable> MapFrom(ParameterValuesBuildingBlock input) =>
         parameterValuesToParametersDataTable(input);

      private List<DataTable> parameterValuesToParametersDataTable(ParameterValuesBuildingBlock input)
      {
         var dt = generateEmptyMoleculeParameterDataTable();
         var parameterValues = input.Select(x => x).Where(x=> x.Value != null);
         foreach (var parameterValue in parameterValues)
         {
            var row = dt.Rows.Add();
            row[_path] = parameterValue.ContainerPath;
            row[_name] = parameterValue.Name;
            row[_value] = (object)parameterValue.ConvertToDisplayUnit(parameterValue.Value);
            row[_unit] = parameterValue.DisplayUnit;
         }

         return new List<DataTable> { dt };
      }

      private DataTable generateEmptyMoleculeParameterDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_path);
         dt.AddColumn<string>(_name);
         dt.AddColumn<double>(_value);
         dt.AddColumn(_unit);
         dt.TableName = AppConstants.Captions.ParameterValue;
         return dt;
      }
   }
}