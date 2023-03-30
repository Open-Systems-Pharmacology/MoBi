using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Services;
using MoBi.Helpers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization;

namespace MoBi.Core
{
   public abstract class concern_for_ProjectClassifiableUpdaterAfterDeserialization : ContextSpecification<IProjectClassifiableUpdaterAfterDeserialization>
   {

      protected override void Context()
      {
         sut = new ProjectClassifiableUpdaterAfterDeserialization();
      }
   }

   public class When_updating_a_project_afer_deserialization : concern_for_ProjectClassifiableUpdaterAfterDeserialization
   {
      private IMoBiProject _project;
      private IClassifiableWrapper _classifiable1;
      private IClassifiableWrapper _classifiable2;
      private IMoBiSimulation _simulation;
      private DataRepository _dataRepository;
      private IClassifiableWrapper _classifiable3;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _simulation = A.Fake<IMoBiSimulation>().WithId("1");
         _dataRepository = new DataRepository("2");
         _classifiable1 = A.Fake<IClassifiableWrapper>().WithId(_simulation.Id);
         _classifiable2 = A.Fake<IClassifiableWrapper>().WithId(_dataRepository.Id);
         _classifiable3 = A.Fake<IClassifiableWrapper>().WithId("UNKNOWN");
         _project.AddObservedData(_dataRepository);
         _project.AddSimulation(_simulation);
         _project.AddClassifiable(_classifiable1);
         _project.AddClassifiable(_classifiable2);
         _project.AddClassifiable(_classifiable3);
      }

      protected override void Because()
      {
         sut.Update(_project);
      }

      [Observation]
      public void should_update_the_subject_for_each_available_classifiable()
      {
         A.CallTo(() => _classifiable1.UpdateSubject(_simulation)).MustHaveHappened();
         A.CallTo(() => _classifiable2.UpdateSubject(_dataRepository)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_classification_that_are_not_found()
      {
         _project.AllClassifiables.ShouldOnlyContain(_classifiable1,_classifiable2);
      }
   }
}