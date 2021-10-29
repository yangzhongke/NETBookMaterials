
using FFmpeg.NET;
using MediaEncoder.Domain;

namespace MediaEncoder.WebAPI.Encoders;
public class ToM4AEncoder : IMediaEncoder
{
    public bool Accept(string outputFormat)
    {
        return "m4a".Equals(outputFormat, StringComparison.OrdinalIgnoreCase);
    }

    public async Task EncodeAsync(string sourceFileName, string destFileName, string destFormat, string[]? args = null, CancellationToken cancellationToken = default)
    {
        //可以用“FFmpeg.AutoGen”，因为他是bingding库，不用启动独立的进程，更靠谱。但是编程难度大，这里重点不是FFMPEG，所以先用命令行实现
        var inputFile = new InputFile(sourceFileName);
        var outputFile = new OutputFile(destFileName);
        var ffmpeg = new Engine("D:/greeninstall/ffmpeg/bin/ffmpeg.exe");
        string errorMsg = null;
        ffmpeg.Error += (s, e) =>
        {
            errorMsg = e.Exception.Message;
        };
        await ffmpeg.ConvertAsync(inputFile, outputFile, cancellationToken);
        if (errorMsg != null)
        {
            throw new Exception(errorMsg);
        }
    }
}
