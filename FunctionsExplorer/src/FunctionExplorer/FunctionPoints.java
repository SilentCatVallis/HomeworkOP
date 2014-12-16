package FunctionExplorer;

import java.awt.Dimension;

public class FunctionPoints {

	private int width;
	private int height;
	private int[] pointsX;
	private int[] pointsY;

	public FunctionPoints(Dimension dim, Function function) {
		this.width = (int) dim.getWidth();
		this.height = (int) dim.getHeight();
		this.pointsX = new int[width];
		for (int i = 0; i < width; i++) 
			pointsX[i] = i;
		this.pointsY = new int[width];
		for (int i = 0; i < width; i++) 
			pointsY[i] = (int) (-function.getResult((double)(pointsX[i] - width/2) / 25) * 25) + height/2; //resize coordinates
	}
	
	public int[] getX() {
		return pointsX;
	}
	
	public int[] getY() {
		return pointsY;
	}
	
	public int getPointsCount() {
		return width;
	}
}
