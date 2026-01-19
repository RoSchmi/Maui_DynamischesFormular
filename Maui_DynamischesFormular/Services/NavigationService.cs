using System;
using System.Collections.Generic;
using System.Text;
using RoSchmi.Maui.Interfaces;

namespace RoSchmi.Maui.Services
{
    public class NavigationService : INavigationService
    {
        public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        {
            if (parameters is null) return Shell.Current.GoToAsync(route);

            var query = new ShellNavigationQueryParameters();
            foreach (var kvp in parameters)
            {
                query.Add(kvp.Key, kvp.Value);
            }

            return Shell.Current.GoToAsync(route, query);
        }

        public Task GoBackAsync()
            => Shell.Current.GoToAsync("..");
    }
}
