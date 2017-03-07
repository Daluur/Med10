using System.Collections;
using System.Collections.Generic;

namespace CombatWorld.Map {
	public class Pathfinding {

		Queue<Node> q = new Queue<Node>();
		Dictionary<Node, int> distance = new Dictionary<Node, int>();

		public List<Node> GetAllNodesWithinDistance(Node start, int dist) {
			q = new Queue<Node>();
			distance = new Dictionary<Node, int>();

			distance.Add(start, 0);

			Node current = start;
			foreach (Node neighbour in current.neighbours) {
				if (!distance.ContainsKey(neighbour)) {
					q.Enqueue(neighbour);
					distance.Add(neighbour, distance[current] + 1);
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
					}
				}
			}
			return new List<Node>(distance.Keys);
		}
	}
}