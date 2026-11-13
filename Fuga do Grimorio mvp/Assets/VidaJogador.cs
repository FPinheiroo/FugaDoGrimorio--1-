
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaJogador : MonoBehaviour
{
    [Header("Sistema de Vidas")]
    public int vidasMaximas = 3;
    public TMP_Text textoVidas;
    public float tempoInvencibilidade = 1.5f;

    [Header("Coracoes")]
    public Image[] coracoes;
    public Sprite coracaoCheio;
    public Sprite coracaoVazio;

    [Header("Game Over")]
    public GameObject painelGameOver;

    [Header("Efeito de Dano")]
    public SpriteRenderer spritePip;
    public float intervaloPiscar = 0.12f;

    private int vidasAtuais;
    private float tempoProtecao;
    private float contadorPiscar;

    private bool morreu = false;

    private InventarioCristal inventarioCristal;

    private Color corOriginal;

    void Awake()
    {
        Time.timeScale = 1f;

        inventarioCristal = GetComponent<InventarioCristal>();

        if (spritePip == null)
        {
            spritePip = GetComponentInChildren<SpriteRenderer>();
        }

        if (spritePip != null)
        {
            corOriginal = spritePip.color;
        }
    }

    void Start()
    {
        vidasAtuais = vidasMaximas;
        tempoProtecao = 0f;
        contadorPiscar = 0f;
        morreu = false;

        AtualizarVidas();
        RestaurarSprite();

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "PainelGameOver nao esta conectado!"
            );
        }
    }

    void Update()
    {
        if (morreu)
            return;

        if (tempoProtecao > 0f)
        {
            tempoProtecao = Mathf.Max(
                0f,
                tempoProtecao - Time.deltaTime
            );

            contadorPiscar += Time.deltaTime;

            if (spritePip != null &&
                contadorPiscar >= intervaloPiscar)
            {
                contadorPiscar = 0f;

                Color cor = spritePip.color;

                // Alterna entre visivel e semitransparente
                cor.a = cor.a > 0.75f ? 0.35f : 1f;

                spritePip.color = cor;
            }

            if (tempoProtecao <= 0f)
            {
                RestaurarSprite();
            }
        }
        else
        {
            contadorPiscar = 0f;
            RestaurarSprite();
        }
    }

    // Verifica se Pip esta invencivel
    public bool EstaInvencivel()
    {
        return morreu ||
               tempoProtecao > 0f ||
               (inventarioCristal != null &&
                inventarioCristal.PoderAtivo());
    }

    public void ReceberDano()
    {
        if (EstaInvencivel())
            return;

        // Verifica o Escudo Arcano
        EscudoJogador escudo =
            GetComponent<EscudoJogador>();

        if (escudo != null && escudo.AbsorverDano())
        {
            tempoProtecao = tempoInvencibilidade;
            contadorPiscar = 0f;

            Debug.Log(
                "Ataque bloqueado pelo Escudo Arcano!"
            );

            return;
        }

        vidasAtuais = Mathf.Max(
            0,
            vidasAtuais - 1
        );

        tempoProtecao = tempoInvencibilidade;
        contadorPiscar = 0f;

        AtualizarVidas();

        Debug.Log(
            "Vida perdida! Restam: " + vidasAtuais
        );

        if (vidasAtuais <= 0)
        {
            GameOver();
        }
    }

    // Recupera um coracao
    public bool RecuperarVida()
    {
        if (morreu || vidasAtuais >= vidasMaximas)
            return false;

        vidasAtuais = Mathf.Min(
            vidasAtuais + 1,
            vidasMaximas
        );

        AtualizarVidas();

        Debug.Log(
            "Pocao utilizada! Vidas: " + vidasAtuais
        );

        return true;
    }

    void AtualizarVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasAtuais;
        }

        if (coracoes == null)
            return;

        for (int i = 0; i < coracoes.Length; i++)
        {
            if (coracoes[i] == null)
                continue;

            coracoes[i].sprite =
                i < vidasAtuais
                    ? coracaoCheio
                    : coracaoVazio;
        }
    }

    void RestaurarSprite()
    {
        if (spritePip == null)
            return;

        Color cor = spritePip.color;
        cor.a = corOriginal.a;
        spritePip.color = cor;
    }

    void GameOver()
    {
        if (morreu)
            return;

        morreu = true;

        RestaurarSprite();

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.PararPontuacao();
        }

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
            painelGameOver.transform.SetAsLastSibling();
        }

        Time.timeScale = 0f;

        Debug.Log("GAME OVER!");
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
