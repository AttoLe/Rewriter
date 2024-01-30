using Microsoft.Extensions.Options;
using Microsoft.Office.Core;
using Rewriter.Attributes;
using Rewriter.Configuration;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace Rewriter.Workers;

[Extension(".ppa", ".ppt", ".pptm" , ".pptx")]
public class PowerPointConverter(ILogger<WordConverter> logger) : PdfConverter
{
    private FileOutputOptions _options; 
    
    public PowerPointConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor, ILogger<WordConverter> logger) : this(logger)
    {
        _options = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(option => _options = option);
    }

    public override void ConvertFile(string fullPath)
    {
        var app = new PowerPoint.Application
        {
            Visible = MsoTriState.msoTrue
        };

        var document = app.Presentations.Open(fullPath);
        document.SaveAs(FileName: ConvertPath(fullPath, _options.FolderPath), FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
    } 
}