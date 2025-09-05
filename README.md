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

## Configuration

The application uses a JSON-based configuration system to persist user preferences and settings.

### Configuration Location

The configuration file is automatically created and managed at:
`C:\TeklaStructuresFirm\NBGM\<VERSION>\TeklaApplications\Drawing_Center_Release\config\config.json`

### Available Settings

The configuration file supports the following options:

- `IsDarkMode`: Controls the application theme
    - `true`: Dark theme (default)
    - `false`: Light theme
- `StayOpen`: Controls window behavior after drawing editor is closed
    - `true`: Keep the application window open
    - `false`: Close the application window (default)
- `CoarseAdjustment`: Controls shift amount for double arrow controls
    - `<integer>`: Adjustment amount. Default: 20
- `FineAdjustement`: Controls shift amount for single arrow controls
    - `<integer>`: Adjustment amount. Default: 10

### Configuration Management

- Settings are automatically loaded when the application starts
- Changes made through the UI are saved immediately
- If the configuration file is missing or corrupted, it will be recreated with default values
- The configuration directory is created during installation if it doesn't exist

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
   - Adjust fine/coarse adjustment values

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

## Technical Notes

- The plugin must be run from within Tekla to ensure proper TeklaAPI connectivity
- Changes are automatically committed with descriptive messages
- In headless mode (no GUI), drawings are automatically saved and closed after processing

## System Requirements

- Tekla Structures 2023.4
- Windows 10 or later

- .NET Framework 4.8
