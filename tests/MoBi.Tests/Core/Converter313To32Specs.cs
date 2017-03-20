using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Serialization.Converter.v3_2;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core
{
   public abstract class concern_for_Converter313To32 : ContextSpecification<Converter313To32>
   {
      protected IDimensionConverter _dimensionConverter;
      private IMoBiDimensionFactory _dimensionFactory;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormulaMapper _formulaMapper;
      private IDimensionMapper _dimensionMapper;
      private IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _dimensionConverter = A.Fake<IDimensionConverter>();
         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaMapper>();
         _dimensionMapper = A.Fake<IDimensionMapper>();
         _eventPublisher = A.Fake<IEventPublisher>();
         sut = new Converter313To32(_dimensionConverter, _dimensionFactory, _objectBaseFactory, _formulaMapper, _dimensionMapper, _eventPublisher);
      }
   }

   class When_converting_an_mobi_project : concern_for_Converter313To32
   {
      private IMoBiProject _mobiProject;
      private DataRepository _observedData;

      protected override void Context()
      {
         base.Context();
         _mobiProject = new MoBiProject();
         _observedData = new DataRepository();
         _mobiProject.AddObservedData(_observedData);
      }

      protected override void Because()
      {
         sut.Convert(_mobiProject, null);
      }

      [Observation]
      public void should_call_dimension_converter_for_data_repositories()
      {
         A.CallTo(() => _dimensionConverter.ConvertDimensionIn(_observedData)).MustHaveHappened();
      }
   }

   public class When_converting_an_old_dimension_xml_files : concern_for_Converter313To32
   {
      private XElement _element;

      protected override void Context()
      {
         base.Context();
         _element = new XElement("DimensionFactory",
                                 new XElement("Dimensions",
                                              new XElement("Dimension", new XAttribute("name", "dim1"), new XAttribute("BaseUnit", "unit1"), new XAttribute("DefaultUnit", "unit2")),
                                              new XElement("Dimension", new XAttribute("name", "dim2"), new XAttribute("BaseUnit", "unit1"), new XAttribute("DefaultUnit", "unit2")),
                                              new XElement("Dimension", new XAttribute("name", "dim3"), new XAttribute("BaseUnit", "unit1"))
                                    )
            );
      }

      protected override void Because()
      {
         sut.ConvertXml(_element, null);
      }

      [Observation]
      public void should_have_replaced_the_attributes_to_lower_case_values()
      {
         var allBaseUnits = _element.DescendantsAndSelf().Where(x => x.Attribute("baseUnit") != null);
         allBaseUnits.Count().ShouldBeEqualTo(3);

         var allDefaultUnits = _element.DescendantsAndSelf().Where(x => x.Attribute("defaultUnit") != null);
         allDefaultUnits.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_converting_an_observed_building_block_element_without_builder : concern_for_Converter313To32
   {
      private XElement _element;

      protected override void Context()
      {
         base.Context();
         base.Context();
         _element = new XElement("ObserverBuildingBlock");
      }

      [Observation]
      public void should_not_crash()
      {
         sut.ConvertXml(_element, null);
      }
   }
}