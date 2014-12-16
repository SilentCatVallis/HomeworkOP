package FunctionExplorer;

import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JDialog;

public class FakeParametresView implements FunctionParametresViewInterface {

	private JButton saveToListButton;
	private JButton openChartButton;

	public FakeParametresView(ActionListener listener) {
		saveToListButton = new JButton("Save to list");
		saveToListButton.addActionListener(listener);
		openChartButton = new JButton("Open chart");
		openChartButton.addActionListener(listener);
	}
	
	@Override
	public void updateUi(FunctionParametresModel model) {}
	
	public void clickSaveInList() {
		saveToListButton.doClick();
	}

	public void clickOpenChart() {
		openChartButton.doClick();
	}

	@Override
	public void askFunctionName(JDialog dialog) {
		// TODO Auto-generated method stub
	}

}
