using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Serialization.Converter
{
    public interface IProjectClassificationConverter
    {
        void Convert(MoBiProject project);
    }

    public class ProjectClassificationConverter : IProjectClassificationConverter
    {
        public void Convert(MoBiProject project)
        {
            foreach (var simulation in project.Simulations)
            {
                project.AddClassifiable(new ClassifiableSimulation { Subject = simulation });
            }

            foreach (var observedData in project.AllObservedData)
            {
                project.AddClassifiable(new ClassifiableObservedData { Subject = observedData });
            }
        }
    }
}