package DataStructuresPackage;

import static org.junit.Assert.*;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Random;

import org.junit.Test;

public class AutocompletionTest {

	@Test
	public void defaultTest() {
		Autocompletion au = new Autocompletion();
		for (int i = 0; i < 10000; i++)
			au.Add(String.valueOf(i));
		for (int i = 1; i < 100; i++) {
			ArrayList<String> answer = au.GetAutocompletions(String.valueOf(i));
			assertEquals(10, answer.size());
		}
	}

	@Test
	public void test1() {
		Autocompletion au = new Autocompletion();
		for (int i = 0; i < 5000; i++)
			au.Add(String.valueOf(i));
		au.Add("qwerty1");
		au.Add("qwerty2");
		au.Add("q2werty");
		ArrayList<String> answer = au.GetAutocompletions("qwerty");
		assertEquals(2, answer.size());
		String[] ans = new String[2];
		ans[0] = "qwerty1";
		ans[1] = "qwerty2";
		assertArrayEquals(ans, answer.toArray(new String[answer.size()]));
	}
	
	@Test
	public void test2() {
		Autocompletion au = new Autocompletion();
		
		for (int i = 0; i < 10000; i++)
			au.Add(String.valueOf(i));
		au.Add("qwerty1");
		au.Add("qwerty2");
		au.Add("q2werty");

		for (int i = 0; i < 100000; i++)
			au.Add(String.valueOf(i));

		au.Add("zqwerty1");
		au.Add("zqwerty2");
		au.Add("zq2werty");
		ArrayList<String> answer = au.GetAutocompletions("zqwerty");
		assertEquals(2, answer.size());
	}
	
	@Test
	public void test3() {
		Random rnd = new Random();
		Autocompletion au = new Autocompletion();
		
		for (int i = 0; i < 100000; i++)
			au.Add(String.valueOf(rnd.nextInt(i + 1)));

		ArrayList<String> answer = au.GetAutocompletions("zqwerty");
		assertEquals(0, answer.size());

		ArrayList<String> answer2 = au.GetAutocompletions("a");
		assertEquals(0, answer2.size());

		ArrayList<String> answer3 = au.GetAutocompletions("-00");
		assertEquals(0, answer3.size());
	}
	
	@Test
	public void test4() {
		Random rnd = new Random();
		Autocompletion au = new Autocompletion();
		
		for (int i = 0; i < 10000; i++)
			au.Add(String.valueOf(rnd.nextInt(i + 1) % 10000));

		for (int i = 0; i < 10000; i++) {
			ArrayList<String> answer = au.GetAutocompletions(String.valueOf(rnd.nextInt() + 1e7));
			assertEquals(0, answer.size());
		}
	}
	
	@Test
	public void test5() {
		Random rnd = new Random();
		Autocompletion au = new Autocompletion();
		
		for (int i = 0; i < 10000; i++)
			au.Add(String.valueOf(rnd.nextInt(i + 1) % 10000));

		au.Add("aaabc");
		au.Add("aaaasf");
		au.Add("aaagg");
		au.Add("aaaqq");
		au.Add("aaa34gg");
		au.Add("aaartnen");
		au.Add("aaavb");
		au.Add("aaa22222222222222");
		au.Add("aaassr");
		au.Add("aaaokijnpiejr");	
		
		for (int i = 0; i < 10000; i++) {
			ArrayList<String> answer = au.GetAutocompletions("a");
			assertEquals(10, answer.size());
		}
	}
}
