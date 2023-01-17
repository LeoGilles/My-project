using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleShield : MonoBehaviour
{
    [SerializeField]
    private BulletTarget shieldTarget;

    // Update is called once per frame
    void Update()
    {
        float scale = (shieldTarget.health / shieldTarget.maxHealth) * 0.4f;
        transform.localScale = new Vector3(scale,scale,scale);
    }
}
