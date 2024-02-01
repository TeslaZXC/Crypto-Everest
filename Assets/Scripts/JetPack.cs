using System.Collections;
using UnityEngine;

public class JetPack : Item
{
    [SerializeField] private ParticleSystem[] trusterParticles;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void DetectItem()
    {
        if (Player.instance.isFly) return;

        Player.instance.Fly(this);
    }

    public void EnableTrusters(bool option)
    {
        foreach (var particle in trusterParticles)
        {
            if (option)
            {
                audioSource.Play();
                particle.Play();
            }
            else
            {
                audioSource.Stop();
                particle.Stop();
            }
                
        }
    }
}