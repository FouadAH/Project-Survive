using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputProvider))]
public class EnemyController : Entity
{
    public override void Update()
    {
        base.Update();
    }

    [ContextMenu("Jump")]
    public void Jump()
    {
        inputProvider.OnJump();
    }

    [ContextMenu("Start Hover")]
    public void StartHover()
    {
        inputProvider.OnStartHover();
    }

    [ContextMenu("Stop Hover")]
    public void StopHover()
    {
        inputProvider.OnStopHover();
    }
}
