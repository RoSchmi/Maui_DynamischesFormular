using System;


namespace Common.Interfaces;

public interface ISettingsService
{
    public Task<T> Get<T>(string key, T defaultValue);
    public Task Save<T>(string key, T value);
}

