package FunctionExplorer;

import java.util.ArrayList;

public class Parametres implements ParametresInterface, Subject{

	private ArrayList<SingleParameter> parametres;
	private ArrayList<Observer> observers;
	private Observer mainObserver;
	
	public Parametres() {
		parametres = new ArrayList<SingleParameter>();
		observers = new ArrayList<Observer>();
	}
	
	protected Parametres(ArrayList<SingleParameter> parametres) {
		super();
		this.parametres = parametres;
	}
	
	protected void addParameter(SingleParameter parameter) {
		parametres.add(parameter);
		parameter.addParent(this);
	}
	
	@Override
	public ArrayList<SingleParameter> getParametres() {
		return parametres;
	}

	@Override
	public double getResult(double x) {
		return 0;
	}

	@Override
	public Parametres copy() {
		ArrayList<SingleParameter> newParametres = new ArrayList<SingleParameter>();
		for (SingleParameter parameter : parametres) {
			newParametres.add(parameter.copy());
		}
		Parametres parametres = new Parametres();
		if (this instanceof LinearFunctionParametres)
			parametres = new LinearFunctionParametres(newParametres);
		else if (this instanceof CubicFunctionParametres)
			parametres = new CubicFunctionParametres(newParametres);
		else if (this instanceof QuadraticFunctionParametres)
			parametres = new QuadraticFunctionParametres(newParametres);
		else if (this instanceof SinusFunctionParametres)
			parametres = new SinusFunctionParametres(newParametres);
		else throw new RuntimeException(this.getClass().getName() + " deep copy not implemented");
		for (SingleParameter p : parametres.getParametres()) {
			p.addParent(parametres);
		}
		return parametres;
	}

	@Override
	public void registerObserver(Observer o) {
		if (observers == null)
			observers = new ArrayList<Observer>();
		observers.add(o);
		System.out.println(observers.size());
	}

	@Override
	public void removeObserver(Observer o) {
		// TODO Auto-generated method stub
		observers.remove(o);
	}

	@Override
	public void notifyAllObservers() {
		// TODO Auto-generated method stub
		if (mainObserver != null)
			mainObserver.update(this);
		if (observers == null)
			return;
		ArrayList<Observer> localObservers = new ArrayList<Observer>(observers);
		for (int i = 0; i < localObservers.size(); i++) {
			if (localObservers.get(i) != null)
				localObservers.get(i).update(this);
			else
				observers.remove(i);
		}
	}

	@Override
	public void registerMainObserver(Observer o) {
		mainObserver = o;
	}

	protected String toPrettyString() {
		return "y(x) = ";
	}

}
