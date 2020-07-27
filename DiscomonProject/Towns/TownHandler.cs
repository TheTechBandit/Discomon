using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Storage;
using DiscomonProject.Storage.Implementations;
using Discord.WebSocket;

namespace DiscomonProject
{
    public static class TownHandler
    {
        public static readonly string filepath;
        private static Dictionary<ulong, TownAccount> _dic;
        private static JsonStorage _jsonStorage;

        static TownHandler()
        {
            System.Console.WriteLine("Loading Town Accounts...");
            
            //Access JsonStorage to load user list into memory
            filepath = "Towns/TownList";

            _dic = new Dictionary<ulong, TownAccount>();
            _jsonStorage = new JsonStorage();

            foreach(KeyValuePair<ulong, TownAccount> entry in _jsonStorage.RestoreObject<Dictionary<ulong, TownAccount>>(filepath))
            {
                _dic.Add(entry.Key, (TownAccount)entry.Value);
            }

            foreach(KeyValuePair<ulong, TownAccount> kvp in _dic)
            {
                Console.WriteLine($"Key: {kvp.Key}\nValue: {kvp.Value}\n");
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} users.");
        }

        public static TownAccount GetTown(ContextIds ids)
        {
            return GetTown(ids.GuildId);
        }

        public static TownAccount GetTown(ulong id)
        {
            if(DoesTownExist(id))
            {
                return _dic[id];
            }
            else
            {
                CreateNewTown(id);
                return _dic[id];
            }
        }

        public static TownAccount CreateNewTown(ulong id)
        {
            System.Console.WriteLine($"Creating new town with ID: {id}");

            TownAccount acc = new TownAccount(true)
            {
                GuildId = id
            };
            _dic.Add(id, acc);
            SaveTowns();
            return acc;
        }

        private static void SaveTowns()
        {
            System.Console.WriteLine("Saving towns...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void ClearTownData()
        {
            System.Console.WriteLine("Deleting all towns.");
            Dictionary<ulong, TownAccount> emptyDic = new Dictionary<ulong, TownAccount>();
            emptyDic.Add(0, new TownAccount(true)
            {
                GuildId = 0
            });
            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static bool DoesTownExist(ulong id)
        {
            return _dic.ContainsKey(id);
        }

    }
}