using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeDoor : MonoBehaviour
{
    public Material matGreen;
    public Material matRed;
    public GameObject door;
    public MeshRenderer doorColor;
    public Material[] mat;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        doorColor = door.GetComponent<MeshRenderer>();
        mat = doorColor.materials;
        mat[0] = matGreen;
        mat[1] = matRed;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.hasKey == true)
        {
            doorColor.material = mat[0];
        }
        if(!player.hasKey)
        {
            doorColor.material = mat[1];
        }
        print(player.hasKey);
    }
    
}
