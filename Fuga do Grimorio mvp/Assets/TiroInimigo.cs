
using UnityEngine;

public class TiroInimigo : MonoBehaviour
{
    public GameObject prefabRaio;
    public float intervaloTiro = 2f;
    public float velocidadeTiro = 7f;

    private Transform jogador;
    private float contador;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jogador = player.transform;
        }

        contador = intervaloTiro;
    }

    void Update()
    {
        if (jogador == null) return;

        contador -= Time.deltaTime;

        if (contador <= 0f)
        {
            Atirar();
            contador = intervaloTiro;
        }
    }

    void Atirar()
    {
        if (prefabRaio == null || jogador == null) return;

        Vector3 direcao = (jogador.position - transform.position).normalized;

        Vector3 posicao = transform.position + direcao * 0.8f;

        GameObject tiro = Instantiate(
            prefabRaio,
            posicao,
            Quaternion.identity
        );

        Projetil projetil = tiro.GetComponent<Projetil>();

        if (projetil != null)
        {
            projetil.tiroInimigo = true;
            projetil.velocidade = velocidadeTiro;

            float angulo = Mathf.Atan2(
                direcao.y,
                direcao.x
            ) * Mathf.Rad2Deg;

            tiro.transform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }
}
