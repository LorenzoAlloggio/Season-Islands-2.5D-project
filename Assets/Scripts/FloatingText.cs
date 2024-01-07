using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public Vector3 Offset = new Vector3(0f, 0.1f, 0f);
    public float DestroyTime = 3f;
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition += Offset;

        Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y);
        Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z);
    }
}
