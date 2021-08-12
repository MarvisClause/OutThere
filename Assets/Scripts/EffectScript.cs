using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    // Disable effect after animation is finished
    public void DisableEffect()
    {
        gameObject.SetActive(false);
    }
}
