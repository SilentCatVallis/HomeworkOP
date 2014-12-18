package DataStructuresPackage;

import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.HashMap;

//import junit.framework.Assert;

import java.util.Random;

import org.junit.Test;

public class SimpleMapTest {

	private SimpleMap<String, Integer> InitialiseSimpleMap() {
		SimpleMap<String, Integer> simpleMap = new SimpleMap<String, Integer>();
		simpleMap.put("0", 0);
		simpleMap.put("5", 5);
		simpleMap.put("111", 111);
		simpleMap.put("12", 12);
		simpleMap.put("10", 10);
		return simpleMap;
	}
	
	@Test
	public void easyTest() {
		SimpleMap<String, Integer> myMap = InitialiseSimpleMap();
		assertEquals(new Integer(5), myMap.get("5"));	
		assertEquals(new Integer(0), myMap.get("0"));	
		assertEquals(new Integer(111), myMap.get("111"));	
		assertEquals(new Integer(12), myMap.get("12"));	
		assertNotEquals(new Integer(11), myMap.get("11"));	
	}
	
	@Test
	public void hardTest1() {
		SimpleMap<Integer, Integer> simpleMap = new SimpleMap<Integer, Integer>();
		for (int i = 0; i < 1e6; i++) {
			simpleMap.put(i, -i);
		}
		for (int i = (int) (1e6 - 1); i >= 0; i--) {
			assertEquals(new Integer(-i), simpleMap.get(i));
		}
	}
	
	@Test
	public void hardTest2() {
		SimpleMap<String, Float> simpleMap = new SimpleMap<String, Float>();
		for (int i = 0; i < 1e6; i++) {
			simpleMap.put(String.valueOf(i), (float)i + 0.5f);
		}
		for (int i = (int) (1e6 - 1); i >= 0; i--) {
			if (i % 2 == 0) {
				simpleMap.remove(String.valueOf(i));
				assertEquals(null, simpleMap.get(String.valueOf(i)));
			}
			else {
				float a = (float)i + 0.5f;
				float b = simpleMap.get(String.valueOf(i));
				assertEquals(a, b, 1e-9);
			}
		}
	}
	
	@Test
	public void hardTest1Default() {
		HashMap<Integer, Integer> defaultMap = new HashMap<Integer, Integer>();
		for (int i = 0; i < 1e6; i++) {
			defaultMap.put(i, -i);
		}
		for (int i = (int) (1e6 - 1); i >= 0; i--) {
			assertEquals(new Integer(-i), defaultMap.get(i));
		}
	}
}
