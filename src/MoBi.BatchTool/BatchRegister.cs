using MoBi.BatchTool.Runners;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.BatchTool
{
   public class BatchRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan => scan.AssemblyContainingType<BatchRegister>());
         container.AddScanner(scan =>
            {
               scan.AssemblyContainingType<BatchRegister>();
               scan.Include(x => x.IsAnImplementationOf<IBatchRunner>());
               scan.WithConvention<ConcreteTypeRegistrationConvention>();
            }
         );

         container.Register<ISimulationDiagramManager, SimulationDiagramManager>();
         container.Register<IMoBiReactionDiagramManager, MoBiReactionDiagramManager>();
         container.Register<ISpatialStructureDiagramManager, SpatialStructureDiagramManager>();
      }
   }
}