﻿using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Engine.Sbml;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.SBML
{
   public class HelperTests : ContextForSBMLIntegration<ISpeciesImporter>
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
         sut.DowncastTo<SpeciesImporter>().GetNewFactor(_amountDimension, _sizeDimension).ShouldBeEqualTo(res);
      }
   }

   public class MoleculeCreationTests : ContextForSBMLIntegration<ISpeciesImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("Species_simple.xml");
      }

      [Observation]
      public void MoleculeBuildingBlockCreationTest()
      {
         var moleculeBuildingBlock = SBMLModule.Molecules;
         moleculeBuildingBlock.ShouldNotBeNull();
         moleculeBuildingBlock.Name.ShouldBeEqualTo(SBMLConstants.SBML_SPECIES_BB);
      }

      [Observation]
      public void SpeciesCreationTest()
      {
         var mbb = SBMLModule.Molecules;
         mbb.ShouldNotBeNull();
         mbb.Any(molecule => molecule.Name == "S1").ShouldBeTrue();
         mbb.Any(molecule => molecule.Name == "S2").ShouldBeTrue();

         var msvbb = SBMLModule.InitialConditionsCollection.FirstOrDefault();
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

   public class SpeciesSameNameTests : ContextForSBMLIntegration<ISpeciesImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("Species_sameName_diffComp.xml");
      }

      [Observation]
      public void SpeciesSameNameDiffCompartmentsTest()
      {
         var mbb = SBMLModule.Molecules;
         mbb.ShouldNotBeNull();
         mbb.Any(molecule => molecule.Name == "abc").ShouldBeTrue();
      }
   }

   public class When_setting_molecule_start_value : ContextForSBMLIntegration<ISpeciesImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("tiny_example_12.xml");
      }

      [Observation]
      public void should_understand_litre_as_unit()
      {
         var msvbb = SBMLModule.InitialConditionsCollection.FirstOrDefault();
         msvbb.FirstOrDefault().Dimension.Name.ShouldBeEqualTo("Amount");
      }

      [Observation]
      public void should_assign_molecule_start_values_as_concentrations()
      {
         var msvbb = SBMLModule.InitialConditionsCollection.FirstOrDefault();
         var glucose = msvbb.First();
         glucose.Formula.ToString().ShouldBeEqualTo("5000 * V");
         var volumePath = glucose.Formula.ObjectPaths.First();
         volumePath.ToString().ShouldBeEqualTo("..|Volume");
         volumePath.Alias.ShouldBeEqualTo("V");
         volumePath.Count.ShouldBeEqualTo(2);
      }
   }
}