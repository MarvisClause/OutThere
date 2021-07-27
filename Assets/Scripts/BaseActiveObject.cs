using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract object for all interactable objects in game
public abstract class BaseActiveObject : MonoBehaviour
{
    #region Variables

    // Object renderers
    private Renderer[] _renderers;
    // Variables for boundaries analyze
    private bool _isWrappingX = false;
    private bool _isWrappingY = false;
    #endregion

    #region Unity

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckForConstraints();
    }

    #endregion

    #region Methods

    // Checking if object flew out of camera view
    protected void CheckForConstraints()
    {
        // Checking object visibility
        bool isVisible = CheckRenderers();
        if (isVisible)
        {
            _isWrappingX = false;
            _isWrappingY = false;
            return;
        }
        if (_isWrappingX && _isWrappingY)
        {
            return;
        }
        // Getting camera component
        Camera cam = Camera.main;
        // Getting view port position of camera
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);
        // Checking if object has flown out of boundaries
        Vector3 newPosition = transform.position;
        if (!_isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            _isWrappingX = true;
        }
        if (!_isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            _isWrappingY = true;
        }
        // Changing position
        transform.position = newPosition;
    }

    // Checking for renderers visibility
    private bool CheckRenderers()
    {
        foreach (var renderer in _renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }

        // Otherwise, the object is invisible
        return false;
    }

    #endregion
}
