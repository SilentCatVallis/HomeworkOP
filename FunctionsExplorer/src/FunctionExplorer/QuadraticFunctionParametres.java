package FunctionExplorer;

import java.util.ArrayList;

public class QuadraticFunctionParametres extends Parametres {
	
	public QuadraticFunctionParametres() {
		super();
		addParameter(new SingleParameter("A", 1, 1, -10, 10));
		addParameter(new SingleParameter("B", 1, 1, -20, 20));
		addParameter(new SingleParameter("C", 0, 1, -10, 10));
	}

	public QuadraticFunctionParametres(ArrayList<SingleParameter> newParametres) {
		super(newParametres);
	}

	@Override
	public double getResult(double x) {
		// TODO Auto-generated method stub
		return getParametres().get(0).getValue() * x * x + getParametres().get(1).getValue() * x + getParametres().get(2).getValue();
	}
	
	public String toPrettyString() {
		return String.format("y(x) = %1$2sx^2 + %2$2sx + %3$2s",
				(int)getParametres().get(0).getValue(),
				(int)getParametres().get(1).getValue(),
				(int)getParametres().get(2).getValue());
	}

}
