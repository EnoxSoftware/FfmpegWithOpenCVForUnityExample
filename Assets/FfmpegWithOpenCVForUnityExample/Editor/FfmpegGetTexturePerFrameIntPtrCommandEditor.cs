using FfmpegUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegWithOpenCVForUnity.UnityUtils.Helper
{
    [CustomEditor(typeof(FfmpegGetTexturePerFrameIntPtrCommandCustom))]
    public class FfmpegGetTexturePerFrameIntPtrCommandCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegPlayerCommand ffmpegPlayerCommand = (FfmpegPlayerCommand)target;

            ffmpegPlayerCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegPlayerCommand.ExecuteOnStart);

            EditorGUILayout.LabelField("Input Options");
            ffmpegPlayerCommand.InputOptions = EditorGUILayout.TextArea(ffmpegPlayerCommand.InputOptions);

            ffmpegPlayerCommand.DefaultPath = (FfmpegPath.DefaultPath)EditorGUILayout.EnumPopup("Default Path", ffmpegPlayerCommand.DefaultPath);
            ffmpegPlayerCommand.InputPath = EditorGUILayout.TextField("Input Path", ffmpegPlayerCommand.InputPath);

            ffmpegPlayerCommand.AutoStreamSettings = EditorGUILayout.Toggle("Auto Settings", ffmpegPlayerCommand.AutoStreamSettings);
            if (!ffmpegPlayerCommand.AutoStreamSettings && !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                if (ffmpegPlayerCommand.Streams == null || ffmpegPlayerCommand.Streams.Length < 2)
                {
                    ffmpegPlayerCommand.Streams = new FfmpegPlayerCommand.FfmpegStream[] {
                        new FfmpegPlayerCommand.FfmpegStream()
                        {
                            CodecType = FfmpegPlayerCommand.FfmpegStream.Type.VIDEO,
                            Width = 640,
                            Height = 480,
                        }
                    };
                }

                Vector2Int videoSize = EditorGUILayout.Vector2IntField("Video Size",
                    new Vector2Int(ffmpegPlayerCommand.Streams[0].Width, ffmpegPlayerCommand.Streams[0].Height));
                ffmpegPlayerCommand.Streams[0].Width = videoSize.x;
                ffmpegPlayerCommand.Streams[0].Height = videoSize.y;

                ffmpegPlayerCommand.FrameRate = EditorGUILayout.FloatField("Video Frame Rate", ffmpegPlayerCommand.FrameRate);
            }

            EditorGUILayout.LabelField("Options");
            ffmpegPlayerCommand.PlayerOptions = EditorGUILayout.TextArea(ffmpegPlayerCommand.PlayerOptions);

            ffmpegPlayerCommand.VideoBuffersCount = EditorGUILayout.IntField("Video Outputs", ffmpegPlayerCommand.VideoBuffersCount);

            ffmpegPlayerCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegPlayerCommand.PrintStdErr);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegPlayerCommand);
            }
        }
    }
}
