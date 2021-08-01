using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : BaseActiveObject
{
    [SerializeField]protected float _projectileSpeed;
    private bool wasShot; 

    private void OnEnable()
    {
        wasShot = false; 
    }
    protected override void Update()
    {
        base.Update(); 
        if(wasShot==false)
        {
            _objectRigidbody.velocity = transform.up * _projectileSpeed;
            wasShot = true;
        }
    }
    protected override void Hit(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
