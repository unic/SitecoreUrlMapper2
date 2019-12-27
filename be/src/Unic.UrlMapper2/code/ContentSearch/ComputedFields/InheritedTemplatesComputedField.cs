namespace Unic.UrlMapper2.ContentSearch.ComputedFields
{
    using System.Collections.Generic;
    using Sitecore;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;

    // Borrowed from https://github.com/blipson89/Synthesis/blob/master/Source/Synthesis/ContentSearch/ComputedFields/InheritedTemplates.cs
    [Sitecore.Annotations.UsedImplicitly]
    internal class InheritedTemplatesComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public virtual object ComputeFieldValue(IIndexable indexable) => this.GetAllTemplates(indexable as SitecoreIndexableItem);

        protected virtual List<string> GetAllTemplates(Item item)
        {
            if (item == null)
            {
                return new List<string>();
            }

            var stringList = new List<string>
            {
                IdHelper.NormalizeGuid(item.TemplateID)
            };

            this.RecurseTemplates(stringList, item.Template);
            return stringList;
        }

        protected virtual void RecurseTemplates(ICollection<string> list, TemplateItem template)
        {
            foreach (var baseTemplate in template.BaseTemplates)
            {
                list.Add(IdHelper.NormalizeGuid(baseTemplate.ID));
                if (baseTemplate.ID != TemplateIDs.StandardTemplate)
                {
                    this.RecurseTemplates(list, baseTemplate);
                }
            }
        }
    }
}