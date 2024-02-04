using Microsoft.Extensions.Options;
using Microsoft.Office.Core;
using Rewriter.Attributes;
using Rewriter.Configuration;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace Rewriter.Converters;

[Extension(".ppa", ".ppt", ".pptm" , ".pptx")]
public class PowerPointConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor, ILogger<PowerPointConverter> logger)
    : AbstractConverter(optionsMonitor)
{
    protected override void ConvertFile(string fullPath)
    {
        var app = new PowerPoint.Application
        {
            Visible = MsoTriState.msoTrue
        };
        
        //logger
        
        var document = app.Presentations.Open(fullPath);
        document.SaveAs(FileName: ConvertPath(fullPath, Options.FolderPath), FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
    }
}