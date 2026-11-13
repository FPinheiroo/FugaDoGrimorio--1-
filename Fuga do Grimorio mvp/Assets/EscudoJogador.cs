
using UnityEngine;

public class EscudoJogador : MonoBehaviour
{
    [Header("Escudo Arcano")]
    public GameObject efeitoEscudo;

    private bool escudoAtivo = false;

    void Start()
    {
        if (efeitoEscudo != null)
            efeitoEscudo.SetActive(false);
    }

    public void AtivarEscudo()
    {
        if (escudoAtivo) return;

        escudoAtivo = true;

        if (efeitoEscudo != null)
            efeitoEscudo.SetActive(true);

        Debug.Log("Escudo Arcano ativado!");
    }

    public bool AbsorverDano()
    {
        if (!escudoAtivo) return false;

        escudoAtivo = false;

        if (efeitoEscudo != null)
            efeitoEscudo.SetActive(false);

        Debug.Log("Escudo Arcano absorveu o ataque!");

        return true;
    }
}
