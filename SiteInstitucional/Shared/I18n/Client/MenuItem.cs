using System;
using System.Collections.Generic;

namespace SiteInstitucional.Shared.I18n.Client
{
    public class MenuItem
    {
        public string Route { get; }
        public string CompleteRoute => $"/{Route ?? String.Empty}";
        public string Description { get; }
        public MenuItem[] ChildItems { get; }

        public MenuItem(string route, string description)
        {
            Route = route;
            Description = description;
        }

        public MenuItem(string route, string description, params MenuItem[] childItems) : this(route, description)
        {
            ChildItems = childItems;
        }

        public static implicit operator MenuItem(string description) => new MenuItem(null, description);
        public static implicit operator MenuItem(KeyValuePair<string, string> item) => new MenuItem(item.Key, item.Value);
    }
}
