using MoBi.Core.Services;
using MoBi.Engine.Sbml;
using OSPSuite.Core;
using OSPSuite.Utility.Container;

namespace MoBi.Engine
{
   public class EngineRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(x =>
         {
            x.AssemblyContainingType<EngineRegister>();
            x.WithConvention(new OSPSuiteRegistrationConvention(registerConcreteType: true));
            x.ExcludeType<UnitDefinitionImporter>();
            x.ExcludeType<CompartmentImporter>();
            x.ExcludeType<SpeciesImporter>();
            x.ExcludeType<ParameterImporter>();
            x.ExcludeType<FunctionDefinitionImporter>();
            x.ExcludeType<ReactionImporter>();
            x.ExcludeType<AssignmentImporter>();
            x.ExcludeType<EventImporter>();
            x.ExcludeType<InitialAssignmentImporter>();
            x.ExcludeType<RuleImporter>();
            x.ExcludeType<SBMLImporterRepository>();
            x.ExcludeType<ASTHandler>();
         });

         container.Register<SBMLImporterRepository, SBMLImporterRepository>();
         container.Register<CompartmentImporter, CompartmentImporter>();
         container.Register<SpeciesImporter, SpeciesImporter>();
         container.Register<IUnitDefinitionImporter, UnitDefinitionImporter>(LifeStyle.Singleton);
         container.Register<IFunctionDefinitionImporter, FunctionDefinitionImporter>(LifeStyle.Singleton);
         container.Register<EventImporter, EventImporter>();
         container.Register<AssignmentImporter, AssignmentImporter>();
         container.Register<InitialAssignmentImporter, InitialAssignmentImporter>();
         container.Register<RuleImporter, RuleImporter>();
         container.Register<ParameterImporter, ParameterImporter>();
         container.Register<ReactionImporter, ReactionImporter>();
         container.Register<ASTHandler, ASTHandler>();
         container.Register<ISbmlTask, SbmlTask>();
      }
   }
}