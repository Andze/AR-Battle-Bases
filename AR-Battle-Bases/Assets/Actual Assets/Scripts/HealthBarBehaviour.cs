using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBehaviour : MonoBehaviour
{
    private int maxHP = 100;
    private float originalX = 1.0f;

    private void Start()
    {
        originalX = transform.GetChild(0).localScale.x;
    }

    public void UpdateVisuals(int value)
    {
        transform.GetChild(0).localScale = new Vector3((originalX / (float)maxHP) * (float)value, originalX, originalX);
    }
}
