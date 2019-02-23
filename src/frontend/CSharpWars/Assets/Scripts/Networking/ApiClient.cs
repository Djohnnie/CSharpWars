using System;
using Assets.Scripts.Model;
using RestSharp;

namespace Assets.Scripts.Networking
{
    public static class ApiClient
    {
        private static readonly String _baseUrl = "http://localhost:5000/api";

        public static Arena GetArena()
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("arena", Method.GET);
            var response = client.Execute<Arena>(request);
            return response.Data;
        }
    }
}