package FunctionExplorer;

import static org.junit.Assert.*;

import javax.swing.DefaultListModel;
import javax.swing.JList;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;

import org.junit.Test;

public class TestFunctionSelectorController {

	private FunctionSelectorModel functionSelectorModel;
	private FunctionParametresModel functionParametresModel;
	private FunctionSelectorController functionSelectorController;
	private FunctionParametresController functionParametresController;
	private FakeSelectorView fakeSelectorView;
	private FakeParametresView fakeParametresView;
	
	private void initFields() {
		functionSelectorModel = new FunctionSelectorModel();	
		functionParametresModel = new FunctionParametresModel(functionSelectorModel.GetFunctions()[0]);
				
		functionSelectorController = new FunctionSelectorController(functionSelectorModel);
		functionParametresController = new FunctionParametresController(functionParametresModel);
		
		fakeSelectorView = new FakeSelectorView(functionSelectorController);
		fakeParametresView = new FakeParametresView(functionParametresController);
		
		functionParametresController.setFunctionSelectorController(functionSelectorController).setFunctionParametresView(fakeParametresView);
		functionSelectorController.setFunctionParametresController(functionParametresController).setFunctionSelectorView(fakeSelectorView);
		fakeSelectorView.updateUi(functionSelectorModel);
	}
	
	@Test
	public void test1() {
		initFields();
		fakeSelectorView.selectItem(0);
		assertEquals(functionSelectorModel.GetFunctions()[0], functionParametresModel.getFunction());
	}

	@Test
	public void test2() {
		initFields();
		fakeSelectorView.selectItem(1);
		assertEquals(functionSelectorModel.GetFunctions()[1], functionParametresModel.getFunction());
	}
	
	@Test
	public void test3() {
		initFields();
		fakeSelectorView.selectItem(2);
		assertEquals(functionSelectorModel.GetFunctions()[2], functionParametresModel.getFunction());
	}
}
