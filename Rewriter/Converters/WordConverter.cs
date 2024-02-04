using Microsoft.Extensions.Options;
using Rewriter.Attributes;
using Rewriter.Configuration;
using Word = Microsoft.Office.Interop.Word;

namespace Rewriter.Converters;

[Extension(".doc", ".docm", ".docx" , ".htm", ".html")]
public class WordConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor, ILogger<WordConverter> logger)
    : AbstractConverter(optionsMonitor)
{
    //[LoggerMessage(0, LogLevel.Information, "Writing hello world response to {dat}")]
    //partial void LogHelloWorld(ILogger logger, string dat);

    protected override void ConvertFile(string fullPath)
    {
        var app = new Word.Application
        {
            Visible = true,
        };
        
        //logger

        var document = app.Documents.Open(fullPath);
        document.SaveAs(FileName: ConvertPath(fullPath, Options.FolderPath), FileFormat: Word.WdSaveFormat.wdFormatPDF);
    }
}
