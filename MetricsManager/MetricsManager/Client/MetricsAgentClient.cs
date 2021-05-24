using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using MetricsManager.Requests;
using MetricsManager.Responses;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DTO;
using System.Text;
using Newtonsoft.Json;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;

        public MetricsAgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s") + "Z";
            var toParameter = request.ToTime.ToString("s") + "Z";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/hdd/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                var allHddMetricsApiResponse = JsonConvert.DeserializeObject<AllHddMetricsApiResponse>(jsonString);

                return allHddMetricsApiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s") + "Z";
            var toParameter = request.ToTime.ToString("s") + "Z";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/ram/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                var allRamMetricsApiResponse = JsonConvert.DeserializeObject<AllRamMetricsApiResponse>(jsonString);

                return allRamMetricsApiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AllDotNetMetricsApiResponse GetAllDonNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s") + "Z";
            var toParameter = request.ToTime.ToString("s") + "Z";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/dotnet/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                var allDotNetMetricsApiResponse = JsonConvert.DeserializeObject<AllDotNetMetricsApiResponse>(jsonString);

                return allDotNetMetricsApiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s") + "Z";
            var toParameter = request.ToTime.ToString("s") + "Z";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/cpu/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                var allCpuMetricsApiResponse = JsonConvert.DeserializeObject<AllCpuMetricsApiResponse>(jsonString);

                return allCpuMetricsApiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.ToString("s") + "Z";
            var toParameter = request.ToTime.ToString("s") + "Z";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/network/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

                string jsonString = response.Content.ReadAsStringAsync()
                                               .Result
                                               .Replace("\\", "")
                                               .Trim(new char[1] { '"' });

                var allNetworkMetricsApiResponse = JsonConvert.DeserializeObject<AllNetworkMetricsApiResponse>(jsonString);

                return allNetworkMetricsApiResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
