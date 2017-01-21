﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    // Static (global) vars. Don't use globals, except for the GLOBAL GAME JAM!!!
    public static List<Battery> __global_Batteries = new List<Battery>();

    // Events.
    public delegate void ChargeValueChangedHandler(object sender, float chargeValue);
    public event ChargeValueChangedHandler ChargeValueChangedEvent;

    // Public vars.
    public float ChargeValue { get { return chargeValue; } }

    // Private vars.
    private CrystalInfo crystalInfo;

    private float chargeValue = 1f;

    // Unity callbacks.
    void Start()
    {
        if (!__global_Batteries.Contains(this))
        {
            __global_Batteries.Add(this);
        }

        crystalInfo = GetComponent<CrystalInfo>();
    }

    // Public methods.
    public void Charge(float percentage)
    {
        chargeValue += percentage;

        // Don't go over 100%.
        chargeValue = Mathf.Min(chargeValue, 1f);

        ChargeValueChangedEvent(this, chargeValue);
    }

    public void Drain(float percentage)
    {
        chargeValue -= percentage;

        // Don't go under 0%.
        chargeValue = Mathf.Max(chargeValue, 0f);

        //Debug.Log(chargeValue);

        ChargeValueChangedEvent(this, chargeValue);
    }

    public bool IsFriendlyWith(PlayerInfo somePlayerInfo)
    {
        //PlayerInfo playerInfo = otherObject.GetComponent<PlayerInfo>();

        if (somePlayerInfo)
        {
            Debug.Log(somePlayerInfo.gameObject.name + " has index " + somePlayerInfo.PlayerIndex + ", crystal has index " + crystalInfo.PlayerIndex);
        }

        if (somePlayerInfo
            && somePlayerInfo.PlayerIndex == crystalInfo.PlayerIndex)
        {
            return true;
        }

        return false;
    }

    // Static (global) methods.
    public static Battery FindNearestBatteryWithinDistance(Vector3 position, float distance)
    {
        float shortestDistance = Mathf.Infinity;

        Battery result = null;

        foreach (Battery b in __global_Batteries)
        {
            float d = Vector3.Distance(b.transform.position, position);

            if (d < distance 
                && d < shortestDistance)
            {
                shortestDistance = d;

                result = b;
            }
        }

        return result;
    }
}
