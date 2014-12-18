package DataStructuresPackage;

import java.io.PrintStream;
import java.util.ArrayList;
import java.util.Random;

public class SimpleMap<TKey extends Comparable<TKey>, TValue> 
		implements SimpleMapInterface<TKey, TValue> {
	private int size;
	private SimpleMapItem<TKey, TValue> head = null;
	private static Random random = new Random();
	
	public SimpleMap() {
		size = 0;
	}
	
	private SimpleMap(SimpleMapItem<TKey, TValue> newHead){
		this.head = newHead;
		size = 0;
	}
	
	@Override
	public TValue get(TKey key) {
		SimpleMapItem<TKey, TValue> element = findElement(key); 
		if (element == null)
			return null;
		return element.getValue();
	}

	private SimpleMapItem<TKey, TValue> findElement(TKey key) {
		if (this.head == null)
			return null;
		int comareResult = key.compareTo(this.head.GetKey()); 
		if (comareResult == 0)
			return this.head;
		else if (comareResult > 0) {
			if (this.head.GetRightMap() == null)
				return null;
			return new SimpleMap<TKey, TValue>(this.head.GetRightMap()).findElement(key);
		}
		else {
			if (this.head.GetLeftMap() == null)
				return null;
			return new SimpleMap<TKey, TValue>(this.head.GetLeftMap()).findElement(key);
		}
	}

	@Override
	public TValue put(TKey key, TValue value) {
		if (this.head == null) {
			this.head = new SimpleMapItem<TKey, TValue>(key, random.nextInt(), value);
			size++;
			return null;
		}
		SimpleMapItem<TKey, TValue> element = findElement(key);
		if (element != null) {
			TValue lastValue = element.getValue();
			element.setValue(value);
			return lastValue;			
		}
		SimpleMapItem<TKey, TValue> l = null;
		SimpleMapItem<TKey, TValue> r = null;
	    SimpleMapItemPair<TKey, TValue> LR = Split(key, l, r, false);
	    SimpleMapItem<TKey, TValue> m = new SimpleMapItem<TKey, TValue>(key, random.nextInt(), value);
	    this.head = Merge(Merge(LR.GetLeftMapItem(), m), LR.GetRightMapItem());
	    size++;
	    return null;
	}

	@Override
	public TValue remove(TKey key) {
		SimpleMapItem<TKey, TValue> element = findElement(key);
		if (element == null)
			return null;
		SimpleMapItem<TKey, TValue> l = null, m = null, r = null;
	    SimpleMapItemPair<TKey, TValue> LR1 = Split(key, l, r, true);
	    l = LR1.GetLeftMapItem();
	    r = LR1.GetRightMapItem();
	    SimpleMapItemPair<TKey, TValue> LR2 = new SimpleMap<TKey, TValue>(r).Split(key, m, r, false);
	    m = LR2.GetLeftMapItem();
	    r = LR2.GetRightMapItem();
	    this.head = Merge(l, r);
	    size--;
	    return m.getValue();
	}

	private SimpleMapItem<TKey, TValue> Merge(SimpleMapItem<TKey, TValue> L, SimpleMapItem<TKey, TValue> R)	{
	    if (L == null) return R;
	    if (R == null) return L;
	    if (L.GetPriority() > R.GetPriority())
	    {
	    	SimpleMapItem<TKey, TValue> newR = Merge(L.GetRightMap(), R);
	    	SimpleMapItem<TKey, TValue> answer = new SimpleMapItem<TKey, TValue>(L.GetKey(), L.GetPriority(), L.getValue(), L.GetLeftMap(), newR);
	    	if (L.GetLeftMap() != null)
	    		L.GetLeftMap().SetParent(answer);
	    	if (newR != null)
	    		newR.SetParent(answer);
	    	return answer;
	    }
	    else
	    {
	    	SimpleMapItem<TKey, TValue> newL = Merge(L, R.GetLeftMap());
	    	SimpleMapItem<TKey, TValue> answer =  new SimpleMapItem<TKey, TValue>(R.GetKey(), R.GetPriority(), R.getValue(), newL, R.GetRightMap());
	    	if (newL != null)
	    		newL.SetParent(answer);
	    	if (R.GetRightMap() != null)
	    		R.GetRightMap().SetParent(answer);
	    	return answer;
	    }
	}
	
	private SimpleMapItemPair<TKey, TValue> Split(TKey x, SimpleMapItem<TKey, TValue> L, SimpleMapItem<TKey, TValue> R, Boolean IsExceptionalCase)
	{
		SimpleMapItem<TKey, TValue> newTree = null;
		if (head == null)
			return new SimpleMapItemPair<TKey, TValue>(null, null);
		int comparisonResult = head.GetKey().compareTo(x);
		Boolean boolComparisonResult = IsExceptionalCase ? comparisonResult < 0 : comparisonResult <= 0;  
	    if (boolComparisonResult)
	    {
	        if (head.GetRightMap() == null)
	            R = null;
	        else {
	        	SimpleMapItemPair<TKey, TValue> LR = new SimpleMap<TKey, TValue>(this.head.GetRightMap()).Split(x, newTree, R, IsExceptionalCase);
	        	newTree = LR.GetLeftMapItem();
	        	R = LR.GetRightMapItem();
	        }
	        L = new SimpleMapItem<TKey, TValue>(head.GetKey(), head.GetPriority(), head.getValue(), head.GetLeftMap(), newTree);
	        if (head.GetLeftMap() != null)
	        	head.GetLeftMap().SetParent(L);
	        if (newTree != null)
	        	newTree.SetParent(L);
	        return new SimpleMapItemPair<TKey, TValue>(L, R);
	    }
	    else
	    {
	        if (head.GetLeftMap() == null)
	            L = null;
	        else {
	        	SimpleMapItemPair<TKey, TValue> LR = new SimpleMap<TKey, TValue>(this.head.GetLeftMap()).Split(x, L, newTree, IsExceptionalCase);
	        	L = LR.GetLeftMapItem();
	        	newTree = LR.GetRightMapItem();
	        }
	        R = new SimpleMapItem<TKey, TValue>(head.GetKey(), head.GetPriority(), head.getValue(), newTree, head.GetRightMap());
	        if (newTree != null)
	        	newTree.SetParent(R);
	        if (head.GetRightMap() != null)
	        	head.GetRightMap().SetParent(R);
	        return new SimpleMapItemPair<TKey, TValue>(L, R);
	    }
	}
	
	public int Size() {
		return size;
	}

	@Override
	public Iterator<TKey, TValue> GetIterator(TKey key) {
		SimpleMapItem<TKey, TValue> currentItem = head;
		while (true) {
			int compareResult = key.compareTo(currentItem.GetKey());
			if (compareResult == 0 || 
					(compareResult < 0 && currentItem.GetLeftMap() == null) ||
					(compareResult > 0 && currentItem.GetRightMap() == null)) {
				return new Iterator<TKey, TValue>(currentItem);
			}
			else if (compareResult < 0) {
				currentItem = currentItem.GetLeftMap();
			}
			else {
				currentItem = currentItem.GetRightMap();	
			}
		}
	}
	
	/*public TKey GetMinKey() {
		SimpleMapItem<TKey, TValue> key = head;
		while (key.GetLeftMap() != null) {
			key = key.GetLeftMap();
		}
		return key.GetKey();
	}*/
}
