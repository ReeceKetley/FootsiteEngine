using System;
using System.Collections.Generic;
using FootsiteEngine.Items;

namespace FootsiteEngine.Configuration
{
    public class PageConfiguration
    {
        public Uri PageUrl { get; }
        public List<ExtractionItem> Items { get; }

        public PageConfiguration(Uri pageUrl, List<ExtractionItem> items)
        {
            PageUrl = pageUrl;
            Items = items;
        }
    }
}