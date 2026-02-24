using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Maui_DynamischesFormular.Models;

namespace DataTemplates
{
    public class TypeDataTemplateSelector : Microsoft.Maui.Controls.DataTemplateSelector
    {
        public DataTemplate StringTypeTemplate { get; set; }

        public DataTemplate StringTypeReadOnlyTemplate { get; set; }

        public DataTemplate StringTypeDontShowTemplate { get; set; }

        public DataTemplate StringTypeSwipeTemplate { get; set; }

        public DataTemplate StringTypePickerTemplate { get; set; }

        public DataTemplate StringTypeFloatTemplate { get; set; }

        public DataTemplate BoolTypeTemplate { get; set; }

        public DataTemplate BoolTypeReadOnlyTemplate { get; set; }

        public DataTemplate BoolTypeDontShowTemplate { get; set; }

        public DataTemplate DateTypeTemplate { get; set; }

        public DataTemplate DateTypeReadOnlyTemplate { get; set; }

        public DataTemplate DateTypeDontShowTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var returnTemplate = StringTypeTemplate;

            switch (((WorkItem)item).TypeIdentifier)
            {
                case WorkItem.TypeID.RsBoolean:
                    {
                        returnTemplate = BoolTypeTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsBooleanRo:
                    {
                        returnTemplate = BoolTypeReadOnlyTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsBooleanNo:
                    {
                        returnTemplate = BoolTypeDontShowTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsDateTime:
                    {
                        returnTemplate = DateTypeTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsDateTimeRo:
                    {
                        returnTemplate = DateTypeReadOnlyTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsDateTimeNo:
                    {
                        returnTemplate = DateTypeDontShowTemplate;
                        break;
                    }

                case WorkItem.TypeID.RsString:
                    {
                        returnTemplate = StringTypeTemplate;
                        break;
                    }

                case WorkItem.TypeID.RsStringRo:

                    {
                        returnTemplate = StringTypeReadOnlyTemplate;
                        break;
                    }

                case WorkItem.TypeID.RsStringNo:
                    {
                        returnTemplate = StringTypeDontShowTemplate;
                        break;
                    }

                case WorkItem.TypeID.RsStringSw:
                    {
                        returnTemplate = StringTypeSwipeTemplate;
                        break;
                    }
                case WorkItem.TypeID.RsStringPi:
                    {
                        returnTemplate = StringTypePickerTemplate;
                        break;
                    }

                case WorkItem.TypeID.RsStringFlo:
                    {
                        returnTemplate = StringTypeFloatTemplate;
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException("Unexpected Type: " + item);
                    }

                    //  break;
            }

            return returnTemplate;


        }
    }
}

