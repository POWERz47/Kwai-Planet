using UnityEngine;

public class SeedChange : MonoBehaviour
{
    public Material spaceMaterial; // Assign the material with your space shader
    public string seedPropertyName = "_Seed"; // Make sure this matches the shader's property name
    public float changeAmount = 0.001f;
    public float changeInterval = 0.1f; // Time in seconds between each change

    private float currentSeed = 0f;
    private float timer = 0f;

    void Start()
    {
        if (spaceMaterial.HasProperty(seedPropertyName))
        {
            currentSeed = spaceMaterial.GetFloat(seedPropertyName);
        }
        else
        {
            Debug.LogWarning("The material doesn't have a property called " + seedPropertyName);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeInterval)
        {
            timer = 0f;
            currentSeed += changeAmount;
            spaceMaterial.SetFloat(seedPropertyName, currentSeed);
        }
    }
}
