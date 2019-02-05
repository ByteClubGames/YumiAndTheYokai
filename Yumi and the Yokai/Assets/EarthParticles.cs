


using UnityEngine;

public class EarthParticles : MonoBehaviour
{
    public int maxSpells = 3;
    public int destroyTime = 5;

    // Starts a timer to destroy the particle
    void Start()
    {
        Destroy(this.gameObject, destroyTime); //Destroy timer starts on creation
    }

    private void Update()
    {
        //destroy the earliest spell when there are too many
        if (GameObject.FindGameObjectsWithTag("Earth Particle Object").Length > maxSpells)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Earth Particle Object")[0]);
        }
    }
}