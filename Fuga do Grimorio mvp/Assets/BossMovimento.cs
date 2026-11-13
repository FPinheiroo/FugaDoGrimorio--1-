
using UnityEngine;

public class BossMovimento : MonoBehaviour
{
    [Header("Movimento Vertical")]
    public float velocidade = 1f;
    public float distancia = 1.5f;

    private Vector2 posicaoInicial;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        posicaoInicial = rb != null
            ? rb.position
            : (Vector2)transform.position;
    }

    void FixedUpdate()
    {
        float novoY = posicaoInicial.y +
            Mathf.Sin(Time.fixedTime * velocidade) * distancia;

        if (rb != null)
        {
            rb.MovePosition(
                new Vector2(posicaoInicial.x, novoY)
            );
        }
        else
        {
            transform.position = new Vector3(
                posicaoInicial.x,
                novoY,
                transform.position.z
            );
        }
    }
}
