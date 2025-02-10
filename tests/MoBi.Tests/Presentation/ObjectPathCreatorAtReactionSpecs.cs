using System;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Presentation
{
   public abstract class concern_for_ObjectPathCreatorAtReactionSpecs : ContextSpecification<IObjectPathCreatorAtReaction>
   {
      protected IMoBiContext _context;

      protected override void Context()
      {
         IAliasCreator aliasCreator = new AliasCreator();
         _context = A.Fake<IMoBiContext>();
         sut = new ObjectPathCreatorAtReaction(new ObjectPathFactory(aliasCreator),aliasCreator,_context);
      }
   }

   class When_creating_a_realtiv_path_to_a_local_moleculeProperty_at_a_reaction : concern_for_ObjectPathCreatorAtReactionSpecs
   {
      private DummyParameterDTO _concentrationDTO;
      private IContainer _localCompartment;
      private ReferenceDTO _referenceDTO;
      private IDimension _dimension;

      protected override void Context()
      {
         base.Context();
         var concentrationParameter =
            new Parameter().WithName("Concentration")
               .WithId("CondId")
               .WithDimension(_dimension)
               .WithParentContainer(new MoleculeBuilder().WithName("Drug"));
         _concentrationDTO = new DummyParameterDTO(concentrationParameter).WithId("Dum");
         _concentrationDTO.ModelParentName = "Drug";

         _localCompartment = new Container().WithName("Plasma");
         IContainer moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(_localCompartment);
         _concentrationDTO.Parent = moleculeProperties;
         _dimension = A.Fake<IDimension>();
         concentrationParameter.BuildMode = ParameterBuildMode.Local;
      }

      protected override void Because()
      {
         _referenceDTO = sut.CreatePathFromParameterDummy(_concentrationDTO, shouldCreateAbsolutePaths: false, refObject: _localCompartment, editedObject: A.Fake<IUsingFormula>());
      }

      [Observation]
      public void should_return_correct_reference_dto()
      {
         _referenceDTO.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
         _referenceDTO.Path.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, "Drug", "Concentration");
         
      }
   }
}	