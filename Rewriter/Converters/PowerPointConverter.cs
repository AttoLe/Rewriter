using Microsoft.Extensions.Options;
using Microsoft.Office.Core;
using Rewriter.Attributes;
using Rewriter.Configuration;
using Rewriter.Extensions;
using Rewriter.FileDeleter;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace Rewriter.Converters;

[Extension(".ppa", ".ppt", ".pptm" , ".pptx")]
public class PowerPointConverter
    (IOptionsMonitor<FileOutputOptions> optionsMonitor, IFileDeleter fileDeleter, ILogger<PowerPointConverter> logger)
    : AbstractConverter(optionsMonitor, fileDeleter)
{
    private PowerPoint.Application _application = new()
    {
        FileValidation = MsoFileValidationMode.msoFileValidationSkip,
    };

    protected override void ConvertFile(string fullPath)
    {
        var document = _application.Presentations.Open(fullPath, 0, 0, 0);
        logger.LogFileConverting(fullPath);
       
        document.SaveAs(FileName: ConvertToNewPath(fullPath), 
            FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
        
        document.Close();
        logger.LogFileConverted(fullPath);
    }

    public override void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public override void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
        _application.Quit();
    }
}