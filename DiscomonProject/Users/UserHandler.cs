using System;
using System.Collections.Generic;
using DiscomonProject.Discord;
using DiscomonProject.Storage;
using DiscomonProject.Storage.Implementations;
using Discord.WebSocket;

namespace DiscomonProject
{
    public static class UserHandler
    {
        public static readonly string filepath;
        private static Dictionary<ulong, UserAccount> _dic;
        private static JsonStorage _jsonStorage;

        static UserHandler()
        {
            System.Console.WriteLine("Loading User Accounts...");

            //Access JsonStorage to load user list into memory
            filepath = "Users/UserList";

            _dic = new Dictionary<ulong, UserAccount>();
            _jsonStorage = new JsonStorage();
            
            foreach(KeyValuePair<ulong, UserAccount> entry in _jsonStorage.RestoreObject<Dictionary<ulong, UserAccount>>(filepath))
            {
                _dic.Add(entry.Key, (UserAccount)entry.Value);
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} users.");
        }

        public static UserAccount GetUser(ContextIds ids)
        {
            return GetUser(ids.UserId);
        }

        public static UserAccount GetUser(ulong id)
        {
            if(DoesUserExist(id))
            {
                return _dic[id];
            }
            else
            {
                CreateNewUser(id);
                SaveUsers();
                return _dic[id];
            }
        }

        public static UserAccount CreateNewUser(ulong id)
        {
            System.Console.WriteLine($"Creating new user with ID: {id}");

            UserAccount acc = new UserAccount 
            {
                UserId = id
            };
            _dic.Add(id, acc);
            SaveUsers();
            return acc;
        }

        private static void SaveUsers()
        {
            System.Console.WriteLine("Saving users...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static bool DoesUserExist(ulong id)
        {
            return _dic.ContainsKey(id);
        }
    }
}