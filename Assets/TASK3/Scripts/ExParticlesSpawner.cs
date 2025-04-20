using AxGrid.Base;
using AxGrid.Model;
using Slots;
using UnityEngine;

public class ExParticlesSpawner : MonoBehaviourExtBind
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystemRenderer particlesRenderer;

    private static int mainTexId = Shader.PropertyToID("_MainTex");

    private MaterialPropertyBlock materialPropertyBlock;


    [OnStart]
    private void StartThis()
    {
        materialPropertyBlock = new();
    }


    [Bind(Names.PARTICLES_START_REQUEST)]
    private void StartParticles(Fruit fruit)
    {
        if (fruit.Type == Fruits.None || fruit.Texture == null) return;

        particlesRenderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetTexture(mainTexId, fruit.Texture);
        particlesRenderer.SetPropertyBlock(materialPropertyBlock);
        particles.Play();
    }


    [Bind(Names.PARTICLES_STOP_REQUEST)]
    private void StopParticles() => particles.Stop();


    private void Reset()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        particlesRenderer = particles.GetComponent<ParticleSystemRenderer>();
    }
}
