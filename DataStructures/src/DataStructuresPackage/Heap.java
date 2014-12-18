package DataStructuresPackage;

import java.util.ArrayList;
import java.util.Random;
import java.util.Stack;

public class Heap <T extends Comparable<T>, V> implements HeapInterfase <T, V> {
	private HeapElement<T, V> head;
	private static Random random = new Random();
	
	public Heap() {
		head = null;
	}
	
	HeapElement<T, V> Merge (HeapElement<T, V> t1, HeapElement<T, V> t2) {
		if (t1 == null || t2 == null)
			return t1 == null ? t2 : t1;
		if (t2.GetKey().compareTo(t1.GetKey()) < 0) {
			HeapElement<T, V> tmp = t1;
			t1 = t2;
			t2 = tmp;
		}
		if ((random.nextInt() & 1) == 1) {
			HeapElement<T, V> tmp = t1.GetLeft();
			t1.SetLeft(t1.GetRight());
			t1.SetRight(tmp);
		}
		t1.SetLeft(Merge(t1.GetLeft(), t2));
		return t1;
	}

	@Override
	public void add(T key, V value) {
		// TODO Auto-generated method stub
		HeapElement<T, V> newElement = new HeapElement<T, V>(key, value);
		head = Merge(head, newElement);
	}

	@Override
	public V findMin() {
		// TODO Auto-generated method stub
		return head.GetValue();
	}

	@Override
	public V deleteMin() {
		// TODO Auto-generated method stub
		V value = head.GetValue();
		head = Merge(head.GetLeft(), head.GetRight());
		return value;
	}
	
	public void deleteValue(V value) {
		Stack<StackElement> stack = new Stack<StackElement>();
		stack.push(new StackElement(head));
		while (!stack.isEmpty()) {
			StackElement element = stack.peek();
			Comparable<V> localValue = (Comparable<V>)element.element.GetValue();
			if (localValue.compareTo(value) == 0) {
				element.element = Merge(element.element.GetLeft(), element.element.GetRight());
				return;
			}
			else if (!element.isLeftVisited && element.element.GetLeft() != null) {
				stack.push(new StackElement(element.element.GetLeft()));
				element.isLeftVisited = true;
				continue;
			}
			else if (!element.isRightVisited && element.element.GetRight() != null) {
				stack.push(new StackElement(element.element.GetRight()));
				element.isRightVisited = true;
				continue;
			}
			else stack.pop();
		}
	}
	
	private void delete(HeapElement<T, V> localElement, V value) {
		Comparable<V> localValue = (Comparable<V>)localElement.GetValue();
		if (localValue == null)
			return;
		if (localValue.compareTo(value) == 0) {
			localElement = Merge(localElement.GetLeft(), localElement.GetRight());
		}
		else if (localElement.GetLeft() != null) {
			delete(localElement.GetLeft(), value);
		}
		else if (localElement.GetRight() != null) {
			delete(localElement.GetRight(), value);
		}
	}
	
	class StackElement {
		HeapElement<T, V> element;
		boolean isRightVisited;
		boolean isLeftVisited;
		
		public StackElement(HeapElement<T, V> item) {
			element = item;
			isRightVisited = false;
			isLeftVisited = false;
		}
	}
}
