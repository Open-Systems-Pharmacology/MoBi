using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation
{
   public abstract class concern_for_ObjectPathCreatorAtTransportSpecs : ContextSpecification<IObjectPathCreatorAtTransport>
   {
      protected ReferenceDTO _result;
      protected readonly string _moleculeName = "Drug";
      protected readonly string _name = "Concentration";
      protected readonly string _localCompartmentName = "Compartment 1";
      protected DummyParameterDTO _concentrationDTO;
      protected IParameter _localParameter;
      protected IContainer _moleculeProperties;
      protected TransportBuilder _transportBuilder;

      private IMoBiContext _context;
      private IObjectPathFactory _objectPathFactory;
      private IContainer _localCompartment;

      protected override void Context()
      {
         IAliasCreator aliasCreator = new AliasCreator();
         _context = A.Fake<IMoBiContext>();
         _objectPathFactory = new ObjectPathFactory(aliasCreator);
         var concentrationParameter =
            new Parameter().WithName(_name)
               .WithParentContainer(new MoleculeBuilder().WithName(_moleculeName));
         _concentrationDTO = new DummyParameterDTO(concentrationParameter).WithId("Dum");
         _concentrationDTO.ModelParentName = _moleculeName;
         _concentrationDTO.Name = _name;
         _localCompartment = new Container().WithName(_localCompartmentName);
         _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithParentContainer(_localCompartment);
         _concentrationDTO.Parent = _moleculeProperties;
         _localCompartment = new Container().WithName("Plasma");
         _localParameter = new Parameter().WithName("PRef").WithParentContainer(_localCompartment);
         concentrationParameter.BuildMode = ParameterBuildMode.Local;
         sut = new ObjectPathCreatorAtTransport(_objectPathFactory, aliasCreator, _context);
         _transportBuilder = new TransportBuilder().WithName("Trans");
         sut.ProcessBuilder = _transportBuilder;
      }
   }

   internal class When_creating_an_absolute_path_for_parameterDummy : concern_for_ObjectPathCreatorAtTransportSpecs
   {
      protected override void Because()
      {
         _result = sut.CreatePathFromParameterDummy((ObjectBaseDTO)_concentrationDTO, true, _localParameter, _localParameter);
      }

      [Observation]
      public void should_return_correct_path()
      {
         _result.Path.ToString().ShouldBeEqualTo($"{_localCompartmentName}|{_moleculeName}|{_name}");
      }
   }

   internal class When_creating_an_relative_path_for_parameterDummy : concern_for_ObjectPathCreatorAtTransportSpecs
   {
      protected override void Because()
      {
         _result = sut.CreatePathFromParameterDummy((ObjectBaseDTO)_concentrationDTO, false, _localParameter, _localParameter);
      }

      [Observation]
      public void should_return_correct_path()
      {
         _result.Path.ToString().ShouldBeEqualTo($"..|..|..|..|{_localCompartmentName}|{"MOLECULE"}|{_name}");
      }
   }

   internal class When_creating_an_relative_path_for_parameterDummy_molecule : concern_for_ObjectPathCreatorAtTransportSpecs
   {
      protected override void Context()
      {
         base.Context();
         _transportBuilder.SourceCriteria = Create.Criteria(x => x.With("Compartment 1"));
      }

      protected override void Because()
      {
         _result = sut.CreatePathFromParameterDummy((ObjectBaseDTO)_concentrationDTO, false, _localParameter, _localParameter);
      }

      [Observation]
      public void should_return_correct_path()
      {
         _result.Path.ToString().ShouldBeEqualTo($"SOURCE|MOLECULE|{_name}");
      }
   }

   class When_creating_an_absolute_path_to_a_global_parameter_at_a_transport : concern_for_ObjectPathCreatorAtTransportSpecs
   {
      private ReferenceDTO _referenceDTO;
      private Parameter _concentrationParameter;
      private TransportBuilder _transport;

      protected override void Context()
      {
         base.Context();
         _concentrationParameter =
            new Parameter().WithName("Concentration")
               .WithId("CondId");

         _concentrationParameter.BuildMode = ParameterBuildMode.Global;
         _transport = new TransportBuilder().WithName("Transport");
         _transport.AddParameter(_concentrationParameter);
         sut.ProcessBuilder = _transport;
      }

      protected override void Because()
      {
         _referenceDTO = sut.CreatePathsFromEntity(_concentrationParameter, true, null, _transport);
      }

      [Observation]
      public void the_path_should_be_the_transport_and_parameter_name()
      {
         _referenceDTO.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
         _referenceDTO.Path.ShouldOnlyContainInOrder("Transport", "Concentration");
      }
   }

   class When_creating_a_relative_path_to_a_local_parameter_at_a_transport : concern_for_ObjectPathCreatorAtTransportSpecs
   {
      private ReferenceDTO _referenceDTO;
      private Parameter _concentrationParameter;
      private TransportBuilder _transport;

      protected override void Context()
      {
         base.Context();
         _concentrationParameter =
            new Parameter().WithName("Concentration")
               .WithId("CondId");

         _concentrationParameter.BuildMode = ParameterBuildMode.Local;
         _transport = new TransportBuilder();
         _transport.AddParameter(_concentrationParameter);
         sut.ProcessBuilder = _transport;
      }

      protected override void Because()
      {
         _referenceDTO = sut.CreatePathsFromEntity(_concentrationParameter, true, null, _transport);
      }

      [Observation]
      public void the_path_should_be_only_the_parameter_name()
      {
         _referenceDTO.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
         _referenceDTO.Path.ShouldOnlyContainInOrder("Concentration");
      }
   }
}