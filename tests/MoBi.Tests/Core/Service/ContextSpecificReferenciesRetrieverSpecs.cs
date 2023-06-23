using System.Collections.Generic;
using OSPSuite.Utility.Validation;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Services;

using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Core.Service
{
   public abstract class concern_for_ContetxSpecificeferenciesRetrieverSpecs : ContextSpecification<ContextSpecificReferencesRetriever>
   {
      protected override void Context()
      {
         sut = new ContextSpecificReferencesRetriever();
      }
   }

   class When_retrieving_the_local_reference_point_for_a_local_reaction_parameter : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IEntity _result;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         var reactionBuilder = new ReactionBuilder().WithName("R1");
         _parameter = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Local).WithParentContainer(reactionBuilder);
      }

      protected override void Because()
      {
         _result = sut.RetrieveLocalReferencePoint(_parameter);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   class When_retrieving_the_local_reference_point_for_a_global_reaction_parameter : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IEntity _result;
      private IParameter _parameter;
      private ReactionBuilder _reactionBuilder;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder = new ReactionBuilder().WithName("R1");
         _parameter = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Global).WithParentContainer(_reactionBuilder);
      }

      protected override void Because()
      {
         _result = sut.RetrieveLocalReferencePoint(_parameter);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeEqualTo(_parameter);
      }
   }

   class When_retrieving_the_local_reference_point_for_a_parameter : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IEntity _result;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         var container = new Container().WithName("C1");
         _parameter = new Parameter().WithName("P1").WithMode(ParameterBuildMode.Global).WithParentContainer(container);
      }

      protected override void Because()
      {
         _result = sut.RetrieveLocalReferencePoint(_parameter);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeEqualTo(_parameter);
      }
   }



   class When_getting_local_referencies_for_a_parameter : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IParameter _parameter;
      private IContainer _container;
      private IParameter _otherParameter;
      private IEnumerable<IObjectBase> _result;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("Container");
         _parameter = new Parameter().WithName("ReferencFrom").WithParentContainer(_container);
         _otherParameter = new Parameter().WithName("ToReference").WithParentContainer(_container);
      }

      protected override void Because()
      {
         _result =sut.RetrieveFor(_parameter,A.Fake<IBuildingBlock>());
      }

      [Observation]
      public void should_only_return_all_parameters()
      {
         _result.ShouldOnlyContain(_otherParameter,_parameter);
      }
   }

   class When_getting_local_referencies_for_a_parameter_at_reaction : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IParameter _parameter;
      private ReactionBuilder _reaction1;
      private IParameter _otherParameter;
      private IEnumerable<IObjectBase> _result;
      private ReactionBuildingBlock _reactionBuilidingBlock;
      private ReactionBuilder _reaction2;
      private IParameter _otherParameter2;
      private ReactionBuilder _reaction3;

      protected override void Context()
      {
         base.Context();
         
         _reaction1 = new ReactionBuilder().WithName("Reaction");
         _parameter = new Parameter().WithName("ReferencFrom").WithParentContainer(_reaction1);
         _otherParameter = new Parameter().WithName("ToReference").WithParentContainer(_reaction1);

         _reaction2 = new ReactionBuilder().WithName("Reaction2");
         _reaction3 = new ReactionBuilder().WithName("A Reaction");
         _otherParameter2 = new Parameter().WithName("ToReference").WithParentContainer(_reaction2);
         _reactionBuilidingBlock = new ReactionBuildingBlock();
         _reactionBuilidingBlock.Add(_reaction1);
         _reactionBuilidingBlock.Add(_reaction2);
         _reactionBuilidingBlock.Add(_reaction3);
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_parameter,_reactionBuilidingBlock);
      }

      [Observation]
      public void should_only_return_all_parameters_and_other_reactions_eachtpype_orderd_by_name()
      {
         _result.ShouldOnlyContainInOrder(_parameter, _otherParameter, _reaction3, _reaction2);
      }
   }

   class When_getting_local_referencies_for_a_parameter_at_a_transporter_molecule : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private IParameter _parameter;
      private IContainer _container;
      private IParameter _otherParameter;
      private IEnumerable<IObjectBase> _result;
      private IContainer _root;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName("Root");
         _container = new TransporterMoleculeContainer().WithName("Container").WithParentContainer(_root);
         _parameter = new Parameter().WithName("ReferencFrom").WithParentContainer(_container);
         _otherParameter = new Parameter().WithName("ToReference").WithParentContainer(_container);
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_parameter, A.Fake<IBuildingBlock>());
      }

      [Observation]
      public void should_only_return_other_parameters()
      {
         _result.ShouldOnlyContain((IObjectBase) _root);
      }
   }

   class When_getting_local_referencies_for_a_event_assingment : concern_for_ContetxSpecificeferenciesRetrieverSpecs
   {
      private EventAssignmentBuilder _eventAssignment;
      private IEnumerable<IObjectBase> _result;
      private IContainer _root;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName("Root");
         _eventAssignment = new EventAssignmentBuilder().WithName("Container").WithParentContainer(_root);
         
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_eventAssignment);
      }

      [Observation]
      public void should_only_return_other_parameters()
      {
         _result.ShouldOnlyContain((IObjectBase) _root);
      }
   }
}	