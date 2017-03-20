using System;
using libsbmlcs;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
    public class InitialAssignmentImporter : AssignmentImporterBase
    {
       public InitialAssignmentImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory,astHandler, context)
       {
       }

       protected override void Import(Model model)
        {
           for (long i = 0; i < model.getNumInitialAssignments(); i++)
            {
               ImportInitialAssignment(model.getInitialAssignment(i), model);
            }
            AddToProject();
        }

        private void ImportInitialAssignment(InitialAssignment initialAssignment, Model model)
        {
           var symbol = initialAssignment.getSymbol();
           if (IsParameter(symbol))
            {
                var parameter = GetParameter(symbol);
                SetPSV(initialAssignment.getMath(), parameter, String.Empty);
                return;
            }

            if (IsContainerSizeParameter(symbol))
            {
                var sizeParameter = GetContainerSizeParameter(symbol);
                SetPSV(initialAssignment.getMath(), sizeParameter, symbol);
                return;
            }

            if (IsSpeciesAssignment(symbol))
                DoSpeciesAssignment(symbol, initialAssignment.getMath(), isInitialAssignment: true);

            CheckSpeciesReferences(initialAssignment.getId(), symbol, model);
        }

        public override void AddToProject() {}
    }
}
