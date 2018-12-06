using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public Transform player;
	public LayerMask unwalkableMask;//variable of LayerMask type that stores a layer
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;//2d array of node type
	float nodeDiameter;
	int gridSizeX;
	int gridSizeY;
	//size of grid in nodes
	void Start()
	{
		nodeDiameter = 2 * nodeRadius;
		gridSizeX =Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY =Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}
	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft=transform.position-Vector3.right*gridWorldSize.x/2-Vector3.forward *gridWorldSize.y/2;

		for (int x=0;x<gridSizeX;x++)
		{
			for(int y=0;y<gridSizeY;y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
				grid[x, y] = new Node(walkable, worldPoint,x,y);
			}
		}
	}
	public List<Node> GetNeighbours(Node node)//for scanning nodes from bottom left of node
	{
		List<Node> neighbours = new List<Node>();
		for(int x=-1; x<=1;x++)
		{
			for(int y=-1;y<=1;y++)
			{
				if(x==0 && y==0)//skip centre of node search
				{
					continue;
				}
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX>=0 && checkX<gridSizeX && checkY>=0 && checkY<gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}
	public Node NodeFromWorldPoint(Vector3 worldPosition)//used to return a 
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;//check y and z values later
		percentX = Mathf.Clamp01(percentX);// keeps the percentage between 0 and 1
		percentY = Mathf.Clamp01(percentY);//keeps the percentage between 0 and 1
		//clamping is done as a way of exception handling in case we get a value outside the grid for whatever reason
		int y = Mathf.RoundToInt((gridSizeY-1)*percentY);
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);//to make sure we're not outside the array
		//this'll become more clear once you try out the maths used above on paper
		return grid[x,y];
	}
	public List<Node> path;
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if(grid!=null)
		{
			Node playerNode = NodeFromWorldPoint(player.position);
			foreach(Node n in grid)
			{
				if(n.walkable)
				{
					Gizmos.color = Color.white;
				}
				else
				{
					Gizmos.color = Color.red;
				}
				if(playerNode==n)
				{
					Gizmos.color = Color.cyan;
				}
				if(path!=null)
				{
					if(path.Contains(n))
					{
						Gizmos.color = Color.black;
					}

				}
				Gizmos.DrawCube(n.worldposition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}
