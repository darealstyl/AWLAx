//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDoc("The **Audio Pool** component allows you to create a pool of audio sources to be used globally by your components via the [Broadcast System](../broadcast-system/introduction).")]
    public class AudioPool : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [AutoDocAs("Channel Name")] [Tooltip("Broadcaster channel name to listen to")] public string channelName;
        [AutoDocAs("Template")] [Tooltip("Template to use when creating new audio sources")] public AudioSource template;

        private List<AudioSource> activeSources;
        private List<object> ignoreRequestsFrom;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            ignoreRequestsFrom = new List<object>();
        }

        private void OnDisable()
        {
            Broadcaster.UnsubscribeFromAll(this);
        }

        private void OnEnable()
        {
            Broadcaster.SubscribeToChannel(this, channelName);
        }

        private void Start()
        {
            activeSources = new List<AudioSource>();
        }

        private void Reset()
        {
            channelName = "Audio";

            template = GetComponentInChildren<AudioSource>();
            if(template == null)
            {
                GameObject go = new GameObject("Template");
                go.transform.SetParent(transform);
                template = go.AddComponent<AudioSource>();
                template.loop = false;
                template.playOnAwake = false;
                go.SetActive(false);
            }
        }

        #endregion

        #region Public Methods

        [AutoDocSuppress]
        public void BroadcastReceived(object sender, string channel, string message, object[] args)
        {
            if (channel != channelName || args.Length == 0 || args[0] == null) return;

            if (ignoreRequestsFrom.Contains(sender)) return;

            foreach(AudioSource audioSource in activeSources)
            {
                if(!audioSource.isPlaying)
                {
                    // We already have a free source
                    PlaySource(audioSource.gameObject, audioSource, sender, args);
                    return;
                }
            }

            // Add source to pool
            GameObject instance = InterfaceManager.ObjectManagement.InstantiateObject(template.gameObject, transform);
            AudioSource audio = instance.GetComponent<AudioSource>();
            instance.AddComponent<DisableAfterAudioPlayed>();
            activeSources.Add(audio);
            PlaySource(instance, audio, sender, args);
        }

        [AutoDoc("Play audio in pool")]
        [AutoDocParameter("Clip to play")]
        public void PlayAudio(AudioClip clip)
        {
            GameObject instance = InterfaceManager.ObjectManagement.InstantiateObject(template.gameObject, transform);
            AudioSource audio = instance.GetComponent<AudioSource>();
            instance.AddComponent<DisableAfterAudioPlayed>();
            activeSources.Add(audio);
            audio.clip = clip;
            audio.Play();
        }

        [AutoDocSuppress]
        public void PublicBroadcastReceived(object sender, string message) { }

        #endregion

        #region Private Methods

        private void PlaySource(GameObject go, AudioSource audio, object sender, object[] args)
        {
            if(args[1] != null)
            {
                go.transform.position = (Vector3)args[1];
            }

            go.SetActive(true);
            audio.clip = args[0] as AudioClip;
            audio.Play();

            if (args.Length >= 3 && (bool)args[2])
            {
                ignoreRequestsFrom.Add(sender);
                StartCoroutine(WaitForComplete(audio, sender));
            }
        }

        private IEnumerator WaitForComplete(AudioSource audio, object remove)
        {
            yield return new WaitForEndOfFrame();
            while(audio.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
            ignoreRequestsFrom.Remove(remove);
        }

        #endregion

    }
}