package FunctionExplorer;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Insets;

import javax.swing.JLabel;
import javax.swing.JPanel;

public class ChartView extends JPanel {

	private static final long serialVersionUID = -6043568813346143944L;
	
	private Function function;
	
	public ChartView() {
		setMinimumSize(new Dimension(300, 300));
		//setBackground(Color.getHSBColor(100, 10, 10));
	}
	
	public void updateUi(Function function) {
		this.function = function;
		repaint();
	}

	@Override
	public void paintComponent(Graphics g) {

		super.paintComponent(g);
		doDrawing(g);
	}

	private void doDrawing(Graphics g) {
		Graphics2D g2d = (Graphics2D) g;
		
		
		
		g2d.setColor(Color.BLACK);

		Dimension size = getSize();
		int step = 25;

		FunctionPoints points = function.getPoints(size);

		String prettyName = function.toPrettyString();
		
		g2d.setColor(Color.lightGray);
		g2d.setStroke(new BasicStroke(0.5f));
		for (int i = (int) ((size.getHeight() / 2) % step); i <= size.getHeight(); i += step) {
			g2d.drawLine(0, i, (int) size.getWidth(), i);
		}

		for (int i = (int) ((size.getWidth() / 2) % step); i <= size.getWidth(); i += step) {
			g2d.drawLine(i, 0, i, (int) size.getHeight());
		}
		
		g2d.setStroke(new BasicStroke(3.0f));
		g2d.drawLine(0, size.height / 2, size.width, size.height /2);
		g2d.drawLine(size.width / 2, 0, size.width / 2, size.height);
		
		g2d.setColor(Color.BLACK);
		g2d.drawPolyline(points.getX(), points.getY(), points.getPointsCount());
		

		g2d.setFont(new Font("Verdana", Font.BOLD, 12));
		g2d.drawChars(prettyName.toCharArray(), 0, prettyName.length(), 10, 10);
	}
}
