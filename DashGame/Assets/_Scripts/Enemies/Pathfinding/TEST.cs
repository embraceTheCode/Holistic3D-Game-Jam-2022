using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private void Awake()
    {
        //Debug.Log(Physics2D.OverlapBox(new Vector2(13- 30.5f, 44 - 40.5f), new Vector2(0.1f, 0.1f), 0f, LayerMask.NameToLayer("Wall")).gameObject.layer.ToString());
    }
}
