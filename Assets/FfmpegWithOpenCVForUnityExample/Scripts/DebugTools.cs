using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegWithOpenCVForUnityExample
{

    public class DebugTools : MonoBehaviour
    {

        public GameObject runtimeHierarchy;

        public GameObject runtimeInspector;

        public GameObject defaultInspectedGameObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Raises the stop button click event.
        /// </summary>
        public void OnDebugButtonClick()
        {
            if(runtimeHierarchy)
                runtimeHierarchy.SetActive(!runtimeHierarchy.activeSelf);
            if (runtimeInspector)
                runtimeInspector.SetActive(!runtimeInspector.activeSelf);

            if(defaultInspectedGameObject)
                runtimeInspector.GetComponent<RuntimeInspectorNamespace.RuntimeInspector>().Inspect(defaultInspectedGameObject);
        }
    }
}
