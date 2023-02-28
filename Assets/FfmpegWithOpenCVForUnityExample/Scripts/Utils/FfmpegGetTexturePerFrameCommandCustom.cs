using FfmpegUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegWithOpenCVForUnity.UnityUtils.Helper
{

    public class FfmpegGetTexturePerFrameCommandCustom : FfmpegGetTexturePerFrameCommand
    {
        public new bool WriteNextTexture()
        {
            return base.WriteNextTexture();
        }
    }
}