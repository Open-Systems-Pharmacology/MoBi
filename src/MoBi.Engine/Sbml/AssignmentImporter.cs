using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;
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
      ///     Creates a Rule and an Initial Assignment Importer to import SBML Rules and Initital Assignments.
      /// </summary>
      protected override void Import(Model model)
      {
         CreateParameterStartValuesBuildingBlock(model);
         _initialAssignmentImporter.DoImport(model,_sbmlProject,_sbmlInformation, _command);
         _ruleImporter.DoImport(model, _sbmlProject, _sbmlInformation, _command);
      }

      /// <summary>
      ///     Creates a Parameter Start Values Building Block and adds it to the MoBi Project.
      /// </summary>
      protected internal void CreateParameterStartValuesBuildingBlock(Model model)
      {
         var parameterStartValuesCreator = IoC.Resolve<IParameterStartValuesCreator>();
         var ss = GetMainSpatialStructure(model);
         var mb = GetMainMoleculeBuildingBlock();
         if (ss == null || mb == null) return;

         var psvBb = parameterStartValuesCreator.CreateFrom(GetMainSpatialStructure(model), GetMainMoleculeBuildingBlock())
            .WithId(SBMLConstants.SBML_PARAMETERSTARTVALUES_BB)
            .WithName(SBMLConstants.SBML_PARAMETERSTARTVALUES_BB);
         _command.AddCommand(new AddBuildingBlockCommand<IParameterStartValuesBuildingBlock>(psvBb).Run(_context));
      }

      //For Rate Rule only
      public override void AddToProject()
      {
         
      }
   }
}