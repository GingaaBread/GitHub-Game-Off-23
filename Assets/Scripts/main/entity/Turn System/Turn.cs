using main.entity.Card_Management.Card_Data;

namespace main.entity.Turn_System
{
    public class Turn
    {
        public UnitTime InitialTime { get; }
        
        public int CurrentTurnNumber { get; set; }
        
        public UnitTime RemainingTime { get; set; }

        public Turn(UnitTime initialTime, int currentTurnNumber, UnitTime remainingTime)
        {
            InitialTime = initialTime;
            CurrentTurnNumber = currentTurnNumber;
            RemainingTime = remainingTime;
        }
    }
}