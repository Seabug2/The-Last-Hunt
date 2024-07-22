using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController_Shoot : MonoBehaviour
{
    public float arrow_damage;

    [SerializeField] private AudioSource audio_s;
    [SerializeField] private Rigidbody arrow_r;
    [SerializeField] private AnimalController_Shoot animal;

    private void Start()
    {
        //TryGetComponent(out audio_s);
        TryGetComponent(out arrow_r);
        arrow_damage = 25f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            Debug.Log("Animal Hit");
            gameObject.transform.SetParent(collision.gameObject.transform);
            animal = GetComponentInParent<AnimalController_Shoot>();
            arrow_r.velocity = Vector3.zero;
            arrow_r.isKinematic = true;
            animal.TakeDamage(arrow_damage);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Ground");
            arrow_r.velocity = Vector3.zero;
            arrow_r.isKinematic = true;
            Destroy(gameObject, 3f);
        }
    } 
}
