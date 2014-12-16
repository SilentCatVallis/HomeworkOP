package FunctionExplorer;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.plaf.basic.BasicArrowButton;

public class SingleParameterController implements ActionListener {

	private SingleParameter parameter;
	private SingleParameterView view;
	
	public SingleParameterController(SingleParameter parameter) {
		this.parameter = parameter;
	}

	public void setView(SingleParameterView view) {
		this.view = view;		
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		BasicArrowButton btn = (BasicArrowButton)e.getSource();
		int coefficient = 1;
		if (btn.getDirection() == BasicArrowButton.SOUTH) 
			coefficient = -1;
		double value = Math.min(parameter.getValue() + coefficient * parameter.getStep(), parameter.getMaxValue());
		value = Math.max(value, parameter.getMinValue());
		parameter.setValue(value);
		view.updateUi(parameter);
	}

}
