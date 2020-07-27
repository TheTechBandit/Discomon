using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DiscomonProject.Storage.Implementations;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscomonProject.Discord
{
    public class DebugCommands : ModuleBase<SocketCommandContext>
    {
        [Command("getids")]
        public async Task GetIds()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.SendMessage(idList, $"Guild ID: {idList.GuildId}\nChannel ID: {idList.ChannelId}\nMessage ID:{idList.MessageId}");
        }

        [Command("partytest")]
        public async Task PartyTest()
        {
            var user = UserHandler.GetUser(Context.User.Id);
            Bitmap image = ImageGenerator.PartyMenu(user.Char.Party);

            using(MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                image.Dispose();
                await Context.Channel.SendFileAsync(stream, "Party.png");
                stream.Close();
            }
        }

        [Command("imagetest")]
        public async Task ImageTest()
        {
            ContextIds idList = new ContextIds(Context);
            Bitmap image = ImageGenerator.MergeTwoImages(Context);

            using(MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Seek(0, SeekOrigin.Begin);
                image.Dispose();
                await Context.Channel.SendFileAsync(stream, "Text.jpg");
            }
        }

        [Command("hurtme")]
        public async Task HurtMe(int amt)
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(Context.User.Id);
            foreach(BasicMon m in user.Char.Party)
            {
                m.CurrentHP -= amt;
            }

            await MessageHandler.SendMessage(idList, $"Your entire party has been injured by {amt}");
        }

        [Command("spawnmon")]
        public async Task SpawnMon([Remainder]string str)
        {
            ContextIds idList = new ContextIds(Context);
            UserAccount user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.ValidCharacterLocation(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            if(!user.Char.IsPartyFull())
            {
                BasicMon mon = MonRegister.StringToMonRegister(str);
                mon.CatcherID = user.UserId;
                mon.OwnerID = user.UserId;
                user.Char.Party.Add(mon);
                await MessageHandler.SendMessage(idList, $"{mon.Nickname} has been added to your party.");
            }
            else
            {
                await MessageHandler.SendMessage(idList, "Your party is full!");
            }
        }

        [Command("deletemon")]
        public async Task DeleteMon(int i)
        {
            ContextIds idList = new ContextIds(Context);
            UserAccount user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.ValidCharacterLocation(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            string nick = user.Char.Party[i-1].Nickname;
            user.Char.Party.RemoveAt(i-1);
            await MessageHandler.SendMessage(idList, $"{nick} has been removed from your party.");
        }

        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("debugplayer")]
        public async Task DebugInfo(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            UserAccount user;

            if(target != null)
            {
                user = UserHandler.GetUser(target.Id);
            }
            else
            {
                user = UserHandler.GetUser(idList.UserId);
            }

            await MessageHandler.SendMessage(idList, user.DebugString());
        }

        [Command("debugresetchar")]
        public async Task DebugResetCharacter()
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);
            user.Char = null;
            user.HasCharacter = false;
            user.PromptState = -1;

            await MessageHandler.SendMessage(ids, $"{user.Mention}, your character has been deleted.");
        }

        [Command("datawipe")]
        public async Task DataWipe()
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "User data cleared. Reboot bot to take effect.");
            UserHandler.ClearUserData();
            CombatHandler2.ClearCombatData();
        }

        [Command("whisper")]
        public async Task Whisper()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.MoveScreen(idList.UserId);
        }

        [Command("emojitest")]
        public async Task EmojiTest()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.EmojiTest(idList);
        }

        [Command("modifyasynctest")]
        public async Task ModifyAsyncTest()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.ModifyAsyncTest(idList, Context.User.Id);
        }

        [Command("typetest")]
        public async Task TypeTest()
        {
            ContextIds idList = new ContextIds(Context);

            var attack = new BeastType(true);
            List<BasicType> defense = new List<BasicType>()
            {
                new BeastType(true),
                new BeastType(true)
            };

            var effect = attack.ParseEffectiveness(defense);

            string defstr = $"{defense[0].Type}";
            if(defense.Count > 1)
                defstr += $"/{defense[1].Type}";

            await MessageHandler.SendMessage(idList, $"{attack.Type} is {effect}x effective against {defstr}");
        }

        [Command("quickstart")]
        public async Task QuickStart([Remainder]string text)
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);

            user.Char = new Character(true);
            user.Char.CurrentGuildId = ids.GuildId;
            user.Char.CurrentGuildName = Context.Guild.Name;
            user.Char.Name = user.Name;

            text = text.ToLower();

            BasicMon mon = MonRegister.StringToMonRegister(text);
            mon.CatcherID = user.UserId;
            mon.OwnerID = user.UserId;
            user.Char.Party.Add(mon);
            user.HasCharacter = true;
            await MessageHandler.SendMessage(ids, $"{user.Mention}, you have chosen {mon.Nickname} as your partner! Good luck on your adventure.");

            user.PromptState = -1;
        }

        [Command("quickduel")]
        public async Task QuickDuel(string mon, string mon2, SocketGuildUser target)
        {
            var fromUser = UserHandler.GetUser(Context.User.Id);
            var toUser = UserHandler.GetUser(target.Id);
            ContextIds ids = new ContextIds(Context);

            fromUser.Char = new Character(true);
            fromUser.Char.CurrentGuildId = ids.GuildId;
            fromUser.Char.CurrentGuildName = Context.Guild.Name;
            fromUser.Char.Name = fromUser.Name;
            mon = mon.ToLower();
            BasicMon m = MonRegister.StringToMonRegister(mon);
            m.CatcherID = fromUser.UserId;
            m.OwnerID = fromUser.UserId;
            fromUser.Char.Party.Add(m);
            fromUser.HasCharacter = true;
            await MessageHandler.SendMessage(ids, $"{fromUser.Mention}, you have chosen {m.Nickname} as your partner! Good luck on your adventure.");
            fromUser.PromptState = -1;

            toUser.Char = new Character(true);
            toUser.Char.CurrentGuildId = ids.GuildId;
            toUser.Char.CurrentGuildName = target.Guild.Name;
            toUser.Char.Name = toUser.Name;
            mon2 = mon2.ToLower();
            BasicMon m2 = MonRegister.StringToMonRegister(mon2);
            m2.CatcherID = toUser.UserId;
            m2.OwnerID = toUser.UserId;
            toUser.Char.Party.Add(m2);
            toUser.HasCharacter = true;
            await MessageHandler.SendMessage(ids, $"{toUser.Mention}, you have chosen {m2.Nickname} as your partner! Good luck on your adventure.");
            toUser.PromptState = -1;

            CombatInstance2 combat = new CombatInstance2(ids, fromUser, toUser);
            CombatHandler2.StoreInstance(CombatHandler2.NumberOfInstances(), combat);
            await combat.StartCombat();
        }

        [Command("commands")]
        public async Task Commands()
        {
            ContextIds idList = new ContextIds(Context);
            var str = "";
            str += "**DEBUG**";
            str += "\nping- The bot responds with \"pong.\" Used to test ping and trigger bot updates.";
            str += "\ndebugplayer {@Mention}- Shows debug info for a player's UserAccount and Character profiles.";
            str += "\ndebugresetchar- Deletes the user's character.";
            str += "\ndatawipe- Wipes all bot data in case of corrupted data or inconsistent values.";
            str += "\nwhisper- Used to test various DM or special case messages.";
            str += "\nemojitest- Temporary test command to showcase custom emoji usage.";
            str += "\ntypetest- Temporary test command used to demonstrate type advantages.";
            str += "\nquickstart {MonName}- Easy alternative to !startadventure for testing purposes. Use carefully.";

            str += "\n\n**BASIC**";
            str += "\nmonstat {Party#}- Shows the stats of the mon at the indicated party number.";
            str += "\nparty- Lists the mons in the user's party.";
            str += "\nenter {Input}- Used as an input method. Needs better implementation.";
            str += "\nstartadventure- Character creation command.";
            
            str += "\n\n**COMBAT**";
            str += "\nduel {@Mention}- Sends a duel request to the mentioned player. Starts a duel if a request has already been recieved from the mentioned player.";
            str += "\nattack- If the fight/move screen has been broken or lost, this will resend it.";
            str += "\nexitcombat- Exits combat, automatically forfeiting.";
            str += "\npheal- Heals the user's party.";

            await MessageHandler.SendMessage(idList, str);
        }

        [Command("debugcombat")]
        public async Task DebugCombat()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList);
            
            string str = "";
            double mod;
            string mess;
            str += $"Owner/Mon: {user.Name}/{user.Char.ActiveMon.Nickname}";
            str += $"\nLevel: {user.Char.ActiveMon.Level}";

            str += $"\n\nAttack: {user.Char.ActiveMon.CurStats[1]}";
            (mod, mess) = user.Char.ActiveMon.ChangeAttStage(0);
            str += $"\nAttack Stage Mod: {mod}";
            str += $"\nAttack Modified: {(int)(user.Char.ActiveMon.CurStats[1]*mod)}";

            str += $"\n\nDefense: {user.Char.ActiveMon.CurStats[2]}";
            (mod, mess) = user.Char.ActiveMon.ChangeDefStage(0);
            str += $"\nDefense Stage Mod: {mod}";
            str += $"\nDefense Modified: {(int)(user.Char.ActiveMon.CurStats[2]*mod)}";

            str += $"\n\nAffinity: {user.Char.ActiveMon.CurStats[3]}";
            (mod, mess) = user.Char.ActiveMon.ChangeAffStage(0);
            str += $"\nAffinity Stage Mod: {mod}";
            str += $"\nAffinity Modified: {(int)(user.Char.ActiveMon.CurStats[3]*mod)}";

            str += $"\n\nSpeed: {user.Char.ActiveMon.CurStats[4]}";
            (mod, mess) = user.Char.ActiveMon.ChangeSpdStage(0);
            str += $"\nSpeed Stage Mod: {mod}";
            str += $"\nSpeed Modified: {(int)(user.Char.ActiveMon.CurStats[4]*mod)}";

            await MessageHandler.SendMessage(idList, str);
        }

    }
}