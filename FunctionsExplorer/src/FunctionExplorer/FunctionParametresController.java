package FunctionExplorer;

import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class FunctionParametresController implements ActionListener {

	private FunctionParametresModel functionParametresModel;
	private FunctionSelectorController functionSelectorController;
	private FunctionParametresViewInterface functionParametresView;

	public FunctionParametresController(FunctionParametresModel functionParametresModel) {
		this.functionParametresModel = functionParametresModel;
		functionSelectorController = null;
	}
	
	public FunctionParametresController setFunctionSelectorController(FunctionSelectorController controller) {
		functionSelectorController = controller;
		return this;
	}
	
	public FunctionParametresController setFunctionParametresView(FunctionParametresViewInterface view) {
		this.functionParametresView = view;
		return this;
	}
	
	public void drawNewFunction(Function function) {
		functionParametresModel.setFunction(function);
		functionParametresView.updateUi(functionParametresModel);
		//mainFrame.revalidate();
	}

	public void addFunctionInSelectedList(String functionName) {
		// TODO Auto-generated method stub
		Function function = functionParametresModel.getFunction().copy();
		function.setName(functionName);
		functionSelectorController.addFunctionInList(function);
	}

	public boolean validName(String name) {
		// TODO Auto-generated method stub
		if (name == null || name.compareTo("") == 0)
			return false;
		return functionSelectorController.isListHasName(name);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		JButton button = (JButton)e.getSource();
		if (button.getText().compareTo("Save to list") == 0) {
			AskNameController askController = new AskNameController(this, "");
			MyNameAskDialog dialog = new MyNameAskDialog(askController, askController);
			askController.setDialog(dialog);
			functionParametresView.askFunctionName(dialog);
		}
		if (button.getText().compareTo("Open chart") == 0) {
			
			SwingUtilities.invokeLater(new Runnable() {
	            @Override
	            public void run() {
	            	JFrame frame = new JFrame();
	    			frame.setMinimumSize(new Dimension(300, 300));
	    			frame.setTitle(functionParametresModel.getFunction().getName());
	    			frame.setLocationRelativeTo(null);
	    			frame.add(functionParametresModel.getChart(true));
	    			frame.show();
	    			//frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	    			frame.setVisible(true);
	            }
	        });
		}
	}
}
