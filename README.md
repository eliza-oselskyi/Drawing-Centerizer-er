# nbg-tekla-drawing-centerizerer Centerizes and makes gooderized NBG Tekla
drawings

Currently this program works only for GA Drawings, but there are future plans
to include fabrication drawings as well!

## Installation:

- Run the "install_script" with powershell to install it into the TeklaStructuresFirm directory. That's it!
    - **NOTE**: If the install script does not work, try right-clicking > "Properties" > check the box "Unblock".

- If you wish to do it manually, just copy "Release\"
    directory into
    `C:\TeklaStructuresFirm\NBGM\2023.4\Macros\modeling\Center_Drawings` and
    copy the macro `Center_Drawings.cs` into
    `C:\TeklaStructuresFirm\NBGM\2023.4\Macros\drawings`

    This will allow you to use the plugin in both drawing and modeling modes.


## Usage:

Open Tekla, refresh the catalog, and run the "Center_Drawings" macro.
The .EXE must be run from within Tekla (thus, through the macro), in order for
the TeklaAPI to connect to the open model.

- To center all drawings, just run the macro
- To center specific drawings, select them in the document manager and run the macro
- To center in a drawing, run the same macro in the drawing, and a GUI should pop up.


## Features:

- If no drawings selected => centers every non-excluded drawing

- Some selected drawings => centers only the selected drawings

- GUI interface when in drawing mode => finds a valid view to center.

- Instead of placing the center point of the view on the true center, it
    instead places it in the visual center and (hopefully) not as likely to
    overplot with other elements.

- Excluding drawings from centering possible. Just open the Center_Drawings
    macro in a drawing view and check the "Exclude from..." box. Or, just change
    the Title_3 to be `X`.

- In model view, it skips over drawings that have more than one valid "main"
    view to center, so it doesn't mess with the layouts that were most likely
    manually done. I might add some logic in later that will give you the
    option to allow centering in those drawings too. The idea is that it will
    center the views and then stack them on top of each other or side by side.

- The plugin is technically only one executable file; It detects if there
    are any currently open drawings. If not, it operates in "headless" mode,
    meaning that it just operates like a standard macro, without any interface.
    If there is a drawing open, it will launch the GUI, to allow more granular
    control over the current drawing you are in.
