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
   public interface IInitialConditionsBuildingBlockToDataTableMapper : IMapper<InitialConditionsBuildingBlock, List<DataTable>>
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

      public List<DataTable> MapFrom(InitialConditionsBuildingBlock input) =>
         parameterValuesToParametersDataTable(input);

      private List<DataTable> parameterValuesToParametersDataTable(InitialConditionsBuildingBlock input)
      {
         var dt = generateEmptyMoleculeParameterDataTable();
         var parameterValues = input.Select(x => x).Where(x => x.Value != null); 
         foreach (var parameterValue in parameterValues)
         {
            var row = dt.Rows.Add();
            row[_path] = parameterValue.ContainerPath;
            row[_moleculeName] = parameterValue.Name;
            row[_isPresent] = parameterValue.IsPresent;
            row[_value] = (object)parameterValue.ConvertToDisplayUnit(parameterValue.Value);
            row[_scaleDivisor] = parameterValue.ScaleDivisor;
            row[_unit] = parameterValue.DisplayUnit;
            row[_negativeValuesAllowed] = parameterValue.NegativeValuesAllowed;
         }

         return new List<DataTable> { dt };
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
         dt.TableName = AppConstants.Captions.InitialConditions;
         return dt;
      }
   }
}