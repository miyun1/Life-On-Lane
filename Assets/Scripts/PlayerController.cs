using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public float moveSpeed = 5f;

    [Header("Aiming")]
    public Camera mainCamera;

    [Header("Shooting")]
    public GameObject damageOrbPrefab;
    public GameObject healOrbPrefab;
    public Transform shootPoint;
    public float projectileForce = 15f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;

    Vector3 movement;
    Vector3 mouseWorldPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- Movement Input ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        movement = new Vector3(h, 0, v).normalized;

        // --- Animation: Walk/Idle ---
        animator.SetBool("isWalking", movement.magnitude > 0.1f);

        // --- Aiming ---
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            mouseWorldPosition = ray.GetPoint(enter);
            Vector3 lookDir = (mouseWorldPosition - transform.position).normalized;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.01f)
                transform.forward = lookDir;
        }

        // --- Shooting ---
        if (Input.GetMouseButtonDown(0)) // Left click: Damage orb
        {
            Shoot(damageOrbPrefab);
        }
        else if (Input.GetMouseButtonDown(1)) // Right click: Heal orb
        {
            Shoot(healOrbPrefab);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot(GameObject orbPrefab)
    {
        animator.SetTrigger("Shoot");
        GameObject orb = Instantiate(orbPrefab, shootPoint.position, Quaternion.identity);
        Vector3 shootDir = (mouseWorldPosition - shootPoint.position).normalized;
        Rigidbody orbRb = orb.GetComponent<Rigidbody>();
        if (orbRb != null)
        {
            orbRb.velocity = shootDir * projectileForce;
        }
    }
}
