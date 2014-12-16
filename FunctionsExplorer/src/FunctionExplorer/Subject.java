package FunctionExplorer;

public interface Subject {
	public void registerObserver(Observer o);
	public void removeObserver(Observer o);
	public void notifyAllObservers();
	public void registerMainObserver(Observer o);
}
