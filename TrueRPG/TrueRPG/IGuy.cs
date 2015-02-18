using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueRPG
{
	public interface IGuy
	{
		Point GetCoordinates();

		int GetHp();

		string GetName();
	}
}
