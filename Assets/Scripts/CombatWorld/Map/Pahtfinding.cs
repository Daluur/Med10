using System.Collections;
using System.Collections.Generic;
using CombatWorld.Units;

namespace CombatWorld.Map {
	public class Pathfinding {

		Queue<Node> q = new Queue<Node>();
		Dictionary<Node, int> distance = new Dictionary<Node, int>();
		Dictionary<Node, Node> path = new Dictionary<Node, Node>();

		public List<Node> GetAllNodesWithinDistance(Node start, int dist) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			distance.Add(start, 0);

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour)) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
					path.Add(neighbour, current);
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if(distance[current] >= dist) {
					continue;
				}
				if (current.HasOccupant()) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour)) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current]+1);
						path.Add(neighbour, current);
					}
				}
			}
			return new List<Node>(distance.Keys);
		}

		public int GetDistanceToNode(Node start, Node end) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			if (start == end) {
				return 0;
			}

			distance.Add(start, 0);

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour) && !neighbour.HasTower() && !neighbour.HasOccupant()) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
					path.Add(neighbour, current);
					if (neighbour == end) {
						return distance[current] + 1;
					}
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour) && !neighbour.HasTower() && !neighbour.HasOccupant()) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current] + 1);
						path.Add(neighbour, current);
						if(neighbour == end) {
							return distance[current] + 1;
						}
					}
				}
			}
			return 0;
			//throw new System.Exception("Could not find end node, is it hidden behind a tower?");
		}

	/*	public List<Node> GetAllReachableNodes(Node start, int dist) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			distance.Add(start, 0);

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour)) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
					path.Add(neighbour, current);
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if (distance[current] >= dist) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour)) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current] + 1);
						path.Add(neighbour, current);
					}
				}
			}
			distance.Remove(start);
			return new List<Node>(distance.Keys);
		}*/

		public List<Node> GetAllNodesWithinDistanceWithhoutOccupants(Node start, int dist) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			distance.Add(start, 0);

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour) && !neighbour.HasOccupant()) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
					path.Add(neighbour, current);
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if (distance[current] >= dist) {
					continue;
				}
				if (current.HasOccupant()) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour) && !neighbour.HasOccupant()) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current] + 1);
						path.Add(neighbour, current);
					}
				}
			}
			distance.Remove(start);
			return new List<Node>(distance.Keys);
		}

		public List<Node> GetPathTo(Node node) {
			List<Node> toReturn = new List<Node>();
			toReturn.Add(node);
			Node prev = node;
			Node temp;
			while (true) {
				temp = path[prev];
				if(temp == null) {
					return toReturn;
				}
				else {
					toReturn.Add(temp);
					prev = temp;
				}
			}
		}

		public List<Node> GetPathFromTo(Node start, Node end) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!path.ContainsKey(neighbour) && !neighbour.HasOccupant()) {
					q.Enqueue(neighbour);
					path.Add(neighbour, current);
					if (neighbour == end) {
						return GetPathTo(end);
					}
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if (current.HasOccupant()) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!path.ContainsKey(neighbour) && !neighbour.HasOccupant()) {
						q.Enqueue(neighbour);
						path.Add(neighbour, current);
						if (neighbour == end) {
							return GetPathTo(end);
						}
					}
				}
			}
			return null;
			//throw new System.NullReferenceException("Could not reach the target node!");
		}

		public List<Node> GetPathFromToWithoutOccupants(Node start, Node end) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!path.ContainsKey(neighbour) && !neighbour.HasTower()) {
					q.Enqueue(neighbour);
					path.Add(neighbour, current);
					if (neighbour == end) {
						return GetPathTo(end);
					}
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();

				foreach (Node neighbour in current.neighbours) {
					if (!path.ContainsKey(neighbour) && !neighbour.HasTower()) {
						q.Enqueue(neighbour);
						path.Add(neighbour, current);
						if (neighbour == end) {
							return GetPathTo(end);
						}
					}
				}
			}
			return null;
			//throw new System.NullReferenceException("Could not reach the target node!");
		}

		public List<Node> GetAllReachableNodes(Node start, int dist) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			distance.Add(start, 0);

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour)) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
					path.Add(neighbour, current);
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if (distance[current] >= dist) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour)) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current] + 1);
						path.Add(neighbour, current);
					}
				}
			}
			distance.Remove(start);
			return new List<Node>(distance.Keys);
		}

		public List<Node> GetReachableNodesForUnitAfterAMove(Unit unit, Node newBlock, Node oldNode) {
			Node start = unit.GetNode();
			int dist = unit.GetMoveDistance();

			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			distance.Add(start, 0);

			Node current = start;
			foreach (Node neighbour in current.neighbours) {
				if(current.HasOccupant() && current != oldNode && current.GetOccupant().GetTeam() != unit.GetTeam()) {
					continue;
				}
				if(current == newBlock) {
					continue;
				}
				if (!distance.ContainsKey(neighbour)) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
				}
			}

			while (q.Count > 0) {
				current = q.Dequeue();
				if (distance[current] >= dist) {
					continue;
				}
				if (current.HasOccupant() && current != oldNode && current.GetOccupant().GetTeam() != unit.GetTeam()) {
					continue;
				}
				if (current == newBlock) {
					continue;
				}
				foreach (Node neighbour in current.neighbours) {
					if (!distance.ContainsKey(neighbour)) {
						q.Enqueue(neighbour);
						distance.Add(neighbour, distance[current] + 1);
					}
				}
			}
			return new List<Node>(distance.Keys);
		}
	}
}