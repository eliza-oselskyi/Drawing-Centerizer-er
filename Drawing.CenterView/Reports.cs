using System;
using System.IO;
using System.Threading;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace Drawing.CenterView;

public abstract partial class QuickCenterClass
{
    private static int _read;
    private static readonly Model Model = new Model();

    private static void GenerateAndDisplayReport(string reportTemplate, string reportString)
    {
        Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Generating Report...");
        
        var formattedReportString = String.Join(
            Environment.NewLine,
            "pagefooter _tmp_1",
            "{",
            "name = \"PageFooter\";",
            "height = 31;",
            "outputpolicy = NONE;",
            " ",
            "text _tmp_4",
            "{",
            "name = \"text_1\";",
            "x1 = 37;",
            "y1 = 28;",
            "x2 = 37;",
            "y2 = 28;",
            $"string = \"{reportString}\";",
            "fontname = \"Arial\";",
            "fontcolor = 161;",
            "fonttype = 2;",
            "fontsize = 2.38;",
            "fontratio = 1;",
            "fontslant = 0;",
            "fontstyle = 0;",
            "angle = 0;",
            "justify = LEFT;",
            "pen = 0;",
            "isalwaysvisible = TRUE;",
            "visibilityrule = \"\";",
            "};",
            "};",
            "};"
        );

        var reportsFolder = Model.GetInfo().ModelPath + @"\Reports\";
        var firmFolder = "";
        TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref firmFolder);
        var reportsTemplateFolder = Path.Combine(firmFolder, @"Reports\");
        var tempTemplate = File.Open($@"{firmFolder}\Macros\modeling\Center_Drawings\Release\report_template\{reportTemplate}_temp.rpt",
            FileMode.Create);
        var template =
            File.Open($@"{firmFolder}\Macros\modeling\Center_Drawings\Release\report_template\{reportTemplate}_test.rpt",
                FileMode.Open);

        //tempTemplate.Close();
        template.CopyTo(tempTemplate);
        tempTemplate.Close();
        using (var writer = File.AppendText(tempTemplate.Name))
        {
            writer.WriteLine(formattedReportString);
        }

        File.Replace(tempTemplate.Name, reportsTemplateFolder + reportTemplate + ".rpt",
            Directory.GetCurrentDirectory().ToString() + "Report_backup.xsr");

        Directory.CreateDirectory(reportsFolder);
        if (File.Exists(reportsFolder + reportTemplate + ".xsr")) File.Delete(reportsFolder + reportTemplate + ".xsr");

        var epochTimeOffset = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalSeconds;
        var dateTime = (int)DateTimeOffset.Now.ToUnixTimeSeconds() + epochTimeOffset;

        Tekla.Structures.Model.Operations.Operation.CreateReportFromAll(reportTemplate,
            reportTemplate + ".xsr",
            dateTime.ToString(),
            "",
            "");

        Thread.Sleep(0);
        if (!File.Exists(reportsFolder + reportTemplate + ".xsr")) return;
        if (IfLockedWait(reportsFolder + reportTemplate + ".xsr"))
            Tekla.Structures.Model.Operations.Operation.DisplayReport(reportsFolder + reportTemplate + ".xsr");
    }

    private static bool IfLockedWait(string fileName)
    {
        // try 10 times
        var retryNumber = 10;
        while (true)
            try
            {
                using var fileStream = new FileStream(
                    fileName, FileMode.Open,
                    FileAccess.ReadWrite, FileShare.ReadWrite);
                var readText = new byte[fileStream.Length];
                fileStream.Seek(0, SeekOrigin.Begin);
                _read = fileStream.Read(readText, 0, (int)fileStream.Length);

                return true;
            }
            catch (IOException)
            {
                // wait one second
                Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Retrying Report Generation...");
                Thread.Sleep(1000);
                retryNumber--;
                if (retryNumber == 0)
                    return false;
            }
    }
}