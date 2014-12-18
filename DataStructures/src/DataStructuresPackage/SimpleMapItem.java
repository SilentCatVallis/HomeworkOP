package DataStructuresPackage;

public class SimpleMapItem<TKey extends Comparable<TKey>, TValue> {
	private TKey key;
	private TValue value = null;
	private int priority;
	private SimpleMapItem<TKey, TValue> left;
	private SimpleMapItem<TKey, TValue> right;
	private SimpleMapItem<TKey, TValue> parent;
	
	public SimpleMapItem(TKey key, int priority, TValue value) {
		this.key = key;
		this.priority = priority;
		this.value = value;
		left = null;
		right = null;
		parent = null;
	}
	
	public SimpleMapItem(TKey key, 
						int priority, 
						TValue value, 
						SimpleMapItem<TKey, TValue> left, 
						SimpleMapItem<TKey, TValue> right) {
		this(key, priority, value);
		this.left = left;
		this.right = right;
	}
	
	public SimpleMapItem<TKey, TValue> GetParent() {
		return parent;
	}
	
	public void SetParent(SimpleMapItem<TKey, TValue> parent) {
		this.parent = parent;
	}
	
	public SimpleMapItem<TKey, TValue> GetLeftMap() {
		return this.left;
	}
	
	public SimpleMapItem<TKey, TValue> GetRightMap() {
		return this.right;
	}
	
	public TKey GetKey(){
		return this.key;
	}
	
	public int GetPriority() {
		return this.priority;
	}
	
	public String toString1() {
		return this.value.toString() + ' ' + this.priority + ' ' + this.key.toString() + ' ' + (this.left == null ? "null" : "") + ' ' + (this.right == null ? "null" : "");
	}
	
	public void SetNewValues(TKey key, 
			int priority, 
			TValue value, 
			SimpleMapItem<TKey, TValue> left, 
			SimpleMapItem<TKey, TValue> right){
		this.key = key;
		this.priority = priority;
		this.value = value;
		this.left = left;
		this.right = right;
	}
	
	public TValue getValue() {
		return value;
	}
	
	public void setValue(TValue val) {
		value = val;
	}
}
