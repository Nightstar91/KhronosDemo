using UnityEngine;
using FMODUnity;

public class DefaultMusicLevelScript : MonoBehaviour
{

    [FMODUnity.ParamRef] public string fmodParameterName;


    void Start()
    {
            RuntimeManager.StudioSystem.setParameterByName(fmodParameterName, 0.75f);
    }

}