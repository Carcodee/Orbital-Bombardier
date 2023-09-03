using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerScriptableObject", order = 1)]
public class PlayerData : ScriptableObject
{
    public int health;
    public int suppliesOnCharge;
    public float weight;
    public float speed;
    public float boostSpeed;
    public float turnForce;
    public float heightSpeedForce;
    public float boostTime;
    public float boostRefreshTime;
    public float cadence;
    public float damageDone;
    public int currentPoints;
    public int totalPoints;

}


