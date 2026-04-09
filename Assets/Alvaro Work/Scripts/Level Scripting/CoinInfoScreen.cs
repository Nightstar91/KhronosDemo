using TMPro;
using UnityEngine;

public class CoinInfoScreen : MonoBehaviour
{
    [SerializeField] public TextMeshPro info;
    [SerializeField] public GameObject gateWithCoinInfo;
    private ObjectiveGate gate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info = gameObject.GetComponentInChildren<TextMeshPro>();
        gate = gateWithCoinInfo.GetComponent<ObjectiveGate>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gate.allCoin > 0)
        {
            info.text = string.Format("Coin Remaining: {0}", gate.allCoin);
        }
        else
        {
            info.text = string.Format("Gate is now open");
        }
    }
}
