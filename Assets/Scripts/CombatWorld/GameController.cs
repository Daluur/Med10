using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatWorld.Map;
using CombatWorld.Units;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class GameController : Singleton<GameController>
	{
		List<Node> allNodes = new List<Node>();
		List<SummonNode> playerSummonNodes = new List<SummonNode>();
		List<SummonNode> AISummonNodes = new List<SummonNode>();
		Team currentTeam;

		Unit selectedUnit;

		void Start()
		{
			Invoke("StartGame", 1);
		}

		void StartGame()
		{
			currentTeam = Team.Player;
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void AddNode(Node node)
		{
			allNodes.Add(node);
		}

		public void AddTeamNode(SummonNode node, Team team)
		{
			if(team == Team.Player)
			{
				playerSummonNodes.Add(node);
			}
			else
			{
				AISummonNodes.Add(node);
			}
		}

		public void EndTurn()
		{
			switch (currentTeam)
			{
				case Team.Player:
					currentTeam = Team.AI;
					break;
				case Team.AI:
					currentTeam = Team.Player;
					break;
				default:
					break;
			}
			ResetAllNodes();
			SelectTeamNodes();
		}

		void ResetAllNodes()
		{
			foreach (Node node in allNodes)
			{
				node.ResetState();
			}
		}

		void SelectTeamNodes()
		{
			if (selectedUnit == null)
			{
				foreach (Node node in allNodes)
				{
					if (node.HasOccupant() && node.GetOccupant().GetTeam() == currentTeam)
					{
						node.SetState(HighlightState.Selectable);
					}
				}
				if (currentTeam == Team.Player)
				{
					foreach (SummonNode node in playerSummonNodes)
					{
						if (!node.HasOccupant())
						{
							node.SetState(HighlightState.Selectable);
						}
					}
				}
			}
			else
			{
				foreach (Node node in selectedUnit.GetNode().GetNeighbours())
				{
					if (node.HasOccupant())
					{
						if (node.GetOccupant().GetTeam() == currentTeam)
						{
							node.SetState(HighlightState.NotMoveable);
						}
						else
						{
							node.SetState(HighlightState.Attackable);
						}
					}
					else
					{
						node.SetState(HighlightState.Moveable);
					}
				}
			}
		}

		public void GotInput()
		{
			ResetAllNodes();
			SelectTeamNodes();
		}

		public void UnitMadeAction()
		{
			selectedUnit = null;
		}

		public void SelectedUnit(Unit unit)
		{
			selectedUnit = unit;
		}

		public Unit GetSelectedUnit()
		{
			return selectedUnit;
		}

	}
}