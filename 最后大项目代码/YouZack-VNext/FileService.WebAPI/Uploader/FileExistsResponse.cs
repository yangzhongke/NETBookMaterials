
namespace FileService.WebAPI.Uploader;

//IsExists是否存在这样的文件，如果存在，则Url代表这个文件的路径
public record FileExistsResponse(bool IsExists, Uri? Url);