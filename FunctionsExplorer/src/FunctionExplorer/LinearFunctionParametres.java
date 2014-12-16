package FunctionExplorer;

import java.util.ArrayList;

public class LinearFunctionParametres extends Parametres {
	
	public LinearFunctionParametres() {
		super();
		addParameter(new SingleParameter("A", 2, 1, -100, 100));
		addParameter(new SingleParameter("B", 0, 1, -10, 10));
	}
	
	public LinearFunctionParametres(ArrayList<SingleParameter> parametres) {
		super(parametres);
	}

	@Override
	public double getResult(double x) {
		// TODO Auto-generated method stub
		return getParametres().get(0).getValue() * x + getParametres().get(1).getValue();
	}

	public String toPrettyString() {
		return String.format("y(x) = %1$2sx + %2$2s",
				(int)getParametres().get(0).getValue(),
				(int)getParametres().get(1).getValue());
	}
}
