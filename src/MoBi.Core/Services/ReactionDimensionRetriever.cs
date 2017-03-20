using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Services
{
   public class ReactionDimensionRetriever : IReactionDimensionRetriever
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public ReactionDimensionRetriever(IDimensionFactory dimensionFactory, IMoBiProjectRetriever projectRetriever)
      {
         _dimensionFactory = dimensionFactory;
         _projectRetriever = projectRetriever;
      }

      public IDimension ReactionDimension
      {
         get { return getDimension(Constants.Dimension.AMOUNT_PER_TIME, Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME);}
      }

      public IDimension MoleculeDimension
      {
         get { return getDimension(Constants.Dimension.AMOUNT, Constants.Dimension.MOLAR_CONCENTRATION); }
      }

      public ReactionDimensionMode SelectedDimensionMode
      {
         get { return _projectRetriever.Current.ReactionDimensionMode; }
      }

      private IDimension getDimension(string amountBased, string concentrationBased)
      {
         if (SelectedDimensionMode == ReactionDimensionMode.AmountBased)
               return _dimensionFactory.GetDimension(amountBased);

         return _dimensionFactory.GetDimension(concentrationBased);
      }
   }
}