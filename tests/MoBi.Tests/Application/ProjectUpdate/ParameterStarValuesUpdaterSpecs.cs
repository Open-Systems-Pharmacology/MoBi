using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.Converter.v3_1;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Application.ProjectUpdate
{
   public abstract class concern_for_ParameterStarValuesUpdaterSpecs : ContextSpecification<IParameterStartValuesUpdater>
   {
      protected IList<ProjectConverterMessage> _messages;
      protected IParameterStartValueDimensionRetriever _dimensionRetriever;
      private IDimensionFactory _dimensionFactory;
      private IProjectConverterLogger _projectConverterLogger;

      protected override void Context()
      {
         _dimensionRetriever = A.Fake<IParameterStartValueDimensionRetriever>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _projectConverterLogger = A.Fake<IProjectConverterLogger>();
         sut = new ParameterStartValuesUpdater(_dimensionRetriever, _dimensionFactory, _projectConverterLogger);
      }
   }

   internal class When_updating_a_psvbb_to_Version_3_1_3 : concern_for_ParameterStarValuesUpdaterSpecs
   {
      private IParameterStartValuesBuildingBlock _psvbb;
      private IParameterStartValue _psv1;
      private IParameterStartValue _psv2;
      private IParameterStartValue _psv3;
      private IDimension _dim;
      private double _startValue;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _psvbb = new ParameterStartValuesBuildingBlock();
         _psv1 = new ParameterStartValue{Path = new ObjectPath("A")};
         _project = A.Fake<IMoBiProject>();

         _psv2 = new ParameterStartValue { Path = new ObjectPath("B") };
         _psv3 = new ParameterStartValue { Path = new ObjectPath(Constants.Distribution.GEOMETRIC_DEVIATION) };
         _psvbb.Add(_psv1);
         _psvbb.Add(_psv2);
         _psvbb.Add(_psv3);
         _startValue = 2;
         _psv3.StartValue = _startValue;
         _dim = A.Fake<IDimension>();
         A.CallTo(_dimensionRetriever).WithReturnType<IDimension>().Returns(_dim);
      }

      protected override void Because()
      {
         sut.UpdateParameterStartvalues(_psvbb, _project);
      }

      [Observation]
      public void should_ask_dimesnion_retirever_for_the_dimesion_of_each_parameter_start_value()
      {
         A.CallTo(() => _dimensionRetriever.GetDimensionFor(_psv1, _psvbb, _project)).MustHaveHappened();
         A.CallTo(() => _dimensionRetriever.GetDimensionFor(_psv2, _psvbb, _project)).MustHaveHappened();
         A.CallTo(() => _dimensionRetriever.GetDimensionFor(_psv3, _psvbb, _project)).MustHaveHappened();
      }

      [Observation]
      public void should_set_dimension_for_all_parameter_start_values()
      {
         _psv1.Dimension.ShouldBeEqualTo(_dim);
         _psv2.Dimension.ShouldBeEqualTo(_dim);
         _psv3.Dimension.ShouldBeEqualTo(_dim);
      }

      [Observation]
      public void should_change_value_of_psv_3()
      {
         _psv3.StartValue.ShouldBeEqualTo(Math.Exp(_startValue));
      }
   }
}