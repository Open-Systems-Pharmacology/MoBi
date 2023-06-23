using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core
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
      private Reaction _inputReaction;
      private IDimension _dim;
      private IFormula _formula;
      private ReactionPartner _educt;
      private MoleculeAmount _eductAmount;
      private MoleculeAmount _productAmount;
      private ReactionPartner _product;
      private ReactionDTO _result;
      private string _kinetic;

      protected override void Context()
      {
         base.Context();
         _dim = A.Fake<IDimension>();
         _inputReaction = new Reaction();
         _formula = A.Fake<Formula>();
         _kinetic = "Kinetic";
         A.CallTo(() => _formula.ToString()).Returns(_kinetic);
         _inputReaction.Dimension = _dim;
         _inputReaction.Formula = _formula;
         _eductAmount = A.Fake<MoleculeAmount>().WithName("Educt");
         _productAmount = A.Fake<MoleculeAmount>().WithName("Product");
         _educt = new ReactionPartner(1.0, _eductAmount);

         _product = new ReactionPartner(1.0, _productAmount);

         _inputReaction.AddEduct(_educt);
         _inputReaction.AddProduct(_product);

         A.CallTo(() => _stoichiometricStringCreate.CreateFrom(A<IEnumerable<ReactionPartner>>._, A<IEnumerable<ReactionPartner>>._))
            .Returns($"1 {_eductAmount.Name} => 1 {_productAmount.Name}");
      }

      protected override void Because()
      {

         _result = sut.MapFrom(_inputReaction);
      }
      [Observation]
      public void should_map_the_properties()
      {
         _result.Kinetic.ShouldBeEqualTo(_kinetic);
      }

      [Observation]
      public void should_generate_stoichiometric_string()
      {
         _result.Stoichiometric.ShouldBeEqualTo($"1 {_eductAmount.Name} => 1 {_productAmount.Name}");
      }
   }
}