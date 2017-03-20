using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class SimulationNode : ObjectWithIdAndNameNode<ClassifiableSimulation>
   {
      public SimulationNode(ClassifiableSimulation classifiableSimulation)
         : base(classifiableSimulation)
      {
      }

      public IMoBiSimulation Simulation
      {
         get { return Tag.Simulation; }
      }
   }
}