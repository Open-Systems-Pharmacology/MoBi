using System.IO;
using MoBi.Core.Exceptions;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_SpatialStructureTask : ContextForIntegration<ISpatialStructureTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetSpatialStructureTask();
   }
}

internal class When_loading_spatial_structure_from_pkml : concern_for_SpatialStructureTask
{
   private SpatialStructure _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_spatial_structure_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("BigVial");
   }
}

internal class When_loading_spatial_structure_from_pkml_that_does_not_contain_one : concern_for_SpatialStructureTask
{
   private string _tempFile;

   protected override void Context()
   {
      base.Context();
      _tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
      Api.GetIndividualTask().ExportToPKML(new IndividualBuildingBlock().WithName("Not a spatial structure"), _tempFile);
   }

   [Observation]
   public void should_throw_not_matching_serialization_file_exception()
   {
      The.Action(() => sut.LoadFromPKML(_tempFile)).ShouldThrowAn<NotMatchingSerializationFileException>();
   }

   public override void Cleanup()
   {
      base.Cleanup();
      File.Delete(_tempFile);
   }
}
