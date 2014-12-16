package FunctionExplorer;

public class LinearFunction extends Function {
	
	public LinearFunction() {
		super();
		name = "Linear";
		parametres = new LinearFunctionParametres();
	}
	
	public String toPrettyString() {
		return parametres.toPrettyString();
	}
}
