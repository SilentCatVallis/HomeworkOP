package DataStructuresPackage;

public interface SimpleMapInterface<TKey extends Comparable<TKey>, TValue> {
	TValue get(TKey key);
	TValue put(TKey key, TValue value);
	TValue remove(TKey key);
	Iterator<TKey, TValue> GetIterator(TKey key);
}
