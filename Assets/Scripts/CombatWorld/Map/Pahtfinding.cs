using System.Collections;
using System.Collections.Generic;

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
			 return GetAllNodesWithinDistance(start, end);
		}

		public List<Node> GetAllNodesWithinDistance(Node start, Node end) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();
			path = new Dictionary<Node, Node>();

			Node current = start;
			path.Add(start, null);
			foreach (Node neighbour in current.neighbours) {
				if (!path.ContainsKey(neighbour)) {
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
					if (!path.ContainsKey(neighbour)) {
						q.Enqueue(neighbour);
						path.Add(neighbour, current);
						if (neighbour == end) {
							return GetPathTo(end);
						}
					}
				}
			}
			throw new System.NullReferenceException("Could not reach the target node!");
		}
	}
}