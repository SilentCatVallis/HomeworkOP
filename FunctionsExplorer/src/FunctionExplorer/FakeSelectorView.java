package FunctionExplorer;

import javax.swing.DefaultListModel;
import javax.swing.JList;
import javax.swing.event.ListSelectionListener;



class FakeSelectorView implements FunctionSelectorViewInterface {

	private ListSelectionListener listener;
	private DefaultListModel<String> model;

	public FakeSelectorView(ListSelectionListener listener) {
		this.listener = listener;
		this.model = new DefaultListModel<String>();
	}
	
	@Override
	public void updateUi(FunctionSelectorModel model) {
		for (Function f : model.GetFunctions()) {
			this.model.addElement(f.getName());
		}
	}
	
	public void selectItem(int index) {
		JList<String> list = new JList<String>(model);
		list.addListSelectionListener(listener);
		list.setSelectedIndex(index);
	}
	
}