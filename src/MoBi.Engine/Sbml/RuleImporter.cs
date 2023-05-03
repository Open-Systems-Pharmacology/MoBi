using System;
using libsbmlcs;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
    public class RuleImporter : AssignmentImporterBase
    {
       private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

       public RuleImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory) : base(objectPathFactory, objectBaseFactory,astHandler, context)
       {
          _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
       }

       protected override void Import(Model model)
        {
            for (long i = 0; i < model.getNumRules(); i++)
            {
                var rule = model.getRule(i);
                if (rule.isAssignment())
                    createAssignmentRule(rule, model);
                if (rule.isRate())
                    createRateRule(rule, model);
                if (rule.isAlgebraic())
                    createAlgebraicRule(model);
            }
            AddToProject();
        }

       /// <summary>
       ///     The creation of Algebraic Rules is not supported. 
       /// </summary>
       private void createAlgebraicRule(Model model)
        {
            var msg = new NotificationMessage(GetMainSpatialStructure(model), MessageOrigin.All, null, NotificationType.Warning)
            {
                Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": SBML Algebraic Rule was not imported."
            };
            _sbmlInformation.NotificationMessages.Add(msg);
        }

       /// <summary>
       ///     Creates a Rate Rule by setting the formula of the affected Species/Compartment/Parameter
       ///     to the Rule's one. 
       /// </summary>
       private void createRateRule(Rule rule, Model model)
        {
            if (IsParameter(rule.getVariable()))
            {
                var parameter = GetParameter(rule.getVariable());
                SetPSV(rule.getMath(), parameter, String.Empty);
                return;
            }

            if (IsContainerSizeParameter(rule.getVariable()))
            {
                var sizeParameter = GetContainerSizeParameter(rule.getVariable());
                SetPSV(rule.getMath(), sizeParameter, rule.getVariable());
                return;
            }

            if (IsSpeciesAssignment(rule.getVariable()))
            {
                doSpeciesAssignment(rule);
            }
            
            CheckSpeciesReferences(rule.getId(), rule.getVariable(), model);
        }

       /// <summary>
       ///     Creates a Assignment Rule
       /// </summary>
       private void createAssignmentRule(Rule rule, Model model)
        {
            if (IsParameter(rule.getVariable()))
            {
                var parameter = GetParameter(rule.getVariable());
                SetPSV(rule.getMath(), parameter, String.Empty);
                return;
            }

            if (IsContainerSizeParameter(rule.getVariable()))
            {
                var sizeParameter = GetContainerSizeParameter(rule.getVariable());
                SetPSV(rule.getMath(), sizeParameter, rule.getVariable());
                return;
            }

            if (IsSpeciesAssignment(rule.getVariable()))
                DoSpeciesAssignment(rule.getVariable(), rule.getMath(), false);

            CheckSpeciesReferences(rule.getId(), rule.getVariable(), model);
        }

        /// <summary>
        ///     Creates a reaction product with the default stoichiometry.
        /// </summary>
        private static ReactionPartnerBuilder createProduct(string molculeName)
        {
            ReactionPartnerBuilder productBuilder = new ReactionPartnerBuilder
            {
                MoleculeName = molculeName
                
            };
            productBuilder.StoichiometricCoefficient = SBMLConstants.SBML_STOICHIOMETRY_DEFAULT;
            return productBuilder;
        }

        /// <summary>
        ///     This is for the special Species Assignment in a RateRule.
        /// </summary>
        private void doSpeciesAssignment(Rule rule)
        {
            var formula = _astHandler.Parse(rule.getMath(), rule.getVariable(), true, _sbmlModule,_sbmlInformation);
            if (formula == null) return;
            var reactionBuilder = ObjectBaseFactory.Create<ReactionBuilder>()
                .WithName(SBMLConstants.RATE_RULE + rule.getMetaId())
                .WithFormula(formula);

            var product = createProduct(rule.getVariable());
            reactionBuilder.AddProduct(product);

            var rbb = GetMainReactionBuildingBlock();
            if (rbb == null)
            {
                rbb = _reactionBuildingBlockFactory.Create()
                    .WithName(SBMLConstants.SBML_REACTION_BB);
               _context.AddToHistory(new AddBuildingBlockToModuleCommand<MoBiReactionBuildingBlock>(rbb, _sbmlModule));
            }
            rbb.FormulaCache.Add(formula);
            rbb.Add(reactionBuilder);
        }

        public override void AddToProject(){}
    }
}
