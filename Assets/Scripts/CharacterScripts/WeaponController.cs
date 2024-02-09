using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class WeaponController : MonoBehaviour
{
    public Animator animator;

    public Transform weaponHolderPivot;
    public Transform weaponAimTransform;
    public Transform weaponNormalTransform;
    [Range(0f, 1f)]
    public float gunRotationSpeed = 0.1f;
    public GunController gun;

    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    [Range(0.5f, 1f)]
    public float aimZoomScale = 0.9f;

    [Range(0f, 1f)]
    public float aimZoomSpeed = 0.1f;

    private CharacterController characterController;
    private CinemachineRecomposer cinemachineRecomposer;
    private CinemachineImpulseSource impulseSource;
    Vector3 targetPosition;

    Vector3 targetRotation;
    Vector3 initialRotation;
    Vector3 currentRotation;

    float targetZoomScale = 1f;
    bool isAiming;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        cinemachineRecomposer = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineRecomposer>();
        initialRotation = gun.transform.rotation.eulerAngles;
    }
    private void Update()
    {
        targetPosition = (isAiming) ? weaponAimTransform.position : weaponNormalTransform.position;

        gun.transform.position = Vector3.Lerp(gun.transform.position, targetPosition, 0.5f);

        currentRotation = Vector3.Lerp(currentRotation, targetRotation, 0.1f);
        gun.transform.rotation.eulerAngles.Set(currentRotation.x, currentRotation.y, currentRotation.z);

        var rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);
        weaponHolderPivot.rotation = Quaternion.Lerp(weaponHolderPivot.rotation, rotation, gunRotationSpeed);

        if (cinemachineRecomposer == null)
        {
            cinemachineRecomposer = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineRecomposer>();
        }
        else
        {
            cinemachineRecomposer.m_ZoomScale = Mathf.Lerp(cinemachineRecomposer.m_ZoomScale, targetZoomScale, aimZoomSpeed);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                isAiming = true;
                targetZoomScale = aimZoomScale;
                targetRotation = initialRotation;
                //animator.SetBool("isAiming", true);
                break;

            case InputActionPhase.Canceled:
                isAiming = false;
                targetZoomScale = 1f;
                targetRotation = Vector3.zero;
                //animator.SetBool("isAiming", false);

                break;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            gun.Fire(characterController);
            impulseSource.GenerateImpulse();

            if(gun.gunItem.gunDataSO.canAutoFire == true)
            {
                gun.isHeld = true;
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            gun.isHeld = false;
        }
        
    }
}
