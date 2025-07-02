using System.Linq;
using FakeItEasy;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Engine.Sbml;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Container;
using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
   public abstract class UnitImporterSpecs : ContextForSBMLIntegration<IUnitDefinitionImporter>
   {
      public Model _sbmlModel;

      protected override void Context()
      {
         base.Context();
         _sbmlModel = new Model(3, 1);
      }
   }

   public class UnitDefinitionImporterTests : UnitImporterSpecs
   {
      private Unit _unit;
      private UnitDefinition _unitDef;
      private Unit _unit2;
      private UnitDefinition _unitDef2;
      private Unit _unit3;
      private UnitDefinition _unitDef3;

      protected override void Because()
      {
         sut = IoC.Resolve<IUnitDefinitionImporter>();
         //create unit "substance"
         _unit = new Unit(3, 1);
         _unit.setExponent(1);
         _unit.setMultiplier(1.0);
         _unit.setScale(0);
         _unit.setKind(libsbml.UNIT_KIND_MOLE);

         //create unitdef "substance"
         _unitDef = _sbmlModel.createUnitDefinition();
         _unitDef.setId("substance");
         _unitDef.setName("substance");
         _unitDef.addUnit(_unit);

         //create unit "volume"
         _unit2 = _sbmlModel.createUnit();
         _unit2.setExponent(1);
         _unit2.setMultiplier(1.0);
         _unit2.setScale(0);
         _unit2.setKind(libsbml.UnitKind_forName("volume"));

         //create unitdef "volume"
         _unitDef2 = _sbmlModel.createUnitDefinition();
         _unitDef2.setId("volume");
         _unitDef2.setName("volume");
         _unitDef2.addUnit(_unit2);

         _unit3 = _sbmlModel.createUnit();
         _unit3.setExponent(-1);
         _unit3.setMultiplier(1 / 86400.0); //86400 seconds in one day
         _unit3.setScale(0);
         _unit3.setKind(libsbml.UnitKind_forName("second"));

         _unitDef3 = _sbmlModel.createUnitDefinition();
         _unitDef3.setId("inverse_day");
         _unitDef3.setName("1/d");
         _unitDef3.addUnit(_unit3);


         _sbmlModel.addUnitDefinition(_unitDef);
         sut.DoImport(_sbmlModel, new Module(), A.Fake<SBMLInformation>(), new MoBiMacroCommand());
      }

      [Observation]
      public void NewDimensionsTests()
      {
         sut.ToMobiBaseUnit("substance", 3e6).value.ShouldBeEqualTo(3e12);
         sut.ToMobiBaseUnit("inverse_day", 10).value.ShouldBeEqualTo(10.0 / 1440); //base unit is in minutes so 1440 minutes in one day
      }
   }

   public class UnitTests : UnitImporterSpecs
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("UnitTestFile.xml");
      }

      [Observation]
      public void ParameterWithVolumeUnitTest()
      {
         var tc = SBMLModule.SpatialStructure.TopContainers.Single(x => x.Name.Equals("TOPCONTAINERSBML "));
         var parameter = tc.GetSingleChildByName<IParameter>("k1");
         parameter.ShouldNotBeNull();
         parameter.DisplayUnit.ToString().ShouldBeEqualTo("l");
      }

      [Observation]
      public void SpeciesWithAmountUnitTest()
      {
         var mbb = SBMLModule.Molecules;
         var mol = mbb.FindByName("s1");
         mol.ShouldNotBeNull();
         mol.Dimension.ShouldNotBeNull();
         mol.Dimension.ToString().ShouldBeEqualTo("Amount");
      }

      [Observation]
      public void SpeciesWithConcentrationUnitTest()
      {
         var mbb = SBMLModule.Molecules;
         var mol = mbb.FindByName("s2");
         mol.ShouldNotBeNull();
         mol.Dimension.ShouldNotBeNull();
         mol.Dimension.ToString().ShouldBeEqualTo("Amount");
      }
   }

   public class When_importing_combined_units : UnitImporterSpecs
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("tiny_example_12.xml");
      }

      [Observation]
      public void should_find_combined_dimensions()
      {
         var tc = SBMLModule.SpatialStructure.TopContainers.Single(x => x.Name.Contains("TOPCONTAINERSBML Keating2019"));
         var parameter = tc.Parameter("Vmax_ATPASE");
         parameter.ShouldNotBeNull();
         parameter.DisplayUnit.ToString().ShouldBeEqualTo("kat");
      }
   }
}