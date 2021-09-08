using System.Collections.Generic;
using libsbmlcs;
using OSPSuite.Utility.Container;

namespace MoBi.Engine.Sbml
{
   public class SBMLImporterRepository
   {
      private readonly IContainer _container;

      public SBMLImporterRepository(IContainer container)
      {
         _container = container;
      }

      public IEnumerable<ISBMLImporter> All()
      {
         yield return _container.Resolve<IUnitDefinitionImporter>();
         yield return _container.Resolve<CompartmentImporter>();
         yield return _container.Resolve<SpeciesImporter>();
         yield return _container.Resolve<ParameterImporter>();
         yield return _container.Resolve<IFunctionDefinitionImporter>();
         yield return _container.Resolve<ReactionImporter>();
         yield return _container.Resolve<AssignmentImporter>();
         yield return _container.Resolve<EventImporter>();
      }

      public IEnumerable<ISBMLImporter> AllFor(Model sbmlModel)
      {
         if (sbmlModel.getNumUnitDefinitions() != 0)
            yield return _container.Resolve<IUnitDefinitionImporter>();
         yield return _container.Resolve<CompartmentImporter>();
         if (sbmlModel.getNumSpecies() != 0)
            yield return _container.Resolve<SpeciesImporter>();
         if (sbmlModel.getNumParameters() != 0)
            yield return _container.Resolve<ParameterImporter>();
         if (sbmlModel.getNumFunctionDefinitions() != 0)
            yield return _container.Resolve<IFunctionDefinitionImporter>();
         if (sbmlModel.getNumReactions() != 0)
            yield return _container.Resolve<ReactionImporter>();
         yield return _container.Resolve<AssignmentImporter>();
         if (sbmlModel.getNumEvents() != 0)
            yield return _container.Resolve<EventImporter>();
      }

      public IEnumerable<SBMLImporter> Assignment()
      {
         yield return _container.Resolve<RuleImporter>();
         yield return _container.Resolve<InitialAssignmentImporter>();
      }
   }
}