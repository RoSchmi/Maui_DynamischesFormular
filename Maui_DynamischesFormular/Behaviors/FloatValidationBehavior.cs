using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Maui_DynamischesFormular.Behaviors
{
    public class FloatValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        { 
            entry.TextChanged += OnTextChanged; 
            base.OnAttachedTo(entry); 
        }
        protected override void OnDetachingFrom(Entry entry) 
        { 
            entry.TextChanged -= OnTextChanged; 
            base.OnDetachingFrom(entry); }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var culture = CultureInfo.CurrentCulture; // z. B. de-DE
            var style = NumberStyles.Float; if (string.IsNullOrWhiteSpace(e.NewTextValue)) 
                return; 
             if (!float.TryParse(e.NewTextValue, style, culture, out _)) 
            { 
                ((Entry)sender).Text = e.OldTextValue; 
            } 
        }
    }
}
