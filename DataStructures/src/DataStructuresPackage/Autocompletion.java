package DataStructuresPackage;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.logging.Logger;

public class Autocompletion implements AutocompletionInterface {
	private Logger log;
	private SimpleMap<String, String> set;
	private Heap<LocalTime, String> queue;
	private SimpleMap<String, Integer> setWithCount;
	
	private final int N = 10000;
	private final int wordCount = 10;
	
	public Autocompletion() {
		
		set = new SimpleMap<String, String>();
		queue = new Heap<LocalTime, String>();
		setWithCount = new SimpleMap<String, Integer>();
		log = Logger.getAnonymousLogger();//.getLogger(Autocompletion.class.getName());
	}
	
	public void SetLogger(Logger logger) {
		log = logger;
	}

	@Override
	public void Add(String key) {
		LocalTime time = LocalTime.now();
		log.info("add: " + key);
	
		if (set.get(key) != null) {
			setWithCount.put(key, setWithCount.get(key) + 1);
			queue.add(time, key);
			return;
		}
		
		if (set.Size() == N) {
			String minKey = queue.deleteMin();
			int cnt = setWithCount.get(minKey);
			setWithCount.put(minKey, cnt - 1);
			while (cnt != 1) {
				minKey = queue.deleteMin();
				cnt = setWithCount.get(minKey);
				setWithCount.put(minKey, cnt - 1);
			}
			set.remove(minKey);
			log.info(minKey + " has been removed");
		}
		
		set.put(key, key);
		setWithCount.put(key, 1);
		queue.add(time, key);
	}

	@Override
	public ArrayList<String> GetAutocompletions(String key) {
		Iterator<String, String> iterator = set.GetIterator(key);
		ArrayList<String> answer = new ArrayList<String>();
		String localKey = iterator.GetNextKey();
		while (localKey != null && answer.size() < wordCount) {
			if (localKey.startsWith(key))
				answer.add(localKey);
			localKey = iterator.GetNextKey();
		}
		return answer;
	}
}
