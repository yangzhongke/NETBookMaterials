using Listening.Domain.ValueObjects;
using Opportunity.LrcParser;

namespace Listening.Domain.Subtitles
{
    /// <summary>
    ///  parser for *.lrc files
    /// </summary>
    class LrcParser : ISubtitleParser
    {
        public bool Accept(string typeName)
        {
            return typeName.Equals("lrc", StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<Sentence> Parse(string subtitle)
        {
            var lyrics = Lyrics.Parse(subtitle);
            if (lyrics.Exceptions.Count > 0)
            {
                throw new ApplicationException("lrc解析失败");
            }
            lyrics.Lyrics.PreApplyOffset();//应用上[offset:500]这样的偏移
            return FromLrc(lyrics.Lyrics);
        }

        private static Sentence[] FromLrc(Lyrics<Line> lyrics)
        {
            var lines = lyrics.Lines;
            Sentence[] sentences = new Sentence[lines.Count];
            for (int i = 0; i < lines.Count - 1; i++)
            {
                var line = lines[i];
                var nextLine = lines[i + 1];
                Sentence sentence = new Sentence(line.Timestamp.TimeOfDay, nextLine.Timestamp.TimeOfDay, line.Content);
                sentences[i] = sentence;
            }
            //last line
            var lastLine = lines.Last();
            TimeSpan lastLineStartTime = lastLine.Timestamp.TimeOfDay;
            //lrc没有结束时间，就极端假定最后一句耗时1分钟
            TimeSpan lastLineEndTime = lastLineStartTime.Add(TimeSpan.FromMinutes(1));
            var lastSentence = new Sentence(lastLineStartTime, lastLineEndTime, lastLine.Content);
            sentences[sentences.Count() - 1] = lastSentence;

            return sentences;
        }
    }
}
