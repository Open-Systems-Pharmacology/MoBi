using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreatePresenter<T> : ICommandCollectorPresenter
   {
      void Edit(T objectToEdit, IEnumerable<IObjectBase> existingObjectsInParent);
   }

   public interface ICanEditPropertiesPresenter : IPresenter
   {
      void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue);
      void RenameSubject();
   }

   public interface IEditPresenterWithParameters : IEditPresenter
   {
      void SelectParameter(IParameter parameter);
   }

   public interface IEditPresenterWithParameters<T> : IEditPresenter<T>, IEditPresenterWithParameters, ICreatePresenter<T>
   {
   }

   public interface IPresenterWithFormulaCache : IPresenter
   {
      IEnumerable<FormulaBuilderDTO> GetFormulas();
      IBuildingBlock BuildingBlock { get; set; }
      IFormulaCache FormulaCache { get; }
   }
}