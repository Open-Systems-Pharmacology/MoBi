using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Services
{
   public interface IProjectClassificationConverter
   {
      void Convert(IMoBiProject project);
   }

   public class ProjectClassificationConverter : IProjectClassificationConverter
   {
      public void Convert(IMoBiProject project)
      {
         foreach (var simulation in project.Simulations)
         {
            project.AddClassifiable(new ClassifiableSimulation {Subject = simulation});
         }

         foreach (var observedData in project.AllObservedData)
         {
            project.AddClassifiable(new ClassifiableObservedData {Subject = observedData});
         }
      }
   }
}