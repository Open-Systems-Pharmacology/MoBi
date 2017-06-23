using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_MoleculeStartValuesBuildingBlockToParameterDataTableMapper : ContextSpecification<MoleculeStartValuesBuildingBlockToParameterDataTableMapper>
   {
      protected DataTable _result;
      protected IMoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      protected IMoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         sut = new MoleculeStartValuesBuildingBlockToParameterDataTableMapper();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moleculeStartValuesBuildingBlock, _moleculeBuildingBlock);
      }
   }

   public class when_mapping_table_with_n_start_values : concern_for_MoleculeStartValuesBuildingBlockToParameterDataTableMapper
   {
      protected override void Context()
      {
         base.Context();
         getMoleculeStartValues().Each(_moleculeStartValuesBuildingBlock.Add);

         _moleculeBuildingBlock.Add(new MoleculeBuilder {Name="name1", Description = "description1"});
      }

      [Observation]
      public void yields_two_row_datatable()
      {
         _result.Rows.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void row_1_contains_right_values()
      {
         var row = _result.Rows[0];

         row.Field<string>(AppConstants.Captions.Name).ShouldBeEqualTo("name");
         row.Field<string>(AppConstants.Captions.Description).ShouldBeEqualTo("description");
         row.Field<string>(AppConstants.Captions.Unit).ShouldBeEqualTo("g");
         row.Field<string>(AppConstants.Captions.Formula).ShouldBeEqualTo(string.Empty);
         row.Field<string>(AppConstants.Captions.Path).ShouldBeEqualTo("path1|path2");
         row.Field<double>(AppConstants.Captions.ScaleDivisor).ShouldBeEqualTo(1.0);
         row.Field<double>(AppConstants.Captions.InitialValue).ShouldBeEqualTo(5.0);
      }

      [Observation]
      public void row_2_contains_right_values()
      {
         var row = _result.Rows[1];

         row.Field<string>(AppConstants.Captions.Name).ShouldBeEqualTo("name1");
         row.Field<string>(AppConstants.Captions.Description).ShouldBeEqualTo("description1");
         row.Field<string>(AppConstants.Captions.Unit).ShouldBeEqualTo("mol");
         row.Field<string>(AppConstants.Captions.Formula).ShouldBeEqualTo("M/V");
         row.Field<string>(AppConstants.Captions.Path).ShouldBeEqualTo("path3|path4");
         row.Field<double>(AppConstants.Captions.ScaleDivisor).ShouldBeEqualTo(4.3);
         row.Field<double>(AppConstants.Captions.InitialValue).ShouldBeEqualTo(7.0);
      }

      private static IEnumerable<MoleculeStartValue> getMoleculeStartValues()
      {
         yield return new MoleculeStartValue
         {
            ContainerPath = new ObjectPath("path1", "path2"), 
            Name = "name",
            Description = "description",
            Dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass),
            DisplayUnit = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass).DefaultUnit,
            Formula = null,
            IsPresent = true,
            StartValue = 5.0
         };

         yield return new MoleculeStartValue
         {
            ContainerPath = new ObjectPath("path3", "path4"),
            Name = "name1",
            Description = "",
            Dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration),
            DisplayUnit = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration).DefaultUnit,
            Formula = new ExplicitFormula("M/V"),
            IsPresent = true,
            ScaleDivisor = 4.3,
            StartValue = 7.0
         };
      }
   }
}
