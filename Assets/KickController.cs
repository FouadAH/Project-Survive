using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickController : MonoBehaviour
{
    public DamageArea damageArea;
    public float kickDamage = 10;
    public Animator animator;
    public InputProvider inputProvider;

    private void OnEnable()
    {
        inputProvider.KickEvent += OnKick;
    }

    private void OnDisable()
    {
        inputProvider.KickEvent -= OnKick;
    }

    public void OnKick()
    {
        damageArea.damageSource.damageValue = kickDamage;
        animator.Play("KickState");
        StartCoroutine(ConeCast());
    }

    IEnumerator ConeCast()
    {
        yield return new WaitForEndOfFrame();

        damageArea.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        damageArea.gameObject.SetActive(false);
    }
}
