using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;

namespace DiscomonProject
{
    public class CombatInstance2
    {
        public ContextIds Location { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserAccount> Players { get; set; }
        public Environment Environment { get; set; }
        /*
        CombatPhase determines what step of combat is currently happening
        PHASES:
        -1- Combat starting (Mons sending out)
        0- Mon(s) sent out, (activate abilities such as Intimidate)
        1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
        2- [INPUT] Awaiting both players to choose attacks
        3- Pre-Attack phase (activate any abilities that trigger before attacks)
        4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
        5- Post-hit on MonOne/MonTwo (activate rough skin, check for death. If dead, go to phase 6)
        6- Death phase. Test if either mon died. If any did, faint them and send out the next mon if any (activate any abilities that trigger when an opponent faints such as Beast Boost)
        7- Return to phase 1

        notes: possibly remove 6, combine 3 & 4
         */
        public int CombatPhase { get; set; }
        public bool IsPvP { get; set; }
        public bool IsOneOnOne { get; set; }
        public bool CombatOver { get; set; }
        //public bool IsSoloMultiBattle { get; set; }
        //public bool IsMultiBattle { get; set; }

        public CombatInstance2()
        {
            
        }

        public CombatInstance2(ContextIds loc, UserAccount one, UserAccount two)
        {
            Location = loc;
            Teams = new List<Team>();
            Players = new List<UserAccount>();

            CreateNewTeam().AddMember(one);
            CreateNewTeam().AddMember(two);

            Environment = new Environment(true);
            IsPvP = true;
            IsOneOnOne = true;
            CombatOver = false;
            CombatPhase = -1;

            one.Char.InCombat = true;
            one.Char.InPvpCombat = true;
            one.Char.CombatRequest = 0;
            one.Char.CombatId = CombatHandler2.NumberOfInstances();
            one.Char.InCombatWith = two.UserId;

            two.Char.InCombat = true;
            two.Char.InPvpCombat = true;
            two.Char.CombatRequest = 0;
            two.Char.CombatId = CombatHandler2.NumberOfInstances();
            two.Char.InCombatWith = one.UserId;
        }

        public Team CreateNewTeam()
        {
            Team newteam = new Team(true);
            Teams.Add(newteam);
            newteam.TeamNum = Teams.Count;
            
            return newteam;
        }

        public void AddTeam(Team team)
        {
            Teams.Add(team);
            team.TeamNum = Teams.Count;
        }

        public Team GetTeam(UserAccount player)
        {
            foreach(Team t in Teams)
            {
                if(t.Members.Contains(player))
                    return t;
            }

            return null;
        }

        public Team GetTeam(BasicMon mon)
        {
            return GetTeam(GetPlayer(mon));
        }

        public UserAccount GetPlayer(BasicMon mon)
        {
            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    if(u.Char.HasMonInParty(mon))
                        return u;
                }
            }

            return null;
        }

        public UserAccount GetOtherPlayer(UserAccount player)
        {
            if(IsOneOnOne)
            {
                if(player.UserId == Teams[0].Members[0].UserId)
                    return Teams[0].Members[0];
                else
                    return Teams[1].Members[0];
            }

            return null;
        }

