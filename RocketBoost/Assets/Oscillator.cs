using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

    [Range(0, 1)]
    [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved

    [SerializeField] float period = 2f;

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Protect agains period is 0
        if(period <= Mathf.Epsilon) { return; }

        // Set movement factor
        // If the time is 2 seconds, it would be one cycle
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; // About 6.28

        // This returns the number from -1 to 1
        float rawSinWave = Mathf.Sin(cycles * tau);

        // Now returns the number fro, 0 to 1
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPosition + offset;
    }
}
