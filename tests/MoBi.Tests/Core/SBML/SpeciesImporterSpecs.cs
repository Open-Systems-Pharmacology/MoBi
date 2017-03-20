using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.SBML
{
   public class HelperTests : ContextForSBMLIntegration<SpeciesImporter>
   {
      private IDimension _amountDimension;
      private IDimension _sizeDimension;
      private Unit _amountUnit;
      private Unit _sizeUnit;
      private long _amountFactor;
      private long _sizeFactor;

      protected override void Because()
      {
         _amountDimension = A.Fake<IDimension>();
         _sizeDimension = A.Fake<IDimension>();
         _amountUnit = A.Fake<Unit>();
         _sizeUnit = A.Fake<Unit>();

         _amountFactor = 3;
         _sizeFactor = 5;
         _amountUnit.Factor = _amountFactor;
         _sizeUnit.Factor = _sizeFactor;
         _amountDimension.DefaultUnit = _amountUnit;
         _sizeDimension.DefaultUnit = _sizeUnit;
      }

      [Observation]
      public void GetNewFactorTest()
      {
         var res = (double) _amountFactor / _sizeFactor;
         sut.GetNewFactor(_amountDimension, _sizeDimension).ShouldBeEqualTo(res);
      }
   }

   public class MoleculeCreationTests : ContextForSBMLIntegration<SpeciesImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("Species_simple.xml");
      }

      [Observation]
      public void MoleculeBuildingBlockCreationTest()
      {
         var moleculeBuildingBlock = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         moleculeBuildingBlock.ShouldNotBeNull();
         moleculeBuildingBlock.Name.ShouldBeEqualTo(SBMLConstants.SBML_SPECIES_BB);
      }

      [Observation]
      public void SpeciesCreationTest()
      {
         var mbb = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         mbb.ShouldNotBeNull();
         mbb.Any(molecule => molecule.Name == "S1").ShouldBeTrue();
         mbb.Any(molecule => molecule.Name == "S2").ShouldBeTrue();

         var msvbb = _moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault();
         msvbb.Any(msv => msv.Name == "S1").ShouldBeTrue();
         msvbb.Any(msv => msv.Name == "S2").ShouldBeTrue();
         var msv1 = ObjectBaseExtensions.FindByName(msvbb, "S1");
         var msv2 = ObjectBaseExtensions.FindByName(msvbb, "S2");
         msv1.ShouldNotBeNull();
         msv2.ShouldNotBeNull();
         msv1.IsPresent.ShouldBeTrue();
         msv2.IsPresent.ShouldBeTrue();
      }
   }

   public class SpeciesSameNameTests : ContextForSBMLIntegration<SpeciesImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("Species_sameName_diffComp.xml");
      }

      [Observation]
      public void SpeciesSameNameDiffCompartmentsTest()
      {
         var mbb = _moBiProject.MoleculeBlockCollection.FirstOrDefault();
         mbb.ShouldNotBeNull();
         mbb.Any(molecule => molecule.Name == "abc").ShouldBeTrue();
      }
   }
}