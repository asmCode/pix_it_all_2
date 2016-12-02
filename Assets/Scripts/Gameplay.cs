using UnityEngine;
using System.Collections;

public class Gameplay
{
    public ImageData Image
    {
        get;
        private set;
    }

    public void Init()
    {
        Image = Game.GetInstance().ImageManager.GetImageById("wrotki");
    }
}
