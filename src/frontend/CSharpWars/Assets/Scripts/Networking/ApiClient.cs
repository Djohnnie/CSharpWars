using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Model;
using RestSharp;

namespace Assets.Scripts.Networking
{
    public interface IApiClient
    {
        Task<Arena> GetArena();

        Task<List<Bot>> GetBots();
    }

    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl = "https://api.djohnnie.be:8801/api";

        public Task<Arena> GetArena()
        {
            return Get<Arena>("arena");
        }

        public Task<List<Bot>> GetBots()
        {
            return Get<List<Bot>>("bots");
        }

        private async Task<TResult> Get<TResult>(string resource) where TResult : new()
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(resource, Method.GET);
            var response = await client.ExecuteTaskAsync<TResult>(request);
            return response.Data;
        }
    }
}