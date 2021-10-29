
using FluentValidation;
using MediaEncoder.Domain;

namespace MediaEncoder.WebAPI.Controllers;
public record StartRequest(Guid MediaId, string SourceFileName, Uri SourceUrl, string OutputFormat, string SourceSystem);

public class StartRequestValidator : AbstractValidator<StartRequest>
{
    public StartRequestValidator(MediaEncoderFactory encoderFactory)
    {
        string[] acceptedExtensions = new string[] { "m4a", "mp3", "mp4", "avi", "wav", "wma", "mpeg", "flv", "ogg" };
        RuleFor(e => e.SourceFileName).NotNull().NotEmpty()
            .Must(fn => acceptedExtensions.Contains(Path.GetExtension(fn).Trim('.'))).WithMessage("只接受如下格式的源文件" + string.Join(",", acceptedExtensions));
        RuleFor(e => e.SourceUrl).NotNull();
        RuleFor(e => e.OutputFormat).NotNull().NotEmpty()
            .Must(format => encoderFactory.Create(format) != null).WithMessage("目标格式没有对应的Encoder");
    }
}