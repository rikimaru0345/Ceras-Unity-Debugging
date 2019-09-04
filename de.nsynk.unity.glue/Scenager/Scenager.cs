using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Glue
{  
    [Serializable]
    [AddComponentMenu("Glue/Scenager [Glue]")]
    [ExecuteAlways]
    public class Scenager : GlueBehaviour
    {
        public string root;
        public List<Scene> scenes;
        public bool overrideGlueInPlayMode;

        bool[] _default;
        bool[] _bools;
        int _count;

        void Start()
        {
            _count = scenes.Count;
            _bools = new bool[_count];
            for (int i = 0; i < _count; i++)
            {
                _bools[i] = scenes[i].IsLoaded;
                scenes[i]._previousState = _bools[i];
            }
            _default = _bools;
        }

        void Update()
        {
            root = gameObject.scene.path;

#if UNITY_EDITOR
            if (overrideGlueInPlayMode) return;
#endif

            if (Application.isPlaying)
            {
                _bools = GlueValue(_default);
                _default = _bools;
                int max = scenes.Count < _bools.Length
                    ? scenes.Count
                    : _bools.Length; 
                for (int i = 0; i < max ; i++)
                    scenes[i].LoadScene(_bools[i]);
            }
        }

        public void Check()
        {
            List<string> deleteCandidates = new List<string>();
#if UNITY_EDITOR
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
#else
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
#endif
            bool isCandidate = true;
            foreach (Scene scene in scenes)
            {
#if UNITY_EDITOR
                if (scene.ScenePath == EditorSceneManager.GetSceneAt(i).path || EditorSceneManager.GetSceneAt(i).path == root)
                {
#else
                if (scene.ScenePath == SceneManager.GetSceneAt(i).path || SceneManager.GetSceneAt(i).path == root)
                {
#endif
                    isCandidate = false;
                }     
                }
                if (isCandidate)
                {
#if UNITY_EDITOR
                deleteCandidates.Add(EditorSceneManager.GetSceneAt(i).path);
#else
                deleteCandidates.Add(SceneManager.GetSceneAt(i).path);
#endif
                }
            }

            foreach (string deleteCandidate in deleteCandidates)
            {
#if UNITY_EDITOR
                Debug.Log("Delete " + EditorSceneManager.GetSceneByPath(deleteCandidate).name);
                EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(deleteCandidate), true);
#else
                Debug.Log("Delete " + SceneManager.GetSceneByPath(deleteCandidate).name);
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByPath(deleteCandidate));
#endif
            }
        }
    }
}
