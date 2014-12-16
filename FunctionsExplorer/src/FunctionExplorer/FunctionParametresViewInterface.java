package FunctionExplorer;

import java.awt.event.ActionListener;

import javax.swing.JDialog;

public interface FunctionParametresViewInterface {
	public void updateUi(FunctionParametresModel model);

	public void askFunctionName(JDialog dialog);
}
