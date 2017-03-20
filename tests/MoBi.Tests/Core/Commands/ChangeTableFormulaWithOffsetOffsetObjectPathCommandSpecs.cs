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
   public abstract class concern_for_ChangeTableFormulaWithOffsetOffsetObjectPathCommand : ContextSpecification<ChangeTableFormulaWithOffsetOffsetObjectPathCommand>
   {
      protected TableFormulaWithOffset _tableFormulaWithOffset;
      private IFormulaUsablePath _oldPath;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _oldPath = A.Fake<IFormulaUsablePath>();
         _oldPath.Alias = AppConstants.OffsetAlias;
         _tableFormulaWithOffset = new TableFormulaWithOffset();
         _tableFormulaWithOffset.AddOffsetObjectPath(_oldPath);
         _tableFormulaWithOffset.OffsetObjectAlias = _oldPath.Alias;
         _context = A.Fake<IMoBiContext>();
         sut = new ChangeTableFormulaWithOffsetOffsetObjectPathCommand(_tableFormulaWithOffset, null, A.Fake<IBuildingBlock>());
      }
   }

   public class When_setting_the_offset_object_to_null : concern_for_ChangeTableFormulaWithOffsetOffsetObjectPathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_old_OffestObjectPath()
      {
         _tableFormulaWithOffset.ObjectPaths.ShouldBeEmpty();
      }
   }

   public class When_executing_the_inverse_of_the_change_offset_path_command : concern_for_ChangeTableFormulaWithOffsetOffsetObjectPathCommand
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
      public void should_add_the_offset_obkect_back_to_the_table_formula()
      {
         _tableFormulaWithOffset.ObjectPaths.Any().ShouldBeTrue();
      }
   }
}