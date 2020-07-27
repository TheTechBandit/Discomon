using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Storage.Implementations;

namespace DiscomonProject
{
    public static class CombatHandler2
    {
        public static readonly string filepath;
        private static Dictionary<int, CombatInstance2> _dic;
        private static JsonStorage _jsonStorage;

        static CombatHandler2()
        {
            System.Console.WriteLine("Loading Combat Instances...");
            
            //Access JsonStorage to load user list into memory
            filepath = "MonGameCore/Combat/CombatInstances2";

            _dic = new Dictionary<int, CombatInstance2>();
            _jsonStorage = new JsonStorage();

            foreach(KeyValuePair<int, CombatInstance2> entry in _jsonStorage.RestoreObject<Dictionary<int, CombatInstance2>>(filepath))
            {
                _dic.Add(entry.Key, (CombatInstance2)entry.Value);
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} combat instances.");
        }

        public static void SaveInstances()
        {
            System.Console.WriteLine("Saving combat...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void StoreInstance(int key, CombatInstance2 inst)
        {
            if (_dic.ContainsKey(key))
            {
                _dic[key] = inst;
                return;
            }

            _dic.Add(key, inst);

            SaveInstances();
        }

        public static CombatInstance2 GetInstance(int key)
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
            Dictionary<ulong, CombatInstance2> emptyDic = new Dictionary<ulong, CombatInstance2>();
            emptyDic.Add(0, new CombatInstance2());

            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static async Task ParseMoveSelection(UserAccount user, int movenum)
        {
            var inst = GetInstance(user.Char.CombatId);

            if(inst.CombatPhase != 2)
            {
                await MessageHandler.AttackInvalid(inst.Location, user);
            }
            else
            {
                var monnum = user.Char.MoveScreenNum;
                user.Char.ActiveMons[monnum].SelectedMove = user.Char.ActiveMons[monnum].ActiveMoves[movenum];
                await MessageHandler.SendDM(user.UserId, $"Selected **{user.Char.ActiveMons[monnum].SelectedMove.Name}**!");

                List<BasicMon> targets = user.Char.ActiveMons[monnum].SelectedMove.DetermineValidTargets(inst, user.Char.ActiveMons[monnum]);
                if(user.Char.ActiveMons[monnum].SelectedMove.DoesMoveRequireTargetingMenu(targets))
                {
                    await MessageHandler.TargetingScreen(user.UserId);
                }
                else
                {
                    user.Char.ActiveMons[monnum].SelectedMove.Targets = targets;
                    
                    user.Char.MoveScreenNum++;
                    if(user.Char.MoveScreenNum > inst.GetTeam(user).MultiNum-1)
                    {
                        user.Char.MoveScreenNum = 0;
                    }
                    else
                    {
                        if(user.Char.ActiveMons[monnum].BufferedMove == null)
                        {
                            await MessageHandler.MoveScreenNew(user.UserId);
                        }
                        else
                        {
                            user.Char.MoveScreenNum++;
                            if(user.Char.MoveScreenNum > inst.GetTeam(user).MultiNum-1)
                            {
                                user.Char.MoveScreenNum = 0;
                            }
                            else
                                await MessageHandler.MoveScreenNew(user.UserId);
                        }
                    }
                }
                await inst.ResolvePhase();
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

        public static void EndCombat(CombatInstance2 inst)
        {
            _dic.Remove(inst.Teams[0].Members[0].Char.CombatId);
            SaveInstances();
        }

    }
}