﻿using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Reporting;
using MoBi.Engine.Sbml;

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
         var msvbb = _moBiProject.MoleculeStartValueBlockCollection.FirstOrDefault();
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
         var psvbb = _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault();
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
         _moBiProject.ShouldNotBeNull();
         _moBiProject.SpatialStructureCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.ShouldNotBeNull();
         _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault().ShouldNotBeNull();

         var psvbb = _moBiProject.ParametersStartValueBlockCollection.FirstOrDefault();
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