using UnityEngine;

[System.Serializable]
public class EnemyData : LivingEntitiesData
{
    public bool IsDead;

    public EnemyData(AIController Enemy) : base(Enemy)
    {
        IsDead = Enemy.GetDead();
        //Debug.Log("Weapons On Enemy " + NumOfWeapons);
    }
}
