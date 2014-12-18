package DataStructuresPackage;

import static org.junit.Assert.*;

import java.io.PrintStream;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Random;

import org.junit.Test;

public class HeapTest {

	@Test
	public void simpleTest() {
		PrintStream out = System.out;
		Heap<Integer, Integer> heap = new Heap<Integer, Integer>();
		for (int i = 0; i <= 10; i++) {
			heap.add(i * 2, i * 2);
		}
		for (int i = 1; i <= 19; i+=2) {
			heap.add(i, i);
		}
		for (int i = 0; i <= 20; i++) {
			int min = (int)heap.findMin();
			heap.deleteMin();
			assertEquals(i, min);
		}
	}

	@Test
	public void hardTest() {
		Heap<Integer, Integer> heap = new Heap<Integer, Integer>();
		for (int i = 0; i <= 1e5; i++) {
			heap.add(i, i);
		}
		for (int i = 0; i <= 1e5; i++) {
			int min = (int)heap.findMin();
			heap.deleteMin();
			assertEquals(i, min);
		}
	}
	
	@Test
	public void hardTest2() {
		Random random = new Random();
		Heap<String, String> heap = new Heap<String, String>();
		ArrayList<String> list = new ArrayList<String>();
		for (int i = 0; i <= 1e5; i++) {
			String val = String.valueOf(Math.abs(random.nextInt(i + 1)));
			heap.add(val, val);
			list.add(val);
		}
		list.sort(null);
		for (int i = 0; i <= 1e5; i++) {
			String min = heap.findMin();
			heap.deleteMin();
			assertEquals(list.get(i), min);
		}
	}
}
