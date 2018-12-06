using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
	public bool walkable;//walkable or not
	public Vector3 worldposition;//xyz coordinates of the node
	public int gridX;
	public int gridY;
	public Node parent;

	public int gCost;
	public int hCost;



	public Node(bool walk,Vector3 worldpos,int _gridX,int _gridY)
	{
		walkable = walk;
		worldposition = worldpos;
		gridX = _gridX;
		gridY = _gridY;
	}


	public int fCost//try using simple variable assignment later.
	{
		get
		{
			return gCost + hCost;
		}
	}
}
