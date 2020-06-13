using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Storage.Implementations;

namespace DiscomonProject
{
    public static class CombatHandler
    {
        public static readonly string filepath;
        private static Dictionary<int, CombatInstance> _dic;
        private static JsonStorage _jsonStorage;

        static CombatHandler()
        {
            System.Console.WriteLine("Loading Combat Instances...");
            
            //Access JsonStorage to load user list into memory
            filepath = "MonGameCore/CombatInstances";

            _dic = new Dictionary<int, CombatInstance>();
            _jsonStorage = new JsonStorage();

            foreach(KeyValuePair<int, CombatInstance> entry in _jsonStorage.RestoreObject<Dictionary<int, CombatInstance>>(filepath))
            {
                _dic.Add(entry.Key, (CombatInstance)entry.Value);
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} combat instances.");
        }

        public static void SaveInstances()
        {
            System.Console.WriteLine("Saving combat...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void StoreInstance(int key, CombatInstance inst)
        {
            if (_dic.ContainsKey(key))
            {
                _dic[key] = inst;
                return;
            }

            _dic.Add(key, inst);

            SaveInstances();
        }

        public static CombatInstance GetInstance(int key)
        {
            if(!_dic.ContainsKey(key))
                throw new ArgumentException($"The provided key '{key}' wasn't found.");
            return _dic[key];
        }

        public static int NumberOfInstances()
        {
            return _dic.Count;
        }

        public static void ClearCombatData()
        {
            System.Console.WriteLine("Deleting all combat instances.");
            Dictionary<ulong, CombatInstance> emptyDic = new Dictionary<ulong, CombatInstance>();
            emptyDic.Add(0, new CombatInstance());

            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static async Task StartCombat(CombatInstance inst)
        {
            //Make sure both users have a valid party
            if(!inst.PlayerOne.Char.HasLivingParty())
            {
                inst.PlayerOne.Char.ExitCombat();
                inst.PlayerTwo.Char.ExitCombat();
                await MessageHandler.SendMessage(inst.Location, $"Duel canceled! {inst.PlayerOne.Mention} does not have a valid party.");
                return;
            }
            else if(!inst.PlayerTwo.Char.HasLivingParty())
            {
                inst.PlayerOne.Char.ExitCombat();
                inst.PlayerTwo.Char.ExitCombat();
                await MessageHandler.SendMessage(inst.Location, $"Duel canceled! {inst.PlayerTwo.Mention} does not have a valid party.");
                return;
            }

            //Send out both mons.
            await MessageHandler.SendMessage(inst.Location, $"The duel between {inst.PlayerOne.Mention} and {inst.PlayerTwo.Mention} will now begin!");

            inst.PlayerOne.Char.ActiveMon = inst.PlayerOne.Char.FirstUsableMon();
            inst.PlayerTwo.Char.ActiveMon = inst.PlayerTwo.Char.FirstUsableMon();
            
            await MessageHandler.SendEmbedMessage(inst.Location, $"{inst.PlayerOne.Mention} sends out **{inst.PlayerOne.Char.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(inst.PlayerOne.Char.ActiveMon));
            inst.PlayerOne.Char.ActiveMon.OnEnteredCombat(inst);

            await MessageHandler.SendEmbedMessage(inst.Location, $"{inst.PlayerTwo.Mention} sends out **{inst.PlayerTwo.Char.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(inst.PlayerTwo.Char.ActiveMon));
            inst.PlayerTwo.Char.ActiveMon.OnEnteredCombat(inst);

            inst.CombatPhase = 0;

            await ResolvePhase(inst);
        }

        public static async Task ParseMoveSelection(UserAccount user, int num)
        {
            var inst = GetInstance(user.Char.CombatId);
            Console.WriteLine($"{inst.CombatPhase}");

            if(user.Char.ActiveMon.SelectedMove != null)
            {
                await MessageHandler.AttackAlreadyEntered(inst.Location, user);
            }
            else if(inst.CombatPhase != 2)
            {
                await MessageHandler.AttackInvalid(inst.Location, user);
            }
            else
            {
                user.Char.ActiveMon.SelectedMove = user.Char.ActiveMon.ActiveMoves[num];
                await MessageHandler.SendDM(user.UserId, $"Selected **{user.Char.ActiveMon.SelectedMove.Name}**!");
                await ResolvePhase(inst);
            }
        }

        public static async Task ResolvePhase(CombatInstance inst)
        {
            //PHASE 0
            if(inst.CombatPhase == 0)
            {
                //0- check if mons need sending out, activate abilities such as Intimidate
                //Enter this phase at combat start or when a Mon faints.
                
                //Decides whether or not to continue combat
                var cont = true;

                //Check if PlayerOne's Mon is missing or fainted.
                if(inst.PlayerOne.Char.ActiveMon == null || inst.PlayerOne.Char.ActiveMon.Fainted)
                {
                    if(inst.PlayerOne.Char.HasLivingParty())
                    {
                        inst.PlayerOne.Char.ActiveMon = inst.PlayerOne.Char.FirstUsableMon();
                        inst.PlayerOne.Char.ActiveMon.OnEnteredCombat(inst);
                        await MessageHandler.SendEmbedMessage(inst.Location, $"{inst.PlayerOne.Mention} sends out **{inst.PlayerOne.Char.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(inst.PlayerOne.Char.ActiveMon));
                    }
                    else
                    {
                        cont = false;
                        //PLAYER IS DEFEATED LOGIC HERE
                        await MessageHandler.OutOfMonsWinner(inst.Location, inst.PlayerTwo, inst.PlayerOne);
                        EndCombat(inst);
                        return;
                    }
                }

                //Check if PlayerTwo's Mon is missing or fainted.
                if(inst.PlayerTwo.Char.ActiveMon == null || inst.PlayerTwo.Char.ActiveMon.Fainted)
                {
                    if(inst.PlayerTwo.Char.HasLivingParty())
                    {
                        inst.PlayerTwo.Char.ActiveMon = inst.PlayerTwo.Char.FirstUsableMon();
                        inst.PlayerTwo.Char.ActiveMon.OnEnteredCombat(inst);
                        await MessageHandler.SendEmbedMessage(inst.Location, $"{inst.PlayerTwo.Mention} sends out **{inst.PlayerTwo.Char.ActiveMon.Nickname}**!", MonEmbedBuilder.FieldMon(inst.PlayerTwo.Char.ActiveMon));
                    }
                    else
                    {
                        cont = false;
                        //PLAYER IS DEFEATED LOGIC HERE
                        await MessageHandler.OutOfMonsWinner(inst.Location, inst.PlayerOne, inst.PlayerTwo);
                        EndCombat(inst);
                        return;
                    }
                }

                //If neither player was defeated, continue combat
                if(cont)
                {
                    inst.CombatPhase = 1;
                    await ResolvePhase(inst);
                }
            }
            //PHASE 1
            else if(inst.CombatPhase == 1)
            {
                //1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"

                //Send fight screens to both players and progress to Phase 2 (wait for input)
                await MessageHandler.FightScreen(inst.PlayerOne.UserId);
                await MessageHandler.FightScreen(inst.PlayerTwo.UserId);
                inst.CombatPhase = 2;
            }
            //PHASE 2
            else if(inst.CombatPhase == 2)
            {
                //2- Awaiting input from players

                //Check if PlayerOne has inputted a move and PlayerTwo hasn't
                if(inst.PlayerOne.Char.ActiveMon.SelectedMove != null && inst.PlayerTwo.Char.ActiveMon.SelectedMove == null)
                {
                    await MessageHandler.AttackEnteredText(inst.Location, inst.PlayerOne);
                }
                //If not, Check if PlayerTwo has inputted a move and PlayerOne hasn't
                else if(inst.PlayerTwo.Char.ActiveMon.SelectedMove != null && inst.PlayerOne.Char.ActiveMon.SelectedMove == null)
                {
                    await MessageHandler.AttackEnteredText(inst.Location, inst.PlayerTwo);
                }

                //If both players have entered a move, apply moves
                if(inst.PlayerOne.Char.ActiveMon.SelectedMove != null && inst.PlayerTwo.Char.ActiveMon.SelectedMove != null)
                {
                    inst.CombatPhase = 3;
                    await ResolvePhase(inst);
                }
            }
            //PHASE 3
            else if(inst.CombatPhase == 3)
            {
                //Pre-attack phase, activate necessary abilities

                inst.CombatPhase = 4;
                await ResolvePhase(inst);
            }
            //PHASE 4
            else if(inst.CombatPhase == 4)
            {
                //4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
                
                if(inst.PlayerOne.Char.ActiveMon.CurStats[4] > inst.PlayerTwo.Char.ActiveMon.CurStats[4])
                {
                    await ApplyMoves(inst, inst.PlayerOne);
                }
                else if(inst.PlayerOne.Char.ActiveMon.CurStats[4] < inst.PlayerTwo.Char.ActiveMon.CurStats[4])
                {
                    await ApplyMoves(inst, inst.PlayerTwo);
                }
                else
                {
                    var rand = RandomGen.Gen.Next(2);
                    if(rand == 0)
                    {
                        await ApplyMoves(inst, inst.PlayerOne);
                    }
                    else if(rand == 1)
                    {
                        await ApplyMoves(inst, inst.PlayerTwo);
                    }
                }

                inst.CombatPhase = 6;
                await ResolvePhase(inst);
            }
            else if(inst.CombatPhase == 6)
            {
                //6- Post turn phase. Reset necessary data

                //PlayerOne Status damage
                var damageType = inst.PlayerOne.Char.ActiveMon.StatusDamage();
                if(damageType == "Burn")
                    await MessageHandler.SendMessage(inst.Location, $"{inst.PlayerOne.Char.ActiveMon.Nickname} takes burn damage!");

                //PlayerTwo Status damage
                damageType = inst.PlayerTwo.Char.ActiveMon.StatusDamage();
                if(damageType == "Burn")
                    await MessageHandler.SendMessage(inst.Location, $"{inst.PlayerTwo.Char.ActiveMon.Nickname} takes burn damage!");

                inst.PlayerOne.Char.ActiveMon.SelectedMove = null;
                inst.PlayerTwo.Char.ActiveMon.SelectedMove = null;
                inst.CombatPhase = 0;
                await ResolvePhase(inst);
            }

        }

        public static async Task ApplyMoves(CombatInstance inst, UserAccount first)
        {
            var second = inst.GetOtherPlayer(first);

            //PlayerOne's Mon attacks first
            MoveResult result1 = first.Char.ActiveMon.SelectedMove.ApplyMove(inst, first.Char.ActiveMon);
            first.Char.ActiveMon.UpdateStats();
            second.Char.ActiveMon.UpdateStats();

            string addon = "";
            //Tests whether or not the moved hit or failed.
            if(result1.Fail)
                addon += $"\n{result1.FailText}";
            else if(result1.Miss)
                addon += "\nBut it missed!";
            else
            {
                if(result1.Hit && result1.EnemyDmg > 0)
                    addon += $"\n{second.Char.ActiveMon.Nickname} takes damage!";
                if(result1.SuperEffective)
                    addon += "\nIt's **super effective**!";
                if(result1.NotEffective)
                    addon += "\nIt's **not very effective**!";
                if(result1.Immune)
                    addon += "\nIt has **no effect**!";
                if(result1.Crit)
                    addon += "\n**Critical Hit**!";
                foreach(string statchange in result1.StatChangeMessages)
                    addon += $"\n{statchange}";
                foreach(string status in result1.StatusMessages)
                    addon += $"\n{second.Char.ActiveMon.Nickname} is afflicted with {status}";
            }

            //Console.WriteLine(result1.ToString());

            await MessageHandler.UseMove(inst.Location, first.Char.ActiveMon, second.Char.ActiveMon, first.Char.ActiveMon.SelectedMove.Name, addon);

            //await DebugPrintMoveResult(first, second, result1, inst.Location);

            /* FOR VALUE TESTING
            string summ = "";
            summ += $"\nOwner/Mon: {first.Name}/{first.Char.ActiveMon.Nickname}";
            summ += $"\nLevel: {first.Char.ActiveMon.Level}";
            summ += $"\nPower: {first.Char.ActiveMon.SelectedMove.Power}";
            summ += $"\nAttack: {first.Char.ActiveMon.CurStats[1]}";
            (double mod, string mess) = first.Char.ActiveMon.ChangeAttStage(0);
            summ += $"\nAttack Stage Mod: {mod}";
            summ += $"\nAttack Modified: {(int)(first.Char.ActiveMon.CurStats[1]*mod)}";
            summ += $"\nDefense: {second.Char.ActiveMon.CurStats[2]}";
            (double mod2, string mess2) = second.Char.ActiveMon.ChangeDefStage(0);
            summ += $"\nDefense Stage Mod: {mod2}";
            summ += $"\nDefense Modified: {(int)(second.Char.ActiveMon.CurStats[2]*mod2)}";
            summ += $"\nModifier: {result1.Mod}";
            summ += $"\nCrit: {result1.ModCrit}";
            summ += $"\nRandom: {result1.ModRand}";
            summ += $"\nType Eff: {result1.ModType}";
            summ += $"\nDamage: {result1.EnemyDmg}";
            await MessageHandler.SendMessage(inst.Location, $"**Move Summary:**{summ}");
            */

            await PostAttackPhase(inst, second.Char.ActiveMon, result1);

            //PlayerTwo's Mon attacks second
            if(!second.Char.ActiveMon.Fainted)
            {
                MoveResult result2 = second.Char.ActiveMon.SelectedMove.ApplyMove(inst, second.Char.ActiveMon);
                first.Char.ActiveMon.UpdateStats();
                second.Char.ActiveMon.UpdateStats();

                addon = "";
                //Tests whether or not the moved hit or failed.
                if(result2.Fail)
                    addon += $"\n{result2.FailText}";
                else if(result2.Miss)
                    addon += "\nBut it missed!";
                else
                {
                    if(result2.Hit && result2.EnemyDmg > 0)
                        addon += $"\n{first.Char.ActiveMon.Nickname} takes damage!";
                    if(result2.SuperEffective)
                        addon += "\nIt's **super effective**!";
                    if(result2.NotEffective)
                        addon += "\nIt's **not very effective**!";
                    if(result2.Immune)
                        addon += "\nIt has **no effect**!";
                    if(result2.Crit)
                        addon += "\n**Critical Hit**!";
                    foreach(string statchange in result2.StatChangeMessages)
                        addon += $"\n{statchange}";
                    foreach(string status in result2.StatusMessages)
                        addon += $"\n{first.Char.ActiveMon.Nickname} is afflicted with {status}";
                }

                //Console.WriteLine(result2.ToString());

                await MessageHandler.UseMove(inst.Location, second.Char.ActiveMon, first.Char.ActiveMon, second.Char.ActiveMon.SelectedMove.Name, addon);

                //await DebugPrintMoveResult(second, first, result2, inst.Location);

                /* FOR VALUE TESTING
                string summ2 = "";
                summ2 += $"\nOwner/Mon: {second.Name}/{second.Char.ActiveMon.Nickname}";
                summ2 += $"\nLevel: {second.Char.ActiveMon.Level}";
                summ2 += $"\nPower: {second.Char.ActiveMon.SelectedMove.Power}";
                summ2 += $"\nAttack: {second.Char.ActiveMon.CurStats[1]}";
                (double mod3, string mess3) = second.Char.ActiveMon.ChangeAttStage(0);
                summ2 += $"\nAttack Stage Mod: {mod3}";
                summ2 += $"\nAttack Modified: {(int)(second.Char.ActiveMon.CurStats[1]*mod3)}";
                summ2 += $"\nDefense: {first.Char.ActiveMon.CurStats[2]}";
                (double mod4, string mess4) = first.Char.ActiveMon.ChangeDefStage(0);
                summ2 += $"\nDefense Stage Mod: {mod4}";
                summ2 += $"\nDefense Modified: {(int)(first.Char.ActiveMon.CurStats[2]*mod4)}";
                summ2 += $"\nModifier: {result2.Mod}";
                summ2 += $"\nCrit: {result2.ModCrit}";
                summ2 += $"\nRandom: {result2.ModRand}";
                summ2 += $"\nType Eff: {result2.ModType}";
                summ2 += $"\nDamage: {result2.EnemyDmg}";
                await MessageHandler.SendMessage(inst.Location, $"**Move Summary:**{summ2}");
                */

                await PostAttackPhase(inst, second.Char.ActiveMon, result2);
            }
        }

        public static async Task DebugPrintMoveResult(UserAccount user, UserAccount otherUser, MoveResult result, ContextIds loc)
        {
            string summ = "";
            summ += $"\nOwner/Mon: {user.Name}/{user.Char.ActiveMon.Nickname}";
            summ += $"\nLevel: {user.Char.ActiveMon.Level}";
            summ += $"\nSpeed: {user.Char.ActiveMon.CurStats[4]}";
            summ += $"\nPower: {user.Char.ActiveMon.SelectedMove.Power}";
            summ += $"\nAccuracy: {user.Char.ActiveMon.SelectedMove.Accuracy}";
            summ += $"\nAttack: {user.Char.ActiveMon.CurStats[1]}";
            (double mod, string mess) = user.Char.ActiveMon.ChangeAttStage(0);
            summ += $"\nAttack Stage Mod: {mod}";
            summ += $"\nAttack Modified: {(int)(user.Char.ActiveMon.CurStats[1]*mod)}";
            summ += $"\nAccuracy Stage: {user.Char.ActiveMon.GetAccStage()}";
            summ += $"\nOpponent Speed: {otherUser.Char.ActiveMon.CurStats[4]}";
            summ += $"\nDefense: {otherUser.Char.ActiveMon.CurStats[2]}";
            (double mod2, string mess2) = otherUser.Char.ActiveMon.ChangeDefStage(0);
            summ += $"\nDefense Stage Mod: {mod2}";
            summ += $"\nDefense Modified: {(int)(otherUser.Char.ActiveMon.CurStats[2]*mod2)}";
            summ += $"\nEvasion Stage: {otherUser.Char.ActiveMon.GetEvaStage()}";
            summ += $"\nChance To Hit: {result.ChanceToHit}";
            summ += $"\nModifier: {result.Mod}";
            summ += $"\nCrit: {result.ModCrit}";
            summ += $"\nRandom: {result.ModRand}";
            summ += $"\nType Eff: {result.ModType}";
            summ += $"\nDamage: {result.EnemyDmg}";
            summ += $"\nMiss: {result.Miss}";
            summ += $"\nHit: {result.Hit}";
            await MessageHandler.SendMessage(loc, $"**Move Summary:**{summ}");
        }

        public static void EndCombat(CombatInstance inst)
        {
            _dic.Remove(inst.PlayerOne.Char.CombatId);

            inst.PlayerOne.Char.ExitCombat();
            inst.PlayerTwo.Char.ExitCombat();
            CombatHandler.SaveInstances();
        }

        //Post attack phase logic. Mon is the mon who was hit.
        public static async Task PostAttackPhase(CombatInstance inst, BasicMon mon, MoveResult result)
        {
            //5- Post attack mini-phase. Check for death/on-hit abilities
            var owner = UserHandler.GetUser(mon.OwnerID);
            var enemy = inst.GetOtherMon(mon);
            var enemyOwner = inst.GetOtherPlayer(UserHandler.GetUser(enemy.OwnerID));

            if(result.Hit && result.EnemyDmg > 0)
            {
                //WHEN HIT LOGIC HERE
            }

            if(enemy.CurrentHP <= 0)
            {
                enemy.Fainted = true;
                await MessageHandler.Faint(inst.Location, enemyOwner, enemy);
                enemy.ResetStatStages();
            }
            if(mon.CurrentHP <= 0)
            {
                mon.Fainted = true;
                await MessageHandler.Faint(inst.Location, owner, mon);
                mon.ResetStatStages();
            }
        }

    }
}