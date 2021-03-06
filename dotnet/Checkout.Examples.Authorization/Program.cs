﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Checkout.Examples.Authorization.Models;
using Newtonsoft.Json;

namespace Checkout.Examples.Authorization
{
    class Program
    {
        // You get these from Merchant Services when onboarding
        // https://payments.collectorbank.se

        private const string USERNAME = "your-username";
        private const string API_KEY = "your-api-key";
        private const string BASE_URL = "https://checkout-api-uat.collector.se";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Creates a SharedKey used to communicate with Collector Checkout");

            // You get these from Merchant Services when onboarding
            // https://payments.collectorbank.se

            var countryCode = "SE";
            var storeId = 0000;

            var initCheckoutRequest = CreateInitCheckoutRequest(countryCode, storeId);
            
            var httpRequestMessage = CreateHttpRequestMessage("/checkout", initCheckoutRequest);
            var requestBody = await httpRequestMessage.Content.ReadAsStringAsync();

            Console.WriteLine("-- authorization header:");
            Console.WriteLine(httpRequestMessage.Headers.Authorization);

            Console.WriteLine("-- request body:");
            Console.WriteLine(requestBody);

            Console.WriteLine();
            Console.WriteLine("[press any key to make real request]");
            Console.WriteLine();

            Console.ReadLine();

            await PerformHttpRequest(httpRequestMessage);
        }

        private static async Task PerformHttpRequest(HttpRequestMessage httpRequestMessage)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri(BASE_URL)};

            var response = httpClient.SendAsync(httpRequestMessage).Result;
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("-- response http status code:");
            Console.WriteLine(response.StatusCode);

            Console.WriteLine("-- response body:");
            Console.WriteLine(responseBody);
        }

        private static InitCheckoutRequest CreateInitCheckoutRequest(string countryCode, int storeId)
        {
            var initCheckoutRequest = new InitCheckoutRequest
            {
                CountryCode = countryCode,
                StoreId = storeId,
                MerchantTermsUri = "https://collector.se",
                NotificationUri = "https://collector.se",
                Reference = "ABC123"
            };

            initCheckoutRequest.Cart.Items.Add(new ArticleItem
            {
                Id = "ARTICLE001",
                Description = "test article 1",
                Sku = Guid.NewGuid().ToString(),
                UnitPrice = 10,
                Quantity = 10, Vat = 25
            });

            return initCheckoutRequest;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(string pathAndQuery, InitCheckoutRequest request)
        {
            var bodyContent = JsonConvert.SerializeObject(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, pathAndQuery);

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("SharedKey", CreateAuthorizationHeader(pathAndQuery, bodyContent));
            httpRequest.Content = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            return httpRequest;
        }

        private static string CreateAuthorizationHeader(string pathAndQuery, string bodyContent = "")
        {
            var sha256Hash = ComputeSha256Hash($"{bodyContent}{pathAndQuery}{API_KEY}");
            var authorizationPair = $"{USERNAME}:{sha256Hash}";

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(authorizationPair));
        }

        private static string ComputeSha256Hash(string input)
        {
            using var crypt = SHA256.Create();
            var hash = string.Empty;
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
        }
    }
}
