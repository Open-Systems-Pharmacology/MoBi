﻿using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SynchronizeParameterValueCommand : ContextSpecification<SynchronizeParameterValueCommand>
   {
      protected ParameterValue _parameterStartValue;
      protected IParameter _parameter;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _parameter = new Parameter();
         _parameterStartValue = new ParameterValue();
         _context = A.Fake<IMoBiContext>();
         sut = new SynchronizeParameterValueCommand(_parameter, _parameterStartValue, new ParameterValuesBuildingBlock());
      }
   }

   public class synchronizing_a_parameter_value_with_a_parameter : concern_for_SynchronizeParameterValueCommand
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Value = 5;
         _parameter.Dimension = A.Fake<IDimension>();
         _parameter.ValueOrigin.Description = "BLA BLA";
         _parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.Assumption;
         _parameter.DisplayUnit = A.Fake<Unit>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_value()
      {
         _parameterStartValue.Value.ShouldBeEqualTo(_parameter.Value);
      }

      [Observation]
      public void should_update_the_dimension()
      {
         _parameterStartValue.Dimension.ShouldBeEqualTo(_parameter.Dimension);
      }

      [Observation]
      public void should_update_the_display_unit()
      {
         _parameterStartValue.DisplayUnit.ShouldBeEqualTo(_parameter.DisplayUnit);
      }

      [Observation]
      public void should_update_the_value_description()
      {
         _parameterStartValue.ValueOrigin.ShouldBeEqualTo(_parameter.ValueOrigin);
      }
   }
}