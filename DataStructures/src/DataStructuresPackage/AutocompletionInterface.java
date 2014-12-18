package DataStructuresPackage;

import java.util.ArrayList;

public interface AutocompletionInterface {
	void Add(String word);
	ArrayList<String> GetAutocompletions(String word);
}
