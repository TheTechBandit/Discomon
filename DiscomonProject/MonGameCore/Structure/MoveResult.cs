using System.Collections.Generic;

namespace DiscomonProject
{
    public class MoveResult
    {
        public BasicMove Move { get; set; }
        //public BasicMon Owner { get; set; }
        //public BasicMon Enemy { get; set; }
        public int EnemyDmg { get; set; }
        public int EnemyHeal { get; set; }
        public int SelfDmg { get; set; }
        public int SelfHeal { get; set; }
        //public List<int> EnemyStatChanges { get; set; }
        //public List<int> SelfStatChanges { get; set; }
        public List<string> StatChangeMessages { get; set; }
        public List<string> StatusMessages { get; set; }
        public double ChanceToHit { get; set; }
        public bool Hit { get; set; }
        public bool Miss { get; set; }
        public bool Fail { get; set; }
        public string FailText { get; set; }
        public bool Crit { get; set; }
        public bool SuperEffective { get; set; }
        public bool NotEffective { get; set; }
        public bool Immune { get; set; }
        public double Mod { get; set; }
        public double ModCrit { get; set; }
        public double ModRand { get; set; }
        public double ModType { get; set; }

        public MoveResult()
        {
            Move = null;
            //Owner = null;
            //Enemy = null;
            EnemyDmg = 0;
            EnemyHeal = 0;
            SelfDmg = 0;
            SelfHeal = 0;
            /*
            EnemyStatChanges = new List<int>()
            {
                0, 0, 0, 0, 0, 0
            };
            SelfStatChanges = new List<int>()
            {
                0, 0, 0, 0, 0, 0
            };
            */
            StatChangeMessages = new List<string>();
            StatusMessages = new List<string>();
            ChanceToHit = 0.0;
            Hit = true;
            Miss = false;
            Fail = false;
            FailText = "But it failed!";
            Crit = false;
            SuperEffective = false;
            NotEffective = false;
            Immune = false;
            Mod = 1.0;
            ModCrit = 1.0;
            ModRand = 1.0;
            ModType = 1.0;
        }

        public override string ToString()
        {
            string str = "";
            str += $"Move: {Move.Name}\nEnemyDmg: {EnemyDmg}\nEnemyHeal: {EnemyHeal}";
            str += $"\nSelfDmg: {SelfDmg}\n SelfHeal: {SelfHeal}\nHit: {Hit}\nMiss: {Miss}\nFail: {Fail}\nCrit: {Crit}";
            str += $"\nSuperEffective: {SuperEffective}\nNotEffective: {NotEffective}\nImmune: {Immune}";
            str += $"\nModCrit: {ModCrit}\nModRand: {ModRand}\nModType: {ModType}";
            return str;
        }
    }
}