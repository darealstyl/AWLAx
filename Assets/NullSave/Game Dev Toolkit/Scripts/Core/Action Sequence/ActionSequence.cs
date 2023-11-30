//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using NullSave.GDTK.JSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence")]
    [AutoDoc("The **Action Sequence** component allows you to create a list of actions to be run on the attached Game Object or the supplied \"Remote Target\".\r\nThe *sequence* is a list of plug-ins run in order from first to last and can be extended to fit your needs.")]
    public class ActionSequence : MonoBehaviour
    {

        #region Fields

        [Tooltip("List of plugins to run in sequence")] public List<ActionSequenceWrapper> plugins;

        [Tooltip("Play sequence on start")] public bool playOnStart;
        [Tooltip("Play sequence on enable")] public bool playOnEnable;
        [Tooltip("GameObject to use as Remote Object")] public GameObject remoteTarget;

        [Tooltip("Event raised when sequence is complete")] public UnityEvent onComplete = new UnityEvent();

        private int index;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (playOnStart) Play();
        }

        private void OnEnable()
        {
            if (playOnEnable) Play();
        }

        private void FixedUpdate()
        {
            if (!isStarted) return;
            DoFixedUpdate();
        }

        private void Update()
        {
            if (!isStarted) return;
            DoUpdate();
        }

        #endregion

        #region Properties

        [AutoDoc("Get if current sequence is complete")] public bool isComplete { get; private set; }

        [AutoDoc("Get if a sequence has started")] public bool isStarted { get; private set; }

        #endregion

        #region Public Methods

        [AutoDoc("Load sequence from stream")]
        [AutoDocParameter("Stream used for loading")]
        public void DataLoad(Stream stream)
        {
            playOnStart = stream.ReadBool();
            int count = stream.ReadInt();
            plugins = new List<ActionSequenceWrapper>();
            for (int i = 0; i < count; i++)
            {
                ActionSequenceWrapper wrapper = new ActionSequenceWrapper();
                wrapper.DataLoad(stream);
                plugins.Add(wrapper);
            }
        }

        [AutoDoc("Save sequence to stream")]
        [AutoDocParameter("Stream used for saving")]
        public void DataSave(Stream stream)
        {
            stream.WriteBool(playOnStart);
            stream.WriteInt(plugins.Count);
            foreach (ActionSequenceWrapper wrapper in plugins)
            {
                wrapper.DataSave(stream);
            }
        }

        [AutoDoc("Export sequence to JSON")]
        public string ExportToJSON()
        {
            return SimpleJson.ToJSON(jsonActionSequence.FromModel(this));
        }

        [AutoDoc("Play sequence")]
        public void Play()
        {
            if (isStarted || plugins.Count == 0) return;
            isStarted = true;
            isComplete = false;

            index = 0;
            foreach (ActionSequenceWrapper item in plugins)
            {
                item.plugin.isComplete = false;
                item.plugin.isStarted = false;
            }

            plugins[0].plugin.StartAction(this);
        }

        [AutoDoc("Queue sequence and play once other sequences are complete")]
        public void QueuePlay()
        {
            StartCoroutine(WaitAndPlay());
        }

        [AutoDoc("Stop current sequence")]
        public void Stop()
        {
            if (!isStarted || isComplete) return;
            isStarted = false;
            isComplete = true;
        }

        #endregion

        #region Private Methods

        private void DoFixedUpdate()
        {
            plugins[index].plugin.FixedUpdateAction();
            if (plugins[index].plugin.isComplete)
            {
                UpdateIndex();
                if (isStarted)
                {
                    DoFixedUpdate();
                }
            }
        }

        private void DoUpdate()
        {
            plugins[index].plugin.UpdateAction();
            if (plugins[index].plugin.isComplete)
            {
                UpdateIndex();
                if (isStarted)
                {
                    DoFixedUpdate();
                }
            }
        }

        private void UpdateIndex()
        {
            index++;
            if (index >= plugins.Count)
            {
                isStarted = false;
                isComplete = true;
                onComplete?.Invoke();
            }
            else
            {
                plugins[index].plugin.StartAction(this);
                if (plugins[index].plugin.isComplete) UpdateIndex();
            }
        }

        private IEnumerator WaitAndPlay()
        {
            while (isStarted)
            {
                yield return new WaitForEndOfFrame();
            }

            Play();
        }

        #endregion

    }
}