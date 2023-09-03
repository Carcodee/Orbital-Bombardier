

[System.Serializable]
public class PlayerOnSaveData
{
    public float [] pos;
    public float  [] rotation;
    
    //scriptableObject
    public int health;
    public int suppliesOnCharge;
    public float speed, boostSpeed, turnForce, heightIncrementForce, boostTime, boostRefreshTime, cadence, damageDone;
    public int currentPoints;
    public int totalPoints;
    public int enemyKilled;
    public float dstTravelled;
    public int supplieDroppedOnSpot;
    public PlayerOnSaveData ( float []pos,float []rotation,int health, int suppliesOnCharge, float speed, float boostSpeed, 
        float turnForce, float heightIncrementForce, float boostTime, float boostRefreshTime, float cadence, 
        float damageDone, int currentPoints,int totalPoints,int enemyKilled,float dstTravelled,int supplieDroppedOnSpot)
    {
        this.pos = pos;
        this.rotation = rotation;
        
        //scriptableObject
        this.health = health;
        this.suppliesOnCharge = suppliesOnCharge;
        this.speed = speed;
        this.boostSpeed = boostSpeed;
        this.turnForce = turnForce;
        this.heightIncrementForce = heightIncrementForce;
        this.boostTime = boostTime;
        this.boostRefreshTime = boostRefreshTime;
        this.cadence = cadence;
        this.damageDone = damageDone;
        this.currentPoints = currentPoints;
        this.totalPoints = totalPoints;
        this.enemyKilled = enemyKilled;
        this.dstTravelled = dstTravelled;
        this.supplieDroppedOnSpot= supplieDroppedOnSpot;
    }
    
}

[System.Serializable]
public class LevelOfDestructionData
{
    public float minValue;
    public float time;
    public LevelOfDestructionData(float minValue,float time)
    {
        this.minValue = minValue;
        this.time = time;
    }
}
[System.Serializable]
public class EnemyData
{
    public float [] pos;
    public float  [] rotation;
    public int enemyAmount;

    public EnemyData(float[] pos, float[] rotation, int enemyAmount)
    {
        this.pos = pos;
        this.rotation = rotation;
        this.enemyAmount = enemyAmount;
    }
}
