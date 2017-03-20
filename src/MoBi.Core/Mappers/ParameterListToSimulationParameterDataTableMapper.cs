using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IParameterListToSimulationParameterDataTableMapper : IMapper<IReadOnlyList<IParameter>, DataTable>
   {

   }

   public class ParameterListToSimulationParameterDataTableMapper : IParameterListToSimulationParameterDataTableMapper
   {
      private readonly IEntityPathResolver _resolver;
      private static readonly string _path = AppConstants.Captions.Path;
      private static readonly string _name = AppConstants.Captions.Name;
      private static readonly string _value = AppConstants.Captions.Value;
      private static readonly string _formula = AppConstants.Captions.Formula;
      private static readonly string _unit = AppConstants.Captions.Unit;
      private const string _description = AppConstants.Captions.Description;
      private static readonly string _rhsFormula = AppConstants.RHSFormula;

      public ParameterListToSimulationParameterDataTableMapper(IEntityPathResolver resolver)
      {
         _resolver = resolver;
      }

      public DataTable MapFrom(IReadOnlyList<IParameter> simulation)
      {
         var dt = generateEmptySimulationParameterDataTable();
         simulation.Each(parameter => addParametersToDataTable(parameter, dt));

         return dt;
      }

      private void addParametersToDataTable(IParameter parameter, DataTable dt)
      {
         var path = _resolver.ObjectPathFor(parameter);
         path.RemoveAt(path.Count - 1);
         var row = dt.Rows.Add();
         row[_path] = path;
         row[_name] = parameter.Name;
         row[_value] = parameter.ConvertToDisplayUnit(parameter.Value);
         row[_formula] = (parameter.Formula.IsConstant() || parameter.Formula == null) ? (object) string.Empty : parameter.Formula;
         row[_rhsFormula] = parameter.RHSFormula ?? (object) string.Empty;
         row[_unit] = parameter.DisplayUnit;
         row[_description] = parameter.Description;
      }

      private DataTable generateEmptySimulationParameterDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_path);
         dt.AddColumn(_name);
         dt.AddColumn<double>(_value);
         dt.AddColumn(_formula);
         dt.AddColumn(_rhsFormula);
         dt.AddColumn(_unit);
         dt.AddColumn(_description);
         dt.TableName = AppConstants.Captions.Parameters;
         return dt;
      }
   }
}