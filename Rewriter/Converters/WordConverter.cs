using Microsoft.Extensions.Options;
using Microsoft.Office.Core;
using Rewriter.Attributes;
using Rewriter.Configuration;
using Rewriter.Extensions;
using Word = Microsoft.Office.Interop.Word;

namespace Rewriter.Converters;

[Extension(".doc", ".docm", ".docx" , ".htm", ".html")]
public class WordConverter
    (IOptionsMonitor<FileOutputOptions> optionsMonitor, ILogger<WordConverter> logger)
    : AbstractConverter
{

    private static Word.Application _application = new()
    {
        Visible = false,
        FileValidation = MsoFileValidationMode.msoFileValidationSkip
    };
    
    protected override void ConvertFile(string fullPath)
    {
        var document = _application.Documents.Open(FileName: fullPath, Visible: false);
        logger.LogFileConverting(fullPath);
        
        document.SaveAs(FileName: ConvertPath(fullPath, optionsMonitor.CurrentValue.FolderPath), FileFormat: Word.WdSaveFormat.wdFormatPDF);
        
        document.Close();
        File.Delete(fullPath);
        
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
