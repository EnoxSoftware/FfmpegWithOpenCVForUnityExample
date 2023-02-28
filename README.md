# Ffmpeg With OpenCVForUnity Example

## Overview
- Integrate "FFmpeg for unity" with "OpenCV for Unity".
- Replace WebCamTextureToMatHelper with FfmpegToMatHelper.
- Video from a network camera (distributed via RTSP) is received by ffmpeg, converted to OpenCV's Mat class, and image processing is performed.


## Environment
- [ATOM Cam Swing](https://www.atomtech.co.jp/products/atomcamswing) + [atomcam_tools](https://github.com/mnakada/atomcam_tools)
- Windows / macOS / Linux / Android / iOS
- Unity >= 2021.1.0f1+
- Scripting backend MONO / IL2CPP
- [OpenCV for Unity](https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088?aid=1011l4ehR) 2.5.2+
- [FFmpeg for Unity](https://assetstore.unity.com/packages/tools/video/ffmpeg-for-unity-199811) 2.4
- [Runtime Inspector & Hierarchy](https://assetstore.unity.com/packages/tools/gui/runtime-inspector-hierarchy-111349) 1.7.0
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068#releases) 1.5.9


## Setup
1. Download the latest release unitypackage. [FfmpegWithOpenCVForUnityExample.unitypackage](https://github.com/EnoxSoftware/FfmpegWithOpenCVForUnityExample/releases)
1. Create a new project. (FfmpegWithOpenCVForUnityExample)
1. Import and Setup [OpenCV for Unity](https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088?aid=1011l4ehR).
    * Download Dnn model files by ExampleAssetsDownloader.
    ![download_dnn_models.png](download_dnn_models.png)
    * Move the files from the "OpenCVForUnity/StreamingAssets/" folder to the "Assets/StreamingAssets" folder.
    ![move_streamingassetsfolder.png](move_streamingassetsfolder.png)
1. Import and Setup [FFmpeg for Unity](https://assetstore.unity.com/packages/tools/video/ffmpeg-for-unity-199811).
1. Import [Runtime Inspector & Hierarchy](https://assetstore.unity.com/packages/tools/gui/runtime-inspector-hierarchy-111349).
1. Import [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068#releases).
1. Import [FfmpegWithOpenCVForUnityExample.unitypackage](https://github.com/EnoxSoftware/FfmpegWithOpenCVForUnityExample/releases).
1. Set the URL of RTSPServer you wish to receive.
    ![ffmpeg_rtsp_settings.png](ffmpeg_rtsp_settings.png)
1. Add the "Assets/FfmpegWithOpenCVForUnityExample/*.unity" files to the "Scenes In Build" list in the "Build Settings" window.
1. Build and Deploy.
    ![setup.png](setup.png)

## ScreenShot
![screenshot01.png](screenshot01.png)
![screenshot02.png](screenshot02.png)
