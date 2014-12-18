package DataStructuresPackage;

public class SimpleMapPair<TKey extends Comparable<TKey>, TValue> {
	private final SimpleMap<TKey, TValue> leftMap;
	private final SimpleMap<TKey, TValue> rightMap;
	
	public SimpleMapPair(SimpleMap<TKey, TValue> leftMap, SimpleMap<TKey, TValue> rightMap) {
		this.leftMap = leftMap;
		this.rightMap = rightMap;
	}
	
	public SimpleMap<TKey, TValue> GetRightMap() {
		return this.rightMap;
	}
	
	public SimpleMap<TKey, TValue> GetLeftMap() {
		return this.leftMap;
	}
}
