using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using EntityExtensions = MoBi.Core.Domain.Extensions.EntityExtensions;

namespace MoBi.Application
{
    public abstract class concern_forEntity_ExtensionsSpecs : StaticContextSpecification
    {
        
    }

    class When_Checking_if_a_Reaction_Parameter_is_a_reaction_parameter : concern_forEntity_ExtensionsSpecs
    {
        private IParameter _reactionParameter;
        private bool _result;

        protected override void Context()
        {
            base.Context();
            _reactionParameter = new Parameter().WithName("RP");
            var reactionBuilder = new ReactionBuilder();
            reactionBuilder.Add(_reactionParameter);
        }

        protected override void Because()
        {
            _result = EntityExtensions.IsAtReaction(_reactionParameter);
        }

        [Observation]
        public void should_return_true()
        {
            _result.ShouldBeTrue();
        }
    }

    class When_Checking_if_a_not_Reaction_Parameter_is_a_reaction_parameter : concern_forEntity_ExtensionsSpecs
    {
        private IParameter _reactionParameter;
        private bool _result;

        protected override void Context()
        {
            base.Context();
            _reactionParameter = new Parameter().WithName("RP");
            var moleculeBuilder = new MoleculeBuilder();
            moleculeBuilder.Add(_reactionParameter);
        }

        protected override void Because()
        {
            _result = EntityExtensions.IsAtReaction(_reactionParameter);
        }

        [Observation]
        public void should_return_false()
        {
            _result.ShouldBeFalse();
        }
    }

    class When_Checking_if_a_Molecule_Parameter_is_a_molecule_parameter : concern_forEntity_ExtensionsSpecs
    {
        private IParameter _moleculeParameter;
        private bool _result;

        protected override void Context()
        {
            base.Context();
            _moleculeParameter = new Parameter().WithName("RP");
            var moleculeBuilder = new MoleculeBuilder();
            moleculeBuilder.Add(_moleculeParameter);
        }

        protected override void Because()
        {
            _result = EntityExtensions.IsAtMolecule(_moleculeParameter);
        }

        [Observation]
        public void should_return_true()
        {
            _result.ShouldBeTrue();
        }
    }

    class When_Checking_if_a_Molecule_Properties_Parameter_is_a_molecule_parameter : concern_forEntity_ExtensionsSpecs
    {
        private IParameter _moleculeParameter;
        private bool _result;

        protected override void Context()
        {
            base.Context();
            _moleculeParameter = new Parameter().WithName("RP");
            var moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
            moleculeProperties.Add(_moleculeParameter);
            var compartment = new Container().WithName("Container");
            compartment.Add(moleculeProperties);
        }

        protected override void Because()
        {
            _result = EntityExtensions.IsAtMolecule(_moleculeParameter);
        }

        [Observation]
        public void should_return_true()
        {
            _result.ShouldBeTrue();
        }
    }

    class When_Checking_if_a_not_Molecule_Parameter_is_a_molecule_parameter : concern_forEntity_ExtensionsSpecs
    {
        private IParameter _moleculeParameter;
        private bool _result;

        protected override void Context()
        {
            base.Context();
            _moleculeParameter = new Parameter().WithName("RP");
            var reactionBuilder = new ReactionBuilder();
            reactionBuilder.Add(_moleculeParameter);
        }

        protected override void Because()
        {
            _result = EntityExtensions.IsAtMolecule(_moleculeParameter);
        }

        [Observation]
        public void should_return_false()
        {
            _result.ShouldBeFalse();
        }
    }
}