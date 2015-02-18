using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueRPG
{
	public class ObjectFabric
	{
		public static Dictionary<char, Func<IObject>> Fabric = new Dictionary<char, Func<IObject>>
		{
			{'0', () => new EmptyCell()},
			{'1', () => new WallCell()},
			{'K', () => new TrapCell()},
			{'L', () => new MedPackCell()}
		};
	}
}
