using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TrueRPG
{
	public interface IForest
	{
		bool AddNewGuy(IGuy guy);

		bool MoveTo(Point position, Direction direction);

		IObject GetObject(Point position);

		IEnumerable<IGuy> EnumerateGuys();
	}

}
