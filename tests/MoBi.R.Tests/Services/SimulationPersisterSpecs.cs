using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.R.Services;
using static MoBi.R.Tests.HelperForSpecs;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_SimulationPersister : ContextForIntegration<ISimulationPersister>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = OSPSuite.R.Api.GetSimulationPersister();
   }
}

internal class When_saving_a_loaded_simulation_to_pkml : concern_for_SimulationPersister
{
   private string _sourceFile;
   private string _savedFile;

   protected override void Context()
   {
      base.Context();
      _sourceFile = DataTestFileFullPath("simulation with two modules.pkml");
      _savedFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
   }

   protected override void Because()
   {
      sut.SaveSimulation(sut.LoadSimulation(_sourceFile), _savedFile);
   }

   [Observation]
   public void should_keep_the_parameter_group_ids_of_the_loaded_file()
   {
      groupIdsIn(_savedFile).ShouldOnlyContainInOrder(groupIdsIn(_sourceFile));
   }

   private static IReadOnlyList<string> groupIdsIn(string pkmlFile)
   {
      return XDocument.Load(pkmlFile)
         .Descendants()
         .Select(x => x.Attribute(Constants.Serialization.Attribute.GROUP_ID)?.Value)
         .Where(x => x != null)
         .OrderBy(x => x)
         .ToList();
   }

   public override void Cleanup()
   {
      base.Cleanup();
      File.Delete(_savedFile);
   }
}
