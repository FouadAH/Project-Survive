using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public float scaleTarget = 9;
    public float scaleDuration = 16;

    private CircleLayout circleLayout;
    private float currentOffset;

    private void Start()
    {
        circleLayout = GetComponent<CircleLayout>();
    }

    private void Update()
    {
        circleLayout.offset = Mathf.Lerp(circleLayout.offset, currentOffset, 0.5f);
    }

    [ContextMenu("Animate")]
    public void AnimateCrosshair()
    {
        currentOffset = 12;
        circleLayout.transform.DOScale(Vector3.one * scaleTarget, scaleDuration).OnComplete(() =>
        {
            circleLayout.transform.DOScale(Vector3.one, scaleDuration);
            currentOffset = 9;
        });
    }

    public void AnimateCrosshair_Hit()
    {
        currentOffset = 15;
        circleLayout.transform.DOScale(Vector3.one * scaleTarget, scaleDuration).OnComplete(() =>
        {
            circleLayout.transform.DOScale(Vector3.one, scaleDuration);
            currentOffset = 9;
        });
    }

    public void AnimateCrosshair_HitWeakSpot()
    {
        currentOffset = 20;
        ColorCrosshair(Color.red);
        circleLayout.transform.DOScale(Vector3.one * scaleTarget, scaleDuration).OnComplete(() =>
        {
            ColorCrosshair(Color.white);
            circleLayout.transform.DOScale(Vector3.one, scaleDuration);
            currentOffset = 9;
        });
    }

    public void ColorCrosshair(Color color)
    {
        foreach(Transform crosshair in circleLayout.transform)
        {
            crosshair.GetComponent<Image>().color = color;
        }
    }
}
