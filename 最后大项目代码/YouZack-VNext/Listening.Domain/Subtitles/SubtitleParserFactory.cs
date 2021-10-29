namespace Listening.Domain.Subtitles
{
    static class SubtitleParserFactory
    {
        private static List<ISubtitleParser> parsers = new();

        static SubtitleParserFactory()
        {
            var parserTypes = typeof(SubtitleParserFactory).Assembly.GetTypes().Where(t => typeof(ISubtitleParser).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var parserType in parserTypes)
            {
                ISubtitleParser parser = (ISubtitleParser)Activator.CreateInstance(parserType);
                parsers.Add(parser);
            }
        }

        public static ISubtitleParser? GetParser(string typeName)
        {
            foreach (var parser in parsers)
            {
                if (parser.Accept(typeName))
                {
                    return parser;
                }
            }
            return null;
        }
    }
}
