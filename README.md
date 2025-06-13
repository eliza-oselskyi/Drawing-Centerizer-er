# Tekla Drawing Centerizer-er

A utility for automatically centering views in both GA (General Arrangement) and Fabrication drawings in Tekla Structures.

## Features

- **Intelligent View Centering**: 
  - Centers views based on their visual content rather than geometric center
  - Supports both GA and Assembly (Fabrication) drawings
  - Handles container views and sheet views appropriately

- **Multiple Operation Modes**
  - **GUI Mode**: Interactive interface for real-time view manipulation
    - Fine and coarse adjustment controls
    - Dark/Light theme support
  - **Batch Mode**: Process multiple drawings from Document Manager
    - Center all drawings
    - Center selected drawings only
    - Filter by drawing type (GA/Fabrication)

- **Smart View Processing**:
  - Validates views before processing to ensure proper centering
  - Maintains drawing integrity by processing only valid view configurations
  - Headless operation support for batch processing

## Installation

### Automatic Installation (Recommended)

1. Download the latest release
2. Extract the ZIP file to a temporary location
3. Run the `install_script.ps1` PowerShell script with administrator privileges:
   - Right-click on `install_script.ps1`
   - Select "Run with PowerShell"
   - If prompted for administrator permissions, click "Yes"

The script will:
- Install the application to `C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing_Center_Release`
- Create macro files in both Drawing and Modeling environments
- Create a backup of any existing installation

### Manual Installation

#### Application Files Installation
1. Create the following folder if it doesn't exist:
   ```
   C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing_Center_Release
   ```
2. Copy all files from the `Release` folder to the above location

#### Macro Installation
1. Create the macro file `Center_Drawings.cs` in both locations:
   - `C:\TeklaStructuresFirm\NBGM\2023.4\Macros\drawing\Center_Drawings.cs`
   - `C:\TeklaStructuresFirm\NBGM\2023.4\Macros\modeling\Center_Drawings.cs`

2. Copy the following content into both macro files:
<details>
<summary>Click to expand</summary>

 ```csharp
  using System.Windows.Forms;
  using System.IO;
  using System.Diagnostics;
  using System;

  namespace Tekla.Technology.Akit.UserScript
  {
      public class Script
      {
          public static void Run(Tekla.Technology.Akit.IScript akit)
          {
              const string exeFile = @"C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing.CenterViewWPF.exe";
              var progFolders = System.Environment.GetEnvironmentVariable("TEKLA_APPLICATION_FOLDER");

              foreach (var progFolder in progFolders.Split(';'))
              {
                  var exePath = Path.Combine(progFolder, exeFile);
                  if (File.Exists(exePath))
                  {
                      var process = new Process();
                      process.StartInfo.FileName = exePath;

                      try
                      {
                          process.Start();
                      }
                      catch (System.Exception exc)
                      {
                          MessageBox.Show("Problem running process\n" + exc.Message + "\n" + exc.StackTrace, String.Format("Starting {0} failed!", exeFile), MessageBoxButtons.OK, MessageBoxIcon.Error);
                      }
                      return;
                  }

              }
              MessageBox.Show(String.Format("{0} was not found.\nVerify exe exists in directory.", exeFile), "File not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
  }
 ```
 </details>

## Configuration

The application uses a JSON-based configuration system to persist user preferences and settings.

### Configuration Location

The configuration file is automatically created and managed at:
`C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing_Center_Release\config\config.json`

### Available Settings

The configuration file supports the following options:

- `IsDarkMode`: Controls the application theme
    - `true`: Dark theme (default)
    - `false`: Light theme
- `StayOpen`: Controls window behavior after drawing editor is closed
    - `true`: Keep the application window open
    - `false`: Close the application window (default)

### Configuration Management

- Settings are automatically loaded when the application starts
- Changes made through the UI are saved immediately
- If the configuration file is missing or corrupted, it will be recreated with default values
- The configuration directory is created during installation if it doesn't exist

### Backup and Recovery

- During installation, if a previous configuration exists, it is backed up to:
  ```
  C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing_Center_Release_backup\config\config.json
  ```
- To restore previous settings, copy the backup configuration file back to the main configuration location

### Usage
After installation, the Drawing Centerizer-er can be launched from either:
- Drawing mode: Run the `Center_Drawings` macro
- Modeling mode: Run the `Center_Drawings` macro

### GUI Mode
1. Open a drawing in Tekla Structures
2. Run the `Center_Drawings` macro
3. Use the interface to:
   - Center views automatically
   - Make fine adjustments using directional controls
   - Make coarse adjustments using double-arrow controls
   - Toggle dark/light theme
   - Enable "Stay Open" for multiple operations

#### Batch Mode
1. Open Document Manager
2. Select drawings (optional)
3. Run the `Center_Drawings` macro
4. Choose from available options:
   - Process all drawings
    - Process all GA drawings
    - Process all Fabrication drawings
- The selection state updates automatically as you select or deselect drawings


### Troubleshooting
- If the macro doesn't appear in the Tekla Structures macros list, reload the catalog in Applications and Components
- Ensure all paths exist and you have write permissions to the installation directories
- Check that DrawingCenter.exe exists in the installation folder
- If automatic installation fails, try running PowerShell as administrator or follow the manual installation steps

### Backup and Recovery
- Automatic installation creates a backup at: `C:\TeklaStructuresFirm\NBGM\2023.4\TeklaApplications\Drawing_Center_Release_backup`
- To restore from backup, rename the backup folder by removing the "_backup" suffix

## Technical Notes

- The plugin must be run from within Tekla to ensure proper TeklaAPI connectivity
- Changes are automatically committed with descriptive messages
- In headless mode (no GUI), drawings are automatically saved and closed after processing

## System Requirements

- Tekla Structures 2023.4
- Windows 10 or later
- .NET Framework 4.8