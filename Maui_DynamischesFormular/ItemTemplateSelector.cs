using Maui_DynamischesFormular.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maui_DynamischesFormular
{

    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate DateTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                TextItem => TextTemplate,
                BooleanItem => BoolTemplate,
                DateItem => DateTemplate,
                _ => null
            };
        }
    }
}
