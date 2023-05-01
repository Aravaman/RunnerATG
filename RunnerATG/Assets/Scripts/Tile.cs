using UnityEngine;
using System;
using System.Collections.Generic;


public class Tile
{
	public float HeightValue { get; set; }
	public int X, Y;

	public Tile Left;
	public Tile Right;
	public Tile Top;
	public Tile Bottom;

	public Tile()
	{
	}
}
