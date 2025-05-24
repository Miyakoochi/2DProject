using ObjectPool;
using UnityEngine;

public class EnemyUnitSpwaner : GameObjectPool
{
    [Range(0.5f, 100.0f)]
    public float spawnerTime = 3.0f;

    public int MaxTurns = 3; 

    private int CurrentTurn = 0;
    private float mSpawnerTimeElapsed = 0.01f;
    

    private void Start()
    {
        CurrentTurn = MaxTurns;
    }

    private void FixedUpdate()
    {
        if(CurrentTurn <= 0) return;
        
        mSpawnerTimeElapsed += Time.fixedDeltaTime;
        
        if (!(mSpawnerTimeElapsed >= spawnerTime)) return;

        var enemy = GetObject();
        enemy.transform.position = transform.position;
    
        CurrentTurn--;
        mSpawnerTimeElapsed = 0.01f;
    }
}
