using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;

namespace DiscomonProject
{
    public static class CombatHandler
    {
        static CombatHandler()
        {

        }

        public static async Task StartCombat(CombatInstance main)
        {
            var other = GetOppositeInstance(main);
            var user = GetUserOfInstance(main);
            var otherUser = GetUserOfInstance(other);

            //Make sure both users have a valid party
            if(!PlayerHasValidParty(main))
            {
                user.Char.ExitCombat();
                otherUser.Char.ExitCombat();
                await MessageHandler.SendMessage(main.Location, $"Duel canceled! {user.Mention} does not have a valid party.");
                return;
            }
            else if(!PlayerHasValidParty(other))
            {
                user.Char.ExitCombat();
                otherUser.Char.ExitCombat();
                await MessageHandler.SendMessage(main.Location, $"Duel canceled! {otherUser.Mention} does not have a valid party.");
                return;
            }

            //Send out both mons.
            var mainMon = user.Char.FirstUsableMon();
            var otherMon = otherUser.Char.FirstUsableMon();

            main.ActiveMon = mainMon;
            other.EnemyMon = mainMon;
            await MessageHandler.SendEmbedMessage(main.Location, $"{user.Mention} sends out **{main.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(main.ActiveMon));

            main.EnemyMon = otherMon;
            other.ActiveMon = otherMon;
            await MessageHandler.SendEmbedMessage(main.Location, $"{otherUser.Mention} sends out **{other.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(other.ActiveMon));

            main.CombatPhase = 0;
            other.CombatPhase = 0;
            await NextStep(main);
        }

        public static async Task NextStep(CombatInstance main)
        {
            var other = GetOppositeInstance(main);
            if(main.CombatPhase == 0)
            {
                //0- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"

                await MessageHandler.AttackStepText(main.Location);
                main.CombatPhase++;
                other.CombatPhase++;
            }

        }

        public static async Task Attack(CombatInstance main)
        {
            var other = GetOppositeInstance(main);

            var user = GetUserOfInstance(main);
            var otherUser = GetUserOfInstance(other);

            if(main.CombatPhase == 1 && other.CombatPhase == 1)
            {
                main.CombatPhase++;
                await MessageHandler.AttackEnteredText(main.Location, user);
            }
            else if(main.CombatPhase == 2 && other.CombatPhase == 1)
            {
                await MessageHandler.AttackAlreadyEntered(main.Location, user);
            }
            else if(main.CombatPhase == 1 && other.CombatPhase == 2)
            {
                main.CombatPhase += 2;
                other.CombatPhase++;
                //3- Pre-Attack phase (activate any abilities that trigger before attacks)

                main.CombatPhase++;
                other.CombatPhase++;
                //4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
                if(main.ActiveMon.CurStats[4] > main.EnemyMon.CurStats[4])
                {
                    await ApplyMoves(main, true);
                }
                else if(main.ActiveMon.CurStats[4] < main.EnemyMon.CurStats[4])
                {
                    await ApplyMoves(other, true);
                }
                else
                {
                    //Random one goes first. Implement later.
                    await MessageHandler.SendMessage(main.Location, "Yo programming is hard. Ghub wins.");
                    user.Char.ExitCombat();
                    otherUser.Char.ExitCombat();
                }
            }
            else
            {
                await MessageHandler.AttackInvalid(main.Location, GetUserOfInstance(main));
            }
        }

        public static async Task ApplyMoves(CombatInstance instance, bool firstLoop)
        {
            var other = GetOppositeInstance(instance);
            var user = GetUserOfInstance(instance);
            var otherUser = GetUserOfInstance(other);

            //instance hits other
            await MessageHandler.UseMove(instance.Location, instance.ActiveMon, "Tackle");
            instance.EnemyMon.TakeDamage(10);
            other.ActiveMon.CurrentHP = instance.EnemyMon.CurrentHP;
            await MessageHandler.TakesDamage(instance.Location, instance.EnemyMon, "10");

            if(instance.EnemyMon.CurrentHP <= 0)
            {
                await MessageHandler.FaintWinner(instance.Location, user, instance.EnemyMon);
                instance.EnemyMon.Fainted = true;
                user.Char.ExitCombat();
                otherUser.Char.ExitCombat();
                return;
            }
            else if(firstLoop)
            {
                await ApplyMoves(other, false);
                instance.CombatPhase = 1;
                other.CombatPhase = 1;
                await MessageHandler.AttackStepText(instance.Location);
            }
            
        }

        public static UserAccount GetUserOfInstance(CombatInstance instance)
        {
            return UserHandler.GetUser(instance.ThisPlayer);
        }

        public static CombatInstance GetOppositeInstance(CombatInstance instance)
        {
            return UserHandler.GetUser(instance.OtherPlayer).Char.Combat;
        }

        public static bool PlayerHasValidParty(CombatInstance instance)
        {
            var user = GetUserOfInstance(instance);

            var count = 0;
            foreach(BasicMon mon in user.Char.Party)
            {
                if(mon.Fainted)
                {
                    count++;
                }
            }
            if(count == user.Char.Party.Count || user.Char.Party.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}