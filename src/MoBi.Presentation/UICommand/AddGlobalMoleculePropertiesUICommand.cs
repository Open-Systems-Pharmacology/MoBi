using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddGlobalMoleculePropertiesUICommand : ActiveObjectUICommand<MoBiSpatialStructure>
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public AddGlobalMoleculePropertiesUICommand(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context, IMoBiSpatialStructureFactory spatialStructureFactory) : base(activeSubjectRetriever)
      {
         _context = context;
         _spatialStructureFactory = spatialStructureFactory;
      }

      protected override void PerformExecute()
      {
         if (Subject.GlobalMoleculeDependentProperties != null)
            return;

         var moleculeProperties = _spatialStructureFactory.CreateGlobalMoleculeDependentProperties();
         _context.AddToHistory(new AddGlobalMoleculePropertiesCommand(Subject, moleculeProperties).RunCommand(_context));
      }
   }
}