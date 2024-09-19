using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_PathAndValuesToDataTableMapper : ContextSpecification<ParameterValuesToParameterValuesDataTableMapper>
   {
      protected IEnumerable<ParameterValue> _entities;
      protected List<DataTable> _result;

      protected override void Context()
      {
         sut = new ParameterValuesToParameterValuesDataTableMapper();
         _entities = getEntities();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_entities);
      }

      private IEnumerable<ParameterValue> getEntities()
      {
         var unit = new Unit("Dimensionless", 1.0, 0.0);

         var containerPath = new ObjectPath("the", "container", "path");
         var entity1 = new ParameterValue { Value = 0.1, ContainerPath = containerPath.Clone<ObjectPath>(), Name = "Parameter 1", DisplayUnit = unit };
         var entity2 = new ParameterValue { Value = null, ContainerPath = containerPath.Clone<ObjectPath>(), Name = "Parameter 2" };

         yield return entity1;
         yield return entity2;
      }
   }

   public class When_mapping_path_and_values_to_data_table : concern_for_PathAndValuesToDataTableMapper
   {
      [Observation]
      public void yields_correct_number_of_data_rows_excluding_null_values()
      {
         _result.First().Rows.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void maps_values_to_columns_correctly()
      {
         var row = _result.First().Rows[0];

         row.Field<string>(AppConstants.Captions.ContainerPath).ShouldBeEqualTo("the|container|path");
         row.Field<string>(AppConstants.Captions.ParameterName).ShouldBeEqualTo("Parameter 1");
         row.Field<double>(AppConstants.Captions.Value).ShouldBeEqualTo(0.1);
         row.Field<string>(AppConstants.Captions.Unit).ShouldBeEqualTo("Dimensionless");
      }

      [Observation]
      public void sets_column_ordinals_correctly()
      {
         var dt = _result.First();
         dt.Columns[AppConstants.Captions.ContainerPath].Ordinal.ShouldBeEqualTo(0);
         dt.Columns[AppConstants.Captions.ParameterName].Ordinal.ShouldBeEqualTo(1);
         dt.Columns[AppConstants.Captions.Value].Ordinal.ShouldBeEqualTo(2);
         dt.Columns[AppConstants.Captions.Unit].Ordinal.ShouldBeEqualTo(3);
      }
   }
}