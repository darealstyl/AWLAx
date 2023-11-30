//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("action-sequence/support-classes")]
    [AutoDoc("Helper class for running multiple action sequences from a single component")]
    public class SequenceManager
    {

        #region Fields

        [Tooltip("Event raised when an action sequence completes")] public SequenceComplete onSequenceComplete;
        [Tooltip("Object to use as remote target")] public GameObject remoteTarget;

        private List<ActionSequenceList> sequences;

        private ActionSequence sequenceRunner;
        private int index;
        private bool m_isPlaying;

        #endregion

        #region Properties

        [AutoDoc("Gets if the Sequence Manager is paused")]
        public bool isPaused { get; private set; }

        [AutoDoc("Gets if the Sequence Manager is playing")]
        public bool isPlaying
        {
            get
            {
                if(m_isPlaying && index >= sequences.Count)
                {
                    m_isPlaying = false;
                    if(sequenceRunner.isStarted)
                    {
                        sequenceRunner.Stop();
                    }
                }
                return m_isPlaying;
            }
            private set
            {
                m_isPlaying = value;
            }
        }

        [AutoDoc("Gets/Sets transform to set as Sequence Manager's parent")]
        public Transform parentTo { get; set; }

        [AutoDoc("Gets the number of sequences to run")]
        public int sequenceCount { get { return sequences.Count; } }

        #endregion

        #region Constructor

        public SequenceManager()
        {
            sequences = new List<ActionSequenceList>();
        }

        public SequenceManager(List<ActionSequenceList> sequenceList)
        {
            sequences = new List<ActionSequenceList>();
            sequences.AddRange(sequenceList);
        }

        public SequenceManager(ActionSequenceList[] sequenceList)
        {
            sequences = new List<ActionSequenceList>();
            sequences.AddRange(sequenceList);
        }

        #endregion

        #region Public Methods

        [AutoDoc("Add a new sequence to the list")]
        [AutoDocParameter("Sequence to add to the list")]
        public void AddSequence(ActionSequenceList sequence)
        {
            sequences.Add(sequence);
        }

        [AutoDoc("Clear sequence list")]
        public void Clear()
        {
            if (isPlaying)
            {
                return;
            }

            sequences.Clear();
            index = 0;
        }

        [AutoDoc("Pause playing sequences")]
        public void Pause()
        {
            if (!isPlaying) return;
            isPaused = true;
        }

        [AutoDoc("Play sequences")]
        public void Play()
        {
            if (isPlaying) return;

            if (sequenceRunner == null)
            {
                GameObject go = new GameObject("GDTK_SequenceManager");
                go.transform.SetParent(parentTo);
                go.transform.localPosition = Vector3.zero;
                go.hideFlags = HideFlags.HideAndDontSave;
                sequenceRunner = go.AddComponent<ActionSequence>();
            }

            sequenceRunner.remoteTarget = remoteTarget;

            isPlaying = true;

            index = 0;
            sequenceRunner.onComplete.AddListener(SequenceComplete);
            if (sequences[0] != null)
            {
                sequences[0].ApplyTo(sequenceRunner);
                sequenceRunner.Play();
            }
            else
            {
                SequenceComplete();
            }
        }

        [AutoDoc("Resume playing sequences")]
        public void Resume()
        {
            if (!isPaused) return;
            isPaused = false;
            if (sequences[index] != null)
            {
                sequences[index].ApplyTo(sequenceRunner);
                sequenceRunner.Play();
            }
            else
            {
                SequenceComplete();
            }
        }

        [AutoDoc("Stop playing sequences")]
        public void Stop()
        {
            if (!isPlaying) return;
            isPlaying = false;
            sequenceRunner.Stop();
        }

        #endregion

        #region Private Methods

        private void SequenceComplete()
        {
            if (index + 1 >= sequences.Count)
            {
                isPlaying = false;
                sequenceRunner.onComplete.RemoveListener(SequenceComplete);
                onSequenceComplete?.Invoke(index);
            }
            else
            {
                onSequenceComplete?.Invoke(index++);
                if (!isPaused)
                {
                    if (sequences[index] != null)
                    {
                        sequences[index].ApplyTo(sequenceRunner);
                        sequenceRunner.Play();
                    }
                    else
                    {
                        SequenceComplete();
                    }
                }
            }
        }

        #endregion

    }
}
