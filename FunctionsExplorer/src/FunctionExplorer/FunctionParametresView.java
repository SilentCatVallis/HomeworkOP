package FunctionExplorer;

import java.awt.event.ActionListener;
import java.util.regex.PatternSyntaxException;

import javax.swing.GroupLayout;
import javax.swing.GroupLayout.Alignment;
import javax.swing.GroupLayout.ParallelGroup;
import javax.swing.GroupLayout.SequentialGroup;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.plaf.basic.BasicArrowButton;

public class FunctionParametresView extends JPanel implements FunctionParametresViewInterface{

	private static final long serialVersionUID = 6656914927059015359L;
	private ActionListener listener;

	public FunctionParametresView(ActionListener listener) {
		this.listener = listener;
	}

	private void ConfigView(FunctionParametresModel model) {
		GroupLayout gl = new GroupLayout(this);

		setLayout(gl);
		
		ParallelGroup globalParallelGroup = gl.createParallelGroup();
		SequentialGroup globalSequentialGroup = gl.createSequentialGroup();
		
		JLabel functionNameLabel = new JLabel(model.getFunction().getName());
		
		for (SingleParameterControl parameter : model.getSingleParameterControls()) {
			globalParallelGroup.addComponent(parameter.getView());
			globalSequentialGroup.addComponent(parameter.getView());
		}
		
		JButton saveToListButton = new JButton("Save to list");
		saveToListButton.addActionListener(listener);
		
		JButton openChartButton = new JButton("Open chart");
		openChartButton.addActionListener(listener);
		
		ChartView chart = model.getChart(false);

		gl.setHorizontalGroup(gl.createParallelGroup(Alignment.TRAILING)
				.addComponent(functionNameLabel)
				.addComponent(chart)
				.addGroup(Alignment.LEADING, gl.createSequentialGroup()
						.addGroup(globalParallelGroup)
						.addGap(100)
						.addGroup(gl.createParallelGroup(Alignment.CENTER)
								.addComponent(openChartButton)
								.addComponent(saveToListButton))));
		gl.setVerticalGroup(gl.createSequentialGroup()
				.addComponent(functionNameLabel)
				.addComponent(chart)
				.addGap(10)
				.addGroup(gl.createParallelGroup()
						.addGroup(globalSequentialGroup)
						.addGroup(gl.createSequentialGroup()
								.addComponent(openChartButton)
								.addComponent(saveToListButton))));
	}
	
	@Override
	public void updateUi(FunctionParametresModel model) {
		removeAll();
		ConfigView(model);
		revalidate();
	}

	@Override
	public void askFunctionName(JDialog dialog) {
		dialog.setModal(true);
		dialog.show();
	}
}
