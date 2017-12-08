using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepMan;
using TOIClasses;
using Xamarin.Forms;
using Newtonsoft.Json;
using TOI_MobileClient.Dependencies;
using TOI_MobileClient.Views;
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

        public IEnumerable<ToiModel> GetTois(string tag)
        {
            return SubscribedServers.Values.SelectMany(ss => ss.GetTois(tag));
        }

        public IEnumerable<ToiModel> GetToisByLocation(GpsLocation location)
        {
            return SubscribedServers.Values.SelectMany(ss => ss.GetToisByLocation(location));
        }

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

        public bool Inited { get; private set; }
        public async void Init()
        {
            var subscribedServers = SettingsManager.ReadServers();
            if (subscribedServers == null || subscribedServers.Count == 0)
            {
                if (await App.Current.MainPage.DisplayAlert("Select feeds", "Please subcribe to a feed", "Select",
                    "Cancel"))
                {
                    App.Current.MainPage.Navigation.PushAsync(new FeedSelectionPage());
                }
                return;
            }

            SubscribedServers = subscribedServers;
            SubscribedServers.ForEach(async ss => await ss.Value.LoadTois());
            RefreshTags();
            Inited = true;
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
                if (!string.IsNullOrEmpty(BaseUrl))
                {
                    LoadTois();
                }
            }
        }

        public SubscribedServer(string name, string url, List<string> contexts)
        {
            Name = name;
            BaseUrl = url;
            Contexts = contexts;
        }

        private readonly List<ToiModel> _cache = new List<ToiModel>();

        public List<ToiModel> GetTois(string tag)
        {
            return Index.TryGetValue(tag, out var value) ? value : _cache;
        }

        public List<ToiModel> GetToisByLocation(GpsLocation location)
        {
            var gps = GpsCache?.Where(t => t.WithinRange(location));
            return gps?.SelectMany(t => Index[t.Id]).ToList() ?? _cache;
        }

        public async Task LoadTois()
        {
            Console.WriteLine($"Fetching Contexts: '{string.Join(", ", Contexts)}' from {BaseUrl}");
            var toiUrl = $"{BaseUrl}/tois?contexts={string.Join(",", Contexts)}";
            var tagUrl = $"{BaseUrl}/tags?contexts={string.Join(",", Contexts)}";
            ToiCache = await ToiHttpClient.Instance.GetMany<ToiModel>(toiUrl);
            TagCache = ToiCache.SelectMany(toi => toi.Tags).ToHashSet();
            GpsCache = (await ToiHttpClient.Instance.GetMany<TagModel>(tagUrl))
                ?.Where(t => t.Type == TagType.Gps)
                .ToHashSet();
            Index = TagCache.ToDictionary(tagId => tagId,
            tagId => ToiCache.Where(toi => toi.Tags.Contains(tagId)).ToList());
        }


        public Dictionary<string, List<ToiModel>> Index { get; set; }

        public List<ToiModel> ToiCache { get; private set; }
        public HashSet<string> TagCache = new HashSet<string>();
        public HashSet<TagModel> GpsCache { get; set; }
        private List<string> _contexts;
    }
}