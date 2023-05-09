using System.Collections.Generic;
using FluentNHibernate.Utils;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.ProjectConversion.v7_3
{
   public class When_converting_a_project_to_7_3_0 : ContextWithLoadedProject
   {
      private MoBiProject _project;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("EB_LV");
      }

      [Observation]
      public void should_have_set_all_parameters_of_all_building_blocks_to_default()
      {
         _project.MoleculeBlockCollection.Each(validateIsDefaultFlagInParameters);
         _project.ReactionBlockCollection.Each(validateIsDefaultFlagInParameters);
         _project.SpatialStructureCollection.Each(validateIsDefaultFlagInParameters);
         _project.EventBlockCollection.Each(validateIsDefaultFlagInParameters);
         _project.PassiveTransportCollection.Each(validateIsDefaultFlagInParameters);
         _project.ParametersValueBlockCollection.Each(validateIsDefaultFlagInParameters);
      }

      private void validateIsDefaultFlagInParameters<T>(IEnumerable<T> buildingBlock) where T : class
      {
         foreach (var builder in buildingBlock)
         {
            if (builder is IContainer container)
               validateIsDefaultFlagInContainerParameters(container);
            else if (builder is ParameterValue psv)
               validateIsDefaultStateIn(psv);
         }
      }

      private void validateIsDefaultFlagInContainerParameters(IContainer container)
      {
         container.GetAllChildren<IParameter>().Each(validateIsDefaultStateIn);
      }

      private void validateIsDefaultStateIn(IWithDefaultState withDefaultState)
      {
         withDefaultState.IsDefault.ShouldBeTrue();
      }
   }
}