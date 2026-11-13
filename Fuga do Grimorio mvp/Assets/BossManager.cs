
using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform pontoSpawnBoss;

    [Header("Spawner de Inimigos")]
    public ObstacleSpawner spawnerInimigos;

    [Header("Pontuacao")]
    public int pontosPorBoss = 5000;

    [Header("Efeito de Invocacao")]
    public FlashBoss flashBoss;

    [Header("Barra de Vida")]
    public BarraSeguirBoss barraSeguirBoss;

    [Header("Cenarios")]
    public GerenciadorCenarios gerenciadorCenarios;

    private int proximaPontuacaoBoss;
    private GameObject bossAtual;
    private bool batalhaAtiva = false;
    private bool transicaoAtiva = false;

    private int bossesDerrotados = 0;

    void Start()
    {
        proximaPontuacaoBoss = pontosPorBoss;
    }

    void Update()
    {
        if (ScoreManager.Instance == null)
            return;

        // Nao inicia outra batalha durante transicoes
        if (batalhaAtiva || transicaoAtiva)
            return;

        if (ScoreManager.Instance.PontuacaoAtual >=
            proximaPontuacaoBoss)
        {
            IniciarBatalha();
        }
    }

    void IniciarBatalha()
    {
        if (bossPrefab == null || pontoSpawnBoss == null)
        {
            Debug.LogWarning(
                "BossPrefab ou PontoSpawnBoss nao configurado!"
            );
            return;
        }

        batalhaAtiva = true;
        transicaoAtiva = true;

        // Pausa os inimigos comuns
        if (spawnerInimigos != null)
        {
            spawnerInimigos.PausarSpawn();
        }

        if (flashBoss != null)
        {
            StartCoroutine(TransicaoEntradaBoss());
        }
        else
        {
            CriarBoss();
            transicaoAtiva = false;
        }

        proximaPontuacaoBoss += pontosPorBoss;
    }

    IEnumerator TransicaoEntradaBoss()
    {
        yield return StartCoroutine(
            flashBoss.ExecutarFlash(CriarBoss)
        );

        transicaoAtiva = false;
    }

    void CriarBoss()
    {
        bossAtual = Instantiate(
            bossPrefab,
            pontoSpawnBoss.position,
            Quaternion.identity
        );

        // Configura os lasers conforme a dificuldade
        BossChoqueHorizontal[] ataques =
            bossAtual.GetComponentsInChildren<BossChoqueHorizontal>();

        foreach (BossChoqueHorizontal ataque in ataques)
        {
            ataque.ConfigurarDificuldade(bossesDerrotados);
        }

        if (barraSeguirBoss != null)
        {
            barraSeguirBoss.DefinirBoss(
                bossAtual.transform
            );
        }

        Debug.Log(
            "BATALHA CONTRA O OLHO ARCANO! " +
            "Nivel " + (bossesDerrotados + 1)
        );
    }

    public void BossDerrotado()
    {
        if (!batalhaAtiva)
            return;

        batalhaAtiva = false;
        transicaoAtiva = true;

        // Remove a referencia do boss na barra
        if (barraSeguirBoss != null)
        {
            barraSeguirBoss.DefinirBoss(null);
        }

        bossAtual = null;

        bossesDerrotados++;

        // Aumenta a dificuldade
        if (spawnerInimigos != null)
        {
            spawnerInimigos.AumentarDificuldade();
        }

        // Troca de cenario com transicao branca
        if (flashBoss != null && gerenciadorCenarios != null)
        {
            StartCoroutine(TransicaoCenario());
        }
        else
        {
            if (gerenciadorCenarios != null)
            {
                gerenciadorCenarios.BossDerrotado();
            }

            FinalizarTransicaoCenario();
        }
    }

    IEnumerator TransicaoCenario()
    {
        yield return StartCoroutine(
            flashBoss.ExecutarFlash(() =>
            {
                gerenciadorCenarios.BossDerrotado();
            })
        );

        FinalizarTransicaoCenario();
    }

    void FinalizarTransicaoCenario()
    {
        // Inimigos so retornam apos o branco desaparecer
        if (spawnerInimigos != null)
        {
            spawnerInimigos.RetomarSpawn();
        }

        transicaoAtiva = false;

        Debug.Log(
            "Boss derrotado! Cenario atualizado. " +
            "Dificuldade nivel " +
            (bossesDerrotados + 1)
        );
    }
}
