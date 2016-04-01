using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	[System.Serializable]
	public class Cell {  
		public bool visited;   
		public GameObject east;  
		public GameObject west;  
		public GameObject south;  
		public GameObject north;

		public float x;
		public float y;

		public Cell (){  
			east = null;  
			west = null;  
			south = null;  
			north = null;

			x = 0;
			y = 0;
		}  
	}  
		
	//for closed grid
	public GameObject wall;
	public float wall_lenght;
	public int x_size;
	public int y_size;
	private Vector3 init_pos;
	private Transform wall_holder;

	//maze
	private Cell[] cells;
	private int total_cells;
	private bool start_building;

	private int current_cell;
	private int current_neighbour;
	private int visited_cells;

	private List<int> last_cells;
	private int backing_up;

	private int wall_to_break;

	//start and goal 
	public GameObject goal;
	public GameObject start;

	public GameObject player;

	//enemey
	[System.Serializable]
	public class EnemySettings{
		public GameObject enemy;
		public int max_spawn;
		public float start_wait;
		public float spawn_rate;
		public Transform enemy_holder;
	}

	public EnemySettings enemy_settings;

	void Start () {
		CreateBoard ();
		CreateWalls ();
		CreateMaze ();
		StartCoroutine(SpawnEnemies ());

		PutStartAndEndPoint ();
	}

	void CreateBoard (){
		GameObject board = GameObject.Find ("Board");
		board.transform.position = new Vector3 (wall_lenght / 2.0f, -0.5f, wall_lenght / 2.0f);
		board.transform.localScale = new Vector3(x_size * wall_lenght, 1.0f, y_size * wall_lenght);
	}

	void CreateWalls(){
		GameObject board = GameObject.Find ("Board");
		wall_holder = board.transform;

		init_pos = new Vector3 ((-x_size / 2.0f) * wall_lenght + wall_lenght / 2.0f, 0.0f, 
			(-y_size / 2.0f) * wall_lenght + wall_lenght / 2.0f);
		Vector3 pos;
		GameObject temp_wall;

		//x
		for (int i = 0; i < y_size; i++) {
			for (int j = 0; j <= x_size; j++) {
				pos = new Vector3 (init_pos.x + j * wall_lenght, 0.0f, init_pos.z + i * wall_lenght  + wall_lenght / 2.0f);
				temp_wall = Instantiate (wall, pos, Quaternion.identity) as GameObject;
				temp_wall.transform.SetParent (wall_holder);
			}
		}

		//y
		for (int i = 0; i <= y_size; i++) {
			for (int j = 0; j < x_size; j++) {
				pos = new Vector3 (init_pos.x + j * wall_lenght + wall_lenght / 2.0f, 0.0f, init_pos.z + i * wall_lenght);
				temp_wall = Instantiate (wall, pos, Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f))) as GameObject;
				temp_wall.transform.SetParent (wall_holder);
			}
		}

		createCells ();
	}

	void createCells(){
		cells = new Cell[x_size * y_size];

		int children = wall_holder.childCount;
		GameObject[] all_walls = new GameObject[children];

		int east_west_process = 0;
		int child_process = 0;

		//get all child
		for(int i = 0; i < children; i++){
			all_walls [i] = wall_holder.GetChild (i).gameObject;
		}

		//assign walls to cells
		for(int cell_process = 0; cell_process < cells.Length; cell_process++){
			cells [cell_process] = new Cell ();
			cells [cell_process].west = all_walls [east_west_process + east_west_process / x_size];
			cells [cell_process].south = all_walls [child_process + (x_size + 1) * y_size];

			east_west_process++;
			child_process++;

			cells[cell_process].east = all_walls [east_west_process + (east_west_process - 1) / x_size];
			cells[cell_process].north = all_walls [(child_process + (x_size + 1) * y_size) + x_size - 1];

			cells [cell_process].x = cells [cell_process].north.transform.position.x;
			cells [cell_process].y = cells [cell_process].east.transform.position.z;
		}
	}

	void CreateMaze(){
		total_cells = x_size * y_size;
		last_cells = new List<int>();

		start_building = false;
		visited_cells = 0;

		while (visited_cells < total_cells) {
			if (start_building) {
				GiveMeNeighbour ();

				if (cells [current_neighbour].visited == false &&
					cells[current_cell].visited == true) {

					BreakWall ();

					cells[current_neighbour].visited = true;
					visited_cells++;
					last_cells.Add (current_cell);
					current_cell = current_neighbour;

					if (last_cells.Count > 0) {
						backing_up = last_cells.Count - 1;
					}
				}
			} else {
				current_cell = Random.Range (0, total_cells);
				cells [current_cell].visited = true;
				visited_cells++;
				start_building = true;
			}
		}
	}

	void BreakWall(){
		switch(wall_to_break){
		case 1:
			Destroy (cells [current_cell].east);
			break;
		case 2:
			Destroy (cells [current_cell].west);
			break;
		case 3:
			Destroy (cells [current_cell].south);
			break;
		case 4:
			Destroy (cells [current_cell].north);
			break;

		}
	}

	void GiveMeNeighbour(){
		int length = 0;
		int[] neighbours = new int[4];
		int[] connecting_wall = new int[4];

		int check = 0;
		check = (current_cell + 1) / x_size;
		check -= 1;
		check *= x_size;
		check += x_size;

		//east
		if(current_cell + 1 < total_cells && (current_cell + 1) != check){
			if (cells [current_cell + 1].visited == false) {
				neighbours [length] = current_cell + 1;
				connecting_wall [length] = 1;
				length++;
			}
		}

		//west
		if(current_cell - 1 >= 0 && current_cell != check){
			if (cells [current_cell - 1].visited == false) {
				neighbours [length] = current_cell - 1;
				connecting_wall [length] = 2;
				length++;
			}
		}

		//south
		if(current_cell - x_size >= 0){
			if (cells [current_cell - x_size].visited == false) {
				neighbours [length] = current_cell - x_size;
				connecting_wall [length] = 3;
				length++;
			}
		}

		//north
		if(current_cell + x_size < total_cells){
			if (cells [current_cell + x_size].visited == false) {
				neighbours [length] = current_cell + x_size;
				connecting_wall [length] = 4;
				length++;
			}
		}

		if (length > 0) {
			int chosen = Random.Range (0, length);
			current_neighbour = neighbours [chosen];
			wall_to_break = connecting_wall [chosen];
		} else {
			if (backing_up > 0) {
				current_cell = last_cells[backing_up];
				backing_up--;
			}
		}
	}

	IEnumerator SpawnEnemies(){
		yield return new WaitForSeconds (enemy_settings.start_wait);

		GameObject player = GameObject.Find ("Player");

		while (true) {
			if (enemy_settings.enemy_holder.childCount < enemy_settings.max_spawn) {
				int r = Random.Range(0, cells.Length);
				Cell c = cells [r];

				GameObject instance = Instantiate (enemy_settings.enemy, new Vector3 (c.x, 1, c.y), Quaternion.identity) as GameObject;

				instance.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().SetTarget(player.transform);
				instance.transform.SetParent (enemy_settings.enemy_holder);

			}
			yield return new WaitForSeconds (enemy_settings.spawn_rate);
		}
	}

	void PutStartAndEndPoint(){
		int r = Random.Range(0, cells.Length);
		Cell c = cells [r];

		GameObject instance = Instantiate (start, new Vector3 (c.x, 2, c.y), start.transform.rotation) as GameObject;

		player.transform.position = instance.transform.position;


		int r1;
		do {
			r1 = Random.Range(0, cells.Length);

			if (r != r1) {
				
				Cell c1 = cells [r1];

				Instantiate (goal, new Vector3 (c1.x, 2, c1.y), goal.transform.rotation);
			}
		} while(r == r1);
	}

}
