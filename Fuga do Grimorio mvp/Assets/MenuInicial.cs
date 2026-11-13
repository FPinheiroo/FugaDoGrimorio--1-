
using UnityEngine;

public class MenuInicial : MonoBehaviour
{
    [Header("Paineis")]
    public GameObject painelMenuInicial;
    public GameObject painelComoJogar;

    void Start()
    {
        painelComoJogar.SetActive(false);

        if (ReiniciarJogo.iniciarDireto)
        {
            ReiniciarJogo.iniciarDireto = false;

            painelMenuInicial.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            painelMenuInicial.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Jogar()
    {
        painelMenuInicial.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ComoJogar()
    {
        painelComoJogar.SetActive(true);
    }

    public void Voltar()
    {
        painelComoJogar.SetActive(false);
    }

    public void Sair()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
