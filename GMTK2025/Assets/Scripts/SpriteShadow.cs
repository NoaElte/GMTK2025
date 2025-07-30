using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    [SerializeField]
    private bool castShadows = true;
    [SerializeField]
    private bool receiveShadows = true;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.receiveShadows = receiveShadows;
        renderer.shadowCastingMode = castShadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
    }
}
