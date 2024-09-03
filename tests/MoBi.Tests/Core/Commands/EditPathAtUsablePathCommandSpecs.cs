using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using MoBi.Core.Domain;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditPathAtUsablePathCommand : ContextSpecification<EditPathAtUsablePathCommand>
   {
      protected IBuildingBlock _buildingBlock;
      private IFormula _formula;
      protected ObjectPath _newObjectPath;
      protected FormulaUsablePath _oldObjectPath;
      protected IMoBiContext _context;
      protected IBuildingBlockVersionUpdater _buildingBlockVersionUpdater;

      protected override void Context()
      {
         _formula = new ExplicitFormula("A+B").WithId("id");
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         _newObjectPath = new FormulaUsablePath("NEW", "PATH").WithAlias("C1");
         _oldObjectPath = new FormulaUsablePath("OLD", "PATH").WithAlias("C1");
         sut = new EditPathAtUsablePathCommand(_formula, _newObjectPath, _oldObjectPath, _buildingBlock);
         _formula.AddObjectPath(_oldObjectPath);
         A.CallTo(() => _context.Get<IFormula>(_formula.Id)).Returns(_formula);
         _buildingBlockVersionUpdater = A.Fake<IBuildingBlockVersionUpdater>();
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(_buildingBlockVersionUpdater);

      }
   }

   public class When_executing_the_edit_path_at_usable_path_command : concern_for_EditPathAtUsablePathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_updated_the_old_path_with_the_new_path()
      {
         _oldObjectPath.PathAsString.ShouldBeEqualTo(_newObjectPath.PathAsString);
      }

      [Observation]
      public void should_increment_the_building_block_version()
      {
         A.CallTo(() => _buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, A<bool>._, A<PKSimModuleConversion>._)).MustHaveHappened();
      }
   }

   public class When_executing_the_inverse_command_of_the_edit_path : concern_for_EditPathAtUsablePathCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_resetted_the_path()
      {
         _oldObjectPath.PathAsString.ShouldBeEqualTo(new[] { "OLD", "PATH" }.ToPathString());
      }
   }
}