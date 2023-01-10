using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectiles : MonoBehaviour
{
    private Rigidbody bulletRigidBody;
    [SerializeField] 
    private Transform sfx;
    public float speed = 40f;
    public float damage = 25;
    public string shooter;
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidBody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        BulletTarget target = other.collider.GetComponent<BulletTarget>();
        if (target != null)
        {
            target.health -= damage;
           
        }
        else
        {
           
        }
        Instantiate(sfx, transform.position, Quaternion.identity);   
        Destroy(gameObject);
    }
}