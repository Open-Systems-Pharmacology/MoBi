using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeTableFormulaWithOffsetTableObjectPathCommand : ContextSpecification<ChangeTableFormulaWithOffsetTableObjectPathCommand>
   {
      protected TableFormulaWithOffset _tableFormulaWithOffset;
      private IFormulaUsablePath _oldPath;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _oldPath = A.Fake<IFormulaUsablePath>();
         _oldPath.Alias = AppConstants.TABLE_ALIAS;
         _tableFormulaWithOffset = new TableFormulaWithOffset();
         _tableFormulaWithOffset.AddTableObjectPath(_oldPath);
         _context = A.Fake<IMoBiContext>();
         sut = new ChangeTableFormulaWithOffsetTableObjectPathCommand(_tableFormulaWithOffset, null, A.Fake<IBuildingBlock>());
      }
   }

   public class When_setting_the_table_object_to_null : concern_for_ChangeTableFormulaWithOffsetTableObjectPathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_old_Table_ObjectPath()
      {
         _tableFormulaWithOffset.ObjectPaths.ShouldBeEmpty();
      }
   }

   public class When_executing_the_inverse_command_of_the_change_offset_object_path_command : concern_for_ChangeTableFormulaWithOffsetTableObjectPathCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_context).WithReturnType<TableFormulaWithOffset>().Returns(_tableFormulaWithOffset);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_add_the_table_alias_back_to_the_table_formula()
      {
         _tableFormulaWithOffset.ObjectPaths.Any().ShouldBeTrue();
      }
   }
}