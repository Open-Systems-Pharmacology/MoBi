using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public class AssignmentImporter : SBMLImporter
   {
      private readonly InitialAssignmentImporter _initialAssignmentImporter;
      private readonly RuleImporter _ruleImporter;

      public AssignmentImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, InitialAssignmentImporter initialAssignmentImporter, RuleImporter ruleImporter, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _initialAssignmentImporter = initialAssignmentImporter;
         _ruleImporter = ruleImporter;
      }

      /// <summary>
      ///    Creates a Rule and an Initial Assignment Importer to import SBML Rules and Initial Assignments.
      /// </summary>
      protected override void Import(Model model)
      {
         CreateParameterValuesBuildingBlock(model);
         _initialAssignmentImporter.DoImport(model, _sbmlModule, _sbmlInformation, _command);
         _ruleImporter.DoImport(model, _sbmlModule, _sbmlInformation, _command);
      }

      /// <summary>
      ///    Creates a Parameter Start Values Building Block and adds it to the MoBi Project.
      /// </summary>
      protected internal void CreateParameterValuesBuildingBlock(Model model)
      {
         var parameterValuesBuildingBlock = new ParameterValuesBuildingBlock().WithId(SBMLConstants.SBML_PARAMETER_VALUES_BB).WithName(SBMLConstants.SBML_PARAMETER_VALUES_BB);
         _command.AddCommand(new AddBuildingBlockToModuleCommand<ParameterValuesBuildingBlock>(parameterValuesBuildingBlock, _sbmlModule).RunCommand(_context));
      }

      //For Rate Rule only
      public override void AddToProject()
      {
      }
   }
}