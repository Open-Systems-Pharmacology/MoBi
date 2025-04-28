using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public abstract class ObjectPathCreatorAtProcessBuilder<TBuilder> : ObjectPathCreatorBase where TBuilder : ProcessBuilder
   {
      public TBuilder ProcessBuilder { protected get; set; }

      protected ObjectPathCreatorAtProcessBuilder(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected FormulaUsablePath CreatePathToProcessProperty(IParameter parameter)
      {
         var parentProcess = GetProcessBuilderFor(parameter);

         // Always Absolute paths
         if (parameter.BuildMode != ParameterBuildMode.Local)
            return CreateFormulaUsablePathFrom(new[] { parentProcess.Name, parameter.Name }, parameter);

         // Parameter is child of process
         if (parentProcess.Equals(ProcessBuilder))
            return CreateFormulaUsablePathFrom(new[] { parameter.Name }, parameter);

         // go from process to other process and then to child parameter
         return CreateFormulaUsablePathFrom(new[] { ObjectPath.PARENT_CONTAINER, parentProcess.Name, parameter.Name }, parameter);
      }

      protected abstract TBuilder GetProcessBuilderFor(IParameter parameter);
   }
}