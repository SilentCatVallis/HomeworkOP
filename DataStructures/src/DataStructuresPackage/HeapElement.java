package DataStructuresPackage;

public class HeapElement<T extends Comparable<T>, V>{
	private T key;
	private V value;
	private HeapElement<T, V> left;
	private HeapElement<T, V> right;
	
	public HeapElement(T key, V value) {
		this.key = key;
		this.value = value;
		left = null;
		right = null;
	}
	
	public T GetKey() {
		return key;
	}
	
	public V GetValue() {
		return value;
	}
	
	public void SetRight(HeapElement<T, V> newValue) {
		right = newValue;
	}
	
	public void SetLeft(HeapElement<T, V> newValue) {
		left = newValue;
	}
	
	public HeapElement<T, V> GetRight() {
		return right;
	}
	
	public HeapElement<T, V> GetLeft() {
		return left;		
	}
}
