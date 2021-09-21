using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Comet : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private float mass;

    private void FixedUpdate()
    {
        if (playerController == null) return;
        playerController.AddGravity(transform.position, mass);
    }
}