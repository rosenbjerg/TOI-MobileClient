using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOIClasses;

namespace TOI_MobileClient.Managers
{
    class SubscriptionManager
    {
        List<SubscribedServer> _subscribedServers = new List<SubscribedServer>();

        public IEnumerable<string> AllTags => _subscribedServers.SelectMany(ss => ss.TagCache);
        
    }

    class SubscribedServer
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

        public List<string> Contexts = new List<string>();
        
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
        
    }
}