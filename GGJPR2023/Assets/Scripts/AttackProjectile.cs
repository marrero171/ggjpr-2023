using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpriteRenderer))]
public class AttackProjectile : MonoBehaviour
{
    public Sprite refSprite;
    public int DamageAmmount = 0;
    public Actor attacker;
    public Vector3 direction;
    public float Speed = 8;
    Rigidbody rigidbody;
    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void LateUpdate() => transform.LookAt(Camera.main.transform);
    void OnEnable()
    {
        renderer.sprite = refSprite;
        renderer.flipX = direction.x > 0 ? true : false;
        renderer.flipY = direction.z > 0 ? true : false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(direction * Speed, ForceMode.Impulse);
    }

    IEnumerator OnCollisionEnter(Collision other)
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}