using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using FakeItEasy;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using Model = libsbmlcs.Model;
using Unit = libsbmlcs.Unit;

namespace MoBi.Core.SBML
{
   public abstract class UnitImporterSpecs : ContextForSBMLIntegration<UnitDefinitionImporter>
   {
      private Model _sbmlModel;

      protected override void Context()
      {
         base.Context();
         _sbmlModel = new Model(3, 1);
      }

      public class UnitDefinitionImporterTests : UnitImporterSpecs
      {
         private Unit _unit;
         private UnitDefinition _unitDef;
         private Unit _unit2;
         private UnitDefinition _unitDef2;
         private IDimension _substanceDimension;
         private IDimension _volumeDimension;
         private IMoBiDimensionFactory _dimensionFactoy;

         protected override void Because()
         {
            _dimensionFactoy = IoC.Resolve<IMoBiDimensionFactory>();
            sut = new UnitDefinitionImporter(IoC.Resolve<IObjectPathFactory>(), IoC.Resolve<IObjectBaseFactory>(), _dimensionFactoy, IoC.Resolve<ASTHandler>(), A.Fake<IMoBiContext>());
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

            _sbmlModel.addUnitDefinition(_unitDef);
            sut.DoImport(_sbmlModel, new MoBiProject(), A.Fake<SBMLInformation>(), new MoBiMacroCommand());
            _substanceDimension = _dimensionFactoy.Dimension("SBML_substance");
            _volumeDimension = _dimensionFactoy.Dimension("Volume");
         }

         [Observation]
         public void NotNullTest()
         {
            _substanceDimension.ShouldNotBeNull();
            _volumeDimension.ShouldNotBeNull();
         }

         [Observation]
         public void UnitImporterTest()
         {
            _substanceDimension.BaseRepresentation.ShouldNotBeNull();
            _substanceDimension.DisplayName.ShouldBeEqualTo("SBML_substance");

            _volumeDimension.ShouldNotBeNull();
            _volumeDimension.Name.ShouldBeEqualTo("Volume");
            _volumeDimension.DefaultUnit.Name.ShouldBeEqualTo("l");
         }
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
         var tc = _moBiProject.SpatialStructureCollection.FirstOrDefault().TopContainers.FirstOrDefault();
         var parameter = tc.GetSingleChildByName<IParameter>("k1");
         parameter.ShouldNotBeNull();
         parameter.DisplayUnit.ToString().ShouldBeEqualTo("l");
      }

      [Observation]
      public void ParameterWithSubstanceUnitTest()
      {
         var tc = _moBiProject.SpatialStructureCollection.FirstOrDefault().TopContainers.FirstOrDefault();
         var parameter = tc.GetSingleChildByName<IParameter>("k2");
         parameter.ShouldNotBeNull();
         parameter.DisplayUnit.ToString().ShouldBeEqualTo("substance");
      }

      [Observation]
      public void SpeciesWithAmountUnitTest()
      {
         var mbb = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         var mol = mbb.FindByName("s1");
         mol.ShouldNotBeNull();
         mol.Dimension.ShouldNotBeNull();
         mol.Dimension.ToString().ShouldBeEqualTo("Amount");
      }

      [Observation]
      public void SpeciesWithConcentrationUnitTest()
      {
         var mbb = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         var mol = mbb.FindByName("s2");
         mol.ShouldNotBeNull();
         mol.Dimension.ShouldNotBeNull();
         mol.Dimension.ToString().ShouldBeEqualTo("Amount");
      }
   }
}