using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using FootsiteEngine.Interfaces;
using FootsiteEngine.Modules;
using FootsiteEngine.Types;
using FootsiteEngine.Various;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FootsiteEngine.Footlocker
{
    class ProductPageHandler
    {
        private readonly IHttpServiceModule _httpService;
        private readonly Uri _productPageUri;

        public ProductPageHandler(IHttpServiceModule httpService, Uri productPageUri)
        {
            _httpService = httpService;
            _productPageUri = productPageUri;
        }

        public FootlockerScrapeResult Scrape()
        {
            var response = _httpService.Get(_productPageUri, new CancellationToken());
            var objects = response.Result.JsonObjects();

            if (objects.Count == 0)
            {
                return new FootlockerScrapeResult(FootLockerResult.Failure, null);
            }

            foreach (var o in objects.Where(o => o.ContainsKey("sku")))
            {
                var sizes = ExtractProductSizes(o);
                var product = new FootlockerProduct(o["name"].Value<string>(), o["brand"].Value<string>(), o["description"].Value<string>(), o["model"].Value<string>(), o["sku"].Value<string>(), new Uri(o["image"].Value<string>()), new Uri("https://footlocker.co.uk" + o["url"].Value<string>()), sizes);
                return new FootlockerScrapeResult(FootLockerResult.Success, product);
            }



            return new FootlockerScrapeResult(FootLockerResult.Failure, null);
        }

        public List<FootLockerSize> ExtractProductSizes(JObject input)
        {
            var sizes = new List<FootLockerSize>();
            var offers = input["offers"];
            if (offers != null)
                foreach (var offer in offers)
                {
                    bool inStock = false;
                    inStock = (offer["availability"] ?? false).Value<string>() != "OutOfStock";
                    var size = (offer["sku"] ?? "0").Value<string>();
                    size = size.Split('-')[1];
                    sizes.Add(new FootLockerSize(offer["sku"].Value<String>(), offer["price"].Value<double>(), inStock, size));
                }

            return sizes;
        }
    }



    internal class FootlockerScrapeResult
    {
        public FootLockerResult Result { get; }
        public object Data { get; }

        public FootlockerScrapeResult(FootLockerResult result, object data)
        {
            Result = result;
            Data = data;
        }
    }

    public enum FootLockerResult
    {
        Success,
        Failure
    }
}

