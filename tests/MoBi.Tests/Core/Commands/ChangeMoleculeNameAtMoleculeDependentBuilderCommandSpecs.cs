using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeMoleculeNameAtMoleculeDependentBuilderCommand : ContextSpecification<ChangeMoleculeNameAtMoleculeDependentBuilderCommandBase>
   {
      protected IMoBiContext _context;
      protected Module _module;
      protected PassiveTransportBuildingBlock _buildingBlock;
      protected TransportBuilder _transportBuilder;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         var projectRetriever = A.Fake<IMoBiProjectRetriever>();
         A.CallTo(() => projectRetriever.Current).Returns(null);
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(new BuildingBlockVersionUpdater(projectRetriever, A.Fake<IEventPublisher>(), A.Fake<IDialogCreator>()));

         _transportBuilder = new TransportBuilder().WithId("transport");
         _transportBuilder.AddMoleculeName("A");
         _transportBuilder.AddMoleculeNameToExclude("E");
         _buildingBlock = new PassiveTransportBuildingBlock { _transportBuilder };
         _buildingBlock.Id = "bb";
         _buildingBlock.Version = 1;
         _module = new Module { IsPKSimModule = true };
         _module.Add(_buildingBlock);

         A.CallTo(() => _context.Get<IBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         A.CallTo(() => _context.Get<IMoleculeDependentBuilder>(_transportBuilder.Id)).Returns(_transportBuilder);
      }

      protected override void Because()
      {
         sut.Execute(_context);
         sut.InvokeInverse(_context);
      }

      [Observation]
      public void should_revert_the_module_to_a_pksim_module()
      {
         _module.IsPKSimModule.ShouldBeTrue();
      }

      [Observation]
      public void should_revert_the_building_block_version()
      {
         _buildingBlock.Version.ShouldBeEqualTo(1u);
      }
   }

   public class When_reverting_a_molecule_name_change_at_a_molecule_dependent_builder_in_a_pksim_module : concern_for_ChangeMoleculeNameAtMoleculeDependentBuilderCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new ChangeMoleculeNameAtMoleculeDependentBuilderCommand("B", "A", _transportBuilder, _buildingBlock);
      }

      [Observation]
      public void should_restore_the_molecule_name()
      {
         _transportBuilder.MoleculeNames().ShouldOnlyContain("A");
      }
   }

   public class When_reverting_an_excluded_molecule_name_change_at_a_molecule_dependent_builder_in_a_pksim_module : concern_for_ChangeMoleculeNameAtMoleculeDependentBuilderCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand("F", "E", _transportBuilder, _buildingBlock);
      }

      [Observation]
      public void should_restore_the_excluded_molecule_name()
      {
         _transportBuilder.MoleculeNamesToExclude().ShouldOnlyContain("E");
      }

      [Observation]
      public void should_not_change_the_included_molecule_names()
      {
         _transportBuilder.MoleculeNames().ShouldOnlyContain("A");
      }
   }
}
