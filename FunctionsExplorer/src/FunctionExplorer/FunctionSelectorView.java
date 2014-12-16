package FunctionExplorer;

import java.awt.Dimension;

import javax.swing.DefaultListModel;
import javax.swing.GroupLayout;
import javax.swing.GroupLayout.Alignment;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.ListModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;

public class FunctionSelectorView extends JPanel implements FunctionSelectorViewInterface {

	private static final long serialVersionUID = 2712255837085666606L;
	
	private ListSelectionListener listener;
	private JList<String> list;
	
	public FunctionSelectorView(ListSelectionListener listener) {
		this.listener = listener;
		initUI();
	}

	private void initUI() {

		GroupLayout gl = new GroupLayout(this);
		setLayout(gl);
		this.setMaximumSize(new Dimension(200, 1000));
		
		JLabel label = new JLabel("Select function type");
		
		DefaultListModel<String> listModel = new DefaultListModel<String>();
		list = new JList<String>(listModel);
		JScrollPane pane = new JScrollPane(list);
		list.addListSelectionListener(listener);
		
		gl.setHorizontalGroup(gl.createParallelGroup(Alignment.CENTER)
				.addComponent(label)
				.addComponent(pane));
		gl.setVerticalGroup(gl.createSequentialGroup()
				.addComponent(label)
				.addComponent(pane));
	}

	@Override
	public void updateUi(FunctionSelectorModel model) {
		DefaultListModel<String> listModel = new DefaultListModel<String>();
		for (Function func : model.GetFunctions())
			listModel.addElement(func.getName());
		list.setModel(listModel);
		list.setSelectedIndex(model.GetFunctions().length - 1);
		revalidate();
	}
}
