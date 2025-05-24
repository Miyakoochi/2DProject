using Core.QFrameWork;
using CsLua;
using GameAbilitySystem.Buff.Buff;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.Unit
{
    public class UnitManager : BaseController
    {
        private IUnitModel mUnitModel;
        private ILuaSystem mLuaSystem;

        private void Awake()
        {
            mUnitModel = this.GetModel<IUnitModel>();
            mLuaSystem = this.GetSystem<ILuaSystem>();
        }

        private void FixedUpdate()
        {
            int index = 0;
        
            //处理存储的所有单位
            while (index < mUnitModel.Units.Count)
            {
                var unit = mUnitModel.Units[index];
                //处理死亡单位
                if (unit.State.IsDead == true)
                {
                    //TODO
                    //unit.UnitDie?.Invoke();
                    mUnitModel.Units.Remove(unit);
                    //_unitEntities.Remove(unit.Id);
                    continue;
                }

                //处理无敌时间
                if (unit.State.ImmuneTime >= 0)
                {
                    var state = unit.State;
                    state.ImmuneTime = unit.State.ImmuneTime - Time.fixedDeltaTime;
                    unit.State = state;
                }

                //处理单位身上Buff
                int buffIndex = 0;
                while (buffIndex < unit.Buffs.Count)
                {
                    var buff = unit.Buffs[buffIndex];
                    buff.TimeElapsed += Time.fixedDeltaTime;
                    if (buff.Permanent == false)
                    {
                        buff.TimeRemaining -= Time.fixedDeltaTime;
                    }

                    //处理Buff Tick
                    if (buff.DataModel is { TickTime: >= 0, OnTick: not null })
                    {
                        if (buff.TimeElapsed % buff.DataModel.TickTime == 0)
                        {
                            var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnTick>(buff.DataModel.OnTick);
                            function?.Invoke(buff);
                            buff.Ticked++;
                        }
                    }

                    //buff过期或者被移除则去除Buff
                    //Finish 考虑每次移除只移除一层 Buff剩余时长设置很长 然后在Tick的时候去减少栈层数即可
                    if (buff.TimeRemaining <= 0 || buff.Stack <= 0)
                    {
                        var function = mLuaSystem.GetLuaFunctionToDelegate<BuffOnRemoved>(buff.DataModel.OnRemoved);
                        function?.Invoke(buff);
                        unit.Buffs.RemoveAt(buffIndex);
                        this.GetSystem<IUnitSystem>().CalculateBuffProperty(unit);
                        continue;
                    }
                
                    buffIndex++;
                }
            
                foreach (var skill in unit.Skills)
                {
                    skill.BuiltInCooldown -= Time.fixedDeltaTime;
                }
            
                index++;
            }

        }
    }
}