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
	void CreateGrid()//fills nodes in the grid
	{
		grid = new Node[gridSizeX, gridSizeY];//values taken from Inspector
		Vector3 worldBottomLeft=transform.position-Vector3.right*gridWorldSize.x/2-Vector3.forward *gridWorldSize.y/2;

		for (int x=0;x<gridSizeX;x++)
		{
			for(int y=0;y<gridSizeY;y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				//Vector3.right is a way of writing (1,0,0) and Vector3.forward is (0,0,1) since we're moving in the x-z plane
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
				//Physics.CheckSphere(centre,search_radius,layer) returns 1 if layer found within radius
				grid[x, y] = new Node(walkable, worldPoint,x,y);//assign a node to the position
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
				//adding coordinate to reference node for searching
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX>=0 && checkX<gridSizeX && checkY>=0 && checkY<gridSizeY)//checks if the node being searched exists in the grid or not
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}
	public Node NodeFromWorldPoint(Vector3 worldPosition)//used to convert an xyz coordinate to a node coordinate 
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
	public List<Node> path;//values initialised in Pathfinding.cs script
	void OnDrawGizmos()//used to visualise the algorithm's working in 3d space
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));//draws the boundary of the grid
		if(grid!=null)// Checks if nodes are present in grid or not
		{
			Node playerNode = NodeFromWorldPoint(player.position);//takes player's xyz coordinate converts to node coordinate and assigns to a variable
			foreach(Node n in grid) //a variation of for loop where you can simply traverse through game objects
			{
				//setting color of nodes
				if(n.walkable)//walkable node= white
				{
					Gizmos.color = Color.white;
				}
				else// non walkable node =red
				{
					Gizmos.color = Color.red;
				}
				if(playerNode==n)//player position node = cyan
				{
					Gizmos.color = Color.cyan;
				}
				if(path!=null)//path colour = black
				{
					if(path.Contains(n))
					{
						Gizmos.color = Color.black;
					}

				}
				Gizmos.DrawCube(n.worldposition, Vector3.one * (nodeDiameter - 0.1f));//draw a node cube of corresponding color
			}
		}
	}
}//end of grid class
