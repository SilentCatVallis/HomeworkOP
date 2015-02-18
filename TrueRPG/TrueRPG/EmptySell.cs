using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueRPG
{
	public class EmptyCell : IObject
	{
		public EmptyCell()
		{
			
		}

		public CollideResult Collide(IGuy otherObject)
		{
			throw new NotImplementedException();
		}
	}
}
