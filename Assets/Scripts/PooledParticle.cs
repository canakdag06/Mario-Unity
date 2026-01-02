using UnityEngine;

public class PooledParticle : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        ps.Play();
        Invoke(nameof(Deactivate), ps.main.duration);
    }

    private void Deactivate()
    {
        ParticlePool.Instance.ReturnToPool(gameObject);
    }
}
