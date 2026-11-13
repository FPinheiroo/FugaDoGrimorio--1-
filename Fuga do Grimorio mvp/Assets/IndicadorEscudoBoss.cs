
using UnityEngine;
using TMPro;

public class IndicadorEscudoBoss : MonoBehaviour
{
    public TMP_Text textoEscudo;

    void Awake()
    {
        if (textoEscudo == null)
            textoEscudo = GetComponent<TMP_Text>();

        Esconder();
    }

    public void Mostrar(float duracao)
    {
        gameObject.SetActive(true);
        AtualizarTempo(duracao);
    }

    public void AtualizarTempo(float segundos)
    {
        if (textoEscudo != null)
        {
            textoEscudo.text =
                "INVENCÍVEL - " +
                segundos.ToString("F1") + "s";
        }
    }

    public void Esconder()
    {
        gameObject.SetActive(false);
    }
}
