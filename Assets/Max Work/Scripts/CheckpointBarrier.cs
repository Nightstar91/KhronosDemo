using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System.Linq;

namespace CheckPointScript
{
    public class CheckpointBarrier : MonoBehaviour
    {
        public enum Lap
        {
            First,
            Second,
            Third,
            Fourth
        }

        [SerializeField] public Lap whichLap;
        [SerializeField] public bool triggerOnce = false;

        [Header("Checkpoint Times")]
        List<float> CheckpointTime = new List<float>();
        float[] CheckpointBest;
        float checkpointTimer;

        LevelObjective levelObjective;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
        {
            levelObjective = GameObject.Find("LevelObjectiveController").GetComponent<LevelObjective>();
        }

        private void Update()
        {
            ControlTimer(true, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (triggerOnce) return;

            if (whichLap == Lap.First)
            {
                CheckpointTime.Add(levelObjective.levelTimer);
                ControlTimer(true, false);
                triggerOnce = true;
            }
            else
            {
                CheckpointTime.Add(checkpointTimer);
                ControlTimer(false, true);
                triggerOnce = true;
            }
        }

        private void ControlTimer(bool firstTimer, bool newTimer)
        {
            if (firstTimer || !newTimer)
            {
                checkpointTimer += 1 * Time.deltaTime;
            }
            else if (checkpointTimer < 0)
            {
                checkpointTimer = 0;
                return;
            }
            else
            {
                checkpointTimer = 0 * Time.deltaTime;
                ControlTimer(false, false);
            }
        }
    }
}

