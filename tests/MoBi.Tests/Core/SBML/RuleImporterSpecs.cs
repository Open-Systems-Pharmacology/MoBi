using System.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core.SBML
{
   public abstract class concern_for_RuleImporter : ContextForSBMLIntegration<RuleImporter>
   {
   }

   public class AssignmentRuleImporterTests : concern_for_RuleImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("AssignmentRuleTestSpecies.xml");
      }

      [Observation]
      public void Species_AssignmentCreationTest()
      {
         var msvbb = _moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault();
         msvbb.ShouldNotBeNull();
         foreach (var msv in msvbb)
         {
            if (msv.Name == "s1" && msv.IsPresent)
               msv.StartValue.ShouldBeEqualTo(7);
         }
      }

      [Observation]
      public void Parameter_AssignmentCreationTest()
      {
         var psvbb = _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault();
         psvbb.ShouldNotBeNull();
         foreach (var psv in psvbb)
         {
            if (psv.Name == "k1")
               psv.Formula.ToString().ShouldBeEqualTo("7");
         }
      }

      [Observation, Ignore("Test is unstable")]
      public void Compartment_AssignmentCreationTest()
      {
         _moBiProject.ShouldNotBeNull();
         _moBiProject.SpatialStructureCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault().ShouldNotBeNull();

         foreach (var psv in _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault())
         {
            if (psv.Name == SBMLConstants.SIZE)
            {
               if (psv.Path.Contains("compartment"))
                  psv.Formula.ToString().ShouldBeEqualTo("7");
            }
         }
      }
   }

   public class RateRuleImporterTests : concern_for_RuleImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("RateRuleTest.xml");
      }

      [Observation]
      public void Species_RateRuleCreationTest()
      {
         _moBiProject.MoleculeStartValueBlockCollection.ShouldNotBeNull();
         var msvbb = _moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault();
         msvbb.ShouldNotBeNull();
         foreach (var msv in msvbb)
         {
            if (msv.Name == "s1" && msv.IsPresent)
               msv.StartValue.ShouldBeEqualTo(7);
         }
      }

      [Observation]
      public void Parameter_RateRuleCreationTest()
      {
         var psvbb = _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault();
         psvbb.ShouldNotBeNull();
         foreach (var psv in psvbb)
         {
            if (psv.Name == "k1")
               psv.Formula.ToString().ShouldBeEqualTo("7");
         }
      }

      [Observation]
      public void Compartment_RateRuleCreationTest()
      {
         _moBiProject.ShouldNotBeNull();
         _moBiProject.SpatialStructureCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault().ShouldNotBeNull();

         var ss = _moBiProject.SpatialStructureCollection.FirstOrDefault();
         ss.ShouldNotBeNull();
         var tc = ss.TopContainers.FirstOrDefault();
         tc.ShouldNotBeNull();
         tc.Children.Any(s => s.Name == "compartment").ShouldBeTrue();

         foreach (var psv in _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault())
         {
            if (psv.Name == SBMLConstants.SIZE)
            {
               if (psv.Path.Contains("compartment"))
                  psv.Formula.ToString().ShouldBeEqualTo("7");
            }
         }
      }
   }
}