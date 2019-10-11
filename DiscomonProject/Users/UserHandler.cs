using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Discord.Handlers;
using DiscomonProject.Exceptions;
using DiscomonProject.Storage.Implementations;

namespace DiscomonProject.Users
{
    public static class UserHandler
    {
        public static readonly string Filepath;
        private static readonly Dictionary<ulong, UserAccount> Accounts;
        private static readonly JsonStorage JsonStorage;

        static UserHandler()
        {
            Console.WriteLine("Loading User Accounts...");
            
            //Access JsonStorage to load user list into memory
            Filepath = "Users/UserList";

            Accounts = new Dictionary<ulong, UserAccount>();
            JsonStorage = new JsonStorage();

            foreach(var (id, account) in JsonStorage.RestoreObject<Dictionary<ulong, UserAccount>>(Filepath))
            {
                Accounts.Add(id, account);
            }

            foreach(var (id, acc) in Accounts)
            {
                Console.WriteLine($"Key: {id}\nValue: {acc}\n");
            }

            Console.WriteLine($"Successfully loaded {Accounts.Count} users.");
        }

        public static UserAccount GetUser(ContextIds ids) 
            => GetUser(ids.UserId);

        public static UserAccount GetUser(ulong id)
        {
            if(DoesUserExist(id)) { return Accounts[id]; }

            CreateNewUser(id);
            return Accounts[id];
        }

        public static UserAccount CreateNewUser(ulong id)
        {
            Console.WriteLine($"Creating new user with ID: {id}");

            var acc = new UserAccount
            {
                UserId = id
            };
            Accounts.Add(id, acc);
            SaveUsers();
            return acc;
        }

        public static void UpdateUserInfo(ulong id, ulong dm, string name, string mention, string avatar)
        {
            var user = GetUser(id);
            user.DmId = dm;
            user.Name = name;
            user.Mention = mention;
            user.AvatarUrl = avatar;
            SaveUsers();
        }

        private static void SaveUsers()
        {
            Console.WriteLine("Saving users...");
            JsonStorage.StoreObject(Accounts, Filepath);
        }

        public static void ClearUserData()
        {
            Console.WriteLine("Deleting all users.");
            var emptyDic = new Dictionary<ulong, UserAccount> {{0, new UserAccount {UserId = 0}}};
            JsonStorage.StoreObject(emptyDic, Filepath);
        }

        public static bool DoesUserExist(ulong id) 
            => Accounts.ContainsKey(id);

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