
using UnityEngine;

public class TransformacaoPip : MonoBehaviour
{
    [Header("Animator do Pip")]
    public Animator animatorPip;

    [Header("Efeito de Transformacao")]
    public ParticleSystem fumacaTransformacao;

    void Start()
    {
        if (fumacaTransformacao != null)
            fumacaTransformacao.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        VoltarAoNormal();
    }

    public void AtivarSuperForma()
    {
        if (animatorPip == null)
        {
            Debug.LogError("Animator do Pip nao configurado!");
            return;
        }

        // Solta a fumaca magica
        if (fumacaTransformacao != null)
        {
            fumacaTransformacao.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            fumacaTransformacao.Play();
        }

        // Ativa a animacao do Super Pip
        animatorPip.SetBool("SuperAtivo", true);

        Debug.Log("Super Pip ativado!");
    }

    public void VoltarAoNormal()
    {
        if (animatorPip == null)
            return;

        animatorPip.SetBool("SuperAtivo", false);

        Debug.Log("Pip voltou ao normal!");
    }
}
