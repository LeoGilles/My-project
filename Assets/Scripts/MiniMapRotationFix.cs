using System.Collections;
using System.Collections.Generic;
using UltimateXR.Extensions.Unity;
using UnityEngine;

public class MiniMapRotationFix : MonoBehaviour
{
    [SerializeField]
    private GameObject anchor;
    [SerializeField]
    private GameObject triangle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.SetPositionAndRotation(gameObject.transform.position, new Quaternion() { eulerAngles = new Vector3() { x = 90, y = anchor.transform.rotation.eulerAngles.y, z = 0 } });
        triangle.transform.SetPositionAndRotation(triangle.transform.position, new Quaternion() { eulerAngles = new Vector3() {x=-90,y= anchor.transform.rotation.eulerAngles.y,z=0 } });
    }
}
