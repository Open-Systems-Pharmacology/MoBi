using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_ParameterListToSimulationParameterDataTableMapper : ContextSpecification<ParameterListToSimulationParameterDataTableMapper>
   {
      protected IMoBiSimulation _simultion;
      protected DataTable _result;

      protected override void Context()
      {
         sut = new ParameterListToSimulationParameterDataTableMapper(
            new EntityPathResolver(
               new ObjectPathFactory(
                  new AliasCreator())));

         _simultion = A.Fake<IMoBiSimulation>();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simultion.Model.Root.GetAllChildren<IParameter>());
      }
   }

   public class when_mapping_multiple_parameters : concern_for_ParameterListToSimulationParameterDataTableMapper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simultion.Model.Root.GetAllChildren<IParameter>()).Returns(generateParameters());
      }

      [Observation]
      public void yields_table_with_correct_number_of_rows()
      {
         _result.Rows.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_correct_data_in_columns()
      {
         var row = _result.Rows[0];

         row.Field<string>(AppConstants.Captions.Path).ShouldBeEqualTo("path1|path2");
         row.Field<string>(AppConstants.Captions.Name).ShouldBeEqualTo("name");
         row.Field<double>(AppConstants.Captions.Value).ShouldBeEqualTo(2.0);
         row.Field<string>(AppConstants.Captions.Formula).ShouldBeEqualTo("F=MA");
         row.Field<string>(AppConstants.RHSFormula).ShouldBeEqualTo("i*i=-1");
         row.Field<string>(AppConstants.Captions.Unit).ShouldBeEqualTo("g");
         row.Field<string>(AppConstants.Captions.Description).ShouldBeEqualTo("description");
      }

      private IReadOnlyList<IParameter> generateParameters()
      {
         return new List<IParameter>
         {
            new Parameter
            {
               ParentContainer = new Container {Name = "path2", ParentContainer = new Container{Name="path1"}},
               Name="name",
               Value = 2.0,
               Formula = new ExplicitFormula("F=MA"),
               RHSFormula = new ExplicitFormula("i*i=-1"),
               DisplayUnit = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass).DefaultUnit,
               Description = "description"
            },
            new Parameter()
         };
      }
   }
}
