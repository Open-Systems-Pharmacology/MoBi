using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;

namespace MoBi.Core.Serialization.Services
{
   public class ProjectClassifiableUpdaterAfterDeserialization : ProjectClassifiableUpdaterAfterDeserializationBase
   {
      protected override IWithId RetrieveSubjectFor(IClassifiableWrapper classifiableWrapper, IProject project)
      {
         return observedDataFor(classifiableWrapper, project) ??
                simulationFor(classifiableWrapper, project) ??
                parameterAnalysableFor(classifiableWrapper, project);
      }

      private IWithId parameterAnalysableFor(IClassifiableWrapper classifiableWrapper, IProject project)
      {
         return project.AllParameterAnalysables.FindById(classifiableWrapper.Id);
      }

      private IWithId observedDataFor(IClassifiableWrapper classifiableWrapper, IProject project)
      {
         return project.ObservedDataBy(classifiableWrapper.Id);
      }

      private IWithId simulationFor(IClassifiableWrapper classifiableWrapper, IProject project)
      {
         var mobiProject = project.DowncastTo<IMoBiProject>();
         return mobiProject.Simulations.FindById(classifiableWrapper.Id);
      }
   }
}