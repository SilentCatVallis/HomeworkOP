using System;
using System.Collections.Generic;
using System.Drawing;

namespace timus1643
{
	public class AttackPlan
	{
		public AttackPlan(Point[] geluPath, Point[] catherinePath)
		{
			GeluPath = geluPath;
			CatherinePath = catherinePath;
		}

		public readonly Point[] GeluPath;
		public readonly Point[] CatherinePath;

		public int Length
		{
			get { return Math.Max(GeluPath.Length, CatherinePath.Length); }
		}
	}

	public class AttackSandroAlgorithms
	{
		public static AttackPlan FindPlan(List<string> mazeLines)
		{
			// Проект attack использует эту функцию и визуализирует её результат. Используйте его для отладки!
			return null;
		}
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			// Используйте AttackSandroAlgorithms.FindPlan(mazeLines);
		}
	}
}
