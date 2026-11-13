
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text textoPontuacao;
    public float pontosPorSegundo = 10f;
    public int pontosPorInimigo = 100;

    private float pontuacao = 0f;
    private bool jogoAtivo = true;

    public int PontuacaoAtual
    {
        get { return Mathf.FloorToInt(pontuacao); }
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!jogoAtivo)
            return;

        pontuacao += pontosPorSegundo * Time.deltaTime;
        AtualizarTexto();
    }

    public void AdicionarPontos(int pontos)
    {
        if (!jogoAtivo)
            return;

        pontuacao += pontos;
        AtualizarTexto();
    }

    public void PararPontuacao()
    {
        jogoAtivo = false;
    }

    void AtualizarTexto()
    {
        if (textoPontuacao != null)
        {
            textoPontuacao.text =
                "Pontos: " + PontuacaoAtual;
        }
    }
}
