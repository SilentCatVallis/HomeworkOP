package DataStructuresPackage;

import java.io.PrintWriter;
import java.util.ArrayList;
//import java.util.Scanner;

public class mainClass {

	public static void main(String[] args) {
		Autocompletion au = new Autocompletion();
		for (int i = 0; i < 10000; i++)
			au.Add(String.valueOf(i));
		for (int i = 0; i < 100; i++) {
			System.out.println(i);
			ArrayList<String> answer = au.GetAutocompletions(String.valueOf(i));
			if (answer.size() <= 2)
				answer = au.GetAutocompletions(String.valueOf(i));
			for (int j = 0; j < answer.size(); j++) {
				System.out.println(answer.get(j));
			}
			System.out.println("\n");
		}
	}
}
