package DataStructuresPackage;

import java.io.PrintWriter;
import java.util.*;

public class Iterator<T extends Comparable<T>, V> implements MapIteratorInterfase<T, V>{
	
	class StackElement {
		SimpleMapItem<T, V> element;
		boolean isRightVisited;
		boolean isLeftVisited;
		
		public StackElement(SimpleMapItem<T, V> item) {
			element = item;
			isRightVisited = false;
			isLeftVisited = false;
		}
	}

	private SimpleMapItem<T, V> currentHead;
	private Stack<StackElement> stack;
	private boolean isHeadVisited;
	
	public Iterator(SimpleMapItem<T, V> minimalItem) {
		currentHead = minimalItem;
		stack = new Stack<StackElement>();
		isHeadVisited = false;
	}
	
	@Override
	public T GetNextKey() {
		if (currentHead == null)
			return null;
		if (stack.isEmpty() && isHeadVisited) {
			SimpleMapItem<T, V> parent = currentHead.GetParent();
			while (parent != null && parent.GetKey().compareTo(currentHead.GetKey()) < 0)
				parent = parent.GetParent();
			if (parent == null)
				return null;
			if (parent.GetKey().compareTo(currentHead.GetKey()) < 0) {
				return null;
			}
			currentHead = parent;
			if (currentHead.GetRightMap() != null)
				stack.push(new StackElement(currentHead.GetRightMap()));
			return currentHead.GetKey();
		}
		if (stack.isEmpty() && !isHeadVisited) {
			isHeadVisited = true;
			if (currentHead.GetRightMap() != null)
				stack.push(new StackElement(currentHead.GetRightMap()));
			return currentHead.GetKey();
		}
		while (true) {
			StackElement curElement = stack.peek();
			if (!curElement.isLeftVisited && curElement.element.GetLeftMap() != null) {
				curElement.isLeftVisited = true;
				stack.push(new StackElement(curElement.element.GetLeftMap()));
				continue;
			}
			if (curElement.element.GetLeftMap() == null)
				curElement.isLeftVisited = true;
			if (!curElement.isRightVisited && curElement.element.GetRightMap() != null) {
				curElement.isRightVisited = true;
				stack.push(new StackElement(curElement.element.GetRightMap()));
				continue;
			}
			if (curElement.element.GetRightMap() == null)
				curElement.isRightVisited = true;
			if (curElement.isLeftVisited && curElement.isRightVisited)
				stack.pop();
			return curElement.element.GetKey();
		}
	}
}
