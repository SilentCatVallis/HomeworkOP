package DataStructuresPackage;

public interface HeapInterfase <T extends Comparable<T>, V> {
	void add(T key, V value);
	V findMin();
	V deleteMin();
}
