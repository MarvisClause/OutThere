using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidEnemy : BaseActiveObject
{
    [SerializeField] private int _enemySpeed;
    private Vector2 _screenBounds;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // Getting screen bounds
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        // Moving in random direction, when instantiated
        _objectRigidbody.AddForce(new Vector2(Random.Range(-_enemySpeed, _enemySpeed), -_enemySpeed), ForceMode2D.Impulse);
        // Spawning object in specific height
        transform.position = new Vector2(Random.Range(0, _screenBounds.x), _screenBounds.y + 2);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
