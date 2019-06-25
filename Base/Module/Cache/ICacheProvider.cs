﻿using System.Collections.Generic;
using Zen.Base.Common;

namespace Zen.Base.Module.Cache
{
    public interface ICacheProvider : IZenProvider
    {
        // Standard cache timeout: 10m (600 secs)
        string this[string key, string oSet = null, int cacheTimeOutSeconds = 600] { get; set; }
        Dictionary<string, ICacheConfiguration> EnvironmentConfiguration { get; set; }
        string ServerName { get; }

        EOperationalStatus OperationalStatus { get; }

        IEnumerable<string> GetAll(string oNamespace);
        bool Contains(string key);
        void Remove(string key, string oSet = null);
        void RemoveAll(string oSet = null);
        void SetSingleton(object value, string fullName = null);
        T GetSingleton<T>(string fullName = null);
    }
}