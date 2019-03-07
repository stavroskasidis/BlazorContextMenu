using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorContextMenu.TestAppsCommon
{
    public interface ISampleDataService
    {
        Task<string[]> GetSampleData();
    }
}
