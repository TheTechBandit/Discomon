using System;
using System.Collections.Generic;
using DiscomonProject.Storage;
using DiscomonProject.Storage.Implementations;
using Discord.WebSocket;

namespace DiscomonProject
{
    public static class UserHandler
    {
        public static readonly string filepath;
        private static InMemoryStorage _storage;
        private static JsonStorage _jsonStorage;

        static UserHandler()
        {
            System.Console.WriteLine("Loading User Accounts...");

            //Access JsonStorage to load user list into memory
            filepath = "Users/UserList";

            _storage = new InMemoryStorage();
            _jsonStorage = new JsonStorage();
            
            _storage.StoreAllObjects(_jsonStorage.RestoreObject<Dictionary<string, object>>(filepath));

            System.Console.WriteLine($"Successfully loaded {_storage.StorageLength()} users.");
        }

        public static UserAccount GetUser(DiscordUser user)
        {
            if(DoesUserExist(user.GetID()))
            {
                System.Console.WriteLine("1");
                SaveUsers();
                System.Console.WriteLine("2");
                return _storage.RestoreObject<UserAccount>(Convert.ToString(user.GetID()));
            }
            else
            {
                System.Console.WriteLine("3");
                return CreateNewUser(user);
            }
        }

        private static UserAccount CreateNewUser(DiscordUser user)
        {
            System.Console.WriteLine($"Creating new user: {user.GetUser().Mention}");

            UserAccount acc = new UserAccount(user);
            _storage.StoreObject(acc, Convert.ToString(acc.getID()));
            SaveUsers();
            return acc;
        }

        private static void SaveUsers()
        {
            System.Console.WriteLine("Saving users...");
            _jsonStorage.StoreObject(_storage.GetDict(), filepath);
        }

        public static bool DoesUserExist(ulong id)
        {
            try
            {
                System.Console.WriteLine("4");
                //THIS LINE IS BEING FUCKY
                _storage.RestoreObject<UserAccount>(Convert.ToString(id));
                System.Console.WriteLine("5");
            } catch(ArgumentException e) 
            {
                System.Console.WriteLine("6");
                return false;
            }
            return true;
        }
    }
}