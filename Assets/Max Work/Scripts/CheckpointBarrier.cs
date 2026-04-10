using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System.Linq;

public class CheckpointBarrier : MonoBehaviour
{
    [Header("Checkpoint Times")]
    List<float> CheckpointTime = new List<float>();
    float[] CheckpointBest;
    float checkpointTimer;

    GameObject Checkpoint;
    LevelObjective levelObjective;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelObjective = GameObject.Find("LevelObjectiveController").GetComponent<LevelObjective>();
        Checkpoint.GetComponent<GameObject>();
    }

    private void Update()
    {
        ControlTimer(true, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (Checkpoint.name.Contains("1"))
        {
            CheckpointTime.Add(levelObjective.levelTimer);
            ControlTimer(true, false);
        }
        else
        {
            CheckpointTime.Add(checkpointTimer);
            ControlTimer(false, true);
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
