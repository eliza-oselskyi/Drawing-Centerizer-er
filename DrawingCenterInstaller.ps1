# Define base path
$teklaBasePath = "C:\TeklaStructuresFirm\NBGM\2023.4"

# Define paths
$SourcePath = ".\Release"
$DestinationFolder = Join-Path $teklaBasePath "TeklaApplications"
$targetPath = Join-Path $DestinationFolder "Drawing_Center_Release"
$backupPath = "${targetPath}_backup"

# Define Tekla Macros locations
$drawingMacroPath = Join-Path $teklaBasePath "Macros\drawing"
$modelingMacroPath = Join-Path $teklaBasePath "Macros\modeling"

$macroContent = @"

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
            const string exeFile = @"$targetPath\Drawing.CenterViewWPF.exe";
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
"@

try {
    # Ensure destination parent folder exists
    if (-not (Test-Path $DestinationFolder)) {
        New-Item -ItemType Directory -Path $DestinationFolder -Force
    }

    # Handle backup: remove existing backup if it exists
    if (Test-Path $backupPath) {
        Write-Host "Removing existing backup at: $backupPath"
        Remove-Item -Path $backupPath -Recurse -Force
    }

    # If Drawing_Center_Release exists, create a backup and remove original
    if (Test-Path $targetPath) {
        Write-Host "Creating backup of existing release: $backupPath"
        Copy-Item -Path $targetPath -Destination $backupPath -Recurse -Force
        Remove-Item -Path $targetPath -Recurse -Force
    }

    # Create new target directory and copy files
    Write-Host "Creating new installation directory and copying files"
    New-Item -Path $targetPath -ItemType Directory -Force
    Copy-Item -Path "$SourcePath\*" -Destination $targetPath -Recurse -Force

    # Create macro files in Tekla locations
    $macroFileName = "Center_Drawings.cs"
    
    # Create drawing macros folder if it doesn't exist
    if (-not (Test-Path $drawingMacroPath)) {
        New-Item -ItemType Directory -Path $drawingMacroPath -Force
        Write-Host "Created Tekla Drawing Macros directory: $drawingMacroPath"
    }

    # Create macro in drawing macros folder
    $drawingMacroFile = Join-Path $drawingMacroPath $macroFileName
    Write-Host "Creating macro at: $drawingMacroFile"
    $macroContent | Out-File -FilePath $drawingMacroFile -Encoding utf8 -Force

    # Create modeling macros folder if it doesn't exist
    if (-not (Test-Path $modelingMacroPath)) {
        New-Item -ItemType Directory -Path $modelingMacroPath -Force
        Write-Host "Created Tekla Modeling Macros directory: $modelingMacroPath"
    }

    # Create macro in modeling macros folder
    $modelingMacroFile = Join-Path $modelingMacroPath $macroFileName
    Write-Host "Creating macro at: $modelingMacroFile"
    $macroContent | Out-File -FilePath $modelingMacroFile -Encoding utf8 -Force

    Write-Host "Installation completed successfully!"
    Write-Host "Files installed to: $targetPath"
    Write-Host "Macro installed to drawing macros: $drawingMacroFile"
    Write-Host "Macro installed to modeling macros: $modelingMacroFile"
}
catch {
    Write-Error "An error occurred during installation: $_"
    exit 1
}