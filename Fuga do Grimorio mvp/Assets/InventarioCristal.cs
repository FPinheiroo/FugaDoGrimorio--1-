
using UnityEngine;
using TMPro;

public class InventarioCristal : MonoBehaviour
{
    [Header("Cristal de Poder")]
    public int cargasPorCristal = 2;
    public int maximoCargas = 6;
    public KeyCode teclaAtivar = KeyCode.E;
    public float duracaoPoder = 5f;

    [Header("Interface")]
    public TMP_Text textoCargas;
    public TMP_Text textoTempo;

    private int cargasAtuais = 0;
    private float tempoRestante = 0f;
    private bool poderAtivo = false;

    private TransformacaoPip transformacao;

    void Start()
    {
        transformacao = GetComponent<TransformacaoPip>();
        AtualizarInterface();
    }

    void Update()
    {
        if (poderAtivo)
        {
            tempoRestante -= Time.deltaTime;

            if (tempoRestante <= 0f)
            {
                DesativarPoder();
            }
        }

        if (Input.GetKeyDown(teclaAtivar))
        {
            AtivarPoder();
        }

        AtualizarInterface();
    }

    public bool AdicionarCristal()
    {
        if (cargasAtuais >= maximoCargas)
            return false;

        cargasAtuais = Mathf.Min(
            cargasAtuais + cargasPorCristal,
            maximoCargas
        );

        AtualizarInterface();

        Debug.Log("Cristal coletado! Cargas: " + cargasAtuais);

        return true;
    }

    public void AtivarPoder()
    {
        if (poderAtivo || cargasAtuais <= 0)
            return;

        cargasAtuais--;
        poderAtivo = true;
        tempoRestante = duracaoPoder;

        if (transformacao != null)
        {
            transformacao.AtivarSuperForma();
        }

        Debug.Log("Cristal ativado!");
    }

    void DesativarPoder()
    {
        poderAtivo = false;
        tempoRestante = 0f;

        if (transformacao != null)
        {
            transformacao.VoltarAoNormal();
        }

        Debug.Log("Cristal desativado!");
    }

    public bool PoderAtivo()
    {
        return poderAtivo;
    }

    void AtualizarInterface()
    {
        if (textoCargas != null)
        {
            textoCargas.text = "x" + cargasAtuais;
        }

        if (textoTempo != null)
        {
            textoTempo.text = poderAtivo
                ? tempoRestante.ToString("F1") + "s"
                : "";
        }
    }
}
