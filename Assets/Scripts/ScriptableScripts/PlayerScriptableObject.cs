using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Properties", menuName = "ScriptableObjects/Player Properties")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField]
    private int baseHealth = 100;
    [SerializeField]
    private string enemyTag = "Enemy";

    public int BaseHealth
    {
        get { return baseHealth; }
        set { baseHealth = value; }
    }
    public string EnemyTag
    {
        get { return enemyTag; }
        set { enemyTag = value; }
    }

    
}
