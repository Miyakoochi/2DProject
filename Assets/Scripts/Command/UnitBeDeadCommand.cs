using GameAbilitySystem.Buff;
using GameAbilitySystem.Buff.Unit;
using QFramework;

namespace Command
{
    public class UnitBeDeadCommand : AbstractCommand
    {
        private IGameAbilityUnit AttackUnit;
        private IGameAbilityUnit DeadUnit;
        
        public UnitBeDeadCommand(IGameAbilityUnit attackUnit, IGameAbilityUnit deadUnit)
        {
            AttackUnit = attackUnit;
            DeadUnit = deadUnit;
        }
        
        protected override void OnExecute()
        {
            this.SendEvent<UnitBeDeadEvent>(new UnitBeDeadEvent()
            {
                DeadUnit = DeadUnit
            });
            
            this.SendEvent<UnitKillEvent>(new UnitKillEvent()
            {
                AttackUnit = AttackUnit
            });
        }
    }
}