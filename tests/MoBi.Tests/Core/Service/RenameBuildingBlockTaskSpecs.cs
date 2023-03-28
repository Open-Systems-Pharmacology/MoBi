using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_RenameBuildingBlockTask : ContextSpecification<IRenameBuildingBlockTask>
   {
      private IMoBiProjectRetriever _projectRetriever;
      protected IMoBiProject _project;

      protected override void Context()
      {
         _project= A.Fake<IMoBiProject>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         sut = new RenameBuildingBlockTask(_projectRetriever);
      }
   }
}	