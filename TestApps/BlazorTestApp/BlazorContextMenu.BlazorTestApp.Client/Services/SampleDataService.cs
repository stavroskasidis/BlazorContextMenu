using BlazorContextMenu.TestAppsCommon;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorContextMenu.BlazorTestApp.Client.Services
{
    public class SampleDataService : ISampleDataService
    {
        private readonly HttpClient _http;

        public SampleDataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string[]> GetSampleData()
        {
            var summaries = await _http.GetJsonAsync<string[]>("/api/SampleData/Summaries");
            return summaries;
        }
    }
}
