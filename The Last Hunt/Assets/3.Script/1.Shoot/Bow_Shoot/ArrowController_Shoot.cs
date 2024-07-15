using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController_Shoot : MonoBehaviour
{
    public float arrow_damage;
    public bool isHit;
    //public LineRenderer lineRenderer;

    [SerializeField] private AudioSource audio_s;
    [SerializeField] private Rigidbody arrow_r;
    [SerializeField] private AnimalController_Shoot animal;

    private void Start()
    {
        //TryGetComponent(out lineRenderer);
        //TryGetComponent(out audio_s);
        TryGetComponent(out arrow_r);
        //lineRenderer
        //lineRenderer.enabled = false;
        arrow_damage = 25f;
        isHit = false;
    }

    private void OnEnable()
    {
        //UIController
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
            isHit = true;
            if (animal.CurrentState == AnimalController_Shoot.WanderState.Dead)
            {
                Destroy(gameObject);
            }
            Destroy(gameObject, 3f);
        }
        else
        {
            Debug.Log("Ground");
            arrow_r.velocity = Vector3.zero;
            arrow_r.isKinematic = true;
        }
    } 
}
