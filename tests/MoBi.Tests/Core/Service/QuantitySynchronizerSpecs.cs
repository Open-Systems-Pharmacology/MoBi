using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_QuantitySynchronizer : ContextSpecification<IQuantitySynchronizer>
   {
      private IMoBiContext _context;
      protected IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;
      protected IEntityPathResolver _entityPathResolver;
      protected IMoBiSimulation _simulation;
      protected IQuantity _quantity;
      protected IMoBiCommand _command;
      protected IBuildingBlockInfo _buildingBlockInfo;
      protected IParameter _parameter;
      protected ObjectPath _objectPath;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _parameter = A.Fake<IParameter>();
      }

      protected override void Context()
      {
         _affectedBuildingBlockRetriever = A.Fake<IAffectedBuildingBlockRetriever>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _context = A.Fake<IMoBiContext>();
         sut = new QuantitySynchronizer(_affectedBuildingBlockRetriever, _entityPathResolver, _context);

         _simulation = A.Fake<IMoBiSimulation>();
         _buildingBlockInfo = A.Fake<IBuildingBlockInfo>();
         _objectPath = new ObjectPath("P");
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(_objectPath);
         A.CallTo(() => _entityPathResolver.PathFor(_parameter)).Returns("P");
         A.CallTo(() => _affectedBuildingBlockRetriever.RetrieveFor(_quantity, _simulation)).Returns(_buildingBlockInfo);
      }

      protected override void Because()
      {
         _command = sut.SynchronizeCommand(_quantity, _simulation);
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_reaction_building_block : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = _parameter;

         base.Context();
         var reaction = new ReactionBuilder();
         reaction.AddParameter(_parameter);
         var reactionBuildingBlock = new ReactionBuildingBlock {reaction};
         _buildingBlockInfo.UntypedBuildingBlock = reactionBuildingBlock;
      }

      [Observation]
      public void should_return_a_synchronize_parameter_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeParameterValueCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_spatiatial_structure_building_block : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = _parameter;

         base.Context();
         var container = new Container();
         container.Add(_parameter);
         var spatialStructure = new SpatialStructure {container};
         _buildingBlockInfo.UntypedBuildingBlock = spatialStructure;
      }

      [Observation]
      public void should_return_a_synchronize_parameter_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeParameterValueCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_molecule_building_block : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = _parameter;

         base.Context();
         var moleculeBuilder = new MoleculeBuilder();
         moleculeBuilder.AddParameter(_parameter);
         var moleculeBuildingBlock = new MoleculeBuildingBlock {moleculeBuilder};
         _buildingBlockInfo.UntypedBuildingBlock = moleculeBuildingBlock;
      }

      [Observation]
      public void should_return_a_synchronize_parameter_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeParameterValueCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_transporter_parameter_in_a_molecule_building_block : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = A.Fake<IParameter>().WithName("P");

         base.Context();
         _parameter.Name = "P";
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Mol1");
         var moleculeBuilder2 = new MoleculeBuilder().WithName("Mol2");
         var transporterMolecule1 = new TransporterMoleculeContainer {TransportName = "TRANSPORT", Name = "TRANSPORTER"};
         var molecule1 = new Container().WithName("Mol1");
         molecule1.Add(transporterMolecule1);
         var molecule2 = new Container().WithName("Mol2");
         var transporterMolecule2 = new TransporterMoleculeContainer {TransportName = "TRANSPORT", Name = "TRANSPORTER"};
         molecule2.Add(transporterMolecule2);
         moleculeBuilder1.AddTransporterMoleculeContainer(transporterMolecule1);
         moleculeBuilder2.AddTransporterMoleculeContainer(transporterMolecule2);
         var moleculeBuildingBlock = new MoleculeBuildingBlock { moleculeBuilder1,moleculeBuilder2 };
         _buildingBlockInfo.UntypedBuildingBlock = moleculeBuildingBlock;
         transporterMolecule1.AddParameter(_parameter);
         _quantity.ParentContainer.Name = transporterMolecule1.TransportName;
         _quantity.ParentContainer.ParentContainer.Name = moleculeBuilder1.Name;
      }

      [Observation]
      public void should_return_a_synchronize_parameter_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeParameterValueCommand>();
      }
   }


   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_parameter_start_value_building_block_for_which_an_entry_does_not_exist : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = _parameter;

         base.Context();
         _buildingBlockInfo.UntypedBuildingBlock = new ParameterStartValuesBuildingBlock();
      }

      [Observation]
      public void should_return_an_add_parameter_start_in_simulation_command()
      {
         _command.ShouldBeAnInstanceOf<AddParameterStartValueFromQuantityInSimulationCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_parameter_start_value_building_block_for_which_an_entry_exists : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         _quantity = _parameter;

         base.Context();
         _buildingBlockInfo.UntypedBuildingBlock = new ParameterStartValuesBuildingBlock {new ParameterStartValue {Path = _objectPath}};
      }

      [Observation]
      public void should_return_an_synchronize_parameter_start_value_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeParameterStartValueCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_molecule_start_value_building_block_for_which_an_entry_does_not_exist : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         var moleculeAmount = A.Fake<IMoleculeAmount>();
         _quantity = moleculeAmount;

         base.Context();
         _buildingBlockInfo.UntypedBuildingBlock = new MoleculeStartValuesBuildingBlock();
      }

      [Observation]
      public void should_return_an_add_parameter_start_in_simulation_command()
      {
         _command.ShouldBeAnInstanceOf<AddMoleculeStartValueFromQuantityInSimulationCommand>();
      }
   }

   public class When_retrieving_the_synchronization_command_for_a_quantity_affecting_a_molecule_start_value_building_block_for_which_an_entry_exists : concern_for_QuantitySynchronizer
   {
      protected override void Context()
      {
         var moleculeAmount = A.Fake<IMoleculeAmount>();
         _quantity = moleculeAmount;

         base.Context();
         A.CallTo(() => _entityPathResolver.ObjectPathFor(moleculeAmount, false)).Returns(_objectPath);
         _buildingBlockInfo.UntypedBuildingBlock = new MoleculeStartValuesBuildingBlock {new MoleculeStartValue {Path = _objectPath}};
      }

      [Observation]
      public void should_return_an_synchronize_parameter_start_value_command()
      {
         _command.ShouldBeAnInstanceOf<SynchronizeMoleculeStartValueCommand>();
      }
   }
}