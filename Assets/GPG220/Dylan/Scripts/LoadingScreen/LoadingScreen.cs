using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen instance;
        public GameObject loadingScreen;
        public Slider bar;
        
        private void Awake()
        {
            instance = this;

            SceneManager.LoadSceneAsync((int) SceneIndexes.Title_Scene, LoadSceneMode.Additive);
        }

        List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
        
        public void LoadGame()
        {
            loadingScreen.SetActive(true);
            
            sceneLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndexes.Title_Scene));
            sceneLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndexes.Level, LoadSceneMode.Additive));

            StartCoroutine(GetSceneLoadProgress());
            //used for initialization stuff
            //StartCoroutine(GetTotalProgress());
        }

        private float totalSceneProgress;
        private float totalSpawnProgress;
        public IEnumerator GetSceneLoadProgress()
        {
            for (int i = 0; i < sceneLoading.Count; i++)
            {
                while (!sceneLoading[i].isDone)
                {
                    totalSceneProgress = 0;

                    foreach (AsyncOperation operation in sceneLoading)
                    {
                        totalSceneProgress += operation.progress;
                    }

                    totalSceneProgress = (totalSceneProgress / sceneLoading.Count) * 100f;

                    //comment the bar.value out if using second coroutine
                    bar.value = Mathf.RoundToInt(totalSceneProgress);
                    
                    yield return null;
                }
            }
            //comment out if using second coroutine
            loadingScreen.SetActive(false);
            
        }

        public IEnumerator GetTotalProgress()
        {
            float totalProgress = 0;

            while (Initialization.current == null || Initialization.current.isDone)
            {
                if (Initialization.current == null)
                {
                    totalSpawnProgress = 0;
                }
                else
                {
                    totalSpawnProgress = Mathf.Round(Initialization.current.progress * 100f);
                }

                totalProgress = Mathf.Round((totalSceneProgress + totalSpawnProgress) / 2f);
                bar.value = Mathf.RoundToInt(totalProgress);

                
            }
            
            loadingScreen.SetActive(false);
            
            yield return null;
        }
    }
}
