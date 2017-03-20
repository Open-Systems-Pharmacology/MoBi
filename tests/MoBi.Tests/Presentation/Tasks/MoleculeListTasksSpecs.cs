using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_MoleculeListTasksSpecs : ContextSpecification<MoleculeListTasks>
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         sut = new MoleculeListTasks(_context);
      }
   }

   internal class When_merging_two_molecule_lists_for_MoleculeDependentBuilder : concern_for_MoleculeListTasksSpecs
   {
      private IMoleculeDependentBuilder _builderToMergeFrom;
      private IMoleculeDependentBuilder _builderToMergeTo;
      private IMoBiCommand _resultCommand;

      protected override void Context()
      {
         base.Context();
         _builderToMergeFrom = new TransportBuilder();
         _builderToMergeTo = new TransportBuilder();
         _builderToMergeFrom.MoleculeList.AddMoleculeName("A");
         _builderToMergeFrom.MoleculeList.AddMoleculeName("B");
         _builderToMergeFrom.MoleculeList.AddMoleculeNameToExclude("D");
         _builderToMergeFrom.MoleculeList.AddMoleculeNameToExclude("F");
         _builderToMergeTo.MoleculeList.AddMoleculeName("A");
         _builderToMergeTo.MoleculeList.AddMoleculeName("C");
         _builderToMergeTo.MoleculeList.AddMoleculeNameToExclude("E");
         _builderToMergeTo.MoleculeList.AddMoleculeNameToExclude("F");
      }

      protected override void Because()
      {
         _resultCommand = sut.MergeMoleculeLists(_builderToMergeFrom, _builderToMergeTo, A.Fake<IBuildingBlock>());
      }

      [Observation]
      public void should_return_an_new_SetMoleculeListCommand()
      {
         _resultCommand.ShouldNotBeNull();
         _resultCommand.ShouldBeAnInstanceOf<SetMoleculeListsCommand>();
      }

      [Observation]
      public void Result_command_should_use_union_molecule_list()
      {
         _builderToMergeTo.MoleculeList.MoleculeNames.ShouldOnlyContain("A", "B", "C");
         _builderToMergeTo.MoleculeList.MoleculeNamesToExclude.ShouldOnlyContain("D", "E", "F");
      }
   }
}