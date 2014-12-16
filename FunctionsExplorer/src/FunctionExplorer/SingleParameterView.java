package FunctionExplorer;

import java.awt.Dimension;
import java.awt.event.ActionListener;

import javax.swing.GroupLayout;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.GroupLayout.ParallelGroup;
import javax.swing.GroupLayout.SequentialGroup;
import javax.swing.plaf.basic.BasicArrowButton;

public class SingleParameterView extends JPanel {

	private static final long serialVersionUID = 685499246533672973L;
	private JLabel label;
	private JTextField field;

	public SingleParameterView(ActionListener listener) {
		
		GroupLayout gl = new GroupLayout(this);
		setLayout(gl);
		
		label = new JLabel();
		field = new JTextField();
		field.setEnabled(false);
		field.setMinimumSize(new Dimension(220, 1));
		BasicArrowButton buttonUp = new BasicArrowButton(BasicArrowButton.NORTH);
		buttonUp.addActionListener(listener);
		BasicArrowButton buttonDown = new BasicArrowButton(BasicArrowButton.SOUTH);
		buttonDown.addActionListener(listener);
		ParallelGroup parallelGroup = gl.createParallelGroup();
		SequentialGroup sequentialGroup = gl.createSequentialGroup();
		parallelGroup.addComponent(label).addComponent(field).addGroup(gl.createSequentialGroup().addComponent(buttonUp).addComponent(buttonDown));
		sequentialGroup.addComponent(label).addComponent(field).addGroup(gl.createParallelGroup().addComponent(buttonUp).addComponent(buttonDown));
		gl.setHorizontalGroup(sequentialGroup);
		gl.setVerticalGroup(parallelGroup);
	}

	public void updateUi(SingleParameter parameter) {
		field.setText(String.valueOf(parameter.getValue()));
		label.setText(parameter.getName() + "= ");
	}
	
}
