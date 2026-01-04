using UnityEngine;

public class PooledParticle : MonoBehaviour
{
    private ParticleSystem ps;
    private ObjectPool pool;


    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        pool = PoolManager.Instance.particlePool;
    }

    private void OnEnable()
    {
        ps.Play();
        Invoke(nameof(Deactivate), ps.main.duration);
    }

    private void Deactivate()
    {
        pool.ReturnToPool(gameObject);
    }
}
