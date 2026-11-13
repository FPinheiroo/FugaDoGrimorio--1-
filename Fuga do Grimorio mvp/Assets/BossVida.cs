
using System.Collections;
using UnityEngine;

public class BossVida : MonoBehaviour
{
    [Header("Vida do Boss")]
    public int vidaMaxima = 100;

    [Header("Escudo")]
    public GameObject escudoVisual;
    public float duracaoEscudo = 10f;

    [Header("Indicador de Escudo")]
    public IndicadorEscudoBoss indicadorEscudo;

    private int vidaAtual;
    private bool invencivel;
    private float tempoRestanteEscudo;

    private int proximoMarco = 75;
    private Coroutine rotinaEscudo;

    private BossManager bossManager;
    private BarraVidaBoss barraVida;

    void Start()
    {
        vidaAtual = vidaMaxima;

        bossManager = FindFirstObjectByType<BossManager>();

        barraVida = FindFirstObjectByType<BarraVidaBoss>(
            FindObjectsInactive.Include
        );

        if (barraVida != null)
            barraVida.MostrarBarra(vidaMaxima);

        if (indicadorEscudo == null)
        {
            indicadorEscudo =
                FindFirstObjectByType<IndicadorEscudoBoss>(
                    FindObjectsInactive.Include
                );
        }

        AtivarEscudo();
    }

    void Update()
    {
        if (!invencivel)
            return;

        tempoRestanteEscudo -= Time.deltaTime;

        if (indicadorEscudo != null)
        {
            indicadorEscudo.AtualizarTempo(
                Mathf.Max(0f, tempoRestanteEscudo)
            );
        }
    }

    void AtivarEscudo()
    {
        if (rotinaEscudo != null)
            StopCoroutine(rotinaEscudo);

        rotinaEscudo = StartCoroutine(RotinaEscudo());
    }

    IEnumerator RotinaEscudo()
    {
        invencivel = true;
        tempoRestanteEscudo = duracaoEscudo;

        if (escudoVisual != null)
            escudoVisual.SetActive(true);

        if (indicadorEscudo != null)
            indicadorEscudo.Mostrar(duracaoEscudo);

        yield return new WaitForSeconds(duracaoEscudo);

        invencivel = false;
        tempoRestanteEscudo = 0f;

        if (escudoVisual != null)
            escudoVisual.SetActive(false);

        if (indicadorEscudo != null)
            indicadorEscudo.Esconder();

        rotinaEscudo = null;
    }

    public void ReceberDano(int dano)
    {
        if (invencivel || vidaAtual <= 0 || dano <= 0)
            return;

        vidaAtual = Mathf.Max(0, vidaAtual - dano);

        if (barraVida != null)
            barraVida.AtualizarVida(vidaAtual);

        if (vidaAtual <= 0)
        {
            Morrer();
            return;
        }

        float percentualVida =
            (float)vidaAtual / vidaMaxima * 100f;

        if (proximoMarco > 0 &&
            percentualVida <= proximoMarco)
        {
            // Avanca o marco para impedir
            // que o escudo seja reativado
            // repetidamente no mesmo percentual.
            while (proximoMarco > 0 &&
                   percentualVida <= proximoMarco)
            {
                proximoMarco -= 25;
            }

            AtivarEscudo();
        }
    }

    void Morrer()
    {
        if (rotinaEscudo != null)
            StopCoroutine(rotinaEscudo);

        if (barraVida != null)
            barraVida.EsconderBarra();

        if (indicadorEscudo != null)
            indicadorEscudo.Esconder();

        if (bossManager != null)
            bossManager.BossDerrotado();

        Destroy(gameObject);
    }
}
