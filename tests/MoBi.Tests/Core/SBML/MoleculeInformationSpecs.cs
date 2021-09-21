using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using MoBi.Engine.Sbml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core.SBML
{
   public class MoleculeInformationTests : ContextForSBMLIntegration<ISbmlTask>
   {
      private List<MoleculeInformation> _moleculeInformation;

      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("MoleculeInformation_simple.xml");
      }

      protected override void Because()
      {
         base.Because();
         _moleculeInformation = _sbmlTask.SBMLInformation.MoleculeInformation;
      }


      [Observation]
      public void NotNullTest()
      {
         _moleculeInformation.ShouldNotBeNull();
      }

      [Observation]
      public void MoleculeInformationCountTest()
      {
         _moleculeInformation.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void RightSpeciesTest()
      {
         _moleculeInformation.Any(info => info.SpeciesIds.Any(s => s == "s1")).ShouldBeTrue();
         _moleculeInformation.Any(info => info.SpeciesIds.Any(s => s == "s2")).ShouldBeTrue();
         _moleculeInformation.Any(info => info.SpeciesIds.Any(s => s == "s3")).ShouldBeTrue();
         _moleculeInformation.Any(info => info.SpeciesIds.Any(s => s == "s4")).ShouldBeTrue();
      }

      [Observation]
      public void RightMoleculeTest()
      {
         _moleculeInformation.Any(info => info.GetMoleculeBuilderName() == "Test").ShouldBeTrue();
         _moleculeInformation.Any(info => info.GetMoleculeBuilderName() == "Test1").ShouldBeTrue();
         _moleculeInformation.Any(info => info.GetMoleculeBuilderName() == "s4").ShouldBeTrue();

         var molinfo = _moleculeInformation.FirstOrDefault(info => info.SpeciesIds.Any(s => s == "s3"));
         molinfo.ShouldNotBeNull();
         molinfo.GetAllSpecies().ShouldNotBeNull();
         molinfo.GetAllSpecies().Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void IsMultipleTimesInOneCompartmentTest()
      {
         _moleculeInformation.Any(info => info.IsMultipleTimesInOneCompartment()).ShouldBeFalse();
      }
   }
}