using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core
{
   public class PureObjectBase : ObjectBase
   {
   }

   public class PureEntity : Entity
   {
   }

   public class PureFormula : Formula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         return 0.0;
      }
   }

   public class MoBiTestObjectBaseFactory : ObjectBaseFactory
   {
      public MoBiTestObjectBaseFactory(IContainer container, IDimensionFactory dimensionFactory, IIdGenerator idGenerator, ICreationMetaDataFactory creationMetaDataFactory)
         : base(container, dimensionFactory, idGenerator,creationMetaDataFactory)
      {
      }
   }

   static public class MoBiCoreGlobalsForSpecs
   {
      /// <summary>
      /// Sets up the global configuration that a MoBi Application need to run
      /// </summary>
      static public void Setup()
      {
         InitDependency();
         InitContext();
      }

      static private void InitContext()
      {
         var context = IoC.Resolve<IMoBiContext>();
      }

      /// <summary>
      /// Initialize the Configuration of the Dependency using Castle
      /// </summary>
      static private void InitDependency()
      {
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);

         // Global Singleton Objects
         container.Register<IMoBiContext, MoBiContext>(LifeStyle.Singleton);
         container.Register<IObjectBaseFactory, MoBiTestObjectBaseFactory>();
         container.Register<IObjectBaseFactory, MoBiTestObjectBaseFactory>();
         container.Register<IMoBiDimensionFactory, MoBiDimensionFactory>(LifeStyle.Singleton);
         container.Register<IDimensionFactory, MoBiMergedDimensionFactory>();
         RegisterSerializationDependencies();
      }

      static private void RegisterSerializationDependencies()
      {
         var serializerRepository = new ModelCoreTestXmlSerializerRepository();
         IoC.Container.RegisterImplementationOf(serializerRepository as IXmlSerializerRepository<SerializationContext>);
      }
   }

   internal class ModelCoreTestXmlSerializerRepository : OSPSuiteXmlSerializerRepository
   {
      protected override void AddInitialSerializer()
      {
         base.AddInitialSerializer();
         this.AddSerializers(x =>
         {
            x.Implementing<IXmlSerializer>();
            x.InAssemblyContainingType<BaseForSpecs>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });
      }
   }
}