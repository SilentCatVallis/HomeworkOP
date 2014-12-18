package DataStructuresPackage;

public class SimpleMapItemPair<TKey extends Comparable<TKey>, TValue> {
	private final SimpleMapItem<TKey, TValue> leftMap;
	private final SimpleMapItem<TKey, TValue> rightMap;
	
	public SimpleMapItemPair(SimpleMapItem<TKey, TValue> leftMap, SimpleMapItem<TKey, TValue> rightMap) {
		this.leftMap = leftMap;
		this.rightMap = rightMap;
	}
	
	public SimpleMapItem<TKey, TValue> GetRightMapItem() {
		return this.rightMap;
	}
	
	public SimpleMapItem<TKey, TValue> GetLeftMapItem() {
		return this.leftMap;
	}
}
