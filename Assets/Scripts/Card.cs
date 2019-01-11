using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{
    public string title;
    public int damage;
    public float reload;
    
}
