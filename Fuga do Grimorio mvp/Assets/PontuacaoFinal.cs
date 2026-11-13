
using UnityEngine;
using TMPro;

public class PontuacaoFinal : MonoBehaviour
{
    public TMP_Text textoPontuacao;

    void OnEnable()
    {
        AtualizarPontuacao();
    }

    public void AtualizarPontuacao()
    {
        if (textoPontuacao == null)
        {
            Debug.LogWarning("Texto da pontuacao nao configurado!");
            return;
        }

        if (ScoreManager.Instance != null)
        {
            textoPontuacao.text =
                ScoreManager.Instance.PontuacaoAtual.ToString();
        }
    }
}
