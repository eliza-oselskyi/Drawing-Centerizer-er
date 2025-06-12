using System;
using System.IO;
using System.Text.Json;

namespace Drawing.CenterViewWPF.Core.Configuration;

public static class ConfigurationManager
{
    private static readonly string TeklaFirmPath = @"C:\TeklaStructuresFirm\NBGM\2023.4";
    private static readonly string ConfigFolderPath = Path.Combine(TeklaFirmPath,
        @"TeklaApplications\Drawing_Center_Release\config");
    
    private static readonly string ConfigFilePath = Path.Combine(ConfigFolderPath, "config.json");
    private static UserConfiguration _currentConfig;

    public static UserConfiguration Current
    {
        get
        {
            if (_currentConfig == null)
            {
                LoadConfiguration();
            }

            return _currentConfig;
        }
    }

    public static void LoadConfiguration()
    {
        try
        {
            if (!Directory.Exists(ConfigFolderPath))
            {
                Directory.CreateDirectory(ConfigFolderPath);
                
            }

            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                _currentConfig = JsonSerializer.Deserialize<UserConfiguration>(json) ?? new UserConfiguration();
            }
            else
            {
                _currentConfig = new UserConfiguration
                {
                    IsDarkMode = true,
                    StayOpen = false,
                };
                SaveConfiguration();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
            _currentConfig = new UserConfiguration();
        }
    }

    public static void SaveConfiguration()
    {
        try
        {
            if (!Directory.Exists(ConfigFilePath))
            {
                File.Create(ConfigFilePath);
            }

            var json = JsonSerializer.Serialize<UserConfiguration>(_currentConfig, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(ConfigFilePath, json);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}