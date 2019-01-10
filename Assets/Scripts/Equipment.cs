using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Equipment : MonoBehaviour
{
    public enum Slot { Head, Torso, Waist, Hand, Feet };
    public static int numSlots = 5;

    public Slot slot;
    public Card card;
    
}
