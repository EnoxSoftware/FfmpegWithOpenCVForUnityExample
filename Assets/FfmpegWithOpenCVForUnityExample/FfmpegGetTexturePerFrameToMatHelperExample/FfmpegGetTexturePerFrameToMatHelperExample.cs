#if !(PLATFORM_LUMIN && !UNITY_EDITOR)

using FfmpegWithOpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnityExample;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FfmpegWithOpenCVForUnityExample
{
    /// <summary>
    /// FfmpegGetTexturePerFrameToMatHelper Example
    /// </summary>
    [RequireComponent(typeof(FfmpegGetTexturePerFrameToMatHelper))]
    public class FfmpegGetTexturePerFrameToMatHelperExample : MonoBehaviour
    {
        ///// <summary>
        ///// The requested resolution dropdown.
        ///// </summary>
        //public Dropdown requestedResolutionDropdown;

        ///// <summary>
        ///// The requested resolution.
        ///// </summary>
        //public ResolutionPreset requestedResolution = ResolutionPreset._640x480;

        ///// <summary>
        ///// The requestedFPS dropdown.
        ///// </summary>
        //public Dropdown requestedFPSDropdown;

        ///// <summary>
        ///// The requestedFPS.
        ///// </summary>
        //public FPSPreset requestedFPS = FPSPreset._30;

        ///// <summary>
        ///// The rotate 90 degree toggle.
        ///// </summary>
        //public Toggle rotate90DegreeToggle;

        ///// <summary>
        ///// The flip vertical toggle.
        ///// </summary>
        //public Toggle flipVerticalToggle;

        ///// <summary>
        ///// The flip horizontal toggle.
        ///// </summary>
        //public Toggle flipHorizontalToggle;

        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The webcam texture to mat helper.
        /// </summary>
        FfmpegGetTexturePerFrameToMatHelper ffmpegGetTexturePerFrameToMatHelper;

        /// <summary>
        /// The FPS monitor.
        /// </summary>
        FpsMonitor fpsMonitor;

        // Use this for initialization
        void Start()
        {
            fpsMonitor = GetComponent<FpsMonitor>();

            ffmpegGetTexturePerFrameToMatHelper = gameObject.GetComponent<FfmpegGetTexturePerFrameToMatHelper>();
            //int width, height;
            //Dimensions(requestedResolution, out width, out height);
            //webCamTextureToMatHelper.requestedWidth = width;
            //webCamTextureToMatHelper.requestedHeight = height;
            //webCamTextureToMatHelper.requestedFPS = (int)requestedFPS;
            ffmpegGetTexturePerFrameToMatHelper.Initialize();

            // Update GUI state
            //requestedResolutionDropdown.value = (int)requestedResolution;
            //string[] enumNames = System.Enum.GetNames(typeof(FPSPreset));
            //int index = Array.IndexOf(enumNames, requestedFPS.ToString());
            //requestedFPSDropdown.value = index;
            //rotate90DegreeToggle.isOn = webCamTextureToMatHelper.rotate90Degree;
            //flipVerticalToggle.isOn = webCamTextureToMatHelper.flipVertical;
            //flipHorizontalToggle.isOn = webCamTextureToMatHelper.flipHorizontal;
        }

        /// <summary>
        /// Raises the webcam texture to mat helper initialized event.
        /// </summary>
        public void OnFfmpegGetTexturePerFrameToMatHelperInitialized()
        {
            Debug.Log("OnFfmpegGetTexturePerFrameToMatHelperInitialized");

            Mat ffmpegMat = ffmpegGetTexturePerFrameToMatHelper.GetMat();

            texture = new Texture2D(ffmpegMat.cols(), ffmpegMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(ffmpegMat, texture);

            gameObject.GetComponent<Renderer>().material.mainTexture = texture;

            gameObject.transform.localScale = new Vector3(ffmpegMat.cols(), ffmpegMat.rows(), 1);
            Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

            if (fpsMonitor != null)
            {
                //fpsMonitor.Add("deviceName", webCamTextureToMatHelper.GetDeviceName().ToString());
                fpsMonitor.Add("width", ffmpegGetTexturePerFrameToMatHelper.GetWidth().ToString());
                fpsMonitor.Add("height", ffmpegGetTexturePerFrameToMatHelper.GetHeight().ToString());
                //fpsMonitor.Add("videoRotationAngle", webCamTextureToMatHelper.GetWebCamTexture().videoRotationAngle.ToString());
                //fpsMonitor.Add("videoVerticallyMirrored", webCamTextureToMatHelper.GetWebCamTexture().videoVerticallyMirrored.ToString());
                //fpsMonitor.Add("camera fps", webCamTextureToMatHelper.GetFPS().ToString());
                //fpsMonitor.Add("isFrontFacing", webCamTextureToMatHelper.IsFrontFacing().ToString());
                //fpsMonitor.Add("rotate90Degree", webCamTextureToMatHelper.rotate90Degree.ToString());
                //fpsMonitor.Add("flipVertical", webCamTextureToMatHelper.flipVertical.ToString());
                //fpsMonitor.Add("flipHorizontal", webCamTextureToMatHelper.flipHorizontal.ToString());
                fpsMonitor.Add("orientation", Screen.orientation.ToString());
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
        public void OnFfmpegGetTexturePerFrameToMatHelperDisposed()
        {
            Debug.Log("OnFfmpegGetTexturePerFrameToMatHelperDisposed");

            if (texture != null)
            {
                Texture2D.Destroy(texture);
                texture = null;
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnFfmpegGetTexturePerFrameToMatHelperErrorOccurred(FfmpegGetTexturePerFrameToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnFfmpegGetTexturePerFrameToMatHelperErrorOccurred " + errorCode);

            if (fpsMonitor != null)
            {
                fpsMonitor.consoleText = "ErrorCode: " + errorCode;
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (ffmpegGetTexturePerFrameToMatHelper.IsPlaying() && ffmpegGetTexturePerFrameToMatHelper.DidUpdateThisFrame())
            {

                Mat rgbaMat = ffmpegGetTexturePerFrameToMatHelper.GetMat();

                Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

                Utils.matToTexture2D(rgbaMat, texture);
            }

        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        void OnDestroy()
        {
            ffmpegGetTexturePerFrameToMatHelper.Dispose();
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
            ffmpegGetTexturePerFrameToMatHelper.Initialize();
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            ffmpegGetTexturePerFrameToMatHelper.Play();
        }

        /// <summary>
        /// Raises the pause button click event.
        /// </summary>
        public void OnPauseButtonClick()
        {
            ffmpegGetTexturePerFrameToMatHelper.Pause();
        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnStopButtonClick()
        {
            ffmpegGetTexturePerFrameToMatHelper.Stop();
        }

        /// <summary>
        /// Raises the change camera button click event.
        /// </summary>
        public void OnChangeCameraButtonClick()
        {
            //webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.requestedIsFrontFacing;
        }

        /// <summary>
        /// Raises the requested resolution dropdown value changed event.
        /// </summary>
        public void OnRequestedResolutionDropdownValueChanged(int result)
        {
            //if ((int)requestedResolution != result)
            //{
            //    requestedResolution = (ResolutionPreset)result;

            //    int width, height;
            //    Dimensions(requestedResolution, out width, out height);

            //    webCamTextureToMatHelper.Initialize(width, height);
            //}
        }

        /// <summary>
        /// Raises the requestedFPS dropdown value changed event.
        /// </summary>
        public void OnRequestedFPSDropdownValueChanged(int result)
        {
            //string[] enumNames = Enum.GetNames(typeof(FPSPreset));
            //int value = (int)System.Enum.Parse(typeof(FPSPreset), enumNames[result], true);

            //if ((int)requestedFPS != value)
            //{
            //    requestedFPS = (FPSPreset)value;

            //    webCamTextureToMatHelper.requestedFPS = (int)requestedFPS;
            //}
        }

        /// <summary>
        /// Raises the rotate 90 degree toggle value changed event.
        /// </summary>
        public void OnRotate90DegreeToggleValueChanged()
        {
            //if (rotate90DegreeToggle.isOn != webCamTextureToMatHelper.rotate90Degree)
            //{
            //    webCamTextureToMatHelper.rotate90Degree = rotate90DegreeToggle.isOn;

            //    if (fpsMonitor != null)
            //        fpsMonitor.Add("rotate90Degree", webCamTextureToMatHelper.rotate90Degree.ToString());
            //}
        }

        /// <summary>
        /// Raises the flip vertical toggle value changed event.
        /// </summary>
        public void OnFlipVerticalToggleValueChanged()
        {
            //if (flipVerticalToggle.isOn != webCamTextureToMatHelper.flipVertical)
            //{
            //    webCamTextureToMatHelper.flipVertical = flipVerticalToggle.isOn;

            //    if (fpsMonitor != null)
            //        fpsMonitor.Add("flipVertical", webCamTextureToMatHelper.flipVertical.ToString());
            //}
        }

        /// <summary>
        /// Raises the flip horizontal toggle value changed event.
        /// </summary>
        public void OnFlipHorizontalToggleValueChanged()
        {
            //if (flipHorizontalToggle.isOn != webCamTextureToMatHelper.flipHorizontal)
            //{
            //    webCamTextureToMatHelper.flipHorizontal = flipHorizontalToggle.isOn;

            //    if (fpsMonitor != null)
            //        fpsMonitor.Add("flipHorizontal", webCamTextureToMatHelper.flipHorizontal.ToString());
            //}
        }

        //public enum FPSPreset : int
        //{
        //    _0 = 0,
        //    _1 = 1,
        //    _5 = 5,
        //    _10 = 10,
        //    _15 = 15,
        //    _30 = 30,
        //    _60 = 60,
        //}

        //public enum ResolutionPreset : byte
        //{
        //    _50x50 = 0,
        //    _640x480,
        //    _1280x720,
        //    _1920x1080,
        //    _9999x9999,
        //}

        //private void Dimensions(ResolutionPreset preset, out int width, out int height)
        //{
        //    switch (preset)
        //    {
        //        case ResolutionPreset._50x50:
        //            width = 50;
        //            height = 50;
        //            break;
        //        case ResolutionPreset._640x480:
        //            width = 640;
        //            height = 480;
        //            break;
        //        case ResolutionPreset._1280x720:
        //            width = 1280;
        //            height = 720;
        //            break;
        //        case ResolutionPreset._1920x1080:
        //            width = 1920;
        //            height = 1080;
        //            break;
        //        case ResolutionPreset._9999x9999:
        //            width = 9999;
        //            height = 9999;
        //            break;
        //        default:
        //            width = height = 0;
        //            break;
        //    }
        //}
    }
}

#endif