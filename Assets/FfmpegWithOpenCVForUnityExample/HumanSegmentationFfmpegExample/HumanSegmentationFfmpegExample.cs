#if !(PLATFORM_LUMIN && !UNITY_EDITOR)

#if !UNITY_WSA_10_0

using FfmpegWithOpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FfmpegWithOpenCVForUnityExample
{
    /// <summary>
    /// Human Segmentation WebCam Example
    /// An example of using OpenCV dnn module with Human Segmentation model.
    /// Referring to https://github.com/opencv/opencv_zoo/tree/master/models/human_segmentation_pphumanseg.
    /// </summary>
    [RequireComponent(typeof(FfmpegToMatHelper))]
    public class HumanSegmentationFfmpegExample : MonoBehaviour
    {
        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The webcam texture to mat helper.
        /// </summary>
        FfmpegToMatHelper ffmpegToMatHelper;

        /// <summary>
        /// The rgb mat.
        /// </summary>
        Mat rgbMat;

        /// <summary>
        /// The mask mat.
        /// </summary>
        Mat maskMat;

        /// <summary>
        /// The net.
        /// </summary>
        Net net;

        /// <summary>
        /// The FPS monitor.
        /// </summary>
        FpsMonitor fpsMonitor;

        /// <summary>
        /// MODEL_FILENAME
        /// </summary>
        protected static readonly string MODEL_FILENAME = "OpenCVForUnity/dnn/human_segmentation_pphumanseg_2021oct.onnx";

        /// <summary>
        /// The model filepath.
        /// </summary>
        string model_filepath;

#if UNITY_WEBGL
        IEnumerator getFilePath_Coroutine;
#endif

        // Use this for initialization
        void Start()
        {
            fpsMonitor = GetComponent<FpsMonitor>();

            ffmpegToMatHelper = gameObject.GetComponent<FfmpegToMatHelper>();

#if UNITY_WEBGL
            getFilePath_Coroutine = GetFilePath();
            StartCoroutine(getFilePath_Coroutine);
#else
            model_filepath = Utils.getFilePath(MODEL_FILENAME);
            Run();
#endif
        }

#if UNITY_WEBGL
        private IEnumerator GetFilePath()
        {
            var getFilePathAsync_0_Coroutine = Utils.getFilePathAsync(MODEL_FILENAME, (result) =>
            {
                model_filepath = result;
            });
            yield return getFilePathAsync_0_Coroutine;

            getFilePath_Coroutine = null;

            Run();
        }
#endif

        // Use this for initialization
        void Run()
        {
            //if true, The error log of the Native side OpenCV will be displayed on the Unity Editor Console.
            Utils.setDebugMode(true);

            if (string.IsNullOrEmpty(model_filepath))
            {
                Debug.LogError(MODEL_FILENAME + " is not loaded. Please read “StreamingAssets/OpenCVForUnity/dnn/setup_dnn_module.pdf” to make the necessary setup.");
            }
            else
            {
                net = Dnn.readNet(model_filepath);
            }

//#if UNITY_ANDROID && !UNITY_EDITOR
//            // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
//            webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
//#endif
            ffmpegToMatHelper.Initialize();
        }

        /// <summary>
        /// Raises the webcam texture to mat helper initialized event.
        /// </summary>
        public void OnFfmpegToMatHelperInitialized()
        {
            Debug.Log("OnFfmpegToMatHelperInitialized");

            Mat ffmpegMat = ffmpegToMatHelper.GetMat();

            if (texture != null)
            {
                Texture2D.Destroy(texture);
                texture = null;
            }
            texture = new Texture2D(ffmpegMat.cols(), ffmpegMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(ffmpegMat, texture);

            gameObject.GetComponent<Renderer>().material.mainTexture = texture;

            gameObject.transform.localScale = new Vector3(ffmpegMat.cols(), ffmpegMat.rows(), 1);
            Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

            if (fpsMonitor != null)
            {
                fpsMonitor.Add("width", ffmpegMat.width().ToString());
                fpsMonitor.Add("height", ffmpegMat.height().ToString());
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

            rgbMat = new Mat(ffmpegMat.rows(), ffmpegMat.cols(), CvType.CV_8UC3);
            maskMat = new Mat(ffmpegMat.rows(), ffmpegMat.cols(), CvType.CV_8UC1);

        }

        /// <summary>
        /// Raises the webcam texture to mat helper disposed event.
        /// </summary>
        public void OnFfmpegToMatHelperDisposed()
        {
            Debug.Log("OnFfmpegToMatHelperDisposed");

            if (rgbMat != null)
                rgbMat.Dispose();

            if (maskMat != null)
                maskMat.Dispose();

            //if (texture != null)
            //{
            //    Texture2D.Destroy(texture);
            //    texture = null;
            //}
        }

        /// <summary>
        /// Raises the webcam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnFfmpegToMatHelperErrorOccurred(FfmpegToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnFfmpegToMatHelperErrorOccurred " + errorCode);
        }

        // Update is called once per frame
        void Update()
        {
            if (ffmpegToMatHelper.IsPlaying() && ffmpegToMatHelper.DidUpdateThisFrame())
            {

                Mat rgbaMat = ffmpegToMatHelper.GetMat();

                if (net == null)
                {
                    Imgproc.putText(rgbaMat, "model file is not loaded.", new Point(5, rgbaMat.rows() - 30), Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                    Imgproc.putText(rgbaMat, "Please read console message.", new Point(5, rgbaMat.rows() - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 0.7, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                }
                else
                {

                    Imgproc.cvtColor(rgbaMat, rgbMat, Imgproc.COLOR_RGBA2RGB);


                    Mat blob = Dnn.blobFromImage(rgbMat, 1.0 / 255.0, new Size(192, 192), new Scalar(0.5, 0.5, 0.5), false, false, CvType.CV_32F);
                    // Divide blob by std.
                    Core.divide(blob, new Scalar(0.5, 0.5, 0.5), blob);


                    net.setInput(blob);

                    Mat prob = net.forward("save_infer_model/scale_0.tmp_1");

                    Mat result = new Mat();
                    Core.reduceArgMax(prob, result, 1);
                    //result.reshape(0, new int[] { 192,192});
                    result.convertTo(result, CvType.CV_8U);
                    //Debug.Log("result.ToString(): " + result.ToString());


                    Mat mask192x192 = new Mat(192, 192, CvType.CV_8UC1, (IntPtr)result.dataAddr());
                    Imgproc.resize(mask192x192, maskMat, rgbaMat.size(), Imgproc.INTER_NEAREST);

                    rgbaMat.setTo(new Scalar(255, 255, 255,255), maskMat);

                    mask192x192.Dispose();
                    result.Dispose();

                    prob.Dispose();
                    blob.Dispose();

                }

                Utils.matToTexture2D(rgbaMat, texture);
            }
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        void OnDestroy()
        {
            ffmpegToMatHelper.Dispose();

            if (net != null)
                net.Dispose();

            Utils.setDebugMode(false);

#if UNITY_WEBGL
            if (getFilePath_Coroutine != null)
            {
                StopCoroutine(getFilePath_Coroutine);
                ((IDisposable)getFilePath_Coroutine).Dispose();
            }
#endif
        }

        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnBackButtonClick()
        {
            SceneManager.LoadScene("FfmpegWithOpenCVForUnityExample");
        }

        /// <summary>
        /// Raises the play button click event.
        /// </summary>
        public void OnPlayButtonClick()
        {
            ffmpegToMatHelper.Play();
        }

        /// <summary>
        /// Raises the pause button click event.
        /// </summary>
        public void OnPauseButtonClick()
        {
            ffmpegToMatHelper.Pause();
        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnStopButtonClick()
        {
            ffmpegToMatHelper.Stop();
        }

        /// <summary>
        /// Raises the change camera button click event.
        /// </summary>
        public void OnChangeCameraButtonClick()
        {
            //webCamTextureToMatHelper.requestedIsFrontFacing = !webCamTextureToMatHelper.requestedIsFrontFacing;
        }

    }
}
#endif

#endif