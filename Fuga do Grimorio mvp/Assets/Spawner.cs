
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configuracao dos Inimigos")]
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Dificuldade - Spawn")]
    [Range(0f, 0.9f)]
    public float reducaoPorBoss = 0.15f;

    [Min(0.1f)]
    public float intervaloMinimo = 0.6f;

    [Header("Dificuldade - Tiros")]
    public float aumentoVelocidadeTiro = 0.15f;
    public float velocidadeMaximaTiro = 18f;

    private float timer = 0f;
    private bool pausado = false;

    private float spawnRateInicial;
    private int bossesDerrotados = 0;

    void Awake()
    {
        spawnRateInicial = Mathf.Max(0.1f, spawnRate);
    }

    void Update()
    {
        if (pausado)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
            return;

        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            randomY,
            0f
        );

        GameObject inimigo = Instantiate(
            obstaclePrefab,
            spawnPos,
            Quaternion.identity
        );

        // Configura os tiros do inimigo criado
        TiroInimigo[] atiradores =
            inimigo.GetComponentsInChildren<TiroInimigo>();

        foreach (TiroInimigo atirador in atiradores)
        {
            float velocidadeBase = atirador.velocidadeTiro;

            float multiplicador = Mathf.Pow(
                1f + aumentoVelocidadeTiro,
                bossesDerrotados
            );

            atirador.velocidadeTiro = Mathf.Min(
                velocidadeMaximaTiro,
                velocidadeBase * multiplicador
            );
        }
    }

    public void AumentarDificuldade()
    {
        bossesDerrotados++;

        float novoIntervalo =
            spawnRateInicial *
            Mathf.Pow(
                1f - reducaoPorBoss,
                bossesDerrotados
            );

        spawnRate = Mathf.Max(
            intervaloMinimo,
            novoIntervalo
        );

        Debug.Log(
            "DIFICULDADE NIVEL " +
            (bossesDerrotados + 1) +
            " | Spawn: " +
            spawnRate.ToString("F2") +
            "s | Multiplicador dos tiros: " +
            Mathf.Pow(
                1f + aumentoVelocidadeTiro,
                bossesDerrotados
            ).ToString("F2")
        );
    }

    public void PausarSpawn()
    {
        pausado = true;
        timer = 0f;

        Debug.Log("Spawn pausado: batalha contra o boss!");
    }

    public void RetomarSpawn()
    {
        pausado = false;
        timer = 0f;

        Debug.Log("Spawn retomado!");
    }
}
