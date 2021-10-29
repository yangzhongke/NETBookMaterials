using System.IO;
using System.Threading.Tasks;

namespace Zack.Commons
{
    public static class IOHelper
    {
        public static async Task<byte[]> ToArrayAsync(this Stream stream)
        {
            using MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;
            return ms.ToArray();
        }

        public static byte[] ToArray(this Stream stream)
        {
            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            return ms.ToArray();
        }

        public static void CreateDir(FileInfo file)
        {
            file.Directory.Create();
        }
    }
}
