using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Drawing.CenterViewWPF.Core.Configuration;

/// <summary>
///     Provides methods to manage application configuration, including loading, saving, and accessing
///     the user configuration settings.
/// </summary>
/// <remarks>
///     The <see cref="ConfigurationManager" /> class handles the configuration storage in a JSON file,
///     maintaining application-specific settings. It ensures that the configuration is loaded into memory
///     when accessed and saved back to disk when changes are made.
/// </remarks>
public static class ConfigurationManager
{
    private static readonly string ConfigFolderPath =
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
            "config");

    private const string ConfigFileName = "config.json";
    private static readonly string ConfigFilePath = Path.Combine(ConfigFolderPath, ConfigFileName);
    
    private static UserConfiguration _currentConfig;

    public static UserConfiguration Current
    {
        get
        {
            if (_currentConfig == null) LoadConfiguration();

            return _currentConfig;
        }
    }

    /// <summary>
    ///     Loads the user configuration settings from a JSON file.
    ///     If the configuration file does not exist, it initializes the default configuration
    ///     and saves it to the file system.
    /// </summary>
    /// <remarks>
    ///     This method ensures the configuration is loaded into memory. If the configuration file
    ///     exists in the specified directory, it reads and deserializes the JSON data into
    ///     a <see cref="UserConfiguration" /> object. If the file is not available,
    ///     a new default configuration is created and persisted to disk.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    ///     Thrown when an error occurs while reading from or writing to the file system.
    /// </exception>
    /// <exception cref="Newtonsoft.Json.JsonException">
    ///     Thrown when deserializing the JSON file fails.
    /// </exception>
    /// <example>
    ///     This method is typically invoked automatically when accessing the <see cref="ConfigurationManager.Current" />
    ///     property.
    /// </example>
    public static void LoadConfiguration()
    {
        try
        {
            if (!Directory.Exists(ConfigFolderPath)) Directory.CreateDirectory(ConfigFolderPath);

            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                _currentConfig = JsonConvert.DeserializeObject<UserConfiguration>(json) ?? new UserConfiguration();
            }
            else
            {
                _currentConfig = new UserConfiguration
                {
                    IsDarkMode = true,
                    StayOpen = false
                };
                SaveConfiguration();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _currentConfig = new UserConfiguration();
        }
    }

    /// <summary>
    ///     Saves the current user configuration settings to a JSON file.
    /// </summary>
    /// <remarks>
    ///     This method serializes the in-memory <see cref="UserConfiguration" /> object to a JSON file.
    ///     If the target directory does not exist, it creates the necessary directory structure.
    ///     Any updates made to the configuration in memory will be persisted to the file system after calling this method.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    ///     Thrown when an error occurs while writing the configuration data to the file system.
    /// </exception>
    /// <exception cref="Newtonsoft.Json.JsonException">
    ///     Thrown when serializing the configuration object fails.
    /// </exception>
    public static void SaveConfiguration()
    {
        try
        {
            if (!Directory.Exists(ConfigFilePath)) Directory.CreateDirectory(ConfigFolderPath);

            var json = JsonConvert.SerializeObject(_currentConfig, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}