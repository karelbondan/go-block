using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stuff", menuName = "ScriptableObjects/Enemy Stuff")]
public class EnemyScriptableObject : ScriptableObject
{

    [Header("Enemy settings")]
    [SerializeField][Tooltip("The maximum health of enemies")]
    private int health = 100;
    [SerializeField][Tooltip("The maximum speed of enemy")]
    private int speed = 10;
    [SerializeField][Tooltip("Damage dealt to player when player is hit")]
    private int damage = 10;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
