using OSPSuite.Utility.Container;

namespace MoBi.Core.SBML
{
    public class SBMLImportRegister:Register
    {
        public override void RegisterInContainer(IContainer container)
        {
            container.Register<SBMLImporterRepository,SBMLImporterRepository>();
            container.Register<CompartmentImporter, CompartmentImporter>();
            container.Register<SpeciesImporter, SpeciesImporter>();
            container.Register<UnitDefinitionImporter, UnitDefinitionImporter>();
            container.Register<FunctionDefinitionImporter, FunctionDefinitionImporter>();
            container.Register<EventImporter, EventImporter>();
            container.Register<AssignmentImporter, AssignmentImporter>();
            container.Register<InitialAssignmentImporter, InitialAssignmentImporter>();
            container.Register<RuleImporter, RuleImporter>();
            container.Register<ParameterImporter, ParameterImporter>();
            container.Register<ReactionImporter, ReactionImporter>();
            container.Register<ASTHandler, ASTHandler>();
        }
    }
}