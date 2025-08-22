using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityIntegration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FfmpegWithOpenCVForUnityExample
{
    public class FfmpegWithOpenCVForUnityExample : MonoBehaviour
    {
        // Constants
        private static float VERTICAL_NORMALIZED_POSITION = 1f;

        // Public Fields
        public Text VersionInfo;
        public ScrollRect ScrollRect;

        // Unity Lifecycle Methods
        private void Start()
        {
            VersionInfo.text = Core.NATIVE_LIBRARY_NAME + " " + OpenCVEnv.GetVersion() + " (" + Core.VERSION + ")";
            VersionInfo.text += " / UnityEditor " + Application.unityVersion;
            VersionInfo.text += " / ";

#if UNITY_EDITOR
            VersionInfo.text += "Editor";
#elif UNITY_STANDALONE_WIN
            VersionInfo.text += "Windows";
#elif UNITY_STANDALONE_OSX
            VersionInfo.text += "Mac OSX";
#elif UNITY_STANDALONE_LINUX
            VersionInfo.text += "Linux";
#elif UNITY_ANDROID
            VersionInfo.text += "Android";
#elif UNITY_IOS
            VersionInfo.text += "iOS";
#elif UNITY_VISIONOS
            VersionInfo.text += "VisionOS";
#elif UNITY_WSA
            VersionInfo.text += "WSA";
#elif UNITY_WEBGL
            VersionInfo.text += "WebGL";
#endif
            VersionInfo.text += " ";
#if ENABLE_MONO
            VersionInfo.text += "Mono";
#elif ENABLE_IL2CPP
            VersionInfo.text += "IL2CPP";
#elif ENABLE_DOTNET
            VersionInfo.text += ".NET";
#endif

            ScrollRect.verticalNormalizedPosition = VERTICAL_NORMALIZED_POSITION;
        }

        private void Update()
        {

        }

        // Public Methods
        public void OnScrollRectValueChanged()
        {
            VERTICAL_NORMALIZED_POSITION = ScrollRect.verticalNormalizedPosition;
        }

        public void OnShowSystemInfoButtonClick()
        {
            SceneManager.LoadScene("ShowSystemInfo");
        }

        public void OnShowLicenseButtonClick()
        {
            SceneManager.LoadScene("ShowLicense");
        }

        public void OnFfmpegAsyncGPUReadback2MatHelperExampleButtonClick()
        {
            SceneManager.LoadScene("FfmpegAsyncGPUReadback2MatHelperExample");
        }

        public void OnObjectDetectionYOLOXExampleButtonClick()
        {
            SceneManager.LoadScene("ObjectDetectionYOLOXExample");
        }

        public void OnHumanSegmentationPPHumanSegExampleButtonClick()
        {
            SceneManager.LoadScene("HumanSegmentationPPHumanSegExample");
        }

    }
}
