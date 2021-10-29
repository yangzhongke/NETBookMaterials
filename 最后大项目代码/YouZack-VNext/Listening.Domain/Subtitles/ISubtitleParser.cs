using Listening.Domain.ValueObjects;

namespace Listening.Domain.Subtitles
{
    interface ISubtitleParser
    {
        bool Accept(string typeName);
        IEnumerable<Sentence> Parse(string subtitle);
    }
}
