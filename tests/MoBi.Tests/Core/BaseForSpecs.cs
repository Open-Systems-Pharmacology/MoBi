using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Core
{
   public class SpecsBaseWithModel<T_Serializer, T_Object> : SpecsBase<T_Serializer, T_Object>
      where T_Serializer : OSPSuiteXmlSerializer<T_Object>
      where T_Object : class
   {
      protected IContainer _comp11;
      protected IParameter _comp11P4;
      protected IContainer _comp21;
      protected ExplicitFormula _formula_3;
      protected ExplicitFormula _formula_P1plusP4;
      protected IDimension _length;
      protected IContainer _model;

      protected IParameter _modelP1;
      protected IContainer _organ1;
      protected IParameter _organ1P2;
      protected IParameter _organ1P3;
      protected IContainer _organ2;
      protected IMoleculeAmount _organ2SX;
      private IObjectPathFactory _pathFactory;

      protected IFormulaUsablePath _path_P2_P1;
      protected IFormulaUsablePath _path_P2_P4;
      protected IDimension _specificMass;
      protected IDimension _temperature;

      [OneTimeSetUp]
      public override void Start()
      {
         base.Setup();
         _pathFactory = IoC.Resolve<IObjectPathFactory>();
         InitializeModel();
      }

      protected virtual void InitializeModel()
      {
         _length = Context.DimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
         _length.AddUnit("mm", 0.001, 0.0);
         _length.AddUnit("cm", 0.01, 0);
         _length.AddUnit("µm", 0.000001, 0);
         _length.AddUnit("nm", 0.000000001, 0);
         _length.DefaultUnit = _length.Unit("mm");

         _temperature = Context.DimensionFactory.AddDimension(new BaseDimensionRepresentation {TemperatureExponent = 1}, "Temperature", "K");
         _temperature.AddUnit( "C", 1, 273.19);

         _specificMass = Context.DimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = -3, MassExponent = 1}, "SpecificMass", "kg/m^3" );
         _specificMass.AddUnit("g/cm^3", 1000, 0.0);
         _specificMass.AddUnit("kg/l", 1000, 0);

         _model = Context.Create<IContainer>(null).WithName("Model");
         _organ1 = Context.Create<Container>(null).WithName("Organ1").WithParentContainer(_model);
         _organ2 = Context.Create<Container>(null).WithName("Organ2").WithParentContainer(_model);
         _comp11 = Context.Create<Container>(null).WithName("Comp11").WithParentContainer(_organ1);
         _comp21 = Context.Create<Container>(null).WithName("Comp21").WithParentContainer(_organ2);

         _modelP1 = Context.Create<Parameter>(null).WithName("P1").WithParentContainer(_model).WithDimension(_length);
         _modelP1.Value = 1.1;
         _modelP1.IsFixedValue = true;

         _comp11P4 = Context.Create<Parameter>(null).WithName("P4").WithParentContainer(_comp11).WithDimension(_length);
         _comp11P4.Value = 4.4;
         _comp11P4.IsFixedValue = true;

         _organ1P2 = Context.Create<Parameter>(null).WithName("P2").WithParentContainer(_organ1).WithDimension(_length);
         _path_P2_P1 = _pathFactory.CreateRelativeFormulaUsablePath(_organ1P2, _modelP1);
         _path_P2_P4 = _pathFactory.CreateRelativeFormulaUsablePath(_organ1P2, _comp11P4);
         _formula_P1plusP4 = Context.Create<ExplicitFormula>(null).WithFormulaString("P1 + P4");
         _formula_P1plusP4.AddObjectPath(_path_P2_P1);
         _formula_P1plusP4.AddObjectPath(_path_P2_P4);
         _organ1P2.Formula = _formula_P1plusP4;

         _organ1P3 = Context.Create<Parameter>(null).WithName("P3").WithParentContainer(_organ1); //Dimension = null
         _formula_3 = Context.Create<ExplicitFormula>(null).WithFormulaString("3");
         _organ1P3.Formula = _formula_3;

         _organ2SX = Context.Create<IMoleculeAmount>(null).WithName("SX").WithParentContainer(_organ2).WithDimension(_specificMass);
         _organ2SX.Value = 8;
      }
   }

   public class SpecsBase<T_Serializer, T_Object> : BaseForSpecs
      where T_Serializer : OSPSuiteXmlSerializer<T_Object>
      where T_Object : class
   {
      protected T_Serializer _sut;

      [OneTimeSetUp]
      public virtual void Start()
      {
      }

      [SetUp]
      public virtual void Setup()
      {

         _sut = (T_Serializer) XmlSerializerRepository.SerializerFor(typeof (T_Object));
      }

      protected T SerializeAndDeserializeGeneral<T>(T x1)
      {
         var serializer = XmlSerializerRepository.SerializerFor(x1);
         XElement xel;
         using (var serializationContext = SerializationTransaction.Create())
         {
            xel = serializer.Serialize(x1,serializationContext);
            
         }
         using (var serializationContext = SerializationTransaction.Create(dimensionFactory: _deserializationContext.DimensionFactory, objectBaseFactory: _deserializationContext.ObjectBaseFactory, withIdRepository: _deserializationContext.ObjectRepository))
         {
            return serializer.Deserialize<T>(xel,serializationContext);
         }
      }

   

      [TearDown]
      public virtual void TearDown()
      {
      }

      [OneTimeTearDown]
      public virtual void End()
      {
      }
   }

   public class BaseForSpecs
   {
      private readonly IMoBiContext _context;

      private readonly IXmlSerializerRepository<SerializationContext> _xmlSerializerRepository;
      protected IMoBiContext _deserializationContext;

      public BaseForSpecs()
      {
         MoBiCoreGlobalsForSpecs.Setup();
         _context = IoC.Resolve<IMoBiContext>();
         _xmlSerializerRepository = IoC.Resolve<IXmlSerializerRepository<SerializationContext>>();

         SetNewDeserializationContext();
      }

      public IMoBiContext Context
      {
         get { return _context; }
      }

      public IMoBiContext DeserializationContext
      {
         get { return _deserializationContext; }
      }

      public IXmlSerializerRepository<SerializationContext> XmlSerializerRepository
      {
         get { return _xmlSerializerRepository; }
      }

      protected void SetNewDeserializationContext()
      {
         _deserializationContext = IoC.Resolve<IMoBiContext>();
      }
   }
}