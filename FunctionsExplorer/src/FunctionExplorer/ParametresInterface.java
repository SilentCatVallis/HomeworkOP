package FunctionExplorer;

import java.util.ArrayList;

public interface ParametresInterface {

	public ArrayList<SingleParameter> getParametres();
	
	public double getResult(double x);

	public ParametresInterface copy();
}
