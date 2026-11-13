
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBoss : MonoBehaviour
{
    public Slider sliderVida;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void MostrarBarra(int vidaMaxima)
    {
        gameObject.SetActive(true);

        sliderVida.minValue = 0;
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vidaMaxima;
    }

    public void AtualizarVida(int vidaAtual)
    {
        sliderVida.value = Mathf.Max(0, vidaAtual);
    }

    public void EsconderBarra()
    {
        gameObject.SetActive(false);
    }
}
