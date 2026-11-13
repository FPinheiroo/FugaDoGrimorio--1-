
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Movimenta pela fisica 2D
            Vector2 novaPosicao =
                rb.position + Vector2.left * speed * Time.fixedDeltaTime;

            rb.MovePosition(novaPosicao);
        }
        else
        {
            // Mantem compatibilidade com inimigos antigos
            transform.Translate(
                Vector2.left * speed * Time.fixedDeltaTime,
                Space.World
            );
        }

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}
