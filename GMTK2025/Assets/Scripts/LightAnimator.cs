using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float duration = 1;
    [SerializeField]
    private bool startOnAwake;
    [SerializeField]
    private bool loop;

    private Light light;
    private bool isAnimationGoing;

    public bool IsAnimationGoing => isAnimationGoing;

    private void Awake()
    {
        light = GetComponent<Light>();

        if (startOnAwake && !isAnimationGoing)
            Animate();
    }

    public void Animate()
    {
        StartCoroutine(AnimateOverTime());
    }

    private IEnumerator AnimateOverTime()
    {
        isAnimationGoing = true;

        float time = 0;

        while (time < duration || loop)
        {
            float t = time / duration;
            float curveValue = animationCurve.Evaluate(t % duration);
            light.intensity = curveValue;
            time += Time.deltaTime;
            yield return null;
        }

        isAnimationGoing = false;
    }
}
