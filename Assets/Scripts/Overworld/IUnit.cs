using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Overworld {
	public interface IUnit {
		Unit.UnitDescription GetDescription();
	}
}