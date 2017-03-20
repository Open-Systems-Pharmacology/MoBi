using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;
using MoBi.Presentation.DTO;
using NUnit.Framework;

namespace MoBi.Presentation
{
   public abstract class concern_for_ImportExcelSheetSelectionDTO : ContextSpecification<ImportExcelSheetSelectionDTO>
   {
      protected override void Context()
      {
         sut = new ImportExcelSheetSelectionDTO();
      }
   }

   public class When_checking_if_an_existing_file_is_valid_for_import : concern_for_ImportExcelSheetSelectionDTO
   {
      private Func<string, bool> _oldExists;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldExists = FileHelper.FileExists;
         FileHelper.FileExists = s => true;
      }

      [Observation]
      [TestCase(@"C:\toto\toto.xls", true)]
      [TestCase(@"C:\toto\toto.xlsx", true)]
      [TestCase(@"C:\toto\toto.radata", false)]
      public void should_return_true_if_the_file_has_the_xls_or_xlsx_extension(string path,bool valid)
      {
         sut.FilePath = path;
         sut.Validate(x=>x.FilePath).IsEmpty.ShouldBeEqualTo(valid);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldExists;
      }
   }
}	