/*
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiBeispielDynamischesFormular.Interfaces
{
    internal class ISettingsService
    {
    }
}
*/

using System;


namespace Common.Interfaces;

public interface ISettingsService
{
    public Task<T> Get<T>(string key, T defaultValue);
    public Task Save<T>(string key, T value);
}

