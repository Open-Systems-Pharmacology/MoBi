using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetMoleculeListsCommandSpecs : ContextSpecification<SetMoleculeListsCommand>
   {
      protected IMoleculeDependentBuilder _builder;
      protected MoleculeList _newMoleculeList;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _builder = new TransportBuilder();
         _builder.MoleculeList.AddMoleculeName("A");
         _builder.MoleculeList.AddMoleculeName("B");
         _newMoleculeList = new MoleculeList();
         _newMoleculeList.AddMoleculeName("A");
         _newMoleculeList.AddMoleculeName("C");
         _newMoleculeList.AddMoleculeNameToExclude("B");
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut = new SetMoleculeListsCommand(_builder,_newMoleculeList,_buildingBlock);
      }
   }

   class When_executing_a_set_MoleculeListCommand : concern_for_SetMoleculeListsCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_Molecule_list_with_new_list()
      {
         _builder.MoleculeList.MoleculeNames.ShouldOnlyContain(_newMoleculeList.MoleculeNames.ToArray());
         _builder.MoleculeList.MoleculeNamesToExclude.ShouldOnlyContain(_newMoleculeList.MoleculeNamesToExclude.ToArray());
      }
   }
}	