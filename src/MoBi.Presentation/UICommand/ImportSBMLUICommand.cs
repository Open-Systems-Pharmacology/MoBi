using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
    public class ImportSbmlUICommand : IUICommand
    {
        private readonly IProjectTask _sbmlTasks;

        public ImportSbmlUICommand(IProjectTask projectTasks)
        {
            _sbmlTasks = projectTasks;
        }

        public void Execute()
        {
            _sbmlTasks.OpenSBMLModel();
        }
    }
}