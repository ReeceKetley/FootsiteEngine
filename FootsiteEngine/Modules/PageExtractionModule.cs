using System.Text;
using FootsiteEngine.Configuration;

namespace FootsiteEngine.Modules
{
    public class PageExtractionModule
    {
        private readonly PageConfiguration _configuration;
        private readonly HttpServiceModule _http;

        public PageExtractionModule(PageConfiguration configuration, HttpServiceModule http)
        {
            _configuration = configuration;
            _http = http;
        }


    }
}
