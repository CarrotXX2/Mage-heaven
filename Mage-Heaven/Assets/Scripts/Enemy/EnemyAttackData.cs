using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyAttackData : ScriptableObject
{
    public string name; // Name of the attack 
    
    public AnimationClip clip; // Animation that gets played 
    public int weight; // How likely the enemy is gonna use this attack 
}
