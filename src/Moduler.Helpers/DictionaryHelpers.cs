using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moduler.Helpers
{
    public static class DictionaryHelpers
    {
        #region ZeroDate
        public static DateTime zeroDate = new DateTime(2022, 04, 13);
        private static SortedDictionary<string, Guid?> ZeroDates;
        public static SortedDictionary<string, Guid?> GetZeroDates(int lenght = 30)
        {
            if (ZeroDates != null) return ZeroDates;
            var result = new SortedDictionary<string, Guid?>();
            for (int i = 0; i < lenght; i++)
            {
                result.Add(zeroDate.AddMilliseconds(i).ToMyString(), Guid.Empty);
            }
            ZeroDates = result;
            return result;
        }
        public static SortedDictionary<string, Guid?> CleanZeroDates(this SortedDictionary<string, Guid?> dictionary, int lenght = 30)
        {
            var temp = dictionary.ToArray();
            var deleted = 0;
            for (int i = 0; i < dictionary.Count - lenght; i++)
            {
                var idate = zeroDate;
                DateTime.TryParse(temp[i].Key, out idate);
                if (idate < zeroDate.AddMinutes(1))
                {
                    dictionary.Remove(temp[i].Key);
                    deleted++;
                }
            }
            return dictionary;
        }
        #endregion
        #region GetOrAdd
        public static TValue GetOrAdd<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue newValue)
        {
            TValue oldValue;
            if (dictionary.TryGetValue(key, out oldValue))
                return oldValue;
            else
            {
                dictionary.Add(key, newValue);
                return newValue;
            }
        }
        public static TValue GetOrAdd<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key)
            where TValue : new()
        {
            TValue oldValue;
            if (dictionary.TryGetValue(key, out oldValue))
                return oldValue;
            else
            {
                var newValue = new TValue();
                dictionary.Add(key, newValue);
                return newValue;
            }
        }
        #endregion
        #region AddMany
        public static bool AddMany<TKey1, TKey2, TValue>(
            this Dictionary<TKey1, Dictionary<TKey2, TValue>> dictionary,
            TKey1 key1,
            TKey2 key2,
            TValue newValue)
        {
            dictionary.GetOrAdd(key1)[key2] = newValue;
            return true;
        }
        public static bool AddMany<TKey1, TKey2, TValue>(
            this Dictionary<TKey1, SortedDictionary<TKey2, TValue>> dictionary,
            TKey1 key1,
            TKey2 key2,
            TValue newValue)
        {
            dictionary.GetOrAdd(key1)[key2] = newValue;
            return true;
        }
        public static bool AddMany<TKey1, TKey2, TKey3, TValue>(
            this Dictionary<TKey1, Dictionary<TKey2, Dictionary<TKey3, TValue>>> dictionary,
            TKey1 key1,
            TKey2 key2,
            TKey3 key3,
            TValue newValue)
        {
            dictionary.GetOrAdd(key1).GetOrAdd(key2)[key3] = newValue;
            return true;
        }
        #endregion
        public static DictionaryUpdateResult MergeDictionariesTo(this Dictionary<string, SortedDictionary<string, string>> newData,
            Dictionary<string, SortedDictionary<string, string>> existingData, bool updateSeen = true)
        {
            var result = new DictionaryUpdateResult();
            foreach (var newKey in newData)
            {
                SortedDictionary<string, string> existingValues;
                if (!existingData.TryGetValue(newKey.Key, out existingValues))
                { //Get cat1
                    existingData.Add(newKey.Key, newKey.Value);
                    result.NewKeyCount++;
                    result.NewValueCount += newKey.Value.Count;
                }
                else
                {
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    if (newKey.Key == "fiyat")
                    {

                    }

                    var beforeCount = existingValues.Count;
                    foreach (var item in newKey.Value)
                    {
                        existingValues[item.Key] = item.Value;
                    }
                    if (existingValues.Count > beforeCount)
                    {
                        var orderedNewValue = existingValues.ToArray();
                        for (int i = 1; i < orderedNewValue.Length; i++)
                        {
                            if (orderedNewValue[i].Value.Trim() == orderedNewValue[i - 1].Value.Trim())
                            {
                                existingValues.Remove(orderedNewValue[i].Key);
                                newKey.Value.Remove(orderedNewValue[i].Key);
                            }
                        }
                        existingData[newKey.Key] = existingValues;
                        result.NewValueCount += newKey.Value.Count;
                        if (newKey.Key == "fiyat" && newKey.Value.Count > 0)
                        { result.HasPriceChanged = true; }
                    }
                    //sw.Stop();
                    //var sec1 = sw.ElapsedTicks;
                }
            }
            if ((result.NewKeyCount + result.NewValueCount) == 0)
            {
                result.NewSeenCount++;
            }
            return result;
        }
        #region UnUsed
        public static DictionaryUpdateResult MergeDictionariesTo(this Dictionary<string, Dictionary<string, string>> newData,
            Dictionary<string, Dictionary<string, string>> existingData, bool updateSeen = true)
        {
            var result = new DictionaryUpdateResult();
            foreach (var newKey in newData)
            {
                Dictionary<string, string> existingValues;
                if (!existingData.TryGetValue(newKey.Key, out existingValues))
                { //Get cat1
                    existingData.Add(newKey.Key, newKey.Value);
                    result.NewKeyCount++;
                    result.NewValueCount += newKey.Value.Count;
                }
                else
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();

                    foreach (var item in newKey.Value)
                    {
                        existingValues[item.Key] = item.Value;
                    }
                    if (existingValues.Count > 1)
                    {
                        var orderedNewValue = existingValues.OrderBy(x => x.Key).ToArray();
                        for (int i = 1; i < orderedNewValue.Length; i++)
                        {
                            if (orderedNewValue[i].Value == orderedNewValue[i - 1].Value)
                            {
                                existingValues.Remove(orderedNewValue[i].Key);
                                newKey.Value.Remove(orderedNewValue[i].Key);
                            }
                        }
                        if (existingValues.Count > 1)
                        {
                            existingData[newKey.Key] = existingValues.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                        }
                        result.NewValueCount += newKey.Value.Count;
                    }
                    sw.Stop();
                    var sec1 = sw.ElapsedTicks;
                }
            }
            return result;
        }
        public static DictionaryUpdateResult MergeDictionariesTo(this Dictionary<string, Dictionary<string, Dictionary<string, Guid?>>> newData,
            Dictionary<string, Dictionary<string, Dictionary<string, Guid?>>> existingData, bool updateSeen = true)
        {
            var result = new DictionaryUpdateResult();
            foreach (var newKey in newData)
            {
                Dictionary<string, Dictionary<string, Guid?>> newValue;
                if (!existingData.TryGetValue(newKey.Key, out newValue))
                { //Fiyatında aynı fiyat yoksa tüm görülmeleriyle birlikte ekle
                    existingData.Add(newKey.Key, newKey.Value);
                    result.NewKeyCount++;
                }
                else //Fiyatında aynı fiyat varsa tüm görülmeleri kontrol et ve ekle
                {
                    foreach (var kvp2 in newKey.Value)
                    {
                        Dictionary<string, Guid?> newSeen;
                        if (!newValue.TryGetValue(kvp2.Key, out newSeen))
                        {
                            newValue.Add(kvp2.Key, kvp2.Value);
                            result.NewValueCount++;
                        }
                        else if (updateSeen)
                        {
                            foreach (var kvp3 in kvp2.Value)
                            {

                                Guid? catcher;
                                if (!newSeen.TryGetValue(kvp3.Key, out catcher))
                                {
                                    newSeen.Add(kvp3.Key, catcher);
                                    result.NewSeenCount++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public static void MergeDictionariesss()
        {
            var oldData = new Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<int, int>>>>();
            var todayData = new Dictionary<string, Dictionary<int, Dictionary<int, Dictionary<int, int>>>>();

            foreach (var kvp in oldData)
            {
                Dictionary<int, Dictionary<int, Dictionary<int, int>>> today;
                if (!todayData.TryGetValue(kvp.Key, out today))
                {
                    today = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();
                    todayData.Add(kvp.Key, today);
                }

                foreach (var kvp2 in kvp.Value)
                {
                    Dictionary<int, Dictionary<int, int>> today2;
                    if (!today.TryGetValue(kvp2.Key, out today2))
                    {
                        today2 = new Dictionary<int, Dictionary<int, int>>();
                        today.Add(kvp2.Key, today2);
                    }

                    foreach (var kvp3 in kvp2.Value)
                    {
                        Dictionary<int, int> today3;
                        if (!today2.TryGetValue(kvp3.Key, out today3))
                        {
                            today3 = new Dictionary<int, int>();
                            today2.Add(kvp3.Key, today3);
                        }

                        foreach (KeyValuePair<int, int> kvp4 in kvp3.Value)
                        {
                            today3[kvp4.Key] = kvp4.Value;
                        }
                    }
                }
            }
        }
        public class DictionaryRepresentationConvention : ConventionBase, IMemberMapConvention
        {
            private readonly DictionaryRepresentation _dictionaryRepresentation;

            public DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation = DictionaryRepresentation.ArrayOfDocuments)
            {
                // see http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/#dictionary-serialization-options

                _dictionaryRepresentation = dictionaryRepresentation;
            }

            public void Apply(BsonMemberMap memberMap)
            {
                memberMap.SetSerializer(ConfigureSerializer(memberMap.GetSerializer(), Array.Empty<IBsonSerializer>()));
            }

            private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer, IBsonSerializer[] stack)
            {
                if (serializer is IDictionaryRepresentationConfigurable dictionaryRepresentationConfigurable)
                {
                    serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(_dictionaryRepresentation);
                }

                if (serializer is IChildSerializerConfigurable childSerializerConfigurable)
                {
                    if (!stack.Contains(childSerializerConfigurable.ChildSerializer))
                    {
                        var newStack = stack.Union(new[] { serializer }).ToArray();
                        var childConfigured = ConfigureSerializer(childSerializerConfigurable.ChildSerializer, newStack);
                        return childSerializerConfigurable.WithChildSerializer(childConfigured);
                    }
                }

                return serializer;
            }
        }
        #endregion
    }

    public class DictionaryUpdateResult
    {
        public int NewKeyCount { get; set; } //Key
        public int NewValueCount { get; set; } //Value
        public int NewSeenCount { get; set; } //Value
        public bool HasPriceChanged { get; set; }
    }
}
