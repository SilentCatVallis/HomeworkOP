package FunctionExplorer;

import java.util.ArrayList;

import javax.swing.JPanel;

public class FunctionParametresModel {
	
	private Function function;
	
	public FunctionParametresModel(Function function) {
		this.function = function;
	}
	
	public ParametresInterface getParametres() {
		return function.getParametres();
	}
	
	public void setParametres(Parametres parametres) {
		function.setParametres(parametres);
	}
	
	public void setFunction(Function function) {
		this.function = function;
	}
	
	public Function getFunction() {
		return function;
	}
	
	public ArrayList<SingleParameterControl> getSingleParameterControls() {
		ArrayList<SingleParameterControl> controls = new ArrayList<SingleParameterControl>();
		for (SingleParameter p : function.getParametres().getParametres()) {
			controls.add(new SingleParameterControl(p));
		}
		return controls;
	}

	public ChartView getChart(Boolean isNeedRegister) {
		ChartController chartController = new ChartController(function);
		if (isNeedRegister)
			function.getParametres().registerObserver(chartController);
		else
			function.getParametres().registerMainObserver(chartController);
		ChartView view = new ChartView();
		chartController.setView(view);
		view.updateUi(function);
		return view;
	}
}
