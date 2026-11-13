
using UnityEngine;

public class PlayerAtirador : MonoBehaviour
{
    public GameObject raioPrefab;

    [Header("Mira automatica")]
    public float alcanceAimbot = 30f;

    private InventarioCristal inventarioCristal;

    void Start()
    {
        inventarioCristal = GetComponent<InventarioCristal>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Atirar();
    }

    void Atirar()
    {
        if (raioPrefab == null)
            return;

        bool superAtivo = inventarioCristal != null &&
                          inventarioCristal.PoderAtivo();

        Transform alvo = superAtivo ? EncontrarInimigo() : null;

        Vector3 direcao;

        if (alvo != null)
        {
            direcao = alvo.position - transform.position;
        }
        else
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );

            mouse.z = 0f;
            direcao = mouse - transform.position;
        }

        float angulo = Mathf.Atan2(
            direcao.y,
            direcao.x
        ) * Mathf.Rad2Deg;

        GameObject tiro = Instantiate(
            raioPrefab,
            transform.position,
            Quaternion.Euler(0f, 0f, angulo)
        );

        // Ativa o tiro teleguiado somente no Super Pip
        if (superAtivo && alvo != null)
        {
            Projetil projetil = tiro.GetComponent<Projetil>();

            if (projetil != null)
                projetil.ConfigurarTeleguiado(alvo);
        }
    }

    Transform EncontrarInimigo()
    {
        BossVida boss = FindFirstObjectByType<BossVida>();

        if (boss != null && boss.isActiveAndEnabled)
        {
            float distancia = Vector2.Distance(
                transform.position,
                boss.transform.position
            );

            if (distancia <= alcanceAimbot)
                return boss.transform;
        }

        GameObject[] inimigos =
            GameObject.FindGameObjectsWithTag("Obstacle");

        Transform maisProximo = null;
        float menorDistancia = alcanceAimbot;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector2.Distance(
                transform.position,
                inimigo.transform.position
            );

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = inimigo.transform;
            }
        }

        return maisProximo;
    }
}
