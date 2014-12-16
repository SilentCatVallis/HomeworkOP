package FunctionExplorer;

public class ChartController implements Observer{

	private ChartView view;
	private Function function;

	public ChartController(Function function) {
		this.function = function;
	}

	public void setView(ChartView view) {
		this.view = view;
	}

	@Override
	public void update(Subject s) {
		view.updateUi(function);
	}

}
