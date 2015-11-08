using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light PointLight;
    public float MinIntensity = 0.5f;
    public float MaxIntensity = 0.7f;
    public float FlickeringSpeed = 20f;
	
	void Update ()
    {
        float range = MaxIntensity - MinIntensity;
        float AverageIntensity = MinIntensity + range / 2;

        PointLight.intensity = Random.Range(MinIntensity, MaxIntensity);
    }
}
