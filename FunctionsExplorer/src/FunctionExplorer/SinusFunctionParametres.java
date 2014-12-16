package FunctionExplorer;

import java.util.ArrayList;

public class SinusFunctionParametres extends Parametres {
	public SinusFunctionParametres() {
		super();
		addParameter(new SingleParameter("A", 1, 1, -100, 100));
		addParameter(new SingleParameter("B", 1, 1, -100, 100));
		addParameter(new SingleParameter("C", 0, 1, -10, 10));
	}

	public SinusFunctionParametres(ArrayList<SingleParameter> newParametres) {
		super(newParametres);
	}

	@Override
	public double getResult(double x) {
		// TODO Auto-generated method stub
		return Math.sin(getParametres().get(1).getValue() * x) * getParametres().get(0).getValue() + getParametres().get(2).getValue();
	}
	
	public String toPrettyString() {
		return String.format("y(x) = %1$2s*Sin(%2$2s*x ) + %3$2s",
				(int)getParametres().get(0).getValue(),
				(int)getParametres().get(1).getValue(),
				(int)getParametres().get(2).getValue());
	}
}
