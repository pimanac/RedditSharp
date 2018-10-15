using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditSharp.Modmail
{
    public class ModmailEntityCollection<T> : IDictionary where T : ModmailObject
    {
        public int Limit { get; set; }

        internal IDictionary<string,T> _entities;

        private IDictionary<string, IEnumerable<string>> _subMap;

        public ModmailEntityCollection()
        {

        }

        internal ModmailEntityCollection(ICollection<T> entities)
        {
            this._entities = new Dictionary<string, T>();
            foreach (var e in entities)
                if (!_entities.ContainsKey(e.Id))
                    _entities.Add(e.Id, e);
        }
       
        /// <inheritdoc />
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_entities.ContainsKey(item.Id))
                return;

            _entities.Add(item.Id, item);
        }

        /// <inheritdoc />
        public bool Contains(object key)
        {
            return _entities.ContainsKey((string)key);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _entities.ContainsKey(item.Id);
        }

        /// <inheritdoc />
        public void Add(object key, object value)
        {
            this.Add((T) value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _entities.Clear();
        }

        /// <inheritdoc />
        public IDictionaryEnumerator GetEnumerator()
        {
            return (IDictionaryEnumerator) _entities.GetEnumerator();
        }

        /// <inheritdoc />
        public void Remove(object key)
        {
            var id = (string) key;
            if (_entities.ContainsKey(id))
                _entities.Remove(id);
        }

        /// <inheritdoc />
        public object this[object key]
        {
            get => _entities[(string)key];
            set => _entities[(string)key] = (T)value;
        }

        public T this[string key]
        {
            get => _entities[(string)key];
            set => _entities[(string)key] = (T)value;
        }

        /// <inheritdoc />
        public ICollection Keys => _entities.Keys.ToList();

        /// <inheritdoc />
        public ICollection Values => _entities.Values.ToList();

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public bool IsFixedSize => false;

        /// <inheritdoc />
        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _entities.Remove(item.Id);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            _entities.ToArray().CopyTo(array, index);
        }

        /// <inheritdoc />
        public int Count => _entities.Count;

        /// <inheritdoc />
        public object SyncRoot { get; }

        /// <inheritdoc />
        public bool IsSynchronized { get; }
    }

    public class ModmailEntityCollectionConverter<T> : Newtonsoft.Json.JsonConverter where T : ModmailObject
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new ModmailEntityCollection<T>();
            var json = JObject.Load(reader);

            if (json.Type == JTokenType.Array)
            {
                if (!json.HasValues)
                    return;

                if ()
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
