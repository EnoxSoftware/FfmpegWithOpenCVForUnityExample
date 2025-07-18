using FfmpegWithOpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityIntegration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FfmpegWithOpenCVForUnityExample
{
    /// <summary>
    /// FfmpegToMatHelper Example
    /// </summary>
    [RequireComponent(typeof(FfmpegToMatHelper))]
    public class FfmpegToMatHelperExample : MonoBehaviour
    {

        // Public Fields
        [Header("Output")]
        /// <summary>
        /// The RawImage for previewing the result.
        /// </summary>
        public RawImage ResultPreview;

        // Private Fields
        /// <summary>
        /// The texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// The webcam texture to mat helper.
        /// </summary>
        private FfmpegToMatHelper _ffmpegToMatHelper;

        /// <summary>
        /// The FPS monitor.
        /// </summary>
        private FpsMonitor _fpsMonitor;

        // Unity Lifecycle Methods
        private void Start()
        {
            _fpsMonitor = GetComponent<FpsMonitor>();

            _ffmpegToMatHelper = gameObject.GetComponent<FfmpegToMatHelper>();

            _ffmpegToMatHelper.Initialize();
        }

        private void Update()
        {
            if (_ffmpegToMatHelper.IsPlaying() && _ffmpegToMatHelper.DidUpdateThisFrame())
            {
                Mat rgbaMat = _ffmpegToMatHelper.GetMat();

                Imgproc.putText(rgbaMat, "W:" + rgbaMat.width() + " H:" + rgbaMat.height() + " SO:" + Screen.orientation, new Point(5, rgbaMat.rows() - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

                OpenCVMatUtils.MatToTexture2D(rgbaMat, _texture);
            }
        }

        private void OnDestroy()
        {
            _ffmpegToMatHelper?.Dispose();
        }

        // Public Methods
        /// <summary>
        /// Raises the webcam texture to mat helper initialized event.
        /// </summary>
        public void OnFfmpegToMatHelperInitialized()
        {
            Debug.Log("OnFfmpegToMatHelperInitialized");

            Mat ffmpegMat = _ffmpegToMatHelper.GetMat();

            _texture = new Texture2D(ffmpegMat.cols(), ffmpegMat.rows(), TextureFormat.RGBA32, false);
            OpenCVMatUtils.MatToTexture2D(ffmpegMat, _texture);

            // Set the Texture2D as the texture of the RawImage for preview.
            ResultPreview.texture = _texture;
            ResultPreview.GetComponent<AspectRatioFitter>().aspectRatio = (float)_texture.width / _texture.height;

            if (_fpsMonitor != null)
            {
                _fpsMonitor.Add("width", _ffmpegToMatHelper.GetWidth().ToString());
                _fpsMonitor.Add("height", _ffmpegToMatHelper.GetHeight().ToString());
                _fpsMonitor.Add("orientation", Screen.orientation.ToString());
            }

            float width = ffmpegMat.width();
            float height = ffmpegMat.height();

            float widthScale = (float)Screen.width / width;
            float heightScale = (float)Screen.height / height;
            if (widthScale < heightScale)
            {
                Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
            }
            else
            {
                Camera.main.orthographicSize = height / 2;
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper disposed event.
        /// </summary>
        public void OnFfmpegToMatHelperDisposed()
        {
            Debug.Log("OnFfmpegToMatHelperDisposed");

            if (_texture != null)
            {
                Texture2D.Destroy(_texture);
                _texture = null;
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnFfmpegToMatHelperErrorOccurred(FfmpegToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnFfmpegToMatHelperErrorOccurred " + errorCode);

            if (_fpsMonitor != null)
            {
                _fpsMonitor.ConsoleText = "ErrorCode: " + errorCode;
            }
        }

        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnBackButtonClick()
        {
            SceneManager.LoadScene("FfmpegWithOpenCVForUnityExample");
        }

        /// <summary>
        /// Raises the initialize button click event.
        /// </summary>
        public void OnInitializeButtonClick()
        {
            _ffmpegToMatHelper.Initialize();
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            _ffmpegToMatHelper.Play();
        }

        /// <summary>
        /// Raises the pause button click event.
        /// </summary>
        public void OnPauseButtonClick()
        {
            _ffmpegToMatHelper.Pause();
        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnStopButtonClick()
        {
            _ffmpegToMatHelper.Stop();
        }
    }
}
