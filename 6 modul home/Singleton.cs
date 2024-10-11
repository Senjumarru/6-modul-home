using System;
using System.Collections.Generic;
using System.IO;

namespace DesignPatterns.Singleton
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, string> _settings;

        private ConfigurationManager()
        {
            _settings = new Dictionary<string, string>();
        }

        public static ConfigurationManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationManager();
                    }
                }
            }
            return _instance;
        }

        public void LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Настройки не найдены.");

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    _settings[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        public string GetSetting(string key)
        {
            if (_settings.ContainsKey(key))
                return _settings[key];

            throw new KeyNotFoundException($"Настройка '{key}' не найдена.");
        }

        public void SetSetting(string key, string value)
        {
            _settings[key] = value;
        }
    }
}