        public List<BasicMon> GetAllMons()
        {
            List<BasicMon> mon = new List<BasicMon>();
            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    foreach(BasicMon m in u.Char.ActiveMons)
                    {
                        mon.Add(m);
                    }
                }
            }

            return mon;
        }

        public List<BasicMon> GetAllEnemies(BasicMon mon)
        {
            List<BasicMon> mons = new List<BasicMon>();
            foreach(Team t in Teams)
            {
                if(!t.Members.Contains(UserHandler.GetUser(mon.OwnerID)))
                {
                    foreach(UserAccount u in t.Members)
                    {
                        foreach(BasicMon m in u.Char.ActiveMons)
                        {
                            mons.Add(m);
                        }
                    }
                }
            }

            return mons;
        }

        public void ExitCombatAll()
        {
            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    u.Char.ExitCombat();
                }
            }
        }

        public void EndCombat()
        {
            CombatOver = true;
            ExitCombatAll();
            CombatHandler2.EndCombat(this);
        }

        public async Task StartCombat()
        {
            //Make sure all users have a valid party
            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    if(!u.Char.HasLivingParty())
                    {
                        ExitCombatAll();
                        await MessageHandler.SendMessage(Location, $"Duel canceled! {u.Mention} does not have a valid party.");
                        return;
                    }
                }
            }

            //Send out all mons. (NEEDS EDITING- COULD BE MORE THAN 2 TEAMS)
            await MessageHandler.SendMessage(Location, $"The battle between Team {Teams[0].TeamName} and Team {Teams[1].TeamName} will now begin!");

            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    for(int i = 0; i < t.MultiNum; i++)
                    {
                        if(u.Char.HasUsableMon())
                        {
                            BasicMon sentMon = u.Char.FirstUsableMon();
                            u.Char.ActiveMons.Add(sentMon);
                            sentMon.IsCombatActive = true;
                            //sentMon.OnEnteredCombat(this);
                            await MessageHandler.SendEmbedMessage(Location, $"{u.Mention} sends out **{sentMon.Nickname}**!", MonEmbedBuilder.FieldMon(sentMon));
                        }
                    }
                }
            }

            CombatPhase = 0;
            await ResolvePhase();
        }

        public async Task CheckTeamElimination(Team team)
        {
            var teamCount = 0;
            var teamDead = 0;
            foreach(UserAccount player in team.Members)
            {
                if(!player.Char.HasLivingParty())
                    teamDead++;
                teamCount++;
            }

            if(teamCount == teamDead)
            {
                if(team.Members.Count > 1)
                    await MessageHandler.SendMessage(Location, $"Team {team.TeamName} has been eliminated!");
                team.Eliminated = true;
                await CheckBattleVictory();
            }
        }

        public async Task CheckBattleVictory()
        {
            Team team = Teams[0];
            var validTeams = 0;
            foreach(Team t in Teams)
            {
                if(!t.Eliminated)
                {
                    team = t;
                    validTeams++;
                }
            }

            if(validTeams == 1)
            {
                await MessageHandler.SendMessage(Location, $"Team {team.TeamName} is victorious!");
                EndCombat();
            }
            else if(validTeams == 0)
            {
                await MessageHandler.SendMessage(Location, $"All teams were eliminated. The battle ended in a draw.");
                EndCombat();
            }
        }

        public async Task ResolvePhase()
        {
            //PHASE 0
            if(CombatPhase == 0)
            {
                Console.WriteLine($"1");
                //0- check if mons need sending out, activate abilities such as Intimidate
                //Enter this phase at combat start or when a Mon faints.
                
                //Decides whether or not to continue combat
                foreach(Team t in Teams)
                {
                    Console.WriteLine($"2");
                    foreach(UserAccount u in t.Members)
                    {
                        Console.WriteLine($"3");
                        //If any mons are fainted, remove them from ActiveMons
                        for(int i = u.Char.ActiveMons.Count-1; i >= 0; i--)
                        {
                            Console.WriteLine($"4");
                            if(u.Char.ActiveMons[i].Fainted)
                            {
                                Console.WriteLine($"5");
                                u.Char.ActiveMons.RemoveAt(i);
                            }
                        }
                        Console.WriteLine($"6");
                        //If the number of ActiveMons is less than the number allowed active, send out the first usable mon if a usable mon exists until the number of ActiveMons equals the number allowed active
                        if(u.Char.ActiveMons.Count < t.MultiNum)
                        {
                            Console.WriteLine($"7");
                            for(int i = u.Char.ActiveMons.Count; i < t.MultiNum; i++)
                            {
                                Console.WriteLine($"8");
                                if(u.Char.HasUsableMon())
                                {
                                    Console.WriteLine($"9");
                                    BasicMon sentMon = u.Char.FirstUsableMon();
                                    Console.WriteLine($"10");
                                    u.Char.ActiveMons.Add(sentMon);
                                    Console.WriteLine($"11");
                                    sentMon.IsCombatActive = true;
                                    Console.WriteLine($"12");
                                    //sentMon.OnEnteredCombat(this);
                                    await MessageHandler.SendEmbedMessage(Location, $"{u.Mention} sends out **{sentMon.Nickname}**!", MonEmbedBuilder.FieldMon(sentMon));
                                    Console.WriteLine($"13");
                                }
                            }
                        }
                        Console.WriteLine($"14");

                        if(u.Char.ActiveMons.Count == 0 || !u.Char.HasLivingParty())
                        {
                            Console.WriteLine($"15");
                            u.Char.CombatEliminated = true;
                            Console.WriteLine($"16");
                            await MessageHandler.SendMessage(Location, $"{u.Mention} has run out of mons!");
                            Console.WriteLine($"17");
                            await CheckTeamElimination(GetTeam(u));
                            if(CombatOver)
                                return;
                            Console.WriteLine($"18");
                        }
                    }
                }

                Console.WriteLine($"19");
                if(CombatOver)
                    return;
                //Continue to the next phase if combat has not ended
                else
                {
                    Console.WriteLine($"20");
                    CombatPhase = 1;
                    await ResolvePhase();
                }
            }
            //PHASE 1
            else if(CombatPhase == 1)
            {
                Console.WriteLine($"21");
                //1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
                foreach(Team t in Teams)
                {
                    Console.WriteLine($"22");
                    foreach(UserAccount u in t.Members)
                    {
                        var allbuffered = true;
                        foreach(BasicMon m in u.Char.ActiveMons)
                        {
                            if(m.BufferedMove == null)
                                allbuffered = false;
                            else
                                m.SelectedMove = m.BufferedMove;
                        }
                            
                        if(allbuffered)
                        {
                            u.Char.CombatMovesEntered = true;
                        }
                        else
                        {
                            u.Char.CombatMovesEntered = false;
                            await MessageHandler.FightScreenNew(u.UserId);
                        }
                    }
                    Console.WriteLine($"25");
                }

                /*Send fight screens to both players and progress to Phase 2 (wait for input)
                if(inst.PlayerOne.Char.ActiveMon.BufferedMove == null)
                    await MessageHandler.FightScreen(inst.PlayerOne.UserId);
                else
                {
                    inst.PlayerOne.Char.ActiveMon.SelectedMove = inst.PlayerOne.Char.ActiveMon.BufferedMove;
                }

                if(inst.PlayerTwo.Char.ActiveMon.BufferedMove == null)
                    await MessageHandler.FightScreen(inst.PlayerTwo.UserId);
                else
                {
                    inst.PlayerTwo.Char.ActiveMon.SelectedMove = inst.PlayerTwo.Char.ActiveMon.BufferedMove;
                }*/

                CombatPhase = 2;
            }
            //PHASE 2
            else if(CombatPhase == 2)
            {
                //2- Awaiting input from players
                var unready = 0;
                Console.WriteLine($"26");
                foreach(Team t in Teams)
                {
                    Console.WriteLine($"27");
                    foreach(UserAccount u in t.Members)
                    {
                        Console.WriteLine($"28");
                        foreach(BasicMon m in u.Char.ActiveMons)
                        {
                            Console.WriteLine($"29");
                            if(m.SelectedMove == null)
                            {
                                Console.WriteLine($"30");
                                unready++;
                                Console.WriteLine($"31");
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine($"32");

                var complete = true;
                foreach(Team t in Teams)
                {
                    Console.WriteLine($"33");
                    foreach(UserAccount u in t.Members)
                    {
                        Console.WriteLine($"34");
                        var finished = true;
                        foreach(BasicMon m in u.Char.ActiveMons)
                        {
                            Console.WriteLine($"35");
                            if(m.SelectedMove == null)
                            {
                                Console.WriteLine($"36");
                                finished = false;
                                complete = false;
                                break;
                            }
                        }
                        Console.WriteLine($"37");
                        if(finished && !u.Char.CombatMovesEntered && unready != 0)
                        {
                            Console.WriteLine($"38");
                            await MessageHandler.AttackEnteredTextNew(Location, u, unready);
                            Console.WriteLine($"39");
                            u.Char.CombatMovesEntered = true;
                            Console.WriteLine($"40");
                        }
                    }
                }
                Console.WriteLine($"41");

                //If all players have entered a move, apply moves
                if(complete)
                {
                    Console.WriteLine($"42");
                    CombatPhase = 3;
                    Console.WriteLine($"43");
                    await ResolvePhase();
                }
            }
            //PHASE 3
            else if(CombatPhase == 3)
            {
                Console.WriteLine($"45");
                //Pre-attack phase, activate necessary abilities
                if(!Environment.Clear)
                    await MessageHandler.SendMessage(Location, Environment.WeatherToString());
                Console.WriteLine($"46");

                CombatPhase = 4;
                await ResolvePhase();
            }
            //PHASE 4
            else if(CombatPhase == 4)
            {
                Console.WriteLine($"47");
                var mons = GetAllMons();
                Console.WriteLine($"48");

                //Sort all mons by their speed
                for (int j = mons.Count-1; j > 0; j--) 
                {
                    Console.WriteLine($"49");
                    for (int i = 0; i < j; i++) 
                    {
                        Console.WriteLine($"50");
                        if (mons[i].GetAdjustedSpd() > mons[i+1].GetAdjustedSpd())
                        {
                            Console.WriteLine($"51");
                            var temp = mons[i];
                            Console.WriteLine($"52");
                            mons[i] = mons[i+1];
                            Console.WriteLine($"53");
                            mons[i+1] = temp;
                            Console.WriteLine($"54");
                        }
                        Console.WriteLine($"55");
                    }
                    Console.WriteLine($"56");
                }

                Console.WriteLine($"57");
                mons.Reverse();
                Console.WriteLine($"58");
                
                //4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
                foreach(BasicMon mon in mons)
                {
                    Console.WriteLine($"59");
                    if(!mon.Fainted)
                        await ApplyMoves(mon);
                    Console.WriteLine($"60");
                }
                Console.WriteLine($"61");

                CombatPhase = 6;
                await ResolvePhase();
            }
            else if(CombatPhase == 6)
            {
                Console.WriteLine($"62");
                //6- Post turn phase. Reset necessary data

                //Mon post-turn ticks and resets
                foreach(BasicMon mon in GetAllMons())
                {
                    Console.WriteLine($"63");
                    //Check if sleepy, if they are, fall asleep
                    if(mon.SleepyCheck())
                        await MessageHandler.SendMessage(Location, $"{mon.Nickname} has been afflicted with *Sleep*");
                    Console.WriteLine($"64");
                    var damageType = mon.StatusDamage();
                    Console.WriteLine($"65");
                    if(damageType == "Burn")
                        await MessageHandler.SendMessage(Location, $"{mon.Nickname} takes burn damage!");
                    Console.WriteLine($"66");
                    if(mon.Status.Flinching)
                        mon.Status.Flinching = false;
                    Console.WriteLine($"67");
                    if(mon.SelectedMove != null)
                        if(!mon.SelectedMove.Buffered)
                            mon.SelectedMove.WipeTargets();
                    Console.WriteLine($"68");
                    mon.SelectedMove = null;
                    Console.WriteLine($"69");
                }

                CombatPhase = 0;
                await ResolvePhase();
            }

        }

        public async Task ApplyMoves(BasicMon mon)
        {
            Console.WriteLine($"70 - {mon.SelectedMove.Targets.Count}");
            List<MoveResult> results = mon.SelectedMove.ApplyMove(this, mon, mon.SelectedMove.Targets);
            Console.WriteLine($"71 - ResultsCount: {results.Count} SelectedMove: {mon.SelectedMove.Name}");
            var allmons =  GetAllMons();
            Console.WriteLine($"72");
            foreach(BasicMon m in allmons)
            {
                Console.WriteLine($"73");
                mon.UpdateStats();
                Console.WriteLine($"74");
            }

            Console.WriteLine($"75");
            await MessageHandler.SendMessage(Location, $"{UserHandler.GetUser(mon.OwnerID).Char.Name}'s {mon.Nickname} uses {mon.SelectedMove.Name}!");
            Console.WriteLine($"76");
            bool allFail = true;
            bool allMiss = true;
            Console.WriteLine($"77");
            foreach(MoveResult result in results)
            {
                Console.WriteLine($"78");
                if(!result.Fail)
                    allFail = false;
                Console.WriteLine($"79");
                if(!result.Miss)
                    allMiss = false;
                Console.WriteLine($"80");
            }
            Console.WriteLine($"81");

            string addon = "";
            Console.WriteLine($"82");
            foreach(MoveResult result in results)
            {
                Console.WriteLine($"83");
                if(results.Count <= 1)
                {
                    Console.WriteLine($"84");
                    if(result.Fail)
                        addon += $"\n{result.FailText}";
                    else if(result.Miss)
                        addon += $"\nBut it missed!";
                    else
                    {
                        Console.WriteLine($"85");
                        foreach(string message in result.Messages)
                            addon += $"\n{message}";
                        Console.WriteLine($"86");
                        if(result.SuperEffective)
                            addon += $"\nIt's **super effective**!";
                        Console.WriteLine($"87");
                        if(result.NotEffective)
                            addon += $"\nIt's **not very effective**!";
                        Console.WriteLine($"88");
                        if(result.Immune)
                            addon += $"\nIt has **no effect**!";
                        Console.WriteLine($"89");
                        if(result.Crit)
                            addon += $"\n**Critical Hit**!";
                        Console.WriteLine($"90");
                        foreach(string statchange in result.StatChangeMessages)
                            addon += $"\n{statchange}";
                        Console.WriteLine($"91");
                        foreach(string status in result.StatusMessages)
                            addon += $"\n{status}";
                        Console.WriteLine($"92");
                        if(result.Move.Type.Type.Equals("Fire") && result.Target.Status.Frozen)
                        {
                            Console.WriteLine($"93");
                            addon += $"\n{result.Target.Nickname} was unthawed by {result.Owner.Nickname}'s fire type move!";
                            Console.WriteLine($"94");
                            result.Target.Status.Frozen = false;
                            Console.WriteLine($"95");
                        }
                        Console.WriteLine($"96");
                    }
                }
                else
                {
                    Console.WriteLine($"97");
                    if(allFail)
                    {
                        addon += $"\n{result.FailText}";
                        break;
                    }
                    else if(allMiss)
                    {
                        addon += $"\nBut it missed!";
                        break;
                    }
                    else if(result.Fail)
                        addon += $"\n{result.FailText}";
                    else if(result.Miss)
                        addon += $"\nIt missed {result.Target}!";
                    else
                    {
                        foreach(string message in result.Messages)
                            addon += $"\n{message}";
                        if(result.SuperEffective)
                            addon += $"\nIt's **super effective** against {result.Target}!";
                        if(result.NotEffective)
                            addon += $"\nIt's **not very effective** against {result.Target}!";
                        if(result.Immune)
                            addon += $"\nIt has **no effect** against {result.Target}!";
                        if(result.Crit)
                            addon += $"\n**Critical Hit** against {result.Target}!";
                        foreach(string statchange in result.StatChangeMessages)
                            addon += $"\n{statchange}";
                        foreach(string status in result.StatusMessages)
                            addon += $"\n{status}";
                        if(result.Move.Type.Type.Equals("Fire") && result.Target.Status.Frozen)
                        {
                            addon += $"\n{result.Target.Nickname} was unthawed by {result.Owner.Nickname}'s fire type move!";
                            result.Target.Status.Frozen = false;
                        }
                    }
                }
                Console.WriteLine($"98");
                if(result.EnemyDmg > 0 && result.Target != null)
                    await MessageHandler.UseMoveNew(Location, result.Target, addon);
                else
                    await MessageHandler.SendMessage(Location, addon);
                Console.WriteLine($"99");
            }
            Console.WriteLine($"100");

            await PostAttackPhase(mon, mon.SelectedMove.Targets, results);
            Console.WriteLine($"122");

            //Console.WriteLine(result1.ToString());

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
            //*/
        }

        //Post attack phase logic. Target is the list of mon who were hit.
        public async Task PostAttackPhase(BasicMon moveUser, List<BasicMon> targets, List<MoveResult> results)
        {
            Console.WriteLine($"101");
            //5- Post attack mini-phase. Check for death/on-hit abilities
            var userOwner = UserHandler.GetUser(moveUser.OwnerID);
            Console.WriteLine($"102");

            var swapout = false;
            Console.WriteLine($"103");
            BasicMon swapMon = null;
            Console.WriteLine($"104");
            foreach(MoveResult result in results)
            {
                Console.WriteLine($"105");
                if(result.Swapout != null)
                {
                    Console.WriteLine($"106");
                    swapout = true;
                    Console.WriteLine($"107");
                    swapMon = result.Swapout;
                }
                Console.WriteLine($"108");

                if(result.Hit && result.EnemyDmg > 0)
                {
                    Console.WriteLine($"109");
                    //WHEN HIT LOGIC HERE
                }
                Console.WriteLine($"110");
            }

            if(moveUser.CurrentHP <= 0)
            {
                Console.WriteLine($"111");
                moveUser.Fainted = true;
                Console.WriteLine($"112");
                await MessageHandler.Faint(Location, userOwner, moveUser);
                Console.WriteLine($"113");
                moveUser.ExitCombat();
                Console.WriteLine($"114");
            }
            else if(swapout)
            {
                Console.WriteLine($"115");
                moveUser.ExitCombat();
                Console.WriteLine($"116");
                var index = userOwner.Char.ActiveMons.IndexOf(moveUser);
                Console.WriteLine($"117");
                userOwner.Char.ActiveMons[index] = swapMon;
                Console.WriteLine($"118");
                userOwner.Char.ActiveMons[index].OnEnteredCombat(this);
            }
            Console.WriteLine($"119");

            foreach(BasicMon t in targets)
            {
                if(t.CurrentHP <= 0)
                {
                    Console.WriteLine($"120");
                    await FaintMon(t);
                    Console.WriteLine($"121");
                }
            }
        }

        public async Task FaintMon(BasicMon mon)
        {
            var user = UserHandler.GetUser(mon.OwnerID);
            mon.Fainted = true;
            await MessageHandler.Faint(Location, user, mon);
            user.Char.ActiveMons.Remove(mon);
            mon.ExitCombat();
        }

    }
}