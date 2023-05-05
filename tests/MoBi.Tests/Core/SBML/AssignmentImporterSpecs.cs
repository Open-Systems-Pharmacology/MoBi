using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Engine.Sbml;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Core.SBML
{
   public class InitialAssignmentImporterTests : ContextForSBMLIntegration<RuleImporter>
   {
      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("InitialAssignmentTest.xml");
      }


      [Observation]
      public void Species_InitialAssignmentCreationTest()
      {
         var msvbb = SBMLModule.InitialConditionsCollection.FirstOrDefault();
         msvbb.ShouldNotBeNull();
         if (msvbb == null) return;
         foreach (var msv in msvbb)
         {
            if (msv.Name == "s1" && msv.IsPresent)
               msv.Formula.ToString().ShouldBeEqualTo("5");
         }
      }

      [Observation]
      public void Parameter_InitialAssignmentCreationTest()
      {
         var psvbb = SBMLModule.ParameterStartValuesCollection.FirstOrDefault();
         psvbb.ShouldNotBeNull();
         foreach (var psv in psvbb)
         {
            if (psv.Name == "k1")
               psv.Formula.ToString().ShouldBeEqualTo("7");
         }
      }

      [Observation]
      public void Compartment_InitialAssignmentCreationTest()
      {
         SBMLModule.ShouldNotBeNull();
         SBMLModule.SpatialStructure.ShouldNotBeNull();
         SBMLModule.ParameterStartValuesCollection.ShouldNotBeNull();
         SBMLModule.ParameterStartValuesCollection.FirstOrDefault().ShouldNotBeNull();

         var psvbb = SBMLModule.ParameterStartValuesCollection.FirstOrDefault();
         foreach (var psv in psvbb)
         {
            if (psv.Name == SBMLConstants.SIZE)
            {
               if (psv.Path.Contains("c1"))
               {
                  psv.Formula.ShouldNotBeNull();
                  psv.Formula.ToString().ShouldBeEqualTo("7");
               }
            }
         }
      }
   }
}