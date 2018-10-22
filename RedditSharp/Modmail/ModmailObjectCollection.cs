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
    public class ModmailObjectCollection<T> : IDictionary, IList where T : ModmailObject
    {
        public int Limit { get; set; }

        internal IList<string> _ids;

        internal IDictionary<string,T> _entities;

        private IDictionary<string, IEnumerable<string>> _subMap;

        [JsonConstructor]
        internal ModmailObjectCollection()
        {
            this._ids = new List<string>();
            this._entities = new Dictionary<string, T>();
        }

        internal ModmailObjectCollection(ICollection<T> entities) : this()
        {
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
            _ids.Add(item.Id);
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
            {
                _entities.Remove(id);
                _ids.Remove(id);
            }
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

        public int Add(object value)
        {
            var x = (T)value;
            Add(x.Id, x);
            return _ids.IndexOf(x.Id);
        }

        public int IndexOf(object value)
        {
            var x = (T)value;
            return _ids.IndexOf(x.Id);
        }

        public void Insert(int index, object value)
        {
            var x = (T)value;
            if (_entities.ContainsKey(x.Id))
                throw new Exception("Can't add this more than once.");

            _entities.Add(x.Id, x);
            _ids.Insert(index, x.Id);
        }

        public void RemoveAt(int index)
        {
            if (index >= _ids.Count)
                return;

            _entities.Remove(_ids[index]);
            _ids.RemoveAt(index);
        }

        /// <inheritdoc />
        public int Count => _entities.Count;

        /// <inheritdoc />
        public object SyncRoot { get; }

        /// <inheritdoc />
        public bool IsSynchronized { get; }

        public T this[string key]
        {
            get => _entities[key];
            set => Add(key, value);
        }

        public T this[int index]
        {
            get => _entities[_ids[index]];
            set => Insert(index, value);
        }

        object IDictionary.this[object key]
        {
            get => _entities[(string)key];
            set => Add(value, (string)key);
        }

        object IList.this[int index]
        {
            get => _entities[_ids[index]];
            set => Insert(index, value);
        }
    }

    public class ModmailEntityCollectionConverter<T> : Newtonsoft.Json.JsonConverter where T : ModmailObject
    {
        public override bool CanConvert(Type objectType)
        {
            return (
                objectType == typeof(ModmailObjectCollection<Message>) ||
                objectType == typeof(ModmailObjectCollection<Conversation>)
            );
          //  throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type type = typeof(T);


            var result = new ModmailObjectCollection<T>();

            var json = JObject.Load(reader);

            if (json.Type != JTokenType.Object)
                throw new Exception("Unexpected token");

            var xx = json.ToString();


            foreach (var item in json.Properties())
            {
                var convo = json[item.Name];
                var obj = JsonSerializer.CreateDefault().Deserialize<T>(convo.CreateReader());

            //    var obj = json[item.Name].ToObject<T>(JsonSerializer.CreateDefault());

                    result[item.Name] = json[item.Name].ToObject<T>();

            }
               

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
