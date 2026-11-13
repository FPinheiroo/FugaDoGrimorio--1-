
using UnityEngine;

public class ChoqueHorizontal : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    [Header("Cores")]
    public Color corAviso = new Color(
        0.7f, 0.2f, 1f, 0.35f
    );

    public Color corChoque = new Color(
        0.85f, 0.5f, 1f, 1f
    );

    [Header("Dano")]
    public int dano = 1;

    private BoxCollider2D colisor;
    private bool ativo = false;

    void Awake()
    {
        colisor = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (colisor != null)
        {
            colisor.isTrigger = true;
            colisor.enabled = false;
        }
    }

    public void IniciarAviso()
    {
        ativo = false;

        if (spriteRenderer != null)
            spriteRenderer.color = corAviso;

        if (colisor != null)
            colisor.enabled = false;
    }

    public void AtivarChoque()
    {
        ativo = true;

        if (spriteRenderer != null)
            spriteRenderer.color = corChoque;

        if (colisor != null)
            colisor.enabled = true;

        // Detecta o jogador mesmo se ele
        // ja estiver dentro da area
        if (colisor != null)
        {
            ContactFilter2D filtro = new ContactFilter2D();
            filtro.NoFilter();

            Collider2D[] resultados = new Collider2D[20];

            int total = colisor.Overlap(
                filtro,
                resultados
            );

            for (int i = 0; i < total; i++)
                TentarDarDano(resultados[i]);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (!ativo)
            return;

        TentarDarDano(outro);
    }

    void OnTriggerStay2D(Collider2D outro)
    {
        if (!ativo)
            return;

        TentarDarDano(outro);
    }

    void TentarDarDano(Collider2D outro)
    {
        VidaJogador vida =
            outro.GetComponentInParent<VidaJogador>();

        if (vida != null)
            vida.ReceberDano();
    }
}
