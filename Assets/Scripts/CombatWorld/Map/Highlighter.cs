using UnityEngine;
using CombatWorld.Utility;

namespace CombatWorld
{
	public class Highlighter : MonoBehaviour
	{
		Highlight highlight;
		public GameObject[] highlights;

		void Awake()
		{
			GetComponent<Tile>().SubscribeToHighlightChange(SetHighlight);
		}

		void Start()
		{
			InputManager.instance.SubscribeToEndTurn(RemoveHighlight);
		}

		public void SetHighlight(Highlight highlight)
		{
			RemoveHighlight();
			this.highlight = highlight;
			switch (this.highlight)
			{
				case Highlight.None:
					RemoveHighlight();
					break;
				case Highlight.UnSelectable:
					highlights[0].SetActive(true);
					break;
				case Highlight.Simple:
					highlights[1].SetActive(true);
					break;
				case Highlight.Special:
					highlights[2].SetActive(true);
					break;
				default:
					break;
			}
		}

		public void RemoveHighlight()
		{
			highlight = Highlight.None;
			foreach (GameObject item in highlights)
			{
				item.SetActive(false);
			}
		}

		void OnDestroy()
		{
			InputManager.instance.UnsubscribeToEndTurn(RemoveHighlight);
		}

	}
}