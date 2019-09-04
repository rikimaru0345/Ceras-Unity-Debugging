using System.Collections.Generic;
using System.Linq;

namespace Glue
{
    public class Frame
    {
        public ulong Counter;
        public Dictionary<string, object> Cargo;

        public int Count
        {
            get
            {
                return Cargo.Count;
            }
        }

        public string GetContentAsString()
        {
            var content = "";
            foreach (var e in Cargo)
            {
                content += $"{e.Key}: {e.Value}\n";
            }
            return content;
        }

        public Dictionary<string, object>.KeyCollection Keys
        {
            get
            {
                return Cargo.Keys;
            }
        }

        public Frame()
        {
            Cargo = new Dictionary<string, object>();
        }

        public Frame(ulong counter)
        {
            Counter = counter;
            Cargo = new Dictionary<string, object>();
        }

        public bool Contains(string key)
        {
            return Cargo.ContainsKey(key);
        }

        public void Add<T>(string key, ref T item)
        {
            if (Cargo.ContainsKey(key))
                Cargo[key] = item;
            else
                Cargo.Add(key, (object)item);
        }

        public void Remove(string key)
        {
            Cargo.Remove(key);
        }

        public T GetCargo<T>(string key, T fallback)
        {
            try
            {
                object item;
                Cargo.TryGetValue(key, out item);
                return (T)item;
            }
            catch (KeyNotFoundException)
            {
                return fallback;
            }
        }

        public void Clear()
        {
            Cargo.Clear();
        }
    }
}
