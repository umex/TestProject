using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemySkeleton : Entity
{
    [Header("Move info")]
    [SerializeField] private float moveSpeed = 1;


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isGrounded || isWallDetected)
        {
            TurnCharacter();
        }
        rb.velocity = new Vector2(moveSpeed * facinDir, rb.velocity.y);
    }
}
