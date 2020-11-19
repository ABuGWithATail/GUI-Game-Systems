using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlatform : MonoBehaviour
{
    private PlayerStats playerStats;
    public Collider playerCol;
    public float Damage = 2;

    // I thought this script would work easy peasy, no worries, but collision is fun.

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == playerCol)
        {
            playerStats.DealDamageOverTime(2);
            Debug.Log(" Damage");
        }
    }
}
