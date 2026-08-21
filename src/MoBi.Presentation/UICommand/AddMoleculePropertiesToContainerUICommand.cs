using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddMoleculePropertiesToContainerUICommand : ObjectUICommand<IContainer>
   {
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public AddMoleculePropertiesToContainerUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var spatialStructure = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         var moleculeProperties = _context.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);

         _context.AddToHistory(new AddContainerToSpatialStructureCommand(Subject, moleculeProperties, spatialStructure).RunCommand(_context));
      }
   }
}
