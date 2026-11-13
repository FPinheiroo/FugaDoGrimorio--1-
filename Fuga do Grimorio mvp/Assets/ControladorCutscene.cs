using UnityEngine;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ControladorCutscene : MonoBehaviour
{
    public string nomeCenaJogo = "SampleScene";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += AoTerminarVideo;
    }

    void Update()
    {
        // Permite pular a cutscene clicando na tela ou apertando Espaço
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            CarregarJogo();
        }
    }

    void AoTerminarVideo(VideoPlayer vp)
    {
        CarregarJogo();
    }

    void CarregarJogo()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }
}