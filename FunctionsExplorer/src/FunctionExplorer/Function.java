package FunctionExplorer;

import java.awt.Dimension;

public class Function {

	protected String name;
	protected Parametres parametres;
	
	public Function() {
		name = "";
		parametres = null;
	}
	
	private Function(String name, Parametres parametres) {
		super();
		this.name = name;
		this.parametres = parametres;
	}
	
	public String getName() {
		return name;
	}

	public double getResult(double x) {
		return parametres.getResult(x);
	}

	public Parametres getParametres() {
		return parametres;
	}

	public void setParametres(Parametres parametres) {
		this.parametres = parametres;
	}

	public Function copy() {
		return new Function(new String(name), parametres.copy());
	}
	
	public void setName(String name) {
		this.name = name;
	}

	public FunctionPoints getPoints(Dimension size) {
		return new FunctionPoints(size, this);
	}
	
	public String toPrettyString() {
		return parametres.toPrettyString();
	}
}
