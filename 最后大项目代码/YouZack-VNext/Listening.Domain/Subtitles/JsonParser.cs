using Listening.Domain.ValueObjects;
using System.Text.Json;

namespace Listening.Domain.Subtitles
{
    class JsonParser : ISubtitleParser
    {
        public bool Accept(string typeName)
        {
            return typeName.Equals("json", StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            return JsonSerializer.Deserialize<IEnumerable<Sentence>>(subtitle);
        }
    }
}
