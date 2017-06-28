using System;

[Serializable]
public class ImageStepTileCoords
{
	public byte X { get; set; }
	public byte Y { get; set; }

	public ImageStepTileCoords(byte x, byte y)
	{
		X = x;
		Y = y;
	}
}
