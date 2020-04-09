using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace solution
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Client client = new Client();
            client.Init();

            await client.GetData();
            await client.PostData("warhammer", "{\"dataId\":\"warhammer\",\"weight\":40000}");

            await client.PostData("lock", "{\"dataId\":\"lock\",\"weight\":1000}");

            await client.PostData("stock", "{\"dataId\":\"stock\",\"weight\":2000}");

            await client.PostData("barrels", "{\"dataId\":\"barrels\",\"weight\":3000}");

            await client.GetDataSorted(true);

            await client.PutData("warhammer", "{\"dataId\":\"warhammer\",\"weight\":30000}");

            await client.GetDataById("warhammer");

            await client.DeleteData("stock");
            await client.GetData();

        }
    }

    class Client
    {

        public Client()
        {
            m_Client = new HttpClient();
        }

        public void Init()
        {
            m_Client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            m_Client.DefaultRequestHeaders.TryAddWithoutValidation("appId", "campus-task");
        }

        public async Task GetData() 
        {

            var result = await m_Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));

            await PrintResultStatus(result);

        }


        public async Task GetDataById(string id) 
        {
            string reqUrl = url + "?id=" + id;

            var result = await m_Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, reqUrl));

            await PrintResultStatus(result);

        }

        public async Task GetDataSorted(bool sorted) 
        {
            string reqUrl = url + "?sorted=" + (sorted ? "True" : "False");

            var result = await m_Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, reqUrl));

            await PrintResultStatus(result);

        }
        public async Task PutData(string id, string data) 
        {
            string reqUrl = url + "?id=" + id;

            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Put, reqUrl);
            msg.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await m_Client.SendAsync(msg);

            await PrintResultStatus(result);

        }

        public async Task PostData(string id, string data)
        {
            string reqUrl = url + "?id=" + id;

            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, reqUrl);
            msg.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var result = await m_Client.SendAsync(msg);

            await PrintResultStatus(result);

        }

        public async Task DeleteData(string id) 
        {
            string reqUrl = url + "/" + id;

            var result = await m_Client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, reqUrl));

            await PrintResultStatus(result);

        }

        private async Task PrintResultStatus(HttpResponseMessage result) 
        {
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Success");
                Console.WriteLine(await result.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine($"Status code: {result.StatusCode}");
                Console.WriteLine($"Message: { await result.Content.ReadAsStringAsync()}");
            }
        }

        private HttpClient m_Client;


        private const string url = "http://localhost:5000/somedata";
    }
}
