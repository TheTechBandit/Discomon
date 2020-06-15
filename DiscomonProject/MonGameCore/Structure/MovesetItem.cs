namespace DiscomonProject
{
    public class MovesetItem : BasicMove
    {
        public int LearnLevel { get; set; } = 0;
        public BasicMove Move { get; set; } = null;

        public MovesetItem() :base()
        {

        }

        public MovesetItem(int level, BasicMove move) 
        {
            LearnLevel = level;
            Move = move;
        }
        
    }
}