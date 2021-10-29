using System;

namespace FileService.SDK.NETCore
{
    public record FileExistsResponse(bool IsExists, Uri? Url);
}
