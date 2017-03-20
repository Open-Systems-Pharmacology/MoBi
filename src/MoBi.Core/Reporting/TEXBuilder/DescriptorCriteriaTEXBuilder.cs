using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class DescriptorCriteriaTEXBuilder : TeXChunkBuilder<DescriptorCriteria>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public DescriptorCriteriaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(DescriptorCriteria descriptorCriteria, BuildTracker buildTracker)
      {
         buildTracker.TeX.Append(TeXChunk(descriptorCriteria));
         buildTracker.Track(descriptorCriteria);
      }

      public override string TeXChunk(DescriptorCriteria descriptorCriteria)
      {
         return _builderRepository.ChunkFor(ListFor(descriptorCriteria));
      }

      private string[] ListFor(DescriptorCriteria descriptorCriteria)
      {
         var strings = new List<string>();
         foreach (var criteria in descriptorCriteria)
         {
            var match = criteria as MatchTagCondition;
            if (match != null)
            {
               strings.Add(String.Format("{0} {1}", Constants.TAGGED_WITH, OSPSuite.TeXReporting.TeX.Converter.DefaultConverter.Instance.StringToTeX(match.Tag)));
            }
            var notmatch = criteria as NotMatchTagCondition;
            if (notmatch != null)
            {
               strings.Add(String.Format("{0} {1}", Constants.NOT_TAGGED_WITH, OSPSuite.TeXReporting.TeX.Converter.DefaultConverter.Instance.StringToTeX(notmatch.Tag)));
            }
            var allmatch = criteria as MatchAllCondition;
            if (allmatch != null)
            {
               strings.Add(allmatch.Tag);
            }
         }

         return strings.ToArray();
      }
   }
}
