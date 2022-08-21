using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Values\n------------------------------")]
    [SerializeField] int iDamage;
    [SerializeField] int iSpeed;
    [SerializeField] int iDestroyTime;

    [Header("Body\n------------------------------")]
    [SerializeField] Rigidbody _rigidBody;

    [Header("Effect\n------------------------------")]
    [SerializeField] GameObject _hitEffect;

    private void Start()
    {
        _rigidBody.velocity = (GameManager._instance._player.transform.position - transform.position).normalized * iSpeed;
        Destroy(gameObject, iDestroyTime);
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.layer == LayerMask.NameToLayer("ChainFence") || _collider.gameObject.CompareTag("PowerUp"))
        {
            return;
        }

        if( _collider.GetComponent<IDamageable>() != null)
        {
            IDamageable isDamageable = _collider.GetComponent<IDamageable>();
            isDamageable.TakeDamage(iDamage);
        }

        Instantiate(_hitEffect, transform.position, _hitEffect.transform.rotation);
        Destroy(gameObject);
    }
}
