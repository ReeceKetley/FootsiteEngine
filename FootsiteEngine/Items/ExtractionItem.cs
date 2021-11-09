using FootsiteEngine.Types;

namespace FootsiteEngine.Items
{
    public class ExtractionItem
    {
        public string Id { get; }
        public ExtractionDataType Type { get; }
        public HtmlElementType HtmlElement { get; }
        public string Start { get; }
        public string End { get; }
        public bool IncludeStartEnd { get; }
        public string SearchToken { get; }

        public ExtractionItem(string id, ExtractionDataType type, HtmlElementType htmlElement, string searchToken)
        {
            this.Id = id;
            Type = type;
            HtmlElement = htmlElement;
            SearchToken = searchToken;
        }

        public ExtractionItem(string id, ExtractionDataType type, HtmlElementType htmlElement, string start, string end, bool includeStartEnd)
        {
            this.Id = id;
            Type = type;
            HtmlElement = htmlElement;
            Start = start;
            End = end;
            IncludeStartEnd = includeStartEnd;
        }

    }
}