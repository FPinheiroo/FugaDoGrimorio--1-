
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 15f;
    public GameObject prefabPontos;
    public bool tiroInimigo = false;

    [Header("Dano no Boss")]
    public int danoBoss = 1;

    [Header("Super Pip - Tiro Teleguiado")]
    public float velocidadeTeleguiado = 35f;
    public float distanciaAcerto = 0.35f;

    private Transform alvo;
    private bool teleguiado = false;
    private bool atingiu = false;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void ConfigurarTeleguiado(Transform novoAlvo)
    {
        if (tiroInimigo || novoAlvo == null)
            return;

        alvo = novoAlvo;
        teleguiado = true;

        // Da mais tempo para o tiro perseguir o alvo
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        if (atingiu)
            return;

        if (teleguiado && alvo != null)
        {
            Vector3 destino = ObterPosicaoAlvo();
            Vector3 direcao = destino - transform.position;
            direcao.z = 0f;

            if (direcao.sqrMagnitude > 0.0001f)
            {
                float angulo = Mathf.Atan2(
                    direcao.y,
                    direcao.x
                ) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(
                    0f, 0f, angulo
                );
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                velocidadeTeleguiado * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, destino)
                <= distanciaAcerto)
            {
                AtingirAlvo();
            }

            return;
        }

        if (teleguiado && alvo == null)
        {
            // O alvo foi destruido antes do impacto
            Destroy(gameObject);
            return;
        }

        transform.Translate(
            Vector3.right * velocidade * Time.deltaTime
        );
    }

    Vector3 ObterPosicaoAlvo()
    {
        Collider2D colisor = alvo.GetComponent<Collider2D>();

        if (colisor == null)
            colisor = alvo.GetComponentInChildren<Collider2D>();

        Vector3 posicao = colisor != null
            ? colisor.bounds.center
            : alvo.position;

        posicao.z = transform.position.z;
        return posicao;
    }

    void AtingirAlvo()
    {
        if (atingiu || alvo == null)
            return;

        BossVida boss = alvo.GetComponentInParent<BossVida>();

        if (boss != null)
        {
            atingiu = true;
            boss.ReceberDano(danoBoss);
            Destroy(gameObject);
            return;
        }

        if (alvo.CompareTag("Obstacle"))
        {
            atingiu = true;
            DestruirInimigo(alvo.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D colisor)
    {
        if (atingiu)
            return;

        // TIRO DO INIMIGO
        if (tiroInimigo)
        {
            if (colisor.CompareTag("Obstacle"))
                return;

            if (colisor.CompareTag("Player"))
            {
                VidaJogador vida =
                    colisor.GetComponentInParent<VidaJogador>();

                if (vida != null)
                    vida.ReceberDano();

                Destroy(gameObject);
            }

            return;
        }

        if (colisor.CompareTag("Player"))
            return;

        // Tiro teleguiado ignora outros inimigos
        // para perseguir exclusivamente seu alvo
        if (teleguiado)
            return;

        BossVida boss =
            colisor.GetComponentInParent<BossVida>();

        if (boss != null)
        {
            atingiu = true;
            boss.ReceberDano(danoBoss);
            Destroy(gameObject);
            return;
        }

        if (colisor.CompareTag("Obstacle"))
        {
            atingiu = true;
            DestruirInimigo(colisor.gameObject);
        }
    }

    void DestruirInimigo(GameObject inimigo)
    {
        int pontos = 100;

        if (ScoreManager.Instance != null)
        {
            pontos = ScoreManager.Instance.pontosPorInimigo;
            ScoreManager.Instance.AdicionarPontos(pontos);
        }

        if (prefabPontos != null)
        {
            Collider2D colisor = inimigo.GetComponent<Collider2D>();

            Vector3 posicao = colisor != null
                ? colisor.bounds.center
                : inimigo.transform.position;

            posicao.z = -1f;

            GameObject popup = Instantiate(
                prefabPontos,
                posicao,
                Quaternion.identity
            );

            PontosPopup scriptPopup =
                popup.GetComponent<PontosPopup>();

            if (scriptPopup != null)
                scriptPopup.Configurar(pontos);
        }

        Destroy(inimigo);
        Destroy(gameObject);
    }
}
