
using UnityEngine;

public class BarraSeguirBoss : MonoBehaviour
{
    public Transform boss;
    public Vector3 deslocamento = new Vector3(0, 1.5f, 0);

    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera cameraPrincipal;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        cameraPrincipal = Camera.main;
    }

    void LateUpdate()
    {
        AtualizarPosicao();
    }

    public void DefinirBoss(Transform novoBoss)
    {
        boss = novoBoss;

        // Posiciona imediatamente, sem esperar o LateUpdate
        AtualizarPosicao();
    }

    public void AtualizarPosicao()
    {
        if (boss == null || cameraPrincipal == null || canvas == null)
            return;

        Vector3 posicaoTela = cameraPrincipal.WorldToScreenPoint(
            boss.position + deslocamento
        );

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rectTransform.position = posicaoTela;
        }
        else
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform.parent as RectTransform,
                posicaoTela,
                canvas.worldCamera,
                out Vector3 posicao
            );

            rectTransform.position = posicao;
        }
    }
}
