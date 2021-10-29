using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Zack.Commons
{
    public static class HttpHelper
    {
        public static async Task SaveToFileAsync(this HttpResponseMessage respMsg, string file,
            CancellationToken cancellationToken = default)
        {
            if (respMsg.IsSuccessStatusCode == false)
            {
                throw new ArgumentException($"StatusCode of HttpResponseMessage is {respMsg.StatusCode}", nameof(respMsg));
            }
            using FileStream fs = new FileStream(file, FileMode.Create);
            await respMsg.Content.CopyToAsync(fs, cancellationToken);
        }

        public static async Task<HttpStatusCode> DownloadFileAsync(this HttpClient httpClient, Uri url, string localFile,
            CancellationToken cancellationToken = default)
        {
            var resp = await httpClient.GetAsync(url, cancellationToken);
            if (resp.IsSuccessStatusCode)
            {
                await SaveToFileAsync(resp, localFile, cancellationToken);
                return resp.StatusCode;
            }
            else
            {
                return HttpStatusCode.OK;
            }
        }

        public static async Task<T?> GetJsonAsync<T>(this HttpClient httpClient, Uri url, CancellationToken cancellationToken = default)
        {
            string json = await httpClient.GetStringAsync(url, cancellationToken);
            return json.ParseJson<T>();
        }
    }
}
