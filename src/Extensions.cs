using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using System.Linq;

namespace SharedSitecore.Extensions.ItemExtensions
{
    public static class Extensions
    {
        /// <summary>
        /// Get Parent of Item with TemplateId and Level (aka: ancestor/inheritance/parent depth/level) filters
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Item GetParent(this Item item, ID templateId = null, int level = -1)
        {
            if (item == null) return null;
            if (level == 0) return item;
            if (level == 1) return item.Parent;
            var current = item.Parent;
            var count = 1;
            while (current != null)
            {
                count++;
                if (current.TemplateID == templateId || (level != -1 && count == level)) return current;
                current = current.Parent;
            }
            return null;
        }

        public static Template GetTemplate([NotNull] this TemplateItem template, [NotNull] ID templateId) => !template.IsDerived(templateId) ? null : TemplateManager.GetTemplate(template);

        public static bool IsDerived([NotNull] this Item item, ID templateId) => TemplateManager.GetTemplate(item).IsDerived(templateId);
        public static bool IsDerived([NotNull] this Template template, [NotNull] ID templateId) => template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
        public static bool IsDerived([NotNull] this TemplateItem template, [NotNull] ID templateId) => template.ID == templateId || template.BaseTemplates.Any(baseTemplate => IsDerived(baseTemplate, templateId));
    }
}