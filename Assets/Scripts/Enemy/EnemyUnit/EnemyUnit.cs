using System.Collections.Generic;
using Audio;
using Core.QFrameWork;
using GameAbilitySystem.Buff;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.Manager;
using Pathfinding.AStar;
using QFramework;
using UnityEngine;

namespace Enemy.EnemyUnit
{
    public class EnemyUnit : BaseController
    {
        public float Speed = 100.0f;
        
        public Vector2 Direction;
        private Rigidbody2D Rigidbody2D;
        private BuffState mBuffState;
        

        public Transform Target;
        private AStar mAStar;
        private List<Node> mRetList;
        
        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            mAStar =  FindObjectOfType<AStar>();
            mBuffState = GetComponent<BuffState>();
            mBuffState.SetHpProperty(50);
        }

        private void OnDrawGizmos()
        {
            if (mRetList != null && mRetList.Count > 0)
            {
                foreach (var node in mRetList)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * 0.5f);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!Rigidbody2D) return;
            
            if (!Target)
            {
                Direction = Vector2.zero;
                Rigidbody2D.velocity = Vector2.zero;
                return;
            }

            var list = mAStar.FindPath(transform.position, Target.position);
            mRetList = list;
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                var node = list[i];
                if (node.IsWalkable == false)
                {
                    continue;
                }
                var value = (node.WorldPosition - transform.position).normalized;
                Direction = new Vector2(value.x, value.y);
                //Debug.Log(Direction);
                Rigidbody2D.velocity = Direction * Speed;
                return;
                //var value = (Target.position - transform.position).normalized;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag.Equals("Player")) return;
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.EnemyBeHit);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                var playerBuff = other.gameObject.GetComponent<BuffState>();
                if (playerBuff)
                {
                    this.GetSystem<IDamageSystem>().CreateDamage(mBuffState, playerBuff, new Damage(2));
                }
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            //Debug.Log("OnCollisionStay2D");
        }
    }
}
