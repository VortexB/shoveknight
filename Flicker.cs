using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class Flicker : MonoBehaviour
{
    
    new Light2D light;
    [SerializeField]float minIntensity = 0f;
    [SerializeField]float maxIntensity = 1f;
    [SerializeField] int smoothing = 5;
    Queue<float> smoothQueue;
    float lastSum = 0;
    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        light = GetComponent<Light2D>();     
    }

    void Update()
    {
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;
        light.intensity = lastSum / smoothQueue.Count;
    }

}
