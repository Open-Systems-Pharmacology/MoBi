using System.Collections;
using System.Collections.Generic;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;

namespace MoBi.Application
{
   public abstract class concern_for_ReactionToDTOReactionMapper : ContextSpecification<IReactionToReactionDTOMapper>
   {
      protected IStoichiometricStringCreator _stoichiometricStringCreate;

      protected override void Context()
      {
         _stoichiometricStringCreate = A.Fake<IStoichiometricStringCreator>();
         sut = new ReactionToReactionDTOMapper(_stoichiometricStringCreate);
      }
   }

   public class When_mapping_from_input_reaction : concern_for_ReactionToDTOReactionMapper
   {
      private IReaction _inputReaction;
      private IDimension _dim;
      private IFormula _formula;
      private IReactionPartner _educt;
      private IMoleculeAmount _eductAmount;
      private IMoleculeAmount _productAmount;
      private IReactionPartner _product;
      private ReactionDTO _result;
      private string _kinetic;

      protected override void Context()
      {
         base.Context();
         _dim = A.Fake<IDimension>();
         _inputReaction = A.Fake<IReaction>();
         _formula = A.Fake<Formula>();
         _kinetic = "Kinetic";
         A.CallTo(() => _formula.ToString()).Returns(_kinetic);
         _inputReaction.Dimension = _dim;
         _inputReaction.Formula = _formula;
         _eductAmount = A.Fake<IMoleculeAmount>().WithName("Educt");
         _productAmount = A.Fake<IMoleculeAmount>().WithName("Product");
         _educt = A.Fake<IReactionPartner>();
         A.CallTo(() => _educt.Partner).Returns(_eductAmount);
         A.CallTo(() => _educt.StoichiometricCoefficient).Returns(1);
         _product = A.Fake<IReactionPartner>();
         A.CallTo(() => _product.Partner).Returns(_productAmount);
         A.CallTo(() => _product.StoichiometricCoefficient).Returns(1);
         A.CallTo(() => _inputReaction.Educts).Returns(new[] { _educt });
         A.CallTo(() => _inputReaction.Products).Returns(new[] { _product });
         A.CallTo(() => _stoichiometricStringCreate.CreateFrom(A<IEnumerable<IReactionPartner>>._, A<IEnumerable<IReactionPartner>>._))
            .Returns(string.Format("1 {0} => 1 {1}", _eductAmount.Name, _productAmount.Name));
   }

      protected override void Because()
      {
         
         _result= sut.MapFrom(_inputReaction);
      }
      [Observation]
      public void should_map_the_properties()
      {
         _result.Kinetic.ShouldBeEqualTo(_kinetic);
      }

      [Observation] 
      public void should_generate_stoichiometric_string()
      {
         _result.Stoichiometric.ShouldBeEqualTo(string.Format("1 {0} => 1 {1}", _eductAmount.Name, _productAmount.Name));
      }
   }
}	