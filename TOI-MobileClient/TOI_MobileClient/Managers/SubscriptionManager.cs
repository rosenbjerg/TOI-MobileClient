using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOIClasses;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;

#pragma warning disable 4014

namespace TOI_MobileClient.Managers
{
    public class SubscriptionManager
    {
        private static SubscriptionManager _instance;

        public static SubscriptionManager Instance => _instance ?? (_instance = new SubscriptionManager());

        public Dictionary<string, SubscribedServer> SubscribedServers =
            new Dictionary<string, SubscribedServer>();

        public void RefreshTags()
        {
            AllTags = SubscribedServers
                .Values
                .Where(ss => ss.Active && !ss.Hidden)
                .SelectMany(ss => ss.TagCache).ToHashSet();
        }

        public HashSet<string> AllTags { get; private set; }

        private void SaveServers()
        {
            SettingsManager.SaveServers(SubscribedServers);
        }

        public void AddServer(string name, string url, List<string> contexts)
        {
            SubscribedServers.Add(url, new SubscribedServer(name, url, contexts));
            SaveServers();
        }

        /// <summary>
        /// Remove url from SubscribedServers, if key exists
        /// </summary>
        /// <param name="url">BaseUrl of Server</param>
        public void RemoveServer(string url)
        {
            if (!SubscribedServers.ContainsKey(url)) return;

            SubscribedServers.Remove(url);
            SaveServers();
        }

        public void UpdateServer(string url, List<string> contexts)
        {
            SubscribedServers[url].Contexts = contexts;
            SaveServers();
        }

        public void Init()
        {
            var subscribedServers = SettingsManager.ReadServers();
            if (subscribedServers == null)
            {
                return;
            }

            SubscribedServers = subscribedServers;
            SubscribedServers.ForEach(ss => ss.Value.LoadTois());
        }
    }

    public class SubscribedServer
    {
        /// <summary>
        /// When inside radius of feed server
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// When the servers tois should be hidden (inactive) e
        /// </summary>
        public bool Hidden { get; set; }

        public string Name { get; set; }
        public string BaseUrl { get; set; }

        public List<string> Contexts
        {
            get => _contexts;
            set
            {
                _contexts = value;
                LoadTois();
            }
        }

        public SubscribedServer(string name, string url, List<string> contexts)
        {
            Name = name;
            BaseUrl = url;
            Contexts = contexts;
        }

        public async Task LoadTois()
        {
            var url = $"{BaseUrl}/tois?contexts={string.Join(",", Contexts)}";
            ToiCache = await ToiHttpClient.Instance.GetMany<ToiModel>(url);
            TagCache = ToiCache.SelectMany(toi => toi.Tags).ToHashSet();
            Index = TagCache.ToDictionary(tagId => tagId,
                tagId => ToiCache.Where(toi => toi.Tags.Contains(tagId)).ToList());
        }

        public Dictionary<string, List<ToiModel>> Index { get; set; }

        public List<ToiModel> ToiCache { get; private set; }
        public HashSet<string> TagCache = new HashSet<string>();
        private List<string> _contexts;
    }
}