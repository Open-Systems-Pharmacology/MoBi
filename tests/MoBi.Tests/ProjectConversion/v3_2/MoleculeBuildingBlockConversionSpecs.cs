using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.ProjectConversion.v3_2
{
   public abstract class concern_for_MoleculeBuildingBlockConversionSpecs : ContextWithLoadedProject
   {
      protected IMoleculeBuildingBlock _moleculeBuildingBlock;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _moleculeBuildingBlock = LoadPKML<IMoleculeBuildingBlock>("Molecules");
      }
   }

   internal class When_converting_a_molecule_building_block : concern_for_MoleculeBuildingBlockConversionSpecs
   {
      [Observation]
      public void should_move_parameters_and_transport_realizations_of_Transporter_container_to_children()
      {
         var transporterContainer = _moleculeBuildingBlock.First().GetChildren<IContainer>().First();
         var parameter = transporterContainer.GetChildren<IParameter>();
         parameter.Count().ShouldBeEqualTo(1);
         var transporter = transporterContainer.GetChildren<ITransportBuilder>();
         transporter.Count().ShouldBeEqualTo(1);
         var transport = transporter.First();
         transport.Children.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_not_move_passive_transports_to_molecule_children()
      {
         var moleculeBuilder = _moleculeBuildingBlock.First();
         var transporter = moleculeBuilder.GetChildren<ITransportBuilder>();
         transporter.Any().ShouldBeFalse();
      }
   }
}