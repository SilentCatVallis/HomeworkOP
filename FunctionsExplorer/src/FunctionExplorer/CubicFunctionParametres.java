package FunctionExplorer;

import java.util.ArrayList;

public class CubicFunctionParametres extends Parametres {

	public CubicFunctionParametres() {
		super();
		addParameter(new SingleParameter("A", 1, 1, -10, 10));
		addParameter(new SingleParameter("B", 1, 1, -10, 10));
		addParameter(new SingleParameter("C", 1, 1, -100, 100));
		addParameter(new SingleParameter("D", 1, 1, -10, 10));
	}

	public CubicFunctionParametres(ArrayList<SingleParameter> newParametres) {
		super(newParametres);
	}

	@Override
	public double getResult(double x) {
		// TODO Auto-generated method stub
		return getParametres().get(0).getValue() * x * x * x + getParametres().get(1).getValue() * x * x + getParametres().get(2).getValue() * x + getParametres().get(3).getValue();
	}
	
	public String toPrettyString() {
		return String.format("y(x) = %1$2sx^3 + %2$2sx^2 + %3$2sx + %4$2s",
				(int)getParametres().get(0).getValue(),
				(int)getParametres().get(1).getValue(),
				(int)getParametres().get(2).getValue(),
				(int)getParametres().get(3).getValue());
	}

}
