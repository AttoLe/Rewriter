using System.Diagnostics.Tracing;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Word;
using Rewriter.Attributes;
using Rewriter.Configuration;
using Word = Microsoft.Office.Interop.Word;

namespace Rewriter.Workers;

[Extension(".doc", ".docm", ".docx" , ".htm", ".html")]
public class WordConverter(ILogger<WordConverter> logger) : PdfConverter
{
    //[LoggerMessage(0, LogLevel.Information, "Writing hello world response to {dat}")]
    //partial void LogHelloWorld(ILogger logger, string dat);
    private FileOutputOptions _options; 
    
    public WordConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor, ILogger<WordConverter> logger) : this(logger)
    {
        _options = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(option => _options = option);
    }

    public override void ConvertFile(string fullPath)
    {
        var app = new Word.Application
        {
            Visible = true,
        };

        var document = app.Documents.Open(fullPath);
        document.SaveAs(FileName: ConvertPath(fullPath, _options.FolderPath), FileFormat: Word.WdSaveFormat.wdFormatPDF);
    }
}
