using UnityEngine;
using UnityEngine.UI;

public class AttachProgressUI : MonoBehaviour
{
    public Image fillImage;
    public PlayerAttachController player;
    public GameObject barPanel;

    void Start()
    {
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.GetComponent<PlayerAttachController>();

        
        barPanel.SetActive(true);
    }

    void Update()
    {
       
        fillImage.fillAmount = player.attachProgress;
    }
}
