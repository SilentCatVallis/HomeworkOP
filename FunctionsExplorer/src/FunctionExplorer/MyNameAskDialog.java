package FunctionExplorer;

import java.awt.Dimension;
import java.awt.event.ActionListener;
import java.awt.event.KeyListener;

import javax.swing.GroupLayout;
import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.GroupLayout.Alignment;

public class MyNameAskDialog extends JDialog implements MyAskDialogInterface {

	private static final long serialVersionUID = 2566117521977533685L;
	
	private ActionListener actionListener;
	private KeyListener keyListener;
	private JTextField field;
	private JLabel label;

	public MyNameAskDialog(ActionListener listener, KeyListener keyListener) {
		this.actionListener = listener;
		this.keyListener = keyListener;
		initUi();
	}

	private void initUi() {
    	setSize(new Dimension(105, 120));
    	setLocationRelativeTo(null);

		GroupLayout gl = new GroupLayout(getContentPane());
		setLayout(gl);
		
		field = new JTextField();
		field.setMinimumSize(new Dimension(100, 25));
		field.setMaximumSize(new Dimension(100, 25));
		field.addKeyListener(keyListener);
		label = new JLabel("set new function name");
		JButton button = new JButton("Create");
		button.addActionListener(actionListener);
		
		gl.setHorizontalGroup(gl.createParallelGroup(Alignment.CENTER)
				.addComponent(field)
				.addComponent(label)
				.addComponent(button));
		gl.setVerticalGroup(gl.createSequentialGroup()
				.addComponent(field)
				.addComponent(label)
				.addComponent(button));
	}

	@Override
	public void askNotValidName() {
		label.setText("this name is invalid!");
		revalidate();
	}

}
