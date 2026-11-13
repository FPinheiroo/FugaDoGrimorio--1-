
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashBoss : MonoBehaviour
{
    [Header("Imagem da Transicao")]
    public Image imagemFlash;

    [Header("Duracao da Transicao")]
    public float tempoEntrada = 0.7f;
    public float tempoBranco = 0.3f;
    public float tempoSaida = 0.8f;

    public IEnumerator ExecutarFlash(Action aoFicarBranco)
    {
        if (imagemFlash == null)
        {
            aoFicarBranco?.Invoke();
            yield break;
        }

        imagemFlash.gameObject.SetActive(true);

        Color cor = Color.white;
        cor.a = 0f;
        imagemFlash.color = cor;

        // Branco aparece suavemente
        float tempo = 0f;

        while (tempo < tempoEntrada)
        {
            tempo += Time.deltaTime;

            cor.a = Mathf.Clamp01(tempo / tempoEntrada);
            imagemFlash.color = cor;

            yield return null;
        }

        // Tela completamente branca
        cor.a = 1f;
        imagemFlash.color = cor;

        // Cria o boss somente agora
        aoFicarBranco?.Invoke();

        yield return new WaitForSeconds(tempoBranco);

        // Branco desaparece suavemente
        tempo = 0f;

        while (tempo < tempoSaida)
        {
            tempo += Time.deltaTime;

            cor.a = 1f - Mathf.Clamp01(tempo / tempoSaida);
            imagemFlash.color = cor;

            yield return null;
        }

        cor.a = 0f;
        imagemFlash.color = cor;

        imagemFlash.gameObject.SetActive(false);
    }
}
