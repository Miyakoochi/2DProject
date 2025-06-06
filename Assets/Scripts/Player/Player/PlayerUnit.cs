﻿using System.Collections.Generic;
using Command;
using Common;
using Core;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using GameAbilitySystem.Buff;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.Skill;
using GameAbilitySystem.Buff.Unit;
using InputSystem;
using NetWorkSystem;
using ObjectPool;
using Pathfinding;
using Pathfinding.Scent;
using Player.Player.Data;
using Player.PlayerManager;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Player
{
    public class PlayerUnit : IController, ICanController
    {
        public PlayerInput PlayerInput;
        public Rigidbody2D Rigidbody;
        public Animator Animator;
        public SpriteRenderer SpriteRenderer;
        public CapsuleCollider2D Collider2D;
        public BuffState PlayerBuff;
        
        private float MoveSpeed;
        
        public Vector2 Direction { get; set; }

        public Vector2 LastMoveDirection { get; set; }

        private float AttackAngleValue;
        private bool IsAttack = false;

        private float MoveAngle;
        private float LastMoveAngle;
        
        public PlayerUnit()
        {
            Owner = new GameObject("PlayerUnit");

            SpriteRenderer = Owner.AddComponent<SpriteRenderer>();
            SpriteRenderer.sortingLayerName = "Unit";

            PlayerInput = Owner.AddComponent<PlayerInput>();
            //PlayerInput.actions = playerModel.PlayerInputAction;

            Rigidbody = Owner.AddComponent<Rigidbody2D>();
            Rigidbody.gravityScale = 0.0f;

            Animator = Owner.AddComponent<Animator>();
            //Animator.runtimeAnimatorController = playerModel.PlayerAnimator;
            
            //MoveSpeed = playerModel.MoveSpeed;
            PlayerBuff = Owner.AddComponent<BuffState>();
            
            //固定为 100 血
            var hp = PlayerBuff.Property.MaxHp;
            hp.BaseValue = 100.0f;
            var property = PlayerBuff.Property;
            property.MaxHp = hp;
            PlayerBuff.Property = property;
            
            this.GetSystem<IUnitSystem>().AddSkill(PlayerBuff, "Skill_Fire");
            this.GetSystem<IUnitSystem>().AddUnit(PlayerBuff);
            
        }
        
        public PlayerUnit(PlayerDataModel playerModel)
        {
            Owner = new GameObject("PlayerUnit")
            {
                tag = "Player"
            };

            SpriteRenderer = Owner.AddComponent<SpriteRenderer>();
            SpriteRenderer.sortingLayerName = SortingLayerName.PlayerLayer;

            PlayerInput = Owner.AddComponent<PlayerInput>();
            PlayerInput.actions = playerModel.PlayerInputAction;

            Rigidbody = Owner.AddComponent<Rigidbody2D>();
            Rigidbody.gravityScale = 0.0f;
            Rigidbody.freezeRotation = true;

            Animator = Owner.AddComponent<Animator>();
            Animator.runtimeAnimatorController = playerModel.PlayerAnimator;

            Collider2D = Owner.AddComponent<CapsuleCollider2D>();
            Collider2D.size = new Vector2(0.7f, 1.4f);

            PlayerBuff = Owner.AddComponent<BuffState>();
            
            //暂时固定为 100 血
            PlayerBuff.SetHpProperty(100);
            
            MoveSpeed = playerModel.MoveSpeed;
            
            this.GetSystem<IUnitSystem>().AddSkill(PlayerBuff, "Skill_Fire");
            this.GetSystem<IUnitSystem>().AddUnit(PlayerBuff);
            
            Owner.layer = LayerMask.NameToLayer("Player");
        }
        
        public void SetPlayerUnitPosition(Vector3 position)
        {
            Owner.transform.position = position;
            //StartAddOdor();
        }

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int LastFaceAngle = Animator.StringToHash("LastFaceAngle");
        private static readonly int FaceAngle = Animator.StringToHash("FaceAngle");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackAngle = Animator.StringToHash("AttackAngle");
        
        public void UpdateAnimator()
        {
            Animator?.SetFloat(LastFaceAngle, MoveAngle);
            Animator?.SetFloat(FaceAngle, LastMoveAngle);
            Animator?.SetFloat(Speed, Direction.normalized.magnitude);
            Animator?.SetBool(Attack, IsAttack);
            Animator?.SetFloat(AttackAngle, AttackAngleValue);
        }
        
        public void UpdateVelocity()
        {
            Rigidbody.velocity = Direction * (Time.fixedDeltaTime * MoveSpeed);
        }
        
        public async void OnUnitFire()
        {
            IsAttack = true;
            var attack = this.GetSystem<IUnitSystem>().CastSkill(PlayerBuff, "Skill_Fire");
            if (attack)
            {
                this.SendCommand<FireCameraShake>();
            }
        }

        public void OnUnitStop()
        {
            Stop();
        }

        public void OnUnitMove(Vector2 direction)
        {
            Move(direction);
        }
        

        private void Move(Vector2 direction)
        {
            //TODO 不直接处理数据 而是从能力出发，如果有这个能力再通过能力处理
            Direction = direction;
            LastMoveDirection = direction;
            MoveAngle = Vector2.SignedAngle(Vector2.right, Direction);
            LastMoveAngle = Vector2.SignedAngle(Vector2.right, LastMoveDirection);
            if (MoveAngle >= 135 || MoveAngle <= -135)
            {
                SpriteRenderer.flipX = true;
            }
            else
            {
                SpriteRenderer.flipX = false;
            }
        }

        private void Stop()
        {
            Direction = Vector2.zero;
            if (LastMoveAngle > 135 || LastMoveAngle < -135)
            {
                SpriteRenderer.flipX = true;
            }
            else
            {
                SpriteRenderer.flipX = false;
            }
        }

        #region Chasing

        /// <summary>
        /// 添加气味
        /// </summary>
        private async void StartAddOdor()
        {
            while (this.GetModel<IPathModel>().IsScentPathing == true)
            {
                await UniTask.WaitForSeconds(0.2f);
                if(Direction.Equals(Vector2.zero) == true) continue;
                
                var scent = this.GetSystem<IObjectPoolSystem>().GetObject<ScentObject>();
                scent.Self.transform.position = Owner.transform.position;
            }
        }

        #endregion

        #region NetWork


        #endregion
        
        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }

        public GameObject Owner { get; set; }
    }
}