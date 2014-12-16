package FunctionExplorer;

import java.awt.Dimension;

import javax.swing.JPanel;

public class SingleParameterControl {

	//private SingleParameter parameter;
	private SingleParameterController controller;
	private SingleParameterView view;
	
	public SingleParameterControl(SingleParameter parameter) {
		this.controller = new SingleParameterController(parameter);
		this.view = new SingleParameterView(controller);
		controller.setView(view);
		view.setMaximumSize(new Dimension(300, 30));
		view.updateUi(parameter);
	}
	
	public JPanel getView() {
		return view;
	}

}
