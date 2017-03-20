using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class DistributionParameterDTO : ValueEditDTO
   {
      public IParameter Parameter { get; private set; }

      public DistributionParameterDTO(IParameter parameter)
      {
         Parameter = parameter;
         Parameter.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
      }

      public override double KernelValue
      {
         get
         {
            try
            {
               //this might throws an exception if the formula is not defined properly
               return Parameter.Value;
            }
            catch (Exception)
            {
               return Double.NaN;
            }
         }
         set
         {
            /*nothing to do is taken care of by commands*/
         }
      }
   }

   public class DistributedParameterDTO : ObjectBaseDTO
   {
      private readonly IDistributedParameter _distributedParameter;
      public DistributionParameterDTO GeometricDeviation { set; get; }
      public DistributionParameterDTO Deviation { get; set; }
      public DistributionParameterDTO Mean { get; set; }
      public DistributionFormulaType FormulaType { get; set; }
      public DistributionParameterDTO Value { get; set; }
      public DistributionParameterDTO Maximum { get; set; }
      public DistributionParameterDTO Minimum { get; set; }

      public DistributedParameterDTO(IDistributedParameter distributedParameter)
      {
         _distributedParameter = distributedParameter;
      }

      public IDimension Dimension
      {
         get { return Value.Dimension; }
         set { Value.Dimension = value; }
      }

      public double Percentile
      {
         get { return _distributedParameter.Percentile; }
         set
         {
            /*nothing to do is taken care of by commands*/
         }
      }
   }

   public enum DistributionFormulaType
   {
      DiscreteDistribution,
      UniformDistribution,
      NormalDistribution,
      LogNormalDistribution
   }
}