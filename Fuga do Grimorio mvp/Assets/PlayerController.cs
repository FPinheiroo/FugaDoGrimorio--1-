using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forceUp = 15f;
    private Rigidbody2D rb;
    private bool isFlying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detecta se o botão esquerdo do mouse, touch ou espaço está pressionado
        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space))
        {
            isFlying = true;
        }
        else
        {
            isFlying = false;
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            // Aplica força para cima enquanto houver input
            rb.AddForce(Vector2.up * forceUp);
        }
    }
}