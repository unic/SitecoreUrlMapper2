﻿namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using Sitecore.Annotations;
    using Sitecore.ContentSearch;
    using Unic.UrlMapper2.Definitions;

    [UsedImplicitly]
    public class TargetUrlComputedField : UrlMapperComputedFieldBase
    {
        protected override object Compute(IIndexable indexable) => indexable?.GetFieldById(Constants.Fields.Redirect.Target)?.Value;
    }
}