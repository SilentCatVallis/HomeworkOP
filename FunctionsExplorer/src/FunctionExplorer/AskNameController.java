package FunctionExplorer;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JTextField;

public class AskNameController implements ActionListener, KeyListener{

	private FunctionParametresController functionParametresController;
	private String model;
	private MyAskDialogInterface dialog;

	public AskNameController(FunctionParametresController functionParametresController, String model) {
		this.functionParametresController = functionParametresController;
		this.model = model;
	}
	
	public AskNameController setDialog(MyAskDialogInterface dialog) {
		this.dialog = dialog;
		return this;
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		if (e.getSource() instanceof JButton) {
			if (functionParametresController.validName(model)) {
				functionParametresController.addFunctionInSelectedList(model);
				((JDialog)dialog).dispose();
			}
			else {
				dialog.askNotValidName();
			}
		}
	}

	@Override
	public void keyPressed(KeyEvent e) {		
	}

	@Override
	public void keyReleased(KeyEvent e) {
		JTextField field = (JTextField)e.getSource();
		model = field.getText();
	}

	@Override
	public void keyTyped(KeyEvent e) {
	}

}
