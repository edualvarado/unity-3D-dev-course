using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 vectorMovement = new Vector3(0f, 4f, 0f);
    [SerializeField] float period = 2f;

    Vector3 startingPos;
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 offset = movementFactor * vectorMovement;
        transform.position = startingPos + offset;
    }
}
