
using UnityEngine;

public class ColetarEscudo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        VidaJogador jogador = collision.GetComponentInParent<VidaJogador>();

        if (jogador == null) return;

        EscudoJogador escudo = jogador.GetComponent<EscudoJogador>();

        if (escudo != null)
        {
            escudo.AtivarEscudo();
            Destroy(gameObject);
        }
    }
}
