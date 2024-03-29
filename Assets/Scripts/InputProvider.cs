using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputProvider : MonoBehaviour
{
    public UnityAction<Vector3> MoveEvent;
    public UnityAction JumpEvent;
    public UnityAction HealEvent_Start;
    public UnityAction HealEvent_Stop;

    public UnityAction KickEvent;

    public UnityAction StartHoverEvent;
    public UnityAction StopHoverEvent;

    public UnityAction DashEvent;
    public UnityAction StartSprintEvent;
    public UnityAction StopSprintEvent;

    public bool isHealing;
    public bool isAiming;

    public Vector3 movementVector { get; private set; }

    public virtual void OnMove(Vector3 vector2)
    {
        movementVector = vector2;
        //MoveEvent?.Invoke(vector2);
    }

    public virtual void OnJump()
    {
        JumpEvent?.Invoke();
    }

    public virtual void OnHeal_Start()
    {
        HealEvent_Start?.Invoke();
    }
    public virtual void OnHeal_Stop()
    {
        HealEvent_Stop?.Invoke();
    }
    public virtual void OnKick()
    {
        KickEvent?.Invoke();
    }
    public virtual void OnDash()
    {
        DashEvent?.Invoke();
    }

    public virtual void OnStartHover()
    {
        StartHoverEvent?.Invoke();
    }

    public virtual void OnStopHover()
    {
        StopHoverEvent?.Invoke();
    }

    public virtual void OnStartSprintEvent()
    {
        StartSprintEvent?.Invoke();
    }

    public virtual void OnStopSprintEvent()
    {
        StopSprintEvent?.Invoke();
    }
}
