using System.Collections.Generic;
using Assets.Scripts.Model;
using RestSharp;

namespace Assets.Scripts.Networking
{
    public static class ApiClient
    {
        private static readonly string _baseUrl = "http://my.djohnnie.be:8801/api";

        public static Arena GetArena()
        {
            return Get<Arena>("arena");
        }

        public static List<Bot> GetBots()
        {
            return Get<List<Bot>>("bots");
        }

        private static TResult Get<TResult>(string resource) where TResult : new()
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(resource, Method.GET);
            var response = client.Execute<TResult>(request);
            return response.Data;
        }
    }
}