package FunctionExplorer;

import java.util.ArrayList;
import java.util.HashMap;

public class FunctionSelectorModel {
	
	private ArrayList<Function> functions;

	public FunctionSelectorModel() {
		functions = new ArrayList<Function>();
		LinearFunction linearFunction = new LinearFunction();
		functions.add(linearFunction);
		QuadraticFunction quadraticFunction = new QuadraticFunction();
		functions.add(quadraticFunction);
		CubicFunction cubicFunction = new CubicFunction();
		functions.add(cubicFunction);
		SinFunction sinus = new SinFunction();
		functions.add(sinus);
	}
	
	public Function[] GetFunctions() {
		return functions.toArray(new Function[functions.size()]);
	}
	
	public Function getFunction(String functionName) {
		System.out.println(functionName);
		for (Function function : functions) 
			if (function.getName().compareTo(functionName) == 0) 
				return function;
		return null;
	}
	
	public Function getFunction(int functionIndex) {
		return functions.get(functionIndex);
	}
	
	public void addFunction(Function function) {
		functions.add(function);
	}

	public boolean isHasName(String name) {
		for (Function function : functions)
			if (function.getName().compareTo(name) == 0)
				return true;
		return false;
	}
}
