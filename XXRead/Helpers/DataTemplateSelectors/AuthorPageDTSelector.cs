using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXRead.Helpers.DataTemplateSelectors
{
    public class AuthorPageDTSelector : DataTemplateSelector
    {
        public DataTemplate AuthorInfoTemplate { get; set; }
        public DataTemplate AuthorStoriesTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (item as string) == "Info" ? AuthorInfoTemplate : AuthorStoriesTemplate;
        }
    }
}
