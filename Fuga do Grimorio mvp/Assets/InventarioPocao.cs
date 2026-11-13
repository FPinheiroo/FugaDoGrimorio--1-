
using UnityEngine;
using TMPro;

public class InventarioPocao : MonoBehaviour
{
    [Header("Configuracoes")]
    public int maximoPocoes = 3;
    public KeyCode teclaUsar = KeyCode.Q;

    [Header("Interface")]
    public TMP_Text textoQuantidade;

    private int quantidadePocoes = 0;
    private VidaJogador vidaJogador;

    void Start()
    {
        vidaJogador = GetComponent<VidaJogador>();
        AtualizarInterface();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaUsar))
        {
            UsarPocao();
        }
    }

    public bool AdicionarPocao()
    {
        if (quantidadePocoes >= maximoPocoes)
            return false;

        quantidadePocoes++;

        AtualizarInterface();

        Debug.Log("Pocao coletada! Total: " + quantidadePocoes);

        return true;
    }

    public void UsarPocao()
    {
        if (quantidadePocoes <= 0 || vidaJogador == null)
            return;

        // So consome a pocao se recuperar vida
        if (vidaJogador.RecuperarVida())
        {
            quantidadePocoes--;
            AtualizarInterface();
        }
    }

    void AtualizarInterface()
    {
        if (textoQuantidade != null)
        {
            textoQuantidade.text = "x" + quantidadePocoes;
        }
    }
}
