using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            foreach(KeyValuePair<ulong, UserAccount> kvp in _dic)
            {
                Console.WriteLine($"Key: {kvp.Key}\nValue: {kvp.Value}\n");
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
                return _dic[id];
            }
        }

        public static UserAccount CreateNewUser(ulong id)
        {
            System.Console.WriteLine($"Creating new user with ID: {id}");

            UserAccount acc = new UserAccount(true)
            {
                UserId = id
            };
            _dic.Add(id, acc);
            SaveUsers();
            return acc;
        }

        public static void UpdateUserInfo(ulong id, ulong dm, string name, string mention, string avatar)
        {
            var user = GetUser(id);
            user.DmId = dm;
            user.Name = name;
            user.Mention = mention;
            if(user.HasCharacter)
                user.Char.Mention = mention;
            user.AvatarUrl = avatar;
            SaveUsers();
        }

        private static void SaveUsers()
        {
            System.Console.WriteLine("Saving users...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void ClearUserData()
        {
            System.Console.WriteLine("Deleting all users.");
            Dictionary<ulong, UserAccount> emptyDic = new Dictionary<ulong, UserAccount>();
            emptyDic.Add(0, new UserAccount(true)
            {
                UserId = 0
            });
            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static bool DoesUserExist(ulong id)
        {
            return _dic.ContainsKey(id);
        }

        public static async Task CharacterExists(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if(!user.HasCharacter)
            {
                await MessageHandler.CharacterMissing(ids);
                throw new InvalidCharacterStateException("character doesn't exist");
            }
        }

        public static async Task OtherCharacterExists(ContextIds ids, UserAccount otherUser)
        {
            if(!otherUser.HasCharacter)
            {
                await MessageHandler.OtherCharacterMissing(ids);
                throw new InvalidCharacterStateException("other player's character doesn't exist");
            }
        }

        public static async Task ValidCharacterLocation(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if(user.Char.CurrentGuildId != ids.GuildId)
            {
                await MessageHandler.InvalidCharacterLocation(ids);
                throw new InvalidCharacterStateException("character in incorrect location");
            }
        }

        public static async Task OtherCharacterLocation(ContextIds ids, UserAccount otherUser)
        {
            var user = GetUser(ids.UserId);
            if(otherUser.Char.CurrentGuildId != ids.GuildId)
            {
                await MessageHandler.InvalidOtherCharacterLocation(ids, otherUser);
                throw new InvalidCharacterStateException("other player's character in a different location");
            }
        }

        public static async Task CharacterInCombat(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if(!user.Char.InCombat)
            {
                await MessageHandler.NotInCombat(ids);
                throw new InvalidCharacterStateException("character not in combat");
            }
        }
    }
}