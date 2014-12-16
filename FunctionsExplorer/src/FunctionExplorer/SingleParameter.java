package FunctionExplorer;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class SingleParameter implements SingleParameterInterface {

	private String name;
	private double step;
	private double minValue;
	private double maxValue;
	private double value;
	private Parametres parentParametres;
	

	public SingleParameter(String name, double value, double step, double minValue, double maxValue) {
		this.name = name;
		this.value = value;
		this.step = step;
		if (minValue > maxValue)
			throw new RuntimeException("minValue must be less or equal then the maxValue");
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
	
	@Override
	public String getName() {
		return name;
	}

	@Override
	public double getStep() {
		return step;
	}

	@Override
	public double getValue() {
		return value;
	}

	@Override
	public void setValue(double value) {
		if (value > maxValue)
			value = maxValue;
		if (value < minValue)
			value = minValue;
		this.value = value;
		parentParametres.notifyAllObservers();
	}

	@Override
	public double getMaxValue() {
		return maxValue;
	}

	@Override
	public double getMinValue() {
		return minValue;
	}

	public SingleParameter copy() {
		return new SingleParameter(new String(name), value, step, minValue, maxValue);
	}

	public void addParent(Parametres parametres) {
		// TODO Auto-generated method stub
		this.parentParametres = parametres;
	}

}
