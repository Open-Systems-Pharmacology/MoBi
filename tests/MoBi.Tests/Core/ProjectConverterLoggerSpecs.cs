using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Helper;
using MoBi.Core.Serialization.Converter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core
{
   public abstract class concern_for_ProjectConverterLogger : ContextSpecification<IProjectConverterLogger>
   {
      protected override void Context()
      {
         sut = new ProjectConverterLogger(new ObjectTypeResolver());
      }
   }

   public class When_retrieving_the_error_messages_defined_in_the_logger : concern_for_ProjectConverterLogger
   {
      private IEnumerable<NotificationMessage> _result;
      private string _error1;
      private string _error2;

      protected override void Context()
      {
         base.Context();
         _error1 = "ERROR 1";
         _error2 = "ERROR 2";
         sut.AddError(_error1, A.Fake<IObjectBase>(),new EventGroupBuildingBlock());
         sut.AddInfo("THIS IS SOME INFO", A.Fake<IObjectBase>(), new EventGroupBuildingBlock());
         sut.AddError(_error2, A.Fake<IObjectBase>(), new EventGroupBuildingBlock());
      }

      protected override void Because()
      {
         _result = sut.AllMessages().Where(x=>x.Type.Equals(NotificationType.Error));
      }

      [Observation]
      public void should_return_the_set_of_all_existing_error_messages()
      {
         _result.Select(X=>X.Message).ShouldOnlyContain(_error1, _error2);
      }
   }

   public class When_clearing_the_project_converter_logger : concern_for_ProjectConverterLogger
   {
      protected override void Context()
      {
         base.Context();
         sut.AddError("Error", A.Fake<IObjectBase>(), A.Fake<IBuildingBlock>());
         sut.AddInfo("THIS IS SOME INFO", A.Fake<IObjectBase>(), A.Fake<IBuildingBlock>());
         sut.AddError("Error", A.Fake<IObjectBase>(), A.Fake<IBuildingBlock>());
      }

      [Observation]
      public void should_not_have_any_messages()
      {
         sut.AllMessages().Any().ShouldBeTrue();
         sut.Clear();
         sut.AllMessages().Any().ShouldBeFalse();
      }
   }
}