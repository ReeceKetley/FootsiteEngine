using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FootsiteEngine.Interfaces;
using FootsiteEngine.Modules;
using FootsiteEngine.Types;
using FootsiteEngine.Various;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FootsiteEngine.Footlocker
{
    public class FootLockerCore
    {
        private readonly string _sizeString;
        private readonly ProductPageHandler _productPageHandler;
        private AddToCartHandler _addToCartHandler;
        private readonly IHttpServiceModule _httpService;
        private readonly Uri _productPageUrl = new Uri("https://www.footlocker.co.uk/en/product/jordan-1-mid-menshoes/314102668804.html");
        private FootLockerSize _selectedSize;

        public FootLockerCore(string productUrl, string sizeString)
        {
            _sizeString = sizeString;
            if (!Uri.TryCreate(productUrl, UriKind.RelativeOrAbsolute,out var productUri))
            {
                throw new Exception("Url is invalid.");
            }

            _productPageUrl = productUri;
            _httpService = new HttpServiceModule();
            _productPageHandler = new ProductPageHandler(_httpService, _productPageUrl);
            Init();
        }

        public void Init()
        {
            CE.WriteLine("Initializing Footlocker.");
            var response = _httpService.Get(new Uri("https://www.footlocker.co.uk/"), new CancellationToken(false));
            if (response.Result.Text().Contains("\"url\": \"https://www.footlocker") || response.Result.Text().Contains("\"url\": \"https://footlocker"))
            {
                CE.WriteLine("Site initialized successfully.");
            }
            else
            {
                throw new Exception("Failled to init Footlocker check connection.");
            }

            Start(new CancellationToken(false));
        }

        public void Start(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                CE.WriteLine("(Init) The task was canceled before it started.");
                return;
            }

            var product = GetProduct(cancellationToken);
            if (product == null)
            {
                throw new Exception("Product returned null.");
            }

            AddToCart(cancellationToken);
            _addToCartHandler = new AddToCartHandler(_httpService, product, _selectedSize);
        }

        public FootlockerProduct GetProduct(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                CE.WriteLine("(GetProduct) Task was canceled before it started.");
                return null;
            }
            var result = _productPageHandler.Scrape();
            var product = (FootlockerProduct)result.Data;
            CE.WriteLine("Product Scrapped: " + product.Name + " - SKU:" + product.Sku);
            foreach (var footLockerSize in product.Sizes.Where(footLockerSize => footLockerSize.Size == _sizeString.Trim()))
            {
                _selectedSize = footLockerSize;
                CE.WriteLine("Selected size found.");
                return product;
            }

            CE.WriteLine("The selected size wasn't found! In/Out of stock. Check size.");
            return null;
        }

        public bool AddToCart(CancellationToken cancellationToken)
        {

            return false;
        }
    }
}

