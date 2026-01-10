using System;
using System.Collections.Generic;
using System.Text;

//namespace ExampleDataTemplates
namespace Maui_DynamischesFormular
{
   
public class SettingTemplateSelector : DataTemplateSelector
{
    public DataTemplate TextTemplate { get; set; }
    public DataTemplate BoolTemplate { get; set; }
    public DataTemplate DateTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var setting = (SettingItem)item;

        return setting.Type switch
        {
            SettingType.Text => TextTemplate,
            SettingType.Boolean => BoolTemplate,
            SettingType.Date => DateTemplate,
            _ => TextTemplate
        };
    }
}
}
