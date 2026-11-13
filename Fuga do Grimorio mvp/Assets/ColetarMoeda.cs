
using UnityEngine;

public class ColetarMoeda : MonoBehaviour
{
    public int pontos = 25;

    private bool coletada = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coletada) return;

        VidaJogador jogador = collision.GetComponentInParent<VidaJogador>();

        if (jogador == null) return;

        coletada = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AdicionarPontos(pontos);
        }

        Destroy(gameObject);
    }
}
