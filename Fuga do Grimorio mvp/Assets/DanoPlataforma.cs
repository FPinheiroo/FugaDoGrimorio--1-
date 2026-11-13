
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoPlataforma : MonoBehaviour
{
    [Header("Colisao da plataforma")]
    [SerializeField] private Collider2D colisorSolido;

    // Guarda os colliders que ja ativaram a passagem
    private readonly HashSet<Collider2D> jogadoresDetectados = new();

    private void OnTriggerEnter2D(Collider2D outro)
    {
        VidaJogador vida =
            outro.GetComponentInParent<VidaJogador>();

        if (vida == null || colisorSolido == null)
            return;

        // Evita processar o mesmo Pip varias vezes
        if (!jogadoresDetectados.Add(outro))
            return;

        // Desativa imediatamente a colisao fisica
        Physics2D.IgnoreCollision(
            colisorSolido,
            outro,
            true
        );

        // O VidaJogador controla dano, escudo e invencibilidade
        vida.ReceberDano();

        Debug.Log("Pip atingiu a frente da plataforma!");

        StartCoroutine(
            EsperarPlataformaPassar(outro)
        );
    }

    private IEnumerator EsperarPlataformaPassar(
        Collider2D pip)
    {
        // A plataforma se move da direita para a esquerda.
        // Esperamos a borda direita dela passar
        // completamente pela borda esquerda do Pip.

        while (pip != null && colisorSolido != null)
        {
            float direitaPlataforma =
                colisorSolido.bounds.max.x;

            float esquerdaPip =
                pip.bounds.min.x;

            // A plataforma ja passou inteiramente pelo Pip
            if (direitaPlataforma < esquerdaPip)
                break;

            yield return new WaitForFixedUpdate();
        }

        if (pip != null && colisorSolido != null)
        {
            Physics2D.IgnoreCollision(
                colisorSolido,
                pip,
                false
            );

            Debug.Log(
                "Plataforma passou! Colisao restaurada."
            );
        }
    }

    private void OnDisable()
    {
        // Restaura as colisoes se o objeto for desativado
        foreach (Collider2D pip in jogadoresDetectados)
        {
            if (pip != null && colisorSolido != null)
            {
                Physics2D.IgnoreCollision(
                    colisorSolido,
                    pip,
                    false
                );
            }
        }

        jogadoresDetectados.Clear();
    }
}
