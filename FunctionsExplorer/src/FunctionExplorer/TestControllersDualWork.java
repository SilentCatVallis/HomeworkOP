package FunctionExplorer;

import static org.junit.Assert.*;

import org.junit.Test;

public class TestControllersDualWork {

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
		fakeSelectorView.selectItem(1);
		assertEquals(3, functionParametresModel.getSingleParameterControls().size());
	}
	
	@Test
	public void test2() {
		initFields();
		fakeSelectorView.selectItem(0);
		assertEquals(2, functionParametresModel.getSingleParameterControls().size());
	}
	
	@Test
	public void test3() {
		initFields();
		fakeSelectorView.selectItem(2);
		assertEquals(4, functionParametresModel.getSingleParameterControls().size());
	}
	
	@Test
	public void test4() {
		initFields();
		functionParametresController.addFunctionInSelectedList(functionParametresModel.getFunction().getName());
		assertEquals(4, functionSelectorModel.GetFunctions().length);
	}
}
