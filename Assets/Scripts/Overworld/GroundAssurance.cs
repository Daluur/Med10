using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
	public class GroundAssurance : MonoBehaviour {
		void Start () {
			if (gameObject.layer != LayerMask.NameToLayer(LayerConstants.GROUNDLAYER)) {
				Debug.LogError("The ground must have the layer: " +  LayerConstants.GROUNDLAYER);
			}
		}
	}
}