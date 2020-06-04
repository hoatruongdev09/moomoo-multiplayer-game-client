using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AquaTurd.Controllers
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    public class SplashScreen : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public AudioSource audioSource;
        public long frame;
        public Image fader;
        bool touched;
        bool isLoading;

        void Awake()
        {
            videoPlayer = Camera.main.GetComponent<VideoPlayer>();
            touched = false;
        }
        private void Start()
        {
            // videoPlayer.enabled = true;
            // videoPlayer.Play();
            LeanTween.alpha(fader.GetComponent<RectTransform>(), 0, 0.5f);
            StartCoroutine(DelaySplash());

        }
        private void Update()
        {
            frame = videoPlayer.frame;
            Debug.Log("frame splash: " + frame);
            if (Input.GetMouseButtonDown(0))
                touched = true;
            if ((frame > 220 || touched) && !isLoading)
            {
                fader.color = Color.Lerp(fader.color, new Color(0, 0, 0, 1), 10 * Time.deltaTime);
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0, 10 * Time.deltaTime);
                if (fader.color.a > 0.99f)
                {
                    isLoading = true;
                    SceneManager.LoadScene(1);
                }

            }

        }
        private IEnumerator DelaySplash()
        {
            yield return new WaitForSeconds(2f);
            LeanTween.alpha(fader.GetComponent<RectTransform>(), 1, 1f).setOnComplete(() => SceneManager.LoadScene(1));

        }
    }
}