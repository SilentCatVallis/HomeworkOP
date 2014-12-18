package DataStructuresPackage;

public class AutocompletionString implements Comparable<AutocompletionString> {
	private String str;
	
	public AutocompletionString(String string) {
		str = string;
	}
	
	public String GetValue() {
		return str;
	}
	
	public int compareTo(AutocompletionString obj) {
		AutocompletionString string = (AutocompletionString)obj;
		String str1 = str.toLowerCase();
		String str2 = string.str.toLowerCase();
		int minLen = Math.min(str1.length(), str2.length());
		for (int i = 0; i < minLen; i++) {
			int cmp =  str1.charAt(i) - str2.charAt(i);
			if (cmp > 0)
				return 1;
			else if (cmp < 0)
				return -1;
			else continue;
		}
		int strLenCompare = str1.length() - str2.length();
		if (strLenCompare > 0)
			return 1;
		else if (strLenCompare < 0)
			return -1;
		else return 0;
		
	}
	
	public String toString() {
		return str;
	}
}
