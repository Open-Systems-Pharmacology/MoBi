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
   public abstract class concern_for_InitialConditionsBuildingBlockToDataTableMapper : ContextSpecification<InitialConditionsToDataTableMapper>
   {
      protected IEnumerable<InitialCondition> _entities;
      protected List<DataTable> _result;

      protected override void Context()
      {
         sut = new InitialConditionsToDataTableMapper();
         _entities = getEntities();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_entities);
      }

      private IEnumerable<InitialCondition> getEntities()
      {
         var unit = new Unit("Dimensionless", 1.0, 0.0);

         var containerPath = new ObjectPath("the", "container", "path");
         var entity1 = new InitialCondition { Value = 0.1, ContainerPath = containerPath.Clone<ObjectPath>(), Name = "Initial Condition 1", DisplayUnit = unit, IsPresent = true, ScaleDivisor = 1, NegativeValuesAllowed = true };
         var entity2 = new InitialCondition { Value =null, ContainerPath = containerPath.Clone<ObjectPath>(), Name = "Initial Condition 2", DisplayUnit = unit, IsPresent = false, ScaleDivisor = 2, NegativeValuesAllowed = false };
         var entity3 = new InitialCondition { Value = 0.1, ContainerPath = containerPath.Clone<ObjectPath>(), Name = "Initial Condition 3", DisplayUnit = unit, IsPresent = true, ScaleDivisor = 3, NegativeValuesAllowed = false };

         yield return entity1;
         yield return entity2;
         yield return entity3;
      }
   }

   public class When_mapping_initial_conditions_to_data_table : concern_for_InitialConditionsBuildingBlockToDataTableMapper
   {
      [Observation]
      public void yields_correct_number_of_data_rows_excluding_null_values()
      {
         _result.First().Rows.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void maps_values_to_columns_correctly()
      {
         var row = _result.First().Rows[0];

         row.Field<string>(AppConstants.Captions.ContainerPath).ShouldBeEqualTo("the|container|path");
         row.Field<string>(AppConstants.Captions.MoleculeName).ShouldBeEqualTo("Initial Condition 1");
         row.Field<double>(AppConstants.Captions.Value).ShouldBeEqualTo(0.1);
         row.Field<string>(AppConstants.Captions.Unit).ShouldBeEqualTo("Dimensionless");
         row.Field<string>(AppConstants.Captions.ScaleDivisor).ShouldBeEqualTo("1");
         row.Field<bool>(AppConstants.Captions.IsPresent).ShouldBeTrue();
         row.Field<bool>(AppConstants.Captions.NegativeValuesAllowed).ShouldBeTrue();
      }

      [Observation]
      public void sets_column_ordinals_correctly()
      {
         var dt = _result.First();

         dt.Columns[AppConstants.Captions.ContainerPath].Ordinal.ShouldBeEqualTo(0);
         dt.Columns[AppConstants.Captions.MoleculeName].Ordinal.ShouldBeEqualTo(1);
         dt.Columns[AppConstants.Captions.IsPresent].Ordinal.ShouldBeEqualTo(2);
         dt.Columns[AppConstants.Captions.Value].Ordinal.ShouldBeEqualTo(3);
         dt.Columns[AppConstants.Captions.Unit].Ordinal.ShouldBeEqualTo(4);
         dt.Columns[AppConstants.Captions.ScaleDivisor].Ordinal.ShouldBeEqualTo(5);
         dt.Columns[AppConstants.Captions.NegativeValuesAllowed].Ordinal.ShouldBeEqualTo(6);
      }
   }
}