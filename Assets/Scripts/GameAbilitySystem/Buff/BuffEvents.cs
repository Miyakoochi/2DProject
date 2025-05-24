using GameAbilitySystem.Buff.Unit;

namespace GameAbilitySystem.Buff
{
    public struct UnitBeDeadEvent
    {
        public IGameAbilityUnit DeadUnit;
    }

    public struct UnitKillEvent
    {
        public IGameAbilityUnit AttackUnit;
    }
}