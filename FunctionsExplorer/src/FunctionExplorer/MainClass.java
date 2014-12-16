package FunctionExplorer;

import java.awt.Dimension;

import javax.swing.GroupLayout;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;
import javax.swing.GroupLayout.Alignment;

public class MainClass extends JFrame{

	private static final long serialVersionUID = -3680729154424718139L;

	public MainClass() {
		FunctionSelectorModel functionSelectorModel = new FunctionSelectorModel();	
		FunctionParametresModel functionParametresModel = new FunctionParametresModel(functionSelectorModel.GetFunctions()[0]);
				
		FunctionSelectorController functionSelectorController = new FunctionSelectorController(functionSelectorModel);
		FunctionParametresController functionParametresController = new FunctionParametresController(functionParametresModel);
		
		FunctionSelectorView functionSelector = new FunctionSelectorView(functionSelectorController);
		FunctionParametresView functionParametres = new FunctionParametresView(functionParametresController);
		

		functionParametresController.setFunctionSelectorController(functionSelectorController).setFunctionParametresView(functionParametres);
		functionSelectorController.setFunctionParametresController(functionParametresController).setFunctionSelectorView(functionSelector);
		
		InitUI(functionSelector, functionParametres);	
		
		functionSelector.updateUi(functionSelectorModel);
		functionParametres.updateUi(functionParametresModel);
	}
	
	private void InitUI(JPanel functionSelector, JPanel functionParametres) {
		
		setLocationRelativeTo(null);
		
		GroupLayout gl = new GroupLayout(getContentPane());
		setLayout(gl);
		
		gl.setHorizontalGroup(gl.createSequentialGroup()
				.addComponent(functionSelector)
				.addComponent(functionParametres));
		
		gl.setVerticalGroup(gl.createParallelGroup(Alignment.CENTER)
				.addComponent(functionSelector)
				.addComponent(functionParametres));
		
		pack();
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(new Runnable() {
            @Override
            public void run() {
            	MainClass form = new MainClass();
            	form.setSize(new Dimension(700, 500));
            	form.setTitle("Function Explorer");
            	form.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
            	form.setVisible(true);
            }
        });
	}

}
