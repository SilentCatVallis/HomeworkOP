package FunctionExplorer;

import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.ListSelectionModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;

public class FunctionSelectorController implements ListSelectionListener{
	
	private FunctionSelectorModel functionSelectorModel;
	private FunctionParametresController functionParametresController;
	private FunctionSelectorViewInterface functionSelectorView;
	
	public FunctionSelectorController(FunctionSelectorModel functionSelectorModel) {
		this.functionSelectorModel = functionSelectorModel;
		functionParametresController = null;
	}
	
	public FunctionSelectorController setFunctionParametresController(FunctionParametresController controller) {
		functionParametresController = controller;
		return this;
	}
	
	public FunctionSelectorController setFunctionSelectorView(FunctionSelectorViewInterface view) {
		this.functionSelectorView = view;
		return this;
	}

	public void drawFunction(String name) {
		functionParametresController.drawNewFunction(functionSelectorModel.getFunction(name));
	}

	public void addFunctionInList(Function function) {
		// TODO Auto-generated method stub
		functionSelectorModel.addFunction(function);
		functionSelectorView.updateUi(functionSelectorModel);
		functionParametresController.drawNewFunction(function);
	}

	public boolean isListHasName(String name) {
		// TODO Auto-generated method stub
		return !functionSelectorModel.isHasName(name);
	}

	@Override
	public void valueChanged(ListSelectionEvent e) {
		// TODO Auto-generated method stub
		if (!e.getValueIsAdjusting()) {
			JList<String> list = (JList<String>)e.getSource();
			String functionName = list.getSelectedValue();
			if (functionName == null)
				return;
			Function f = functionSelectorModel.getFunction(functionName);
			functionParametresController.drawNewFunction(f);
		}
	}
}
