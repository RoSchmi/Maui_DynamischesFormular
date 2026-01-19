using System;
using System.Collections.Generic;
using System.Text;

namespace RoSchmi.Maui.Interfaces
{
    public interface INavigationService
    {
        Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
        Task GoBackAsync();
    }
}
