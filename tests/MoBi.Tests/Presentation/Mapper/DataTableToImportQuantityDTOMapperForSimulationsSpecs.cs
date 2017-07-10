using System.Data;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_DataTableToImportQuantityDTOMapperForSimulations : ContextSpecificationForImportStartValues<DataTableToImportQuantityDTOMapperForSimulations>
   {
      protected IMoBiSimulation _simulation;
      protected QuantityImporterDTO _result;
      protected DataTable _dataTable;

      protected override void Context()
      {
         CreateDimensionFactory();
         sut = new DataTableToImportQuantityDTOMapperForSimulations(_dimensionFactory);
         _simulation = A.Fake<IMoBiSimulation>();
         _dataTable = new DataTableProvider().ImportTables();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataTable, _simulation);
      }
   }

   public abstract class context_for_validating_for_update : concern_for_DataTableToImportQuantityDTOMapperForSimulations
   {
      protected override void Context()
      {
         base.Context();
         var leafContainer = newContainerNamed("This").WithChildContainerNamed("Is").WithChildContainerNamed("The").WithChildContainerNamed("Path").WithChildContainerNamed("0");
         leafContainer.Add(new Parameter {Name = "ParameterName"});
         var root = new Container();
         root.Add(leafContainer.RootContainer);
         A.CallTo(() => _simulation.Model.Root).Returns(root);
      }

      private static Container newContainerNamed(string name)
      {
         return new Container {Name = name};
      }
   }

   public class When_validating_for_update_without_start_value_specified : context_for_validating_for_update
   {
      protected override void Context()
      {
         base.Context();
         _dataTable.Rows[0][2] = string.Empty;
      }

      [Observation]
      public void should_not_import_any_rows_When_a_mandatory_value_is_missing_from_one_row()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_validating_for_update : context_for_validating_for_update
   {
      [Observation]
      public void should_add_one_value_and_warn_about_two_values_that_cannot_be_found_in_the_simulation()
      {
         _result.QuantitDTOs.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void all_but_one_inserted_start_value_dto_should_have_warning()
      {
         _result.Log.Count(message => message.EndsWith("Only updates are allowed")).ShouldBeEqualTo(_dataTable.Rows.Count - 1);
      }
   }

   public class When_validating_for_insert : concern_for_DataTableToImportQuantityDTOMapperForSimulations
   {
      [Observation]
      public void no_new_values_can_be_inserted()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }

      [Observation]
      public void all_inserted_start_value_dto_should_have_warning()
      {
         _result.Log.Count(message => message.EndsWith("Only updates are allowed")).ShouldBeEqualTo(_dataTable.Rows.Count);
      }
   }
}