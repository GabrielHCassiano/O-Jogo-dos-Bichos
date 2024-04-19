using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    private ParticleSystem dustParticle;
    // Start is called before the first frame update
    void Start()
    {
        dustParticle = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDustParticle()
    {
        dustParticle.Play();
    }
}
